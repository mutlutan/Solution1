using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppData.Main.Models
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

        public virtual DbSet<Arac> Arac { get; set; } = null!;
        public virtual DbSet<AracHareket> AracHareket { get; set; } = null!;
        public virtual DbSet<AracHareketDetay> AracHareketDetay { get; set; } = null!;
        public virtual DbSet<AracHareketResim> AracHareketResim { get; set; } = null!;
        public virtual DbSet<AracOzellik> AracOzellik { get; set; } = null!;
        public virtual DbSet<AracOzellikDetay> AracOzellikDetay { get; set; } = null!;
        public virtual DbSet<AracRezervasyon> AracRezervasyon { get; set; } = null!;
        public virtual DbSet<AracRezervasyonDurum> AracRezervasyonDurum { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLog { get; set; } = null!;
        public virtual DbSet<Bolge> Bolge { get; set; } = null!;
        public virtual DbSet<BolgeDetay> BolgeDetay { get; set; } = null!;
        public virtual DbSet<BolgeDetayTur> BolgeDetayTur { get; set; } = null!;
        public virtual DbSet<CariHareketTur> CariHareketTur { get; set; } = null!;
        public virtual DbSet<Cinsiyet> Cinsiyet { get; set; } = null!;
        public virtual DbSet<CuzdanHareketTur> CuzdanHareketTur { get; set; } = null!;
        public virtual DbSet<EmailLetterhead> EmailLetterhead { get; set; } = null!;
        public virtual DbSet<EmailPool> EmailPool { get; set; } = null!;
        public virtual DbSet<EmailPoolStatus> EmailPoolStatus { get; set; } = null!;
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; } = null!;
        public virtual DbSet<Fiyat> Fiyat { get; set; } = null!;
        public virtual DbSet<Kampanya> Kampanya { get; set; } = null!;
        public virtual DbSet<KampanyaIndirimTipi> KampanyaIndirimTipi { get; set; } = null!;
        public virtual DbSet<KampanyaTur> KampanyaTur { get; set; } = null!;
        public virtual DbSet<KampanyaUye> KampanyaUye { get; set; } = null!;
        public virtual DbSet<MobilBildirim> MobilBildirim { get; set; } = null!;
        public virtual DbSet<MobilBildirimUye> MobilBildirimUye { get; set; } = null!;
        public virtual DbSet<Parameter> Parameter { get; set; } = null!;
        public virtual DbSet<Role> Role { get; set; } = null!;
        public virtual DbSet<SarjIstasyonu> SarjIstasyonu { get; set; } = null!;
        public virtual DbSet<SarjIstasyonuHareket> SarjIstasyonuHareket { get; set; } = null!;
        public virtual DbSet<SmsBildirim> SmsBildirim { get; set; } = null!;
        public virtual DbSet<SmsBildirimUye> SmsBildirimUye { get; set; } = null!;
        public virtual DbSet<Tarife> Tarife { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<Uye> Uye { get; set; } = null!;
        public virtual DbSet<UyeCariHareket> UyeCariHareket { get; set; } = null!;
        public virtual DbSet<UyeCuzdanHareket> UyeCuzdanHareket { get; set; } = null!;
        public virtual DbSet<UyeDurum> UyeDurum { get; set; } = null!;
        public virtual DbSet<UyeFaturaBilgisi> UyeFaturaBilgisi { get; set; } = null!;
        public virtual DbSet<UyeGrup> UyeGrup { get; set; } = null!;
        public virtual DbSet<UyeKaraListe> UyeKaraListe { get; set; } = null!;
        public virtual DbSet<Version> Version { get; set; } = null!;
        public virtual DbSet<VwAracStatuLog> VwAracStatuLog { get; set; } = null!;
        public virtual DbSet<VwAuditLog> VwAuditLog { get; set; } = null!;
        public virtual DbSet<VwMobilBildirimLog> VwMobilBildirimLog { get; set; } = null!;
        public virtual DbSet<VwSmsLog> VwSmsLog { get; set; } = null!;
        public virtual DbSet<VwSystemLog> VwSystemLog { get; set; } = null!;
        public virtual DbSet<VwUserLog> VwUserLog { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arac>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(300);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImeiNo).HasMaxLength(20);

                entity.Property(e => e.KilometreSayaci).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.QrKod).HasMaxLength(20);

                entity.Property(e => e.Resim).HasMaxLength(300);

                entity.Property(e => e.SarjOrani).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AracHareket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BaslangicTarih).HasColumnType("datetime");

                entity.Property(e => e.BirimFiyat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BitisTarih).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Mesafe).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Tutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Arac)
                    .WithMany(p => p.AracHareket)
                    .HasForeignKey(d => d.AracId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracHareket_AracId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.AracHareket)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracHareket_UyeId");
            });

            modelBuilder.Entity<AracHareketDetay>(entity =>
            {
                entity.HasIndex(e => e.AracHareketId, "IX_AracHareketDetay_AracHareketId");

                entity.HasIndex(e => e.Tarih, "IX_AracHareketDetay_Tarih");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Tarih).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.AracHareket)
                    .WithMany(p => p.AracHareketDetay)
                    .HasForeignKey(d => d.AracHareketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracHareketDetay_AracHareketId");
            });

            modelBuilder.Entity<AracHareketResim>(entity =>
            {
                entity.HasIndex(e => e.AracHareketId, "IX_AracHareketResim_AracHareketId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ResimUrl).HasMaxLength(250);

                entity.HasOne(d => d.AracHareket)
                    .WithMany(p => p.AracHareketResim)
                    .HasForeignKey(d => d.AracHareketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracHareketResim_AracHareketId");
            });

            modelBuilder.Entity<AracOzellik>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aciklama).HasMaxLength(200);

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<AracOzellikDetay>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Deger).HasMaxLength(150);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Arac)
                    .WithMany(p => p.AracOzellikDetay)
                    .HasForeignKey(d => d.AracId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracOzellikDetay_AracId");

                entity.HasOne(d => d.AracOzellik)
                    .WithMany(p => p.AracOzellikDetay)
                    .HasForeignKey(d => d.AracOzellikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracOzellikDetay_AracOzellikId");
            });

            modelBuilder.Entity<AracRezervasyon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BaslangicTarih).HasColumnType("datetime");

                entity.Property(e => e.BitisTarih).HasColumnType("datetime");

                entity.HasOne(d => d.Arac)
                    .WithMany(p => p.AracRezervasyon)
                    .HasForeignKey(d => d.AracId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracRezervasyon_AracId");

                entity.HasOne(d => d.AracRezervasyonDurum)
                    .WithMany(p => p.AracRezervasyon)
                    .HasForeignKey(d => d.AracRezervasyonDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracRezervasyon_AracRezervasyonDurumId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.AracRezervasyon)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AracRezervasyon_UyeId");
            });

            modelBuilder.Entity<AracRezervasyonDurum>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(20);
            });

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

            modelBuilder.Entity<Bolge>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<BolgeDetay>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(300);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.BolgeDetayTur)
                    .WithMany(p => p.BolgeDetay)
                    .HasForeignKey(d => d.BolgeDetayTurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BolgeDetay_BolgeDetayTurId");

                entity.HasOne(d => d.Bolge)
                    .WithMany(p => p.BolgeDetay)
                    .HasForeignKey(d => d.BolgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BolgeDetay_BolgeId");
            });

            modelBuilder.Entity<BolgeDetayTur>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<CariHareketTur>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<Cinsiyet>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_Cinsiyet_Durum");

                entity.HasIndex(e => e.Ad, "UX_Cinsiyet_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.AdEng).HasMaxLength(50);
            });

            modelBuilder.Entity<CuzdanHareketTur>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
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

            modelBuilder.Entity<Fiyat>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BayramFiyat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HaftaIciFiyat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HaftaSonuFiyat).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Tarife)
                    .WithMany(p => p.Fiyat)
                    .HasForeignKey(d => d.TarifeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fiyat_TarifeId");

                entity.HasOne(d => d.UyeGrup)
                    .WithMany(p => p.Fiyat)
                    .HasForeignKey(d => d.UyeGrupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fiyat_UyeGrupId");
            });

            modelBuilder.Entity<Kampanya>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(100);

                entity.Property(e => e.AdEng).HasMaxLength(100);

                entity.Property(e => e.BaslangicTarihi).HasColumnType("datetime");

                entity.Property(e => e.BitisTarihi).HasColumnType("datetime");

                entity.Property(e => e.GorselUrl).HasMaxLength(300);

                entity.Property(e => e.IndirimDegeri).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.KampanyaIndirimTipi)
                    .WithMany(p => p.Kampanya)
                    .HasForeignKey(d => d.KampanyaIndirimTipiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kampanya_KampanyaIndirimTipiId");

                entity.HasOne(d => d.KampanyaTur)
                    .WithMany(p => p.Kampanya)
                    .HasForeignKey(d => d.KampanyaTurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kampanya_KampanyaTurId");
            });

            modelBuilder.Entity<KampanyaIndirimTipi>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.AdEng).HasMaxLength(50);
            });

            modelBuilder.Entity<KampanyaTur>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.AdEng).HasMaxLength(50);
            });

            modelBuilder.Entity<KampanyaUye>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Kampanya)
                    .WithMany(p => p.KampanyaUye)
                    .HasForeignKey(d => d.KampanyaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KampanyaUye_KampanyaId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.KampanyaUye)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KampanyaUye_UyeId");
            });

            modelBuilder.Entity<MobilBildirim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Baslik).HasMaxLength(100);

                entity.Property(e => e.GonderimTarihi).HasColumnType("datetime");

                entity.Property(e => e.Link).HasMaxLength(200);

                entity.Property(e => e.Mesaj).HasMaxLength(100);

                entity.Property(e => e.ResimUrl).HasMaxLength(300);

                entity.Property(e => e.Tarih).HasColumnType("datetime");
            });

            modelBuilder.Entity<MobilBildirimUye>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.MobilBildirim)
                    .WithMany(p => p.MobilBildirimUye)
                    .HasForeignKey(d => d.MobilBildirimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobilBildirimUye_MobilBildirimId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.MobilBildirimUye)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobilBildirimUye_UyeId");
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AracSarjUyariLimit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EmailHost).HasMaxLength(100);

                entity.Property(e => e.EmailPassword).HasMaxLength(100);

                entity.Property(e => e.EmailUserName).HasMaxLength(100);

                entity.Property(e => e.GoogleMapApiKey).HasMaxLength(100);

                entity.Property(e => e.InstitutionEmail).HasMaxLength(100);

                entity.Property(e => e.MapTexBaseServiceUrl).HasMaxLength(300);

                entity.Property(e => e.MaptexApiKey).HasMaxLength(100);

                entity.Property(e => e.MasterpassServiceUrl).HasMaxLength(100);

                entity.Property(e => e.SiteAddress).HasMaxLength(100);

                entity.Property(e => e.SmsServiceBaseUrl).HasMaxLength(100);

                entity.Property(e => e.SmsServiceBaslik).HasMaxLength(30);

                entity.Property(e => e.SmsServicePassword).HasMaxLength(100);

                entity.Property(e => e.SmsServiceUrl).HasMaxLength(100);

                entity.Property(e => e.SmsServiceUserName).HasMaxLength(100);
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

            modelBuilder.Entity<SarjIstasyonu>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(300);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModelNo).HasMaxLength(50);

                entity.Property(e => e.SeriNo).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.YazilimVersiyon).HasMaxLength(100);

                entity.Property(e => e.YazilimVersiyonNo).HasMaxLength(50);
            });

            modelBuilder.Entity<SarjIstasyonuHareket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BaslangicTarih).HasColumnType("datetime");

                entity.Property(e => e.BitisTarih).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Arac)
                    .WithMany(p => p.SarjIstasyonuHareket)
                    .HasForeignKey(d => d.AracId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SarjIstasyonuHareket_AracId");

                entity.HasOne(d => d.SarjIstasyonu)
                    .WithMany(p => p.SarjIstasyonuHareket)
                    .HasForeignKey(d => d.SarjIstasyonuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SarjIstasyonuHareket_SarjIstasyonuId");
            });

            modelBuilder.Entity<SmsBildirim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Baslik).HasMaxLength(100);

                entity.Property(e => e.GonderimTarihi).HasColumnType("datetime");

                entity.Property(e => e.Mesaj).HasMaxLength(100);

                entity.Property(e => e.Tarih).HasColumnType("datetime");
            });

            modelBuilder.Entity<SmsBildirimUye>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.SmsBildirim)
                    .WithMany(p => p.SmsBildirimUye)
                    .HasForeignKey(d => d.SmsBildirimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SmsBildirimUye_SmsBildirimId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.SmsBildirimUye)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SmsBildirimUye_UyeId");
            });

            modelBuilder.Entity<Tarife>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.BaslangicTarihi).HasColumnType("date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.CreateDate, "IX_User_CreateDate");

                entity.HasIndex(e => e.IsActive, "IX_User_IsActive");

                entity.HasIndex(e => e.UserName, "UX_User_UserName")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.GaSecretKey).HasMaxLength(100);

                entity.Property(e => e.IdentificationNumber).HasMaxLength(11);

                entity.Property(e => e.NameSurname).HasMaxLength(100);

                entity.Property(e => e.ResidenceAddress).HasMaxLength(50);

                entity.Property(e => e.SessionGuid).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.UserPassword).HasMaxLength(100);

                entity.Property(e => e.ValidityDate).HasColumnType("date");
            });

            modelBuilder.Entity<Uye>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.CuzdanBakiye).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DogumTarihi).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FcmRegistrationToken).HasMaxLength(250);

                entity.Property(e => e.Gsm).HasMaxLength(15);

                entity.Property(e => e.KimlikNumarasi).HasMaxLength(50);

                entity.Property(e => e.Sifre).HasMaxLength(100);

                entity.Property(e => e.SifreSifirlamaKod).HasMaxLength(200);

                entity.Property(e => e.Soyad).HasMaxLength(50);

                entity.Property(e => e.UyelikTarihi).HasColumnType("datetime");

                entity.HasOne(d => d.Cinsiyet)
                    .WithMany(p => p.Uye)
                    .HasForeignKey(d => d.CinsiyetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Uye_CinsiyetId");

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

            modelBuilder.Entity<UyeCariHareket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Alacak).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Borc).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Tarih).HasColumnType("datetime");

                entity.HasOne(d => d.CariHareketTur)
                    .WithMany(p => p.UyeCariHareket)
                    .HasForeignKey(d => d.CariHareketTurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UyeCariHareket_CariHareketTurId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.UyeCariHareket)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UyeCariHareket_UyeId");
            });

            modelBuilder.Entity<UyeCuzdanHareket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Alacak).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Borc).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Tarih).HasColumnType("datetime");

                entity.HasOne(d => d.CuzdanHareketTur)
                    .WithMany(p => p.UyeCuzdanHareket)
                    .HasForeignKey(d => d.CuzdanHareketTurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UyeCuzdanHareket_CuzdanHareketTurId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.UyeCuzdanHareket)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UyeCuzdanHareket_UyeId");
            });

            modelBuilder.Entity<UyeDurum>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<UyeFaturaBilgisi>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aciklama).HasMaxLength(150);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DokumanUrl).HasMaxLength(200);

                entity.Property(e => e.Tarih).HasColumnType("datetime");

                entity.Property(e => e.Tutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.UyeFaturaBilgisi)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UyeFaturaBilgisi_UyeId");
            });

            modelBuilder.Entity<UyeGrup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<UyeKaraListe>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aciklama).HasMaxLength(300);

                entity.Property(e => e.BaslangicTarih).HasColumnType("datetime");

                entity.Property(e => e.BitisTarih).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.UyeKaraListe)
                    .HasForeignKey(d => d.UyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UyeKaraListe_UyeId");
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwAracStatuLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwAracStatuLog");

                entity.Property(e => e.ImeiNo).HasMaxLength(50);

                entity.Property(e => e.Tarih).HasColumnType("datetime");
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

            modelBuilder.Entity<VwMobilBildirimLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwMobilBildirimLog");

                entity.Property(e => e.Tarih).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwSmsLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwSmsLog");

                entity.Property(e => e.Tarih).HasColumnType("datetime");
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

                entity.Property(e => e.EkAlan1).HasMaxLength(100);

                entity.Property(e => e.EkAlan2).HasMaxLength(100);

                entity.Property(e => e.EkAlan3).HasMaxLength(100);

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.LogoutDate).HasColumnType("datetime");

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

            modelBuilder.HasSequence<int>("sqArac").StartsAt(10001);

            modelBuilder.HasSequence<int>("sqAracHareket");

            modelBuilder.HasSequence<int>("sqAracHareketDetay");

            modelBuilder.HasSequence<int>("sqAracHareketResim");

            modelBuilder.HasSequence<int>("sqAracHareketTur").StartsAt(11);

            modelBuilder.HasSequence<int>("sqAracOzellik");

            modelBuilder.HasSequence<int>("sqAracOzellikDetay");

            modelBuilder.HasSequence<int>("sqAracRezervasyon");

            modelBuilder.HasSequence<int>("sqAracRezervasyonDurum").StartsAt(11);

            modelBuilder.HasSequence<int>("sqBolge");

            modelBuilder.HasSequence<int>("sqBolgeDetay");

            modelBuilder.HasSequence<int>("sqEmailLetterhead");

            modelBuilder.HasSequence<int>("sqEmailPool");

            modelBuilder.HasSequence<int>("sqFiyat");

            modelBuilder.HasSequence<int>("sqKampanya");

            modelBuilder.HasSequence<int>("sqKampanyaIndirimTipi")
                .StartsAt(100)
                .IncrementsBy(10);

            modelBuilder.HasSequence<int>("sqKampanyaTur")
                .StartsAt(100)
                .IncrementsBy(10);

            modelBuilder.HasSequence<int>("sqKampanyaUye");

            modelBuilder.HasSequence<int>("sqMobilBildirim");

            modelBuilder.HasSequence<int>("sqMobilBildirimUye");

            modelBuilder.HasSequence<int>("sqRole")
                .StartsAt(2001)
                .IncrementsBy(13);

            modelBuilder.HasSequence<int>("sqSarjIstasyonu");

            modelBuilder.HasSequence<int>("sqSarjIstasyonuHareket");

            modelBuilder.HasSequence<int>("sqSmsBildirim");

            modelBuilder.HasSequence<int>("sqSmsBildirimUye");

            modelBuilder.HasSequence<int>("sqTarife");

            modelBuilder.HasSequence<int>("sqUser");

            modelBuilder.HasSequence<int>("sqUye");

            modelBuilder.HasSequence<int>("sqUyeCariHareket");

            modelBuilder.HasSequence<int>("sqUyeCuzdanHareket");

            modelBuilder.HasSequence<int>("sqUyeFaturaBilgisi");

            modelBuilder.HasSequence<int>("sqUyeGrup");

            modelBuilder.HasSequence<int>("sqUyeKaraListe");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
