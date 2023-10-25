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
        public virtual DbSet<Dashboard> Dashboard { get; set; } = null!;
        public virtual DbSet<EmailLetterhead> EmailLetterhead { get; set; } = null!;
        public virtual DbSet<EmailPool> EmailPool { get; set; } = null!;
        public virtual DbSet<EmailPoolStatus> EmailPoolStatus { get; set; } = null!;
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; } = null!;
        public virtual DbSet<Gender> Gender { get; set; } = null!;
        public virtual DbSet<Job> Job { get; set; } = null!;
        public virtual DbSet<Member> Member { get; set; } = null!;
        public virtual DbSet<MemberType> MemberType { get; set; } = null!;
        public virtual DbSet<Parameter> Parameter { get; set; } = null!;
        public virtual DbSet<Role> Role { get; set; } = null!;
        public virtual DbSet<StartType> StartType { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserStatus> UserStatus { get; set; } = null!;
        public virtual DbSet<UserType> UserType { get; set; } = null!;
        public virtual DbSet<Uye> Uye { get; set; } = null!;
        public virtual DbSet<UyeDurum> UyeDurum { get; set; } = null!;
        public virtual DbSet<UyeGrup> UyeGrup { get; set; } = null!;
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

            modelBuilder.Entity<Dashboard>(entity =>
            {
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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmailPool>(entity =>
            {
                entity.HasIndex(e => e.CreateDate, "IX_EmailPool_CreateDate");

                entity.HasIndex(e => e.EmailTemplateId, "IX_EmailPool_EmailTemplateId");

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

                entity.Property(e => e.MethodName).HasMaxLength(100);

                entity.Property(e => e.MethodParams).HasMaxLength(500);

                entity.Property(e => e.StartDayNames)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StartType)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.StartTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_StartTypeId");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasIndex(e => e.UserStatusId, "IX_Member_UserStatusId");

                entity.HasIndex(e => e.Email, "UX_Member_Email")
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

                entity.HasOne(d => d.MemberType)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.MemberTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_MemberTypeId");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_UserStatusId");
            });

            modelBuilder.Entity<MemberType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UX_Role_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<StartType>(entity =>
            {
                entity.HasIndex(e => e.IsActive, "IX_StartType_IsActive");

                entity.HasIndex(e => e.Name, "UX_StartType_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.CreateDate, "IX_User_CreateDate");

                entity.HasIndex(e => e.UserStatusId, "IX_User_UserStatusId");

                entity.HasIndex(e => e.UserTypeId, "IX_User_UserTypeId");

                entity.HasIndex(e => e.Email, "UX_User_Email")
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

            modelBuilder.Entity<Uye>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.CountryCode).HasMaxLength(2);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.GaSecretKey).HasMaxLength(100);

                entity.Property(e => e.NameSurname).HasMaxLength(100);

                entity.Property(e => e.SessionGuid).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.UserPassword).HasMaxLength(100);

                entity.Property(e => e.ValidityDate).HasColumnType("date");

                entity.HasOne(d => d.UyeDurum)
                    .WithMany(p => p.Uye)
                    .HasForeignKey(d => d.UyeDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Uye_UyeDurumId");

                entity.HasOne(d => d.UyeGrup)
                    .WithMany(p => p.Uye)
                    .HasForeignKey(d => d.UyeGrupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Uye_UyeGrupId");
            });

            modelBuilder.Entity<UyeDurum>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<UyeGrup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
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

            modelBuilder.HasSequence<int>("sqDashboard").StartsAt(101);

            modelBuilder.HasSequence<int>("sqEmailLetterhead");

            modelBuilder.HasSequence<int>("sqEmailPool");

            modelBuilder.HasSequence<int>("sqRole").StartsAt(2001);

            modelBuilder.HasSequence<int>("sqUser");

            modelBuilder.HasSequence<int>("sqUye");

            modelBuilder.HasSequence<int>("sqUyeGrup");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
