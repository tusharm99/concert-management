using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConcertManagement.Core.Entities;

public partial class ConcertManagementContext : DbContext
{
    public ConcertManagementContext()
    {
    }

    public ConcertManagementContext(DbContextOptions<ConcertManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Initial Catalog=ConcertManagement;User Id=sa; Password=Tushunew1234;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC07FB70E813");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__VenueId__1209AD79");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC07A7397BE8");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Event).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__Event__1C873BEC");

            entity.HasOne(d => d.TicketType).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__Ticke__1D7B6025");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC0769220692");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PurchaseDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets__Reserva__22401542");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketTy__3214EC07F48F8B23");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketTyp__Event__16CE6296");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Venues__3214EC072CC827C9");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
