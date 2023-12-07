using System;
using System.Collections.Generic;
using FastGuard.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FastGuard.Data
{
	public partial class ApplicationDbContext : IdentityDbContext
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Coach> Coaches { get; set; } = null!;
		public virtual DbSet<Invoice> Invoices { get; set; } = null!;
		public virtual DbSet<Location> Locations { get; set; } = null!;
		public virtual DbSet<PickLocation> PickLocations { get; set; } = null!;
		public virtual DbSet<FastGuard.Data.Route> Routes { get; set; } = null!;
		public virtual DbSet<Ticket> Tickets { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
				optionsBuilder.UseMySql("server=localhost;user id=root;port=3307;database=test_fastguard", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));
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

			modelBuilder.Entity<Invoice>(entity =>
			{
				entity.ToTable("invoices");

				entity.HasIndex(e => e.PickLocationId1, "fk_pick_location");

				entity.HasIndex(e => e.PickLocationId2, "fk_pick_location2");

				entity.HasIndex(e => e.TicketId, "ticket_id");

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

				entity.Property(e => e.SeatNo)
					.HasColumnType("int(11)")
					.HasColumnName("seat_no");

				entity.Property(e => e.TicketId)
					.HasColumnType("int(11)")
					.HasColumnName("ticket_id");

				entity.Property(e => e.UserId).HasColumnName("user_id");

				entity.HasOne(d => d.PickLocationId1Navigation)
					.WithMany(p => p.InvoicePickLocationId1Navigations)
					.HasForeignKey(d => d.PickLocationId1)
					.HasConstraintName("fk_pick_location");

				entity.HasOne(d => d.PickLocationId2Navigation)
					.WithMany(p => p.InvoicePickLocationId2Navigations)
					.HasForeignKey(d => d.PickLocationId2)
					.HasConstraintName("fk_pick_location2");

				entity.HasOne(d => d.Ticket)
					.WithMany(p => p.Invoices)
					.HasForeignKey(d => d.TicketId)
					.HasConstraintName("invoices_ibfk_2");

				entity.HasOne(d => d.User)
					.WithMany()
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("fk_userid_invoices");
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

			modelBuilder.Entity<FastGuard.Data.Route>(entity =>
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
				entity.ToTable("tickets");

				entity.HasIndex(e => e.RouteId, "route_id");

				entity.Property(e => e.TicketId)
					.HasColumnType("int(11)")
					.HasColumnName("ticket_id");

				entity.Property(e => e.BookingDate)
					.HasColumnType("datetime")
					.HasColumnName("booking_date")
					.HasDefaultValueSql("current_timestamp()");

				entity.Property(e => e.RouteId)
					.HasColumnType("int(11)")
					.HasColumnName("route_id");

				entity.HasOne(d => d.Route)
					.WithMany(p => p.Tickets)
					.HasForeignKey(d => d.RouteId)
					.HasConstraintName("tickets_ibfk_1");
			});
			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
