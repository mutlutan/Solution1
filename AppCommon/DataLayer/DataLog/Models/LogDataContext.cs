using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppCommon.DataLayer.DataLog.Models
{
    public partial class LogDataContext : DbContext
    {
        public LogDataContext()
        {
        }

        public LogDataContext(DbContextOptions<LogDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessLog> AccessLog { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLog { get; set; } = null!;
        public virtual DbSet<SystemLog> SystemLog { get; set; } = null!;
        public virtual DbSet<Version> Version { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLog>(entity =>
            {
                entity.HasIndex(e => e.AccountId, "IX_AccessLog_AccountId");

                entity.HasIndex(e => e.AccountName, "IX_AccessLog_AccountName");

                entity.HasIndex(e => e.AccountType, "IX_AccessLog_AccountType");

                entity.HasIndex(e => e.IpAddress, "IX_AccessLog_IpAddress");

                entity.HasIndex(e => e.SessionGuid, "IX_AccessLog_SessionGuid");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.AccountType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Browser)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.LogoutDate).HasColumnType("datetime");

                entity.Property(e => e.SessionGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(e => e.OperationDate, "IX_AuditLog_OperationDate");

                entity.HasIndex(e => e.OperationType, "IX_AuditLog_OperationType");

                entity.HasIndex(e => e.PrimaryKeyField, "IX_AuditLog_PrimaryKeyField");

                entity.HasIndex(e => e.PrimaryKeyValue, "IX_AuditLog_PrimaryKeyValue");

                entity.HasIndex(e => e.TableName, "IX_AuditLog_TableName");

                entity.HasIndex(e => e.UserBrowser, "IX_AuditLog_UserBrowser");

                entity.HasIndex(e => e.UserId, "IX_AuditLog_UserId");

                entity.HasIndex(e => e.UserIp, "IX_AuditLog_UserIp");

                entity.HasIndex(e => e.UserName, "IX_AuditLog_UserName");

                entity.HasIndex(e => e.UserSessionGuid, "IX_AuditLog_UserSessionGuid");

                entity.HasIndex(e => e.UserType, "IX_AuditLog_UserType");

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

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasIndex(e => e.ProcessDate, "IX_SystemLog_ProcessDate");

                entity.HasIndex(e => e.ProcessName, "IX_SystemLog_ProcessName");

                entity.HasIndex(e => e.ProcessTypeId, "IX_SystemLog_ProcessTypeId");

                entity.HasIndex(e => e.UserBrowser, "IX_SystemLog_UserBrowser");

                entity.HasIndex(e => e.UserId, "IX_SystemLog_UserId");

                entity.HasIndex(e => e.UserIp, "IX_SystemLog_UserIp");

                entity.HasIndex(e => e.UserName, "IX_SystemLog_UserName");

                entity.HasIndex(e => e.UserSessionGuid, "IX_SystemLog_UserSessionGuid");

                entity.HasIndex(e => e.UserType, "IX_SystemLog_UserType");

                entity.Property(e => e.Id).ValueGeneratedNever();

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

            modelBuilder.Entity<Version>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
