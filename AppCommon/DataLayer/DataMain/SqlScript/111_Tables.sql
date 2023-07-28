
	/*Database Schema Version */
	CREATE TABLE dbo.Version(
		Id				INT NOT NULL, /*Id => Veritabanı sema versiyonudur*/

		CreateDate		DATETIME,
		Description		NVARCHAR(MAX),  /*işlemin adı*/
		CommandText		NVARCHAR(MAX),  /*işlemin sql komutları*/
		      	
		CONSTRAINT PK_Version PRIMARY KEY (Id)
	);
	INSERT Version (Id, Description, CommandText) VALUES (30, '', '');

	/*bu tabloya eklenen her kayıt log db ye belirli zmanalarda transfer edilecek, triger ile ...*/
	/*doğru kayıt loglanabilmesi için aynı entity savechange() metodunda save edilmeli, kayıt başarısız olsa bile, logu kayıt edilmesi söz konusu oluyor*/
	CREATE TABLE dbo.AuditLog(
		Id				UNIQUEIDENTIFIER NOT NULL,

		UserId			INT NOT NULL,
		UserType		VARCHAR(50), /*User tablosunun adı olabilir, birden çok usr tablosundan gelebilir*/
		UserName		NVARCHAR(50),
		UserIp			VARCHAR(50),
		UserBrowser		VARCHAR(250), /*Tarayici ve versiyonu*/
		UserSessionGuid	VARCHAR(50), /*Session guid*/

		OperationType	CHAR(1), /*CRUD type*/
		OperationDate	DATETIME,

		TableName		VARCHAR(50),
		PrimaryKeyField	VARCHAR(50), 
		PrimaryKeyValue	VARCHAR(50),		
		CurrentValues	NVARCHAR(MAX),
		OriginalValues	NVARCHAR(MAX),
	
		CONSTRAINT PK_AuditLog PRIMARY KEY (Id)
	);

	/*Parametreler - default değerler */
	CREATE TABLE dbo.Parameter(
		Id					INT NOT NULL,

		/*Genel*/
		SiteAddress			NVARCHAR(100) NOT NULL, /*local : http://localhost:5002  sunucu: https://www.qq.com*/
		InstitutionEmail	NVARCHAR(100), /* default alıcı mail adresi*/

		/*Audit yapılacak tablolar*/
		AuditLog			BIT NOT NULL,
		AuditLogTables		NVARCHAR(MAX),
		
		/*SMTP Mail Account bilgileri (giden maillerde kullanılacak account)*/
		EmailHost			NVARCHAR(100),
		EmailPort			INT NOT NULL,
		EmailEnableSsl		BIT NOT NULL,
		EmailUserName		NVARCHAR(100),
		EmailPassword		NVARCHAR(100),

		/*SMS servis bilgileri*/ 
		SmsServiceBaseUrl		NVARCHAR(100),
		SmsServiceUrl			NVARCHAR(100),
		SmsServiceUserName		NVARCHAR(100),
		SmsServicePassword		NVARCHAR(100),
		SmsServiceBaslik		NVARCHAR(30),

		/*Api Key*/
		GoogleMapApiKey			NVARCHAR(100),
		MapTexBaseServiceUrl	NVARCHAR(300),
		MaptexApiKey			NVARCHAR(100),

		/*Araç Rezervasyon Süresi Dakika*/
		AracRezervasyonSure		INT NOT NULL,
		AracSarjUyariLimit		DECIMAL(18,2) NOT NULL,

		/*MasterPass Bilgileri*/
		MasterpassMerchantId BIGINT NOT NULL,
		MasterpassServiceUrl NVARCHAR(100)


		CONSTRAINT PK_Parameter PRIMARY KEY (Id)
	);
	INSERT INTO Parameter (Id, AuditLog, SiteAddress, InstitutionEmail, EmailHost, EmailPort, EmailEnableSsl, EmailUserName, EmailPassword, GoogleMapApiKey, MapTexBaseServiceUrl, MaptexApiKey,AracRezervasyonSure, AracSarjUyariLimit, MasterpassMerchantId) 
	VALUES (1, 0, N'https://qq.com', N'info@qq.com', N'mail.qq.com', 587, 0, N'info@qq.com', N'123', N'AIzaSyDHnQDsewS54EKaP3Rkxuh3npv6uW60mko', N'https://ffsapi.yourassetsonline.com:8446', N'644BEEC9-9875-466C-9A6D-DC2F01285FCC',60,0,0);
	
	/*Role Tanımları*/
	CREATE SEQUENCE dbo.sqRole AS INT START WITH 2001 INCREMENT BY 1; 
	CREATE TABLE dbo.Role(
		Id			INT NOT NULL,

		UniqueId	UNIQUEIDENTIFIER NOT NULL,
		IsActive	BIT NOT NULL,
		LineNumber	INT NOT NULL,
		Name		NVARCHAR(30) NOT NULL,	
		Authority	NVARCHAR(MAX),		/*Yetkiler*/

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,
		
		CONSTRAINT PK_Role PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Role_Name ON Role (Name);
	INSERT INTO Role (Id, UniqueId, IsActive, LineNumber, Name) VALUES (0, newid(), 0, -9, N'');
	INSERT INTO Role (Id, UniqueId, IsActive, LineNumber, Name) VALUES (1001, newid(), 1, -8, N'Admin');
	INSERT INTO Role (Id, UniqueId, IsActive, LineNumber, Name) VALUES (Next Value For dbo.sqRole, newid(), 1, 1, N'User');

	/*Gender - Cinsiyet*/
	CREATE TABLE dbo.Gender(
		Id			INT NOT NULL,

		IsActive	BIT NOT NULL,
		LineNumber	INT NOT NULL,
		Name		NVARCHAR(100) NOT NULL,
		
		CONSTRAINT PK_Gender PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Gender_Name ON Gender (Name);
	CREATE INDEX IX_Gender_IsActive ON Gender (IsActive);
	INSERT INTO Gender (Id, IsActive, LineNumber, Name) VALUES (0, 1, 0, N'');
	INSERT INTO Gender (Id, IsActive, LineNumber, Name) VALUES (1, 1, 1, N'Male');
	INSERT INTO Gender (Id, IsActive, LineNumber, Name) VALUES (2, 1, 2, N'Female');
	INSERT INTO Gender (Id, IsActive, LineNumber, Name) VALUES (3, 1, 3, N'Not want to specify');

	/*Country Ulke*/
	CREATE SEQUENCE dbo.sqCountry AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Country(
		Id			INT NOT NULL,

		IsActive	BIT NOT NULL,
		LineNumber	INT NOT NULL,
		Code		VARCHAR(100) NOT NULL,
		Name		NVARCHAR(100) NOT NULL,
		
		CONSTRAINT PK_Country PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Country_Name ON Country (Name);
	CREATE INDEX IX_Country_IsActive ON Country (IsActive);
	INSERT INTO Country (Id, IsActive, LineNumber, Name) VALUES (0, 1, 0, N'');

	/* Kullanıcı (şifre ile giriş yapması muhtemel olan personel kayıtları)*/
	CREATE SEQUENCE dbo.sqUser AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.[User](
		Id					INT NOT NULL,
		
		IsActive			BIT NOT NULL,
		IsEmailConfirmed	BIT NOT NULL,

		IdentificationNumber	NVARCHAR(11), /*TCNo*/
		NameSurname				NVARCHAR(100), 
		ResidenceAddress		NVARCHAR(50), /*ikamet adresi*/ 
		Avatar					VARCHAR(MAX), /*vesikalık resim*/


		UserName			NVARCHAR(100),	/*email adres olabilir*/
		UserPassword		NVARCHAR(100),	/*Bu alan her update olduğunda, KullaniciSifre tablosuna insert edilecek, history için*/
		RoleIds				NVARCHAR(MAX),
		
		GaSecretKey			NVARCHAR(100),	/*Google Authenticator Secret Key*/

		SessionGuid			NVARCHAR(100),	/*Login olan kullanıcıya verilen unique orturum id*/
		ValidityDate		DATE,			/* Şifre geçerlilik tarihi*/

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		CreateDate			DATETIME,
		CreatedUserId		INT,
		UpdateDate			DATETIME,
		UpdatedUserId		INT,

		CONSTRAINT PK_User PRIMARY KEY  (Id)
    );
	CREATE UNIQUE INDEX UX_User_UserName ON [User] (UserName);
	CREATE INDEX IX_User_IsActive ON [User] (IsActive);
	CREATE INDEX IX_User_CreateDate ON [User] (CreateDate);
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (0, 0, 0, N'', N'', 0, N'', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Admin', N'07', '1001', N'', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Developer1', N'07', '1001', N'', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Developer2', N'07', '1001', N'', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Person1', N'07', '2001', N'', newid());


	/*Dashboard Tanımları*/
	--CREATE SEQUENCE dbo.sqDashboard AS INT START WITH 101 INCREMENT BY 1; 
	--CREATE TABLE dbo.Dashboard(
	--	Id					INT NOT NULL,

	--	IsActive			BIT NOT NULL,
	--	LineNumber			INT NOT NULL,

	--	TemplateName		NVARCHAR(20) NOT NULL, /*template1, template2 ...*/
	--	IconClass			NVARCHAR(50), /*fa fa-user vb... ve renk*/
	--	IconStyle			NVARCHAR(50), /*fa fa-user vb...*/
	--	DetailUrl			NVARCHAR(250),
	--	Query				NVARCHAR(max),
	--	--ValuesJson			NVARCHAR(MAX) NOT NULL,	/* [{"Text":"txt:Aktif", "Value":"sql:Select Count(*) as Val From User", "Info":"txt:Adet" }, {"Text":"txt:Pasif", "Value":"sql2", "Info":"Adet" }] */

	--	UniqueId			UNIQUEIDENTIFIER NOT NULL,
	--	CreateDate			DATETIME,
	--	CreatedUserId		INT,
	--	UpdateDate			DATETIME,
	--	UpdatedUserId		INT,
		
	--	CONSTRAINT PK_Dashboard PRIMARY KEY (Id)
	--);
	--CREATE UNIQUE INDEX UX_Dashboard_Name ON Dashboard (Name);
	--INSERT INTO Role (Id, UniqueId, IsActive, LineNumber, TemplateName, DetailUrl, Query) VALUES (Next Value For dbo.sqRole, newid(), 1, N'teplate1', N'#/User', N'Select Count(*) as Value1 From User');
	
	
	/*EmailLetterhead - EmailAnted*/
	CREATE SEQUENCE dbo.sqEmailLetterhead AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.EmailLetterhead (
		Id				INT NOT NULL,

		Description		NVARCHAR(50),
		Content			NVARCHAR(max), /*html içerik ile editörden yapılandır, replace ile kodlanmış verileri mail merge yap*/

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_EmailLetterhead PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_EmailLetterhead_Description ON EmailLetterhead (Description);
	INSERT INTO EmailLetterhead (Id, Description, Content, UniqueId) VALUES (0, N'Boş', N'', newid()); /*Boş antetli mail gerekirse*/

	/*EmailTemplate - EmailSablon*/

	CREATE TABLE dbo.EmailTemplate (
		Id					INT NOT NULL,
		
		EmailLetterheadId	INT NOT NULL,
		Name				NVARCHAR(50),

		EmailCc				NVARCHAR(max), /*CarbonCopy*/
		EmailBcc			NVARCHAR(max), /*BlindCarbonCopy*/
		EmailSubject		NVARCHAR(max),
		EmailContent		NVARCHAR(max), /*html içerik ile editörden yapılandır, replace ile kodlanmış verileri mail merge yap*/

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_EmailTemplate PRIMARY KEY (Id),
		CONSTRAINT FK_EmailTemplate_EmailLetterheadId FOREIGN KEY (EmailLetterheadId) REFERENCES EmailLetterhead (Id)
	);
	CREATE UNIQUE INDEX UX_EmailTemplate_Name ON EmailTemplate (Name);

	/*EmailPoolStatus*/
	CREATE TABLE dbo.EmailPoolStatus (
		Id		INT NOT NULL,
		
		Name	NVARCHAR(50),

		CONSTRAINT Pk_EmailPoolStatus PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_EmailPoolStatus_Name ON EmailPoolStatus (Name);
	INSERT INTO EmailPoolStatus (Id, Name) VALUES (0, N'Gönderilecek'); /*deneme sayısı 1 artır gönderiliyor yap, göndermeyi dene*/
	INSERT INTO EmailPoolStatus (Id, Name) VALUES (1, N'Gönderiliyor'); /*deneme sayısı 1 artır gönderiliyor yap, göndermeyi tekrar dene*/
	INSERT INTO EmailPoolStatus (Id, Name) VALUES (2, N'Gönderildi'); /*gönderildiğinden emin isen Gönderildi yap*/
	INSERT INTO EmailPoolStatus (Id, Name) VALUES (3, N'Hata'); /*Hata olduysa 3 yaz, açıklamaya yaz*/

	/*EmailPool*/
	CREATE SEQUENCE dbo.sqEmailPool AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.EmailPool (
		Id					INT NOT NULL,
		
		EmailTemplateId		INT NOT NULL,
		EmailPoolStatusId	INT NOT NULL,

		TryQuantity		INT NOT NULL,  /*Deneme Sayısı*/
		LastTryDate		DATETIME,	   /*Son deneme tarihi*/
		Description		NVARCHAR(max), /*log gibi açıklama*/

		EmailTo			NVARCHAR(500), /*gönderilen adres*/
		EmailCc			NVARCHAR(max), /*CarbonCopy*/
		EmailBcc		NVARCHAR(max), /*BlindCarbonCopy*/
		EmailSubject	NVARCHAR(max),
		EmailContent	NVARCHAR(max), /*html içerik ile editörden yapılandır, replace ile kodlanmış verileri mail merge yap*/

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_EmailPool PRIMARY KEY (Id),
		CONSTRAINT FK_EmailPool_EmailTemplateId FOREIGN KEY (EmailTemplateId) REFERENCES EmailTemplate (Id),
		CONSTRAINT FK_EmailPool_EmailPoolStatusId FOREIGN KEY (EmailPoolStatusId) REFERENCES EmailPoolStatus (Id)
	);
	CREATE INDEX IX_EmailPool_EmailTemplateId ON EmailPool (EmailTemplateId);
	CREATE INDEX IX_EmailPool_CreateDate ON EmailPool (CreateDate);


	/*Para Birim */
	CREATE SEQUENCE dbo.sqParaBirim AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.ParaBirim(
		Id				INT NOT NULL,

		IsActive	BIT NOT NULL,
		LineNumber	INT NOT NULL,
		Icon		VARCHAR(10) NOT NULL,
		Code		VARCHAR(10) NOT NULL,
		Name		NVARCHAR(20) NOT NULL,/*Tam para birimi, Üst para birimi (faturada yazıya çevrilirken kullanılacak kısım)*/
		SubName		NVARCHAR(20) NOT NULL,/*Kesirli para birimi, Alt Para Birimi - Kuruş (faturada yazıya çevrilirken kullanılacak kısım)*/

		CONSTRAINT PK_ParaBirim PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_ParaBirim_Ad ON ParaBirim (Ad);
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (0, 1, -9, N'', N'', N'', N'');
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqParaBirim, 0, 1, N'₺', N'TRY', N'Türk Lirası', N'Kuruş');
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqParaBirim, 0, 2, N'$', N'USD', N'Dolar', N'Cent');
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqParaBirim, 0, 3, N'€', N'EUR', N'Euro', N'Cent');
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqParaBirim, 1, 1, N'₮', N'USDT', N'Tether', N'');
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqParaBirim, 0, 2, N'₿', N'BTC', N'Bitcoin', N'');
	INSERT INTO ParaBirim (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqParaBirim, 0, 3, N'⧫', N'ETH', N'Ethereum', N'');
		


	/*  M O D U L   */

	/*Uye(Müşteri) Durumu*/
	CREATE TABLE dbo.UyeDurum (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),

		CONSTRAINT PK_UyeDurum PRIMARY KEY (Id)
	);
	INSERT INTO UyeDurum (Id, Ad) VALUES (0, N'Pasif');
	INSERT INTO UyeDurum (Id, Ad) VALUES (1, N'Aktif');
	INSERT INTO UyeDurum (Id, Ad) VALUES (2, N'Bloke');
	INSERT INTO UyeDurum (Id, Ad) VALUES (3, N'Deleted'); /*(müşteri açısından delete anlamında)*/

	/*Üye Grubu (Standart,Influencer vb.)*/
	CREATE SEQUENCE dbo.sqUyeGrup AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.UyeGrup(
		Id								INT NOT NULL,

		UniqueId						UNIQUEIDENTIFIER NOT NULL,
		Durum							BIT NOT NULL,
		Ad								NVARCHAR(50),

		CONSTRAINT PK_UyeGrup PRIMARY KEY  (Id)
    );
	INSERT INTO UyeGrup (Id, UniqueId, Durum, Ad) VALUES (0, newid(), 0, N'');
	INSERT INTO UyeGrup (Id, UniqueId, Durum, Ad) VALUES (Next Value For dbo.sqUyeGrup, newid(), 1, N'Standart');
	INSERT INTO UyeGrup (Id, UniqueId, Durum, Ad) VALUES (Next Value For dbo.sqUyeGrup, newid(), 1, N'Influencer');

	/*Uye*/
	CREATE SEQUENCE dbo.sqUye AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Uye(
		Id								INT NOT NULL,

		UyeDurumId		INT NOT NULL,/* 0-Aktif, 1-Pasif, 2-Bloke, 3-Deleted(müşteri açısından delete anlamında) */
		UyeGrupId		INT NOT NULL,

		IsConfirmed		BIT NOT NULL, /* Is Email Confirmed */

		NameSurname		NVARCHAR(100), 
		CountryCode		NVARCHAR(2), /*ülke kodu*/
		Avatar			VARCHAR(MAX),

		GeoLocation		GEOGRAPHY, /*Üye Konum bilgisi -  automatik de alınabilir, şart değil*/

		UserName		NVARCHAR(100),	/*email adres olabilir*/
		UserPassword	NVARCHAR(100),	/*Bu alan her update olduğunda, KullaniciSifre tablosuna insert edilecek, history için*/

		SessionGuid		NVARCHAR(100),	/*Login olan kullanıcıya verilen unique orturum id*/
		ValidityDate	DATE,			/* Şifre geçerlilik tarihi*/

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_Uye PRIMARY KEY  (Id),
		CONSTRAINT FK_Uye_UyeDurumId FOREIGN KEY (UyeDurumId) REFERENCES UyeDurum(Id),
		CONSTRAINT FK_Uye_UyeGrupId FOREIGN KEY (UyeGrupId) REFERENCES UyeGrup(Id)
    );




