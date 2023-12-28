using System;
using System.Collections.Generic;
using System.Net.Sockets;
using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySqlConnector;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace FastGuard.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        public string ConnectionString { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public List<object> GetRevenue()
        {
            List<object> list = new List<object>();
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT YEAR(invoice_date) AS 'year', MONTH(invoice_date) AS 'month', SUM(total_money) AS 'total_amount' FROM tickets GROUP BY YEAR(invoice_date), MONTH(invoice_date) ORDER BY YEAR(invoice_date), MONTH(invoice_date);";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ob = new
                        {
                            thang = Convert.ToInt32(reader["month"]),
                            nam = Convert.ToInt32(reader["year"]),
                            doanhThu = Convert.ToDouble(reader["total_amount"])/100
                        };
                        list.Add(ob);
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return list;
        }

        public bool checkCustomerWithTicket(string CustomerId)
        {
            bool check = false;
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM aspnetusers a JOIN tickets c ON a.Id=c.user_id where c.user_id=@CUSTOMERID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("CUSTOMERID", CustomerId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        check = true;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return check;
        }
        public bool checkDriverWithCoach(string driverId)
        {
            bool check = false;
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM aspnetusers a JOIN coaches c ON a.Id=c.user_id where c.user_id=@DRIVERID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("DRIVERID", driverId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        check = true;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return check;
        }
        public bool checkExistUserEdit(string Id,string email)
        {
            bool check = false;
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM aspnetusers where (Email=@EMAIL OR UserName=@EMAIL) AND Id!=@ID;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("EMAIL", email);
                cmd.Parameters.AddWithValue("ID", Id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        check = true;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return check;
        }
        public bool checkExistUser (string email)
        {
            bool check = false;
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM aspnetusers where Email=@EMAIL OR UserName=@EMAIL;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("EMAIL", email);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) 
                    {
                        check = true;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return check;
        }
        public int countTicket()
        {
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            int numberTicket = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as 'countTicket' FROM tickets;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Di chuyển đến hàng đầu tiên trong tập kết quả
                    {
                        numberTicket = Convert.ToInt32(reader["countTicket"].ToString());
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return numberTicket;
        }
        public int countRoutes()
        {
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            int numberRoute = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as 'countRoute' FROM routes;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Di chuyển đến hàng đầu tiên trong tập kết quả
                    {
                        numberRoute = Convert.ToInt32(reader["countRoute"].ToString());
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return numberRoute;
        }
        public int countLocation()
        {
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            int numberLocation = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as 'countLocation' FROM locations;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Di chuyển đến hàng đầu tiên trong tập kết quả
                    {
                        numberLocation = Convert.ToInt32(reader["countLocation"].ToString());
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return numberLocation;
        }
        public int countPickLocation()
        {
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            int numberPickLocation = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as 'countPickLocation' FROM pick_locations;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Di chuyển đến hàng đầu tiên trong tập kết quả
                    {
                        numberPickLocation = Convert.ToInt32(reader["countPickLocation"].ToString());
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return numberPickLocation;
        }
        public int countCoach()
        {
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            int numberCoach = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as 'countCoach' FROM coaches;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Di chuyển đến hàng đầu tiên trong tập kết quả
                    {
                        numberCoach = Convert.ToInt32(reader["countCoach"].ToString());
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return numberCoach;
        }
        public int CreateCoach(Coach c)
        {
            string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
            int rowsAffected = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "insert into coaches(coach_no, user_id, supplier,capacity,description) values(@no, @userid,@supplier,@capacity,@des)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("no", c.CoachNo);
                    cmd.Parameters.AddWithValue("userid", c.UserId);
                    cmd.Parameters.AddWithValue("supplier", c.Supplier);
                    cmd.Parameters.AddWithValue("capacity", c.Capacity);
                    //cmd.Parameters.AddWithValue("status", c.Status);
                    cmd.Parameters.AddWithValue("des", c.Description);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }
		public int CreateTicket(Ticket c)
		{
			string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";
			int rowsAffected = 0;

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string sql = "insert into tickets(`user_id`, `seat_no`, `invoice_date`, `pick_location_id1`, `pick_location_id2`, `route_id`, `total_money`, `ticket_status`) values(@userid,@seat,@currentdate,@pick1,@pick2,@routeid,@total, 0)";
				using (MySqlCommand cmd = new MySqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("userid", c.UserId);
					cmd.Parameters.AddWithValue("seat", c.SeatNo);
                    cmd.Parameters.AddWithValue("currentdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("pick1", c.PickLocationId1);
					cmd.Parameters.AddWithValue("pick2", c.PickLocationId2);
					cmd.Parameters.AddWithValue("routeid", c.RouteId);
					cmd.Parameters.AddWithValue("total", c.TotalMoney);

					rowsAffected = cmd.ExecuteNonQuery();
				}
			}

			return rowsAffected;
		}
		public List<Models.Route> SearchRoute(int locationid1, int locationid2, string startdate)
		{
			string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";

			List < Models.Route > list = new List<Models.Route> ();

			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				conn.Open();
                string str = "SELECT r.route_id, r.coach_id, r.location_id1, r.location_id2, r.start_date, r.end_date, r.price, COUNT(t.seat_no) as SLCL " +
                    "from routes r " +
                    "LEFT JOIN tickets t ON r.route_id = t.route_id " +
					"WHERE location_id1=@locationid1 and location_id2=@locationid2 and DATE(start_date)=@startdate " +
                    "GROUP BY r.route_id, r.start_date, r.end_date, r.price";
				//string str = "SELECT * from routes WHERE location_id1=@locationid1 and location_id2=@locationid2 and DATE(start_date)=@startdate";
				MySqlCommand cmd = new MySqlCommand(str, conn);
				cmd.Parameters.AddWithValue("locationid1", locationid1);
				cmd.Parameters.AddWithValue("locationid2", locationid2);
				cmd.Parameters.AddWithValue("startdate", startdate);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Models.Route route = new Models.Route()
						{
							RouteId = Convert.ToInt32(reader["route_id"]),
							CoachId = Convert.ToInt32(reader["coach_id"]),
							LocationId1 = Convert.ToInt32(reader["location_id1"]),
							LocationId2 = Convert.ToInt32(reader["location_id2"]),
							StartDate = Convert.ToDateTime(reader["start_date"]),
							EndDate = Convert.ToDateTime(reader["end_date"]),
							Price = Convert.ToSingle(reader["price"]),
						};
						list.Add(route);
					}
					reader.Close();
				}

				conn.Close();
			}

			return list;
		}
		public List<Dictionary<string, object>> CountBookedSeat(int locationid1, int locationid2, string startdate)
		{
			string connectionString = "server=localhost;user id=root;port=3307;database=fastguard";

			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				conn.Open();
				string str = "SELECT r.route_id, COUNT(t.seat_no) as SLCL " +
					"from routes r " +
					"LEFT JOIN tickets t ON r.route_id = t.route_id " +
					"WHERE location_id1=@locationid1 and location_id2=@locationid2 and DATE(start_date)=@startdate " +
					"GROUP BY r.route_id";
				MySqlCommand cmd = new MySqlCommand(str, conn);
				cmd.Parameters.AddWithValue("locationid1", locationid1);
				cmd.Parameters.AddWithValue("locationid2", locationid2);
				cmd.Parameters.AddWithValue("startdate", startdate);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var dict = new Dictionary<string, object>();
						dict["RouteId"] = Convert.ToInt32(reader["route_id"]);
						dict["SLCL"] = Convert.ToInt32(reader["SLCL"]);
						list.Add(dict);
					}
					reader.Close();
				}

				conn.Close();
			}

			return list;
		}

		public virtual DbSet<ApplicationUser> Users { get; set; } = null!;
		public virtual DbSet<Coach> Coaches { get; set; } = null!;
		public virtual DbSet<Location> Locations { get; set; } = null!;
		public virtual DbSet<PickLocation> PickLocations { get; set; } = null!;
		public virtual DbSet<Models.Route> Routes { get; set; } = null!;

        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<Seat> Seats { get; set; } = null!;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
				optionsBuilder.UseMySql("server=localhost;user id=root;port=3307;database=fastguard", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.UseCollation("utf8mb4_general_ci")
				.HasCharSet("utf8mb4");
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Coach>(entity =>
			{
				entity.ToTable("coaches");

				entity.HasIndex(e => e.UserId, "user_id");

				entity.Property(e => e.CoachId)
					.HasColumnType("int(11)")
					.HasColumnName("coach_id");

				entity.Property(e => e.Capacity)
					.HasColumnType("int(11)")
					.HasColumnName("capacity");

				entity.Property(e => e.CoachNo)
					.HasMaxLength(50)
					.HasColumnName("coach_no");

				entity.Property(e => e.Description)
					.HasMaxLength(50)
					.HasColumnName("description");

				entity.Property(e => e.Status)
					.HasMaxLength(50)
					.HasColumnName("status")
					.HasDefaultValueSql("'Active'");

				entity.Property(e => e.Supplier)
					.HasMaxLength(50)
					.HasColumnName("supplier");

				entity.Property(e => e.UserId).HasColumnName("user_id");

				entity.HasOne(d => d.User)
					.WithMany()
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("fk_userid");
			});
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("seats");

                entity.Property(e => e.SeatNo)
                    .HasMaxLength(10)
                    .HasColumnName("seatno");
            });

            modelBuilder.Entity<Location>(entity =>
			{
				entity.ToTable("locations");

				entity.Property(e => e.LocationId)
					.HasColumnType("int(11)")
					.HasColumnName("location_id");

				entity.Property(e => e.LocationName)
					.HasMaxLength(50)
					.HasColumnName("location_name");
			});

			modelBuilder.Entity<PickLocation>(entity =>
			{
				entity.ToTable("pick_locations");

				entity.HasIndex(e => e.LocationId, "location_id");

				entity.Property(e => e.PickLocationId)
					.HasColumnType("int(11)")
					.HasColumnName("pick_location_id");

				entity.Property(e => e.LocationId)
					.HasColumnType("int(11)")
					.HasColumnName("location_id");

				entity.Property(e => e.PickLocationName)
					.HasMaxLength(50)
					.HasColumnName("pick_location_name");

				entity.HasOne(d => d.Location)
					.WithMany(p => p.PickLocations)
					.HasForeignKey(d => d.LocationId)
					.HasConstraintName("pick_locations_ibfk_1");
			});

			modelBuilder.Entity<Models.Route>(entity =>
			{
				entity.ToTable("routes");

				entity.HasIndex(e => e.CoachId, "coach_id");

				entity.HasIndex(e => e.LocationId1, "location_id1");

				entity.HasIndex(e => e.LocationId2, "location_id2");

				entity.Property(e => e.RouteId)
					.HasColumnType("int(11)")
					.HasColumnName("route_id");

				entity.Property(e => e.CoachId)
					.HasColumnType("int(11)")
					.HasColumnName("coach_id");

				entity.Property(e => e.EndDate)
					.HasColumnType("datetime")
					.HasColumnName("end_date");

				entity.Property(e => e.LocationId1)
					.HasColumnType("int(11)")
					.HasColumnName("location_id1");

				entity.Property(e => e.LocationId2)
					.HasColumnType("int(11)")
					.HasColumnName("location_id2");

				entity.Property(e => e.Price).HasColumnName("price");

				entity.Property(e => e.StartDate)
					.HasColumnType("datetime")
					.HasColumnName("start_date");

				entity.HasOne(d => d.Coach)
					.WithMany(p => p.Routes)
					.HasForeignKey(d => d.CoachId)
					.HasConstraintName("routes_ibfk_1");

				entity.HasOne(d => d.LocationId1Navigation)
					.WithMany(p => p.RouteLocationId1Navigations)
					.HasForeignKey(d => d.LocationId1)
					.HasConstraintName("routes_ibfk_2");

				entity.HasOne(d => d.LocationId2Navigation)
					.WithMany(p => p.RouteLocationId2Navigations)
					.HasForeignKey(d => d.LocationId2)
					.HasConstraintName("routes_ibfk_3");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tickets");

                entity.HasIndex(e => e.PickLocationId1, "fk_pick_location");

                entity.HasIndex(e => e.PickLocationId2, "fk_pick_location2");

                entity.HasIndex(e => e.RouteId, "route_id");
				entity.Property(e => e.TotalMoney).HasColumnName("total_money");

				entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.InvoiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("invoice_id");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("invoice_date")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.PickLocationId1)
                    .HasColumnType("int(11)")
                    .HasColumnName("pick_location_id1");

                entity.Property(e => e.PickLocationId2)
                    .HasColumnType("int(11)")
                    .HasColumnName("pick_location_id2");

                entity.Property(e => e.RouteId)
                    .HasColumnType("int(11)")
                    .HasColumnName("route_id");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("ticket_status");

                entity.Property(e => e.SeatNo)
					.HasMaxLength(20)
					.HasColumnName("seat_no");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PickLocationId1Navigation)
                    .WithMany(p => p.TicketPickLocationId1Navigations)
                    .HasForeignKey(d => d.PickLocationId1)
                    .HasConstraintName("fk_pick_location");

                entity.HasOne(d => d.PickLocationId2Navigation)
                    .WithMany(p => p.TicketPickLocationId2Navigations)
                    .HasForeignKey(d => d.PickLocationId2)
                    .HasConstraintName("fk_pick_location2");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("tickets_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_userid_invoices");
            });

            OnModelCreatingPartial(modelBuilder);
        }
     

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
