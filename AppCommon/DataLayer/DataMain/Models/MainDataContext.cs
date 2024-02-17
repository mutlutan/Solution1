using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppCommon.DataLayer.DataMain.Models
{
    public partial class MainDataContext : DbContext
    {
        public MainDataContext()
        {
        }

        public MainDataContext(DbContextOptions<MainDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLog> AuditLog { get; set; } = null!;
        public virtual DbSet<Country> Country { get; set; } = null!;
        public virtual DbSet<Currency> Currency { get; set; } = null!;
        public virtual DbSet<Customer> Customer { get; set; } = null!;
        public virtual DbSet<CustomerTransaction> CustomerTransaction { get; set; } = null!;
        public virtual DbSet<CustomerType> CustomerType { get; set; } = null!;
        public virtual DbSet<CustomerWallet> CustomerWallet { get; set; } = null!;
        public virtual DbSet<Dashboard> Dashboard { get; set; } = null!;
        public virtual DbSet<EmailLetterhead> EmailLetterhead { get; set; } = null!;
        public virtual DbSet<EmailPool> EmailPool { get; set; } = null!;
        public virtual DbSet<EmailPoolStatus> EmailPoolStatus { get; set; } = null!;
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; } = null!;
        public virtual DbSet<Gender> Gender { get; set; } = null!;
        public virtual DbSet<Job> Job { get; set; } = null!;
        public virtual DbSet<Parameter> Parameter { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategory { get; set; } = null!;
        public virtual DbSet<Role> Role { get; set; } = null!;
        public virtual DbSet<TransactionType> TransactionType { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserStatus> UserStatus { get; set; } = null!;
        public virtual DbSet<UserType> UserType { get; set; } = null!;
        public virtual DbSet<Version> Version { get; set; } = null!;
        public virtual DbSet<VwAuditLog> VwAuditLog { get; set; } = null!;
        public virtual DbSet<VwSystemLog> VwSystemLog { get; set; } = null!;
        public virtual DbSet<VwUserLog> VwUserLog { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.OperationType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrimaryKeyField)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryKeyValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserBrowser)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserIp)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserSessionGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => e.IsActive, "IX_Country_IsActive");

                entity.HasIndex(e => e.Name, "UX_Country_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasIndex(e => e.Code, "UX_Currency_Code")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "UX_Currency_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.SubName).HasMaxLength(20);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.NameSurname, "IX_Customer_NameSurname");

                entity.HasIndex(e => e.UserStatusId, "IX_Customer_UserStatusId");

                entity.HasIndex(e => e.Email, "UX_Customer_Email")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_Customer_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GaSecretKey).HasMaxLength(100);

                entity.Property(e => e.NameSurname).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.ResidenceAddress).HasMaxLength(50);

                entity.Property(e => e.SessionGuid).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.ValidityDate).HasColumnType("date");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_CustomerTypeId");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_UserStatusId");
            });

            modelBuilder.Entity<CustomerTransaction>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "IX_CustomerTransaction_CustomerId");

                entity.HasIndex(e => e.UniqueId, "UX_CustomerTransaction_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Credit).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Debit).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerTransaction)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransaction_CustomerId");

                entity.HasOne(d => d.CustomerWallet)
                    .WithMany(p => p.CustomerTransaction)
                    .HasForeignKey(d => d.CustomerWalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransaction_CustomerWalletId");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerWallet>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "IX_CustomerWallet_CustomerId");

                entity.HasIndex(e => new { e.CurrencyId, e.WalletNumber }, "UX_CustomerWallet_CurrencyId_WalletNumber")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_CustomerWallet_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.WalletNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerWallet)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerWallet_CustomerId");
            });

            modelBuilder.Entity<Dashboard>(entity =>
            {
                entity.HasIndex(e => e.UniqueId, "UX_Dashboard_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DetailUrl).HasMaxLength(250);

                entity.Property(e => e.IconClass).HasMaxLength(50);

                entity.Property(e => e.IconStyle).HasMaxLength(50);

                entity.Property(e => e.TemplateName).HasMaxLength(20);

                entity.Property(e => e.Title).HasMaxLength(20);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmailLetterhead>(entity =>
            {
                entity.HasIndex(e => e.Description, "UX_EmailLetterhead_Description")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_EmailLetterhead_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmailPool>(entity =>
            {
                entity.HasIndex(e => e.CreateDate, "IX_EmailPool_CreateDate");

                entity.HasIndex(e => e.EmailTemplateId, "IX_EmailPool_EmailTemplateId");

                entity.HasIndex(e => e.UniqueId, "IX_EmailPool_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EmailTo).HasMaxLength(500);

                entity.Property(e => e.LastTryDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.EmailPoolStatus)
                    .WithMany(p => p.EmailPool)
                    .HasForeignKey(d => d.EmailPoolStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailPool_EmailPoolStatusId");

                entity.HasOne(d => d.EmailTemplate)
                    .WithMany(p => p.EmailPool)
                    .HasForeignKey(d => d.EmailTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailPool_EmailTemplateId");
            });

            modelBuilder.Entity<EmailPoolStatus>(entity =>
            {
                entity.HasIndex(e => e.Name, "UX_EmailPoolStatus_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<EmailTemplate>(entity =>
            {
                entity.HasIndex(e => e.Name, "UX_EmailTemplate_Name")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_EmailTemplate_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.EmailLetterhead)
                    .WithMany(p => p.EmailTemplate)
                    .HasForeignKey(d => d.EmailLetterheadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailTemplate_EmailLetterheadId");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasIndex(e => e.IsActive, "IX_Gender_IsActive");

                entity.HasIndex(e => e.Name, "UX_Gender_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasIndex(e => e.IsActive, "IX_Job_IsActive");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CronExpression)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MethodComment).HasMaxLength(500);

                entity.Property(e => e.MethodName).HasMaxLength(100);

                entity.Property(e => e.MethodParams).HasMaxLength(500);
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EmailHost).HasMaxLength(100);

                entity.Property(e => e.EmailPassword).HasMaxLength(100);

                entity.Property(e => e.EmailUserName).HasMaxLength(100);

                entity.Property(e => e.GoogleMapApiKey).HasMaxLength(100);

                entity.Property(e => e.InstitutionEmail).HasMaxLength(100);

                entity.Property(e => e.SiteAddress).HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.ProductCategoryId, "IX_Product_ProductCategoryId");

                entity.HasIndex(e => e.Name, "UX_Product_Name")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_Product_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductCategoryId");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasIndex(e => e.Name, "UX_ProductCategory_Name")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_ProductCategory_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UX_Role_Name")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_Role_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.CreateDate, "IX_User_CreateDate");

                entity.HasIndex(e => e.UserStatusId, "IX_User_UserStatusId");

                entity.HasIndex(e => e.UserTypeId, "IX_User_UserTypeId");

                entity.HasIndex(e => e.Email, "UX_User_Email")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "UX_User_UniqueId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GaSecretKey).HasMaxLength(100);

                entity.Property(e => e.NameSurname).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.ResidenceAddress).HasMaxLength(50);

                entity.Property(e => e.SessionGuid).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.ValidityDate).HasColumnType("date");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserStatusId");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserTypeId");
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwAuditLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwAuditLog");

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.OperationType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrimaryKeyField)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryKeyValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserBrowser)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserIp)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserSessionGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwSystemLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwSystemLog");

                entity.Property(e => e.ProcessDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessName).HasMaxLength(100);

                entity.Property(e => e.UserBrowser)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserIp)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserSessionGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwUserLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwUserLog");

                entity.Property(e => e.ExtraSpace1).HasMaxLength(100);

                entity.Property(e => e.ExtraSpace2).HasMaxLength(100);

                entity.Property(e => e.ExtraSpace3).HasMaxLength(100);

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.LogoutDate).HasColumnType("datetime");

                entity.Property(e => e.SessionGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserBrowser)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserIp)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.HasSequence<int>("sqCountry");

            modelBuilder.HasSequence<int>("sqCurrency").StartsAt(101);

            modelBuilder.HasSequence<int>("sqCustomerTransaction");

            modelBuilder.HasSequence<int>("sqCustomerWallet");

            modelBuilder.HasSequence<int>("sqCutomer");

            modelBuilder.HasSequence<int>("sqDashboard").StartsAt(101);

            modelBuilder.HasSequence<int>("sqEmailLetterhead");

            modelBuilder.HasSequence<int>("sqEmailPool");

            modelBuilder.HasSequence<int>("sqProduct").StartsAt(10001);

            modelBuilder.HasSequence<int>("sqProductCategory").StartsAt(1001);

            modelBuilder.HasSequence<int>("sqRole").StartsAt(2001);

            modelBuilder.HasSequence<int>("sqUser");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
