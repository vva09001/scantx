using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Spec_Project.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCustomer> TblCustomer { get; set; }
        public virtual DbSet<TblScanData> TblScanData { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=sql5045.site4now.net;Integrated Security=False;Database=DB_9A9CCA_scantx;User ID=DB_9A9CCA_scantx_admin;Password=Vbn*34295;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.HasKey(e => e.Cid);

                entity.ToTable("tblCustomer");

                entity.Property(e => e.Cid)
                    .HasColumnName("cid")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreateOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<TblScanData>(entity =>
            {
                entity.HasKey(e => e.ScanId);

                entity.ToTable("tblScanData");

                entity.Property(e => e.ScanId)
                    .HasColumnName("ScanID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DataType).HasColumnType("text");

                entity.Property(e => e.DeletedOn).HasColumnType("date");

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.Payload).HasColumnType("text");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.TblScanData)
                    .HasForeignKey(d => d.Uid)
                    .HasConstraintName("FK_tblScanData_tblUsers");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.ToTable("tblUsers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Authorization)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cid)
                    .HasColumnName("cid")
                    .HasMaxLength(50);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FamilyName).HasMaxLength(150);

                entity.Property(e => e.GivenName).HasMaxLength(150);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PasswordSalt).HasMaxLength(500);

                entity.Property(e => e.RoleID)
                    .HasColumnName("RoleID")
                    .HasMaxLength(50);

                entity.Property(e => e.TypeOfAccount)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.C)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.Cid)
                    .HasConstraintName("FK_tblUsers_tblCustomer");
            });
        }
    }
}
