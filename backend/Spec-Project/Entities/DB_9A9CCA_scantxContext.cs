﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Spec_Project.Entities
{
    public partial class DB_9A9CCA_scantxContext : DbContext
    {
        public DB_9A9CCA_scantxContext()
        {
        }

        public DB_9A9CCA_scantxContext(DbContextOptions<DB_9A9CCA_scantxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCustomer> TblCustomer { get; set; }
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
                    .HasMaxLength(150)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.ToTable("tblUsers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Authorization)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FamilyName).HasMaxLength(150);

                entity.Property(e => e.GivenName).HasMaxLength(150);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TypeOfAccount)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.CompanyNavigation)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.Company)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblUsers_tblCustomer");
            });
        }
    }
}
