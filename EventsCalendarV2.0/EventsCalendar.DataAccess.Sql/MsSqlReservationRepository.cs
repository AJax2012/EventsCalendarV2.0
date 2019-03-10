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
        internal readonly string ConnectionString = ConfigurationManager.ConnectionStrings["EventCalendarDataContext"].ConnectionString;

        public MsSqlReservationRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Reservation> Collection()
        {
            return Context.Reservations
                .Include(res => res.Seat)
                .Include(res => res.Performance)
                .ToList();
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

        public ReservationPrices GetPrices(int performanceId)
        {
            var capacity = new ReservationPrices
            {
                Budget = Context.Reservations
                    .Where(res => res.Seat.SeatType == SeatType.Budget)
                    .First(res => res.PerformanceId == performanceId)
                    .Price,
                Moderate = Context.Reservations
                    .Where(res => res.Seat.SeatType == SeatType.Moderate)
                    .First(res => res.PerformanceId == performanceId)
                    .Price,
                Premier = Context.Reservations
                    .Where(res => res.Seat.SeatType == SeatType.Premier)
                    .First(res => res.PerformanceId == performanceId)
                    .Price
            };

            return capacity;
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

            IEnumerable<SimpleReservation> simpleReservations = reservations as SimpleReservation[] ?? reservations.ToArray();

            foreach (var reservation in simpleReservations)
            {
                var row = dt.NewRow();
                row["Price"] = reservation.Price;
                row["SeatId"] = reservation.SeatId;
                row["PerformanceId"] = performanceId;
                row["IsTaken"] = false;
                dt.Rows.Add(row);
            }

            using (var sourceConnection = new SqlConnection(ConnectionString))
            {
                sourceConnection.Open();

                using (var bulkCopy = new SqlBulkCopy(
                    ConnectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BatchSize = simpleReservations.Count();

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

        public void ChangeReservationPrices(UpdatePricesObject update)
        {
            using (var sourceConnection = new SqlConnection(ConnectionString))
            {
                sourceConnection.Open();

                try
                {
                    var priceParam = new SqlParameter("@price", update.Price);
                    var seatTypeParam = new SqlParameter("@seatType", update.Type);
                    var performanceIdParam = new SqlParameter("@performanceId", update.PerformanceId);
                    Context.Database.ExecuteSqlCommand("dbo.BulkUpdatePrices @price, @seatType, @performanceId", priceParam, seatTypeParam, performanceIdParam);
                    Console.WriteLine("Reservation prices updated successfully.");
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

        public void DeleteAllPerformanceReservations(int performanceId)
        {
            using (var sourceConnection = new SqlConnection(ConnectionString))
            {
                sourceConnection.Open();

                try
                {
                    var performanceIdParam = new SqlParameter("@performanceId", performanceId);
                    Context.Database.ExecuteSqlCommand("dbo.BulkDeletePerformanceReservations @performanceId", performanceIdParam);
                    Console.WriteLine("All reservations for performance deleted successfully.");
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

        public void BulkDeleteReservations(int numberOfReservations, decimal price, int performanceId)
        {
            using (var sourceConnection = new SqlConnection(ConnectionString))
            {
                sourceConnection.Open();

                try
                {
                    numberOfReservations = Math.Abs(numberOfReservations);
                    var numberOfSeatsParam = new SqlParameter("@numberOfReservations", numberOfReservations);
                    var seatTypeParam = new SqlParameter("@price", price);
                    var venueIdParam = new SqlParameter("@performanceId", performanceId);
                    Context.Database.ExecuteSqlCommand("dbo.BulkDeleteReservations @numberOfSeats, @seatType, @venueId", numberOfSeatsParam, seatTypeParam, venueIdParam);
                    Console.WriteLine("Reservations successfully deleted.");
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
