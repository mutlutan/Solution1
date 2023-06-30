using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Main.Migration
{
    public class MoUpdate
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public string CommandText { get; set; } = "";
    }

    public class MyMigration
    {
        public static List<MoUpdate> GetMigrationList(decimal updateId)
        {
            var sqlUpdateList = new List<MoUpdate>() { };

            #region örnekler

            // table not exists
            //IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemXX')
            //BEGIN
            //  Create Table....
            //END

            //column not exists
            //IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemXX' AND COLUMN_NAME = 'Ad')
            //BEGIN
            //  Alter Table TemXX Add Ad	NVARCHAR(100);
            //END

            //SEQUENCE not exists
            //IF OBJECT_ID('dbo.sqTemXX', 'SO') IS NULL
            //BEGIN
            //  CREATE SEQUENCE dbo.sqTemXX AS INT START WITH 1 INCREMENT BY 1;
            //END

            //drop squence
            //IF OBJECT_ID('dbo.sqDepartmanGorev', 'SO') IS NOT NULL
            //BEGIN
            //  DROP SEQUENCE dbo.sqDepartmanGorev;
            //END

            //index exists
            //IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('TemKisi') AND name = 'IX_TemKisi_Kod')
            //BEGIN
            //  DROP INDEX IX_TemKisi_Kod ON TemKisi;
            //END

            //FOREIGN key not exists
            //IF NOT EXISTS(SELECT name FROM sys.foreign_keys WHERE name = 'FK_LisIsletme_SaticiId')
            //BEGIN
            //  ALTER TABLE LisIsletme ADD CONSTRAINT FK_LisIsletme_SaticiId FOREIGN KEY(SaticiId) REFERENCES TemKisi(Id);
            //END

            //FOREIGN key exists
            //IF EXISTS(SELECT name FROM sys.foreign_keys WHERE name = 'FK_RobOgrenci_OkulId')
            //BEGIN
            //    ALTER TABLE RobOgrenci DROP CONSTRAINT FK_RobOgrenci_OkulId;
            //END

            // alter column
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RobProjeSayfa' AND COLUMN_NAME = 'Sira' AND DATA_TYPE = 'int')
            //BEGIN
            //    ALTER TABLE RobProjeSayfa ALTER COLUMN Sira DECIMAL(18,4);
            //END

            //DROP COLUMN
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemKullaniciLisans' AND COLUMN_NAME = 'LisansTurId')
            //BEGIN
            //    ALTER TABLE TemKullaniciLisans DROP COLUMN LisansTurId;
            //END

            // DROP TABLE
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemKisiTur')
            //BEGIN
            //    DROP TABLE dbo.TemKisiTur;
            //END

            // Rename COLUMN
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OturumLog' AND COLUMN_NAME = 'InternetProtokolAdres')
            //BEGIN
            //    EXEC sp_rename 'OturumLog.InternetProtokolAdres', 'ProtokolAdres' , 'COLUMN';
            //END
            #endregion


            // 26.03.2023 15:00 Avni
            sqlUpdateList.Add(
                new MoUpdate
                {
                    Id = 1,
                    Description = "AracHareket tablosunda BitisTarih alanı NULLable yapıldı",
                    CommandText = $@"
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareket' AND COLUMN_NAME = 'BitisTarih')
                       BEGIN
                           ALTER TABLE AracHareket ALTER COLUMN BitisTarih DATETIME;
                       END
                    "
                }
            );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 2,
                   Description = "AracOzellik tablosu ve verileri eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracOzellik')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqAracOzellik AS INT START WITH 1 INCREMENT BY 1;
                        	CREATE TABLE dbo.AracOzellik(
                        		Id					INT NOT NULL,
                        
                        		UniqueId			UNIQUEIDENTIFIER NOT NULL,
                        		Durum				BIT NOT NULL,
                        		Ad					NVARCHAR(50),
                        		Aciklama			NVARCHAR(200),
                        
                        		CONSTRAINT PK_AracOzellik PRIMARY KEY  (Id)
                            );    

                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'iot_sw', 'IOT Software Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'iot_hw', 'IOT Hardware Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'ecu_fw', 'ECU Firmware Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'ecu_hw', 'ECU Hardware Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'ble_fw', 'Bluetooth Firmware Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'blk_fw', 'Battery Lock Firmware Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'hlk_fw', 'Hub Lock Firmware Version');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'iccid', 'ICCID');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'vehicle_serial', 'Serial Number');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'iot_serial', 'IoT Serial Number');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'ecu_serial', 'ECU Serial Number');
                            INSERT INTO AracOzellik (Id, UniqueId, Durum, Ad, Aciklama) VALUES(Next Value For dbo.sqAracOzellik, newid(), 1, 'ble_mac', 'Bluetooth MAC Address');

                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 3,
                   Description = "AracOzellikDetay tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracOzellikDetay')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqAracOzellikDetay AS INT START WITH 1 INCREMENT BY 1;
                        	CREATE TABLE dbo.AracOzellikDetay(
                        		Id					INT NOT NULL,
                        
                        		AracOzellikId		INT NOT NULL,
                        		AracId			INT NOT NULL, 
                        		Deger				NVARCHAR(150),
                        
                        		CreateDate		DATETIME,
                        		CreatedUserId	INT,
                        		UpdateDate		DATETIME,
                        		UpdatedUserId	INT,
                        
                        		CONSTRAINT PK_AracOzellikDetay PRIMARY KEY  (Id),
                        		CONSTRAINT FK_AracOzellikDetay_AracOzellikId FOREIGN KEY (AracOzellikId) REFERENCES AracOzellik(Id),
                        		CONSTRAINT FK_AracOzellikDetay_AracId FOREIGN KEY (AracId) REFERENCES Arac(Id)
                            ); 

                        END
                   "
               }
           );

            sqlUpdateList.Add(
                new MoUpdate
                {
                    Id = 4,
                    Description = "Arac tablosuna KilometreSayaci ve SarjOluyorMu alanları eklendi",
                    CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Arac' AND COLUMN_NAME = 'KilometreSayaci')
                        BEGIN
                           ALTER TABLE Arac ADD KilometreSayaci	DECIMAL(18,2);
                        END      

                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Arac' AND COLUMN_NAME = 'SarjOluyorMu')
                        BEGIN
                           ALTER TABLE Arac ADD SarjOluyorMu BIT;
                        END  
 
                    "
                }
             );
           
            sqlUpdateList.Add(
                new MoUpdate
                {
                    Id = 5,
                    Description = "Arac tablosu KilometreSayaci ve SarjOluyorMu alanları NOT NULL yapıldı .",
                    CommandText = $@"
                        UPDATE Arac SET KilometreSayaci = 0 WHERE KilometreSayaci is null;
                        IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Arac' AND COLUMN_NAME = 'KilometreSayaci')
                        BEGIN
                           ALTER TABLE Arac ALTER COLUMN KilometreSayaci DECIMAL(18,2) NOT NULL;
                        END  

                        UPDATE Arac SET SarjOluyorMu = 0 WHERE SarjOluyorMu is null;
                        IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Arac' AND COLUMN_NAME = 'SarjOluyorMu')
                        BEGIN
                           ALTER TABLE Arac ALTER COLUMN SarjOluyorMu	BIT NOT NULL;
                        END 
                    "
                }
            );

            sqlUpdateList.Add(
                new MoUpdate
                {
                    Id = 6,
                    Description = "UyeDurum tablosuna Deleted datası eklendi ",
                    CommandText = $@"
                        INSERT INTO UyeDurum (Id, Ad) VALUES (3, N'Deleted');
                    "
                }
            );

            sqlUpdateList.Add(
                new MoUpdate
                {
                    Id = 7,
                    Description = "AracHareketDetay tablosundan UniqueId çıkarıldı ",
                    CommandText = $@"
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareketDetay' AND COLUMN_NAME = 'UniqueId')
                       BEGIN
                           ALTER TABLE AracHareketDetay DROP COLUMN UniqueId;
                       END
                    "
                }
            );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 8,
                   Description = "UyeFaturaBilgisi tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UyeFaturaBilgisi')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqUyeFaturaBilgisi AS INT START WITH 1 INCREMENT BY 1;
                        	CREATE TABLE dbo.UyeFaturaBilgisi(
                        		Id					INT NOT NULL,
                        
                        		UyeId				INT NOT NULL,
                        		Tutar				DECIMAL(18,2) NOT NULL, 
                        		Tarih				DATETIME NOT NULL,
                        		Aciklama			NVARCHAR(150),
                        		DokumanUrl			NVARCHAR(200),
                        
                        		CreateDate		DATETIME,
                        		CreatedUserId	INT,
                        		UpdateDate		DATETIME,
                        		UpdatedUserId	INT,
                        
                        		CONSTRAINT PK_UyeFaturaBilgisi PRIMARY KEY  (Id),
                        		CONSTRAINT FK_UyeFaturaBilgisi_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id)
                            );

                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 9,
                   Description = "AracRezervasyonDurum tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracRezervasyonDurum')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqAracRezervasyonDurum AS INT START WITH 11 INCREMENT BY 1;
                        	CREATE TABLE dbo.AracRezervasyonDurum(
                        		Id						INT NOT NULL,

                        		Durum					BIT NOT NULL,
                        		Ad						NVARCHAR(20),
                        
                        		CONSTRAINT PK_AracRezervasyonDurum PRIMARY KEY  (Id)
                        	);
                        	INSERT INTO AracRezervasyonDurum (Id, Durum, Ad) VALUES (Next Value For dbo.sqAracRezervasyonDurum, 1, N'Rezerve');
                        	INSERT INTO AracRezervasyonDurum (Id, Durum, Ad) VALUES (Next Value For dbo.sqAracRezervasyonDurum, 1, N'Kullanıldı');
                            INSERT INTO AracRezervasyonDurum (Id, Durum, Ad) VALUES (Next Value For dbo.sqAracRezervasyonDurum, 1, N'Kullanılmadı');
                        	INSERT INTO AracRezervasyonDurum (Id, Durum, Ad) VALUES (Next Value For dbo.sqAracRezervasyonDurum, 1, N'İptal Edildi');
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 10,
                   Description = "AracRezervasyon tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracRezervasyon')
                        BEGIN
                            CREATE SEQUENCE dbo.sqAracRezervasyon AS INT START WITH 1 INCREMENT BY 1;
                            CREATE TABLE dbo.AracRezervasyon(
                            	Id						INT NOT NULL,
                            
                            	UniqueId				UNIQUEIDENTIFIER NOT NULL,
                            	AracId				INT NOT NULL,
                            	UyeId					INT NOT NULL,
                            	AracRezervasyonDurumId	INT NOT NULL,
                            	
                            	BaslangicTarih			DATETIME NOT NULL,
                            	BitisTarih				DATETIME NOT NULL,	
                            	Sure					INT NOT NULL, 
                            
                            	Konum					GEOGRAPHY, 
                            
                            	CONSTRAINT PK_AracRezervasyon PRIMARY KEY  (Id),
                            	CONSTRAINT FK_AracRezervasyon_AracId FOREIGN KEY (AracId) REFERENCES Arac(Id),
                            	CONSTRAINT FK_AracRezervasyon_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id),
                            	CONSTRAINT FK_AracRezervasyon_AracRezervasyonDurumId FOREIGN KEY (AracRezervasyonDurumId) REFERENCES AracRezervasyonDurum(Id),
                            );
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 11,
                   Description = "AracHareket tablosuna AracRezervasyonId eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareket' AND COLUMN_NAME = 'AracRezervasyonId')
                        BEGIN
                           ALTER TABLE AracHareket ADD AracRezervasyonId INT;
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 12,
                   Description = "AracHareket tablosunda AracRezervasyonId alanı NOT NULL yapmak. ",
                   CommandText = $@"
                       UPDATE AracHareket SET AracRezervasyonId = 0 WHERE AracRezervasyonId is null;
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareket' AND COLUMN_NAME = 'AracRezervasyonId')
                        BEGIN
                           ALTER TABLE AracHareket ALTER COLUMN AracRezervasyonId INT NOT NULL;
                        END  
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 13,
                   Description = "Parameter tablosuna AracRezervasyonSure alanı eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'AracRezervasyonSure')
                        BEGIN
                           ALTER TABLE Parameter ADD AracRezervasyonSure INT;
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 14,
                   Description = "Parameter tablosunda AracRezervasyonSure alanı NOT NULL yapmak. ",
                   CommandText = $@"
                       UPDATE Parameter SET AracRezervasyonSure = 60 WHERE AracRezervasyonSure is null;
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'AracRezervasyonSure')
                        BEGIN
                           ALTER TABLE Parameter ALTER COLUMN AracRezervasyonSure INT NOT NULL;
                        END  
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 15,
                   Description = "Parameter tablosuna MasterpassMerchantId ve MasterpassServiceUrl alanları eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'MasterpassMerchantId')
                        BEGIN
                           ALTER TABLE Parameter ADD MasterpassMerchantId BIGINT;                           
                        END

                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'MasterpassServiceUrl')
                        BEGIN
                           ALTER TABLE Parameter ADD MasterpassServiceUrl NVARCHAR(100);
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 16,
                   Description = "Uye tablosuna MsisdnDogrulama alanı eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Uye' AND COLUMN_NAME = 'MsisdnDogrulama')
                        BEGIN
                           ALTER TABLE Uye ADD MsisdnDogrulama BIT;
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 17,
                   Description = "Uye tablosuna MsisdnDogrulama alanı NOT NULL yapmak. ",
                   CommandText = $@"
                       UPDATE Uye SET MsisdnDogrulama = 0 WHERE MsisdnDogrulama is null;
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Uye' AND COLUMN_NAME = 'MsisdnDogrulama')
                        BEGIN
                           ALTER TABLE Uye ALTER COLUMN MsisdnDogrulama BIT NOT NULL;
                        END  
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 18,
                   Description = "Parameter tablosuna MasterpassMerchantId alanı NOT NULL yapmak. ",
                   CommandText = $@"
                       UPDATE Parameter SET MasterpassMerchantId = 0 WHERE MasterpassMerchantId is null;
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'MasterpassMerchantId')
                        BEGIN
                           ALTER TABLE Parameter ALTER COLUMN MasterpassMerchantId BIGINT NOT NULL;
                        END  
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 19,
                   Description = "Uye tablosuna FcmRegistrationToken alanı eklendi. ",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Uye' AND COLUMN_NAME = 'FcmRegistrationToken')
                        BEGIN
                           ALTER TABLE Uye ADD FcmRegistrationToken NVARCHAR(250);
                        END
                   "
               }
           );


            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 20,
                   Description = "Parameter tablosuna AracSarjUyariLimit alanı eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'AracSarjUyariLimit')
                        BEGIN
                           ALTER TABLE Parameter ADD AracSarjUyariLimit DECIMAL(18,2);
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 21,
                   Description = "Parameter tablosunda AracSarjUyariLimit alanı NOT NULL yapmak. ",
                   CommandText = $@"
                       UPDATE Parameter SET AracSarjUyariLimit = 0 WHERE AracSarjUyariLimit is null;
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Parameter' AND COLUMN_NAME = 'AracSarjUyariLimit')
                        BEGIN
                           ALTER TABLE Parameter ALTER COLUMN AracSarjUyariLimit DECIMAL(18,2) NOT NULL;
                        END  
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 22,
                   Description = "AracHareketDetay tablosuna ReportId alanı eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareketDetay' AND COLUMN_NAME = 'ReportId')
                        BEGIN
                           ALTER TABLE AracHareketDetay ADD ReportId INT;
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 23,
                   Description = "AracHareketDetay tablosunda ReportId alanı NOT NULL yapmak. ",
                   CommandText = $@"
                       UPDATE AracHareketDetay SET ReportId = 0 WHERE ReportId is null;
                       IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareketDetay' AND COLUMN_NAME = 'ReportId')
                        BEGIN
                           ALTER TABLE AracHareketDetay ALTER COLUMN ReportId INT NOT NULL;
                        END  
                   "
               }
           );

			sqlUpdateList.Add(
			   new MoUpdate
			   {
				   Id = 24,
				   Description = "Arac tablosuna QrKod alanı eklendi",
				   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Arac' AND COLUMN_NAME = 'QrKod')
                        BEGIN
                           ALTER TABLE Arac ADD QrKod NVARCHAR(20);
                        END
                   "
			   }
		   );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 25,
                   Description = "AracHareketResim tablosu oluşturuldu",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareketResim')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqAracHareketResim AS INT START WITH 1 INCREMENT BY 1;
                        	CREATE TABLE dbo.AracHareketResim(
                        		Id						INT NOT NULL,
                        
                        		AracHareketId		INT NOT NULL,
                        		ResimUrl			NVARCHAR(250),
                        
                        		CONSTRAINT PK_AracHareketResim PRIMARY KEY  (Id),
                        		CONSTRAINT FK_AracHareketResim_AracHareketId FOREIGN KEY (AracHareketId) REFERENCES AracHareket(Id)
                        	);
                        	CREATE INDEX IX_AracHareketResim_AracHareketId ON AracHareketResim (AracHareketId);
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 26,
                   Description = "Uye tablosuna MobileAppState alanı eklendi",
                   CommandText = $@"
                       IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Uye' AND COLUMN_NAME = 'MobileAppState')
                        BEGIN
                           ALTER TABLE Uye ADD MobileAppState NVARCHAR(MAX);
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 27,
                   Description = "KampanyaIndirimTipi tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KampanyaIndirimTipi')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqKampanyaIndirimTipi AS INT START WITH 100 INCREMENT BY 10;
                        	CREATE TABLE dbo.KampanyaIndirimTipi(
                        		Id					INT NOT NULL,
                        
                        		Durum				BIT NOT NULL,
                        		Sira				INT NOT NULL,
                        
                        		Ad					NVARCHAR(50),
                        		AdEng				NVARCHAR(50),
                        
                        		CONSTRAINT PK_KampanyaIndirimTipi PRIMARY KEY  (Id)
                            );
                        	INSERT INTO KampanyaIndirimTipi (Id, Durum, Sira, Ad, AdEng) VALUES (Next Value For dbo.sqKampanyaIndirimTipi, 1, 1, N'Yüzde İndirimi',N'Percent Discount');
                        	INSERT INTO KampanyaIndirimTipi (Id, Durum, Sira, Ad, AdEng) VALUES (Next Value For dbo.sqKampanyaIndirimTipi, 1, 2, N'Tutar İndirimi',N'Amount Discount');
                        	INSERT INTO KampanyaIndirimTipi (Id, Durum, Sira, Ad, AdEng) VALUES (Next Value For dbo.sqKampanyaIndirimTipi, 1, 3, N'Dakika İndirimi',N'Minute Discount');
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 28,
                   Description = "KampanyaTur tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KampanyaTur')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqKampanyaTur AS INT START WITH 100 INCREMENT BY 10;
                        	CREATE TABLE dbo.KampanyaTur(
                        		Id					INT NOT NULL,
                        
                        		Durum				BIT NOT NULL,
                        		Sira				INT NOT NULL,
                        
                        		Ad					NVARCHAR(50),
                        		AdEng				NVARCHAR(50),
                        
                        		CONSTRAINT PK_KampanyaTur PRIMARY KEY  (Id)
                            );
                        	INSERT INTO KampanyaTur (Id, Durum, Sira, Ad, AdEng) VALUES (Next Value For dbo.sqKampanyaTur, 1, 1, N'Standart',N'Standard');
                        	INSERT INTO KampanyaTur (Id, Durum, Sira, Ad, AdEng) VALUES (Next Value For dbo.sqKampanyaTur, 1, 2, N'Yeni Üye',N'New Member');
                        	INSERT INTO KampanyaTur (Id, Durum, Sira, Ad, AdEng) VALUES (Next Value For dbo.sqKampanyaTur, 1, 3, N'Doğum Günü',N'Birthday');
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 29,
                   Description = "Kampanya tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Kampanya')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqKampanya AS INT START WITH 1 INCREMENT BY 1;
                        	CREATE TABLE dbo.Kampanya(
                        		Id						INT NOT NULL,
                        
                        		Durum					BIT NOT NULL,
                        		KampanyaIndirimTipiId	INT NOT NULL,
                        		KampanyaTurId			INT NOT NULL,
                        		UyeGrupIds				NVARCHAR(MAX),
                        
                        		Ad						NVARCHAR(100),
                        		AdEng					NVARCHAR(100),
                        		GorselUrl				NVARCHAR(300),
                        		SayfaIcerik				NVARCHAR(MAX),
                        		SayfaIcerikEng			NVARCHAR(MAX),
                        		BaslangicTarihi			DATETIME,
                        		BitisTarihi				DATETIME,
                        
                        		IndirimDegeri			DECIMAL(18,2) NOT NULL, 
                        		CokluKullanim			BIT NOT NULL,
                        
                        		CONSTRAINT PK_Kampanya PRIMARY KEY  (Id),
                        		CONSTRAINT FK_Kampanya_KampanyaIndirimTipiId FOREIGN KEY (KampanyaIndirimTipiId) REFERENCES KampanyaIndirimTipi(Id),
                        		CONSTRAINT FK_Kampanya_KampanyaTurId FOREIGN KEY (KampanyaTurId) REFERENCES KampanyaTur(Id)
                        	);	
                        END
                   "
               }
           );

            sqlUpdateList.Add(
               new MoUpdate
               {
                   Id = 30,
                   Description = "KampanyaUye tablosu eklendi",
                   CommandText = $@"
                        IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KampanyaUye')
                        BEGIN
                        	CREATE SEQUENCE dbo.sqKampanyaUye AS INT START WITH 1 INCREMENT BY 1;
                        	CREATE TABLE dbo.KampanyaUye(
                        		Id								INT NOT NULL,
                        
                        		KampanyaId						INT NOT NULL,
                        		UyeId							INT NOT NULL,
                        
                        
                        		CONSTRAINT PK_KampanyaUye PRIMARY KEY  (Id),
                        		CONSTRAINT FK_KampanyaUye_KampanyaId FOREIGN KEY (KampanyaId) REFERENCES Kampanya(Id),
                        		CONSTRAINT FK_KampanyaUye_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id),
                            );
                        END
                   "
               }
           );

            ///------------------------------------
            sqlUpdateList = sqlUpdateList
                .Where(c => c.Id > 0)
                .Where(c => c.Id > updateId)
                .ToList();

            return sqlUpdateList;
        }

    }

}
