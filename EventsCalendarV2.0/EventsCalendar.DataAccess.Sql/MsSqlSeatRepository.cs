using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlSeatRepository : ISeatRepository
    {
        internal DataContext Context;
        internal readonly string connectionString = ConfigurationManager.ConnectionStrings["EventCalendarDataContext"].ConnectionString;

        public MsSqlSeatRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Seat> Collection()
        {
            return Context.Seats.ToList();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var seat = Context.Seats.Single(s => s.Id == id);

            if (Context.Entry(seat).State == EntityState.Detached)
                Context.Seats.Attach(seat);

            Context.Seats.Remove(seat);
        }

        public Seat Find(int id)
        {
            return Context.Seats
                .Include(s => s.Venue)
                .SingleOrDefault(s => s.Id == id);
        }

        public void Insert(Seat seat)
        {
            Context.Seats.Add(seat);
        }

        public void Update(Seat seat)
        {
            Context.Seats.Attach(seat);
            Context.Entry(seat).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            Context.Configuration.AutoDetectChangesEnabled = enabled;
        }

        public void BulkInsertSeats(int numberOfSeats, SeatType type, int venueId)
        {
            var dt = MakeTable();

            for (var i = 0; i <= numberOfSeats; i++)
            {
                DataRow row = dt.NewRow();
                row["SeatType"] = (int) type;
                row["VenueId"] = venueId;
                dt.Rows.Add(row);
            }

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    connectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BatchSize = numberOfSeats;

                    bulkCopy.DestinationTableName = "Seats";

                    try
                    {
                        bulkCopy.ColumnMappings.Add("SeatType", "SeatType");
                        bulkCopy.ColumnMappings.Add("VenueId", "VenueId");
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

        public void DeleteAllVenueSeats(int venueId)
        {
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                try
                {
                    var venueIdParam = new SqlParameter("@venueId", venueId);
                    Context.Database.ExecuteSqlCommand("EXEC dbo.BulkDeleteSeats @venueId", venueIdParam);
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

        public void BulkDeleteSeats(int numberOfSeats, SeatType type, int venueId)
        {
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                try
                {
                    numberOfSeats = Math.Abs(numberOfSeats);
                    int typeInt = (int) type;
                    var numberOfSeatsParam = new SqlParameter("@numberOfSeats", numberOfSeats);
                    var seatTypeParam = new SqlParameter("@seatType", typeInt);
                    var venueIdParam = new SqlParameter("@venueId", venueId);
                    Context.Database.ExecuteSqlCommand("dbo.BulkDeleteSeats @numberOfSeats, @seatType, @venueId", numberOfSeatsParam, seatTypeParam, venueIdParam);
                    Console.WriteLine("Seats successfully deleted.");
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

        private DataTable MakeTable()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn()
            {
                DataType = typeof(int),
                ColumnName = "SeatType"
            });

            dt.Columns.Add(new DataColumn()
            {
                DataType = typeof(int),
                ColumnName = "VenueId"
            });

            return dt;
        }
    }
}
