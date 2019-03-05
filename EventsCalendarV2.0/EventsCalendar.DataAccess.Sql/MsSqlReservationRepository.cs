using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlReservationRepository : IReservationRepository
    {
        internal DataContext Context;
        internal readonly string connectionString = ConfigurationManager.ConnectionStrings["EventCalendarDataContext"].ConnectionString;

        public MsSqlReservationRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Reservation> Collection()
        {
            return Context.Reservations.ToList();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var reservation = Context.Reservations.Single(r => r.Id == id);

            if (Context.Entry(reservation).State == EntityState.Detached)
                Context.Reservations.Attach(reservation);

            Context.Reservations.Remove(reservation);
        }

        public Reservation Find(Guid id)
        {
            return Context.Reservations
                .Include(r => r.Performance)
                .Include(r => r.Seat)
                .SingleOrDefault(r => r.Id == id);
        }

        public void Insert(Reservation reservation)
        {
            Context.Reservations.Add(reservation);
        }

        public void Update(Reservation reservation)
        {
            Context.Reservations.Attach(reservation);
            Context.Entry(reservation).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            Context.Configuration.AutoDetectChangesEnabled = enabled;
        }

        public void BulkInsertReservations(IEnumerable<SimpleReservation> reservations, int performanceId)
        {
            var dt = MakeTable();

            foreach (var reservation in reservations)
            {
                DataRow row = dt.NewRow();
                row["Price"] = reservation.Price;
                row["SeatId"] = reservation.SeatId;
                row["PerformanceId"] = performanceId;
                row["IsTaken"] = false;
                dt.Rows.Add(row);
            }

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    connectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BatchSize = reservations.Count();

                    bulkCopy.DestinationTableName = "Reservations";

                    try
                    {
                        bulkCopy.ColumnMappings.Add("Price", "Price");
                        bulkCopy.ColumnMappings.Add("SeatId", "SeatId");
                        bulkCopy.ColumnMappings.Add("PerformanceId", "PerformanceId");
                        bulkCopy.ColumnMappings.Add("IsTaken", "IsTaken");
                        bulkCopy.WriteToServer(dt);
                        Console.WriteLine("Bulk data stored successfully");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        sourceConnection.Close();
                    }
                }
            }
        }

        public void DeleteAllPerformanceReservations(int performanceId)
        {
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                try
                {
                    var performanceIdParam = new SqlParameter("@performanceId", performanceId);
                    Context.Database.ExecuteSqlCommand("dbo.BulkDeletePerformanceReservations @performanceId", performanceIdParam);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    sourceConnection.Close();
                }
            }
        }

        public void BulkDeleteReservations(int numberOfReservations, SeatType type, int performanceId)
        {
            throw new NotImplementedException();
        }

        private DataTable MakeTable()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn()
            {
                DataType = typeof(decimal),
                ColumnName = "Price"
            });

            dt.Columns.Add(new DataColumn()
            {
                DataType = typeof(int),
                ColumnName = "SeatId"
            });

            dt.Columns.Add(new DataColumn()
            {
                DataType = typeof(int),
                ColumnName = "PerformanceId"
            });

            dt.Columns.Add(new DataColumn()
            {
                DataType = typeof(bool),
                ColumnName = "IsTaken"
            });

            return dt;
        }
    }
}
