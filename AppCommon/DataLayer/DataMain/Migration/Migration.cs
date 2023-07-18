using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.DataLayer.DataMain.Migration
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


            //// 26.03.2023 15:00 
            //sqlUpdateList.Add(
            //    new MoUpdate
            //    {
            //        Id = 1,
            //        Description = "AracHareket tablosunda BitisTarih alanı NULLable yapıldı",
            //        CommandText = $@"
            //           IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AracHareket' AND COLUMN_NAME = 'BitisTarih')
            //           BEGIN
            //               ALTER TABLE AracHareket ALTER COLUMN BitisTarih DATETIME;
            //           END
            //        "
            //    }
            //);


            ///------------------------------------
            sqlUpdateList = sqlUpdateList
                .Where(c => c.Id > 0)
                .Where(c => c.Id > updateId)
                .ToList();

            return sqlUpdateList;
        }

    }

}
