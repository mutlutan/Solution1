
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
		UseAuthenticator	BIT NOT NULL,
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

		/*Api Key*/
		GoogleMapApiKey			NVARCHAR(100),

		CONSTRAINT PK_Parameter PRIMARY KEY (Id)
	);
	INSERT INTO Parameter (Id, UseAuthenticator, SiteAddress, InstitutionEmail, AuditLog, EmailHost, EmailPort, EmailEnableSsl, EmailUserName, EmailPassword, GoogleMapApiKey) 
	VALUES (1, 0, N'https://qq.com', N'info@qq.com', 0, N'mail.qq.com', 587, 0, N'info@qq.com', N'123', N'AIzaSyDHnQDsewS54EKaP3Rkxuh3npv6uW60mko');

	/*Job Tanımları*/
	CREATE TABLE dbo.Job(
		Id				UNIQUEIDENTIFIER NOT NULL,

		IsActive		BIT NOT NULL,

		MethodName		NVARCHAR(100) NOT NULL,
		MethodParams	NVARCHAR(500), /*jsontext olabilir*/
		MethodComment	NVARCHAR(500), /*Açıklama*/
		CronExpression	VARCHAR(50),
		
		CONSTRAINT PK_Job PRIMARY KEY (Id)
	);
	CREATE INDEX IX_Job_IsActive ON Job (IsActive);
	INSERT INTO Job (Id, IsActive, MethodName, CronExpression) VALUES (NewId(), 1, N'LocalWebRequest', '*/15 * * * *');
	INSERT INTO Job (Id, IsActive, MethodName, CronExpression) VALUES (NewId(), 1, N'SetAuditLogToDbLogFromDbMain', '*/5 * * * *');
	INSERT INTO Job (Id, IsActive, MethodName, CronExpression) VALUES (NewId(), 1, N'MailJobMailHareklerdenBekliyorOlanlariGoder', '*/5 * * * *');
	INSERT INTO Job (Id, IsActive, MethodName, CronExpression) VALUES (NewId(), 1, N'MailJobMailHarekleriTekrarDene', '*/5 * * * *');

	/*Role Tanımları*/
	CREATE SEQUENCE dbo.sqRole AS INT START WITH 2001 INCREMENT BY 1; 
	CREATE TABLE dbo.Role(
		Id			INT NOT NULL,

		
		IsActive	BIT NOT NULL,
		LineNumber	INT NOT NULL,
		Name		NVARCHAR(30) NOT NULL,	
		Authority	NVARCHAR(MAX),		/*Yetkiler*/

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,
		
		CONSTRAINT PK_Role PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Role_Name ON Role (Name);
	CREATE UNIQUE INDEX UX_Role_UniqueId ON Role (UniqueId);
	INSERT INTO Role (Id, IsActive, LineNumber, Name, UniqueId, CreateDate) VALUES (0, 0, -9, N'', newid(), getdate());
	INSERT INTO Role (Id, IsActive, LineNumber, Name, UniqueId, CreateDate) VALUES (1001, 1, -8, N'Admin', newid(), getdate());
	INSERT INTO Role (Id, IsActive, LineNumber, Name, UniqueId, CreateDate) VALUES (Next Value For dbo.sqRole, 1, 1, N'User', newid(), getdate());

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
	INSERT INTO Country (Id, IsActive, LineNumber, Code, Name) VALUES (0, 1, 0, N'', N'');
	
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
	CREATE UNIQUE INDEX UX_EmailLetterhead_UniqueId ON EmailLetterhead (UniqueId);
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
	CREATE UNIQUE INDEX UX_EmailTemplate_UniqueId ON EmailTemplate (UniqueId);

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
	CREATE UNIQUE INDEX IX_EmailPool_UniqueId ON EmailPool (UniqueId);
	CREATE INDEX IX_EmailPool_EmailTemplateId ON EmailPool (EmailTemplateId);
	CREATE INDEX IX_EmailPool_CreateDate ON EmailPool (CreateDate);


	/*Currency Para Birim */
	CREATE SEQUENCE dbo.sqCurrency AS INT START WITH 101 INCREMENT BY 1;
	CREATE TABLE dbo.Currency(
		Id				INT NOT NULL,

		IsActive		BIT NOT NULL,
		LineNumber		INT NOT NULL,
		Icon			VARCHAR(10) NOT NULL,
		Code			VARCHAR(10) NOT NULL,
		Name			NVARCHAR(20) NOT NULL,/*Tam para birimi, Üst para birimi (faturada yazıya çevrilirken kullanılacak kısım)*/
		SubName			NVARCHAR(20) NOT NULL,/*Kesirli para birimi, Alt Para Birimi - Kuruş (faturada yazıya çevrilirken kullanılacak kısım)*/

		CONSTRAINT PK_Currency PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Currency_Code ON Currency (Code);
	CREATE UNIQUE INDEX UX_Currency_Name ON Currency (Name);
	--INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (0, 1, -9, N'', N'', N'', N'');
	--INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqCurrency, 0, 1, N'₺', N'TRY', N'Türk Lirası', N'Kuruş');
	--INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqCurrency, 0, 2, N'$', N'USD', N'Dolar', N'Cent');
	--INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqCurrency, 0, 3, N'€', N'EUR', N'Euro', N'Cent');
	INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqCurrency, 1, 1, N'₮', N'USDT', N'Tether', N'');
	INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqCurrency, 0, 2, N'₿', N'BTC', N'Bitcoin', N'');
	INSERT INTO Currency (Id, IsActive, LineNumber, Icon, Code, Name, SubName) VALUES (Next Value For dbo.sqCurrency, 0, 3, N'⧫', N'ETH', N'Ethereum', N'');


	/*Dashboard Tanımları*/
	CREATE SEQUENCE dbo.sqDashboard AS INT START WITH 101 INCREMENT BY 1; 
	CREATE TABLE dbo.Dashboard(
		Id					INT NOT NULL,

		IsActive			BIT NOT NULL,
		LineNumber			INT NOT NULL,

		TemplateName		NVARCHAR(20) NOT NULL, /*template1, template2 ...*/
		Title				NVARCHAR(20) NOT NULL, 
		IconClass			NVARCHAR(50), /*fa fa-user vb... ve renk*/
		IconStyle			NVARCHAR(50), /*fa fa-user vb...*/
		DetailUrl			NVARCHAR(250),
		Query				NVARCHAR(max),
		--ValuesJson			NVARCHAR(MAX) NOT NULL,	/* [{"Text":"txt:Aktif", "Value":"sql:Select Count(*) as Val From User", "Info":"txt:Adet" }, {"Text":"txt:Pasif", "Value":"sql2", "Info":"Adet" }] */

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		CreateDate			DATETIME,
		CreatedUserId		INT,
		UpdateDate			DATETIME,
		UpdatedUserId		INT,
		
		CONSTRAINT PK_Dashboard PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Dashboard_UniqueId ON Dashboard (UniqueId);
	INSERT INTO Dashboard (Id, UniqueId, IsActive, LineNumber, TemplateName, Title, DetailUrl, IconClass, IconStyle, Query) VALUES (Next Value For dbo.sqRole, newid(), 1, 1, N'teplate1', N'User', N'#/User', N'fa fa-fw fa-4x fa-users', N'color:red', N'Select Count(*) From [User]');
	INSERT INTO Dashboard (Id, UniqueId, IsActive, LineNumber, TemplateName, Title, DetailUrl, IconClass, IconStyle, Query) VALUES (Next Value For dbo.sqRole, newid(), 1, 2, N'teplate1', N'Customer', N'#/Customer', N'fa fa-fw fa-4x fa-id-card-o', N'color:blue', N'Select Count(*) From [Customer]');
	

	/*UserStatus*/
	CREATE TABLE dbo.UserStatus (
		Id			INT NOT NULL,
		
		Name		NVARCHAR(50),

		CONSTRAINT PK_UserStatus PRIMARY KEY (Id)
	);
	INSERT INTO UserStatus (Id, Name) VALUES (0, N'Passive');
	INSERT INTO UserStatus (Id, Name) VALUES (1, N'Active');
	INSERT INTO UserStatus (Id, Name) VALUES (2, N'Blocked');
	INSERT INTO UserStatus (Id, Name) VALUES (3, N'Deleted'); /*(kullanıcı açısından delete anlamında)*/

	/*UserType*/
	CREATE TABLE dbo.UserType (
		Id			INT NOT NULL,
		
		Name		NVARCHAR(50),

		CONSTRAINT PK_UserType PRIMARY KEY (Id)
	);
	INSERT INTO UserType (Id, Name) VALUES (0, N'');
	INSERT INTO UserType (Id, Name) VALUES (11, N'Admin');
	INSERT INTO UserType (Id, Name) VALUES (21, N'Member');
	INSERT INTO UserType (Id, Name) VALUES (31, N'Customer');

	/* Kullanıcı (şifre ile giriş yapması muhtemel olan personel kayıtları)*/
	CREATE SEQUENCE dbo.sqUser AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.[User](
		Id					INT NOT NULL,
		
		UserStatusId		INT NOT NULL,
		UserTypeId			INT NOT NULL,

		IsEmailConfirmed	BIT NOT NULL,
		NameSurname			NVARCHAR(100), 
		ResidenceAddress	NVARCHAR(50), /*ikamet adresi*/ 
		Avatar				VARCHAR(MAX), /*vesikalık resim*/
		GeoLocation			GEOGRAPHY, /*Üye Konum bilgisi -  automatik de alınabilir, şart değil*/

		Email				NVARCHAR(100),
		Password			NVARCHAR(100),	/*Bu alan her update olduğunda, KullaniciSifre tablosuna insert edilecek, history için*/
		RoleIds				NVARCHAR(MAX),
		Authority			NVARCHAR(MAX),		/*Yetkiler*/

		GaSecretKey			NVARCHAR(100),	/*Google Authenticator Secret Key*/
		SessionGuid			NVARCHAR(100),	/*Login olan kullanıcıya verilen unique orturum id*/
		ValidityDate		DATE,			/* Şifre geçerlilik tarihi*/

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		CreateDate			DATETIME,
		CreatedUserId		INT,
		UpdateDate			DATETIME,
		UpdatedUserId		INT,

		CONSTRAINT PK_User PRIMARY KEY  (Id),
		CONSTRAINT FK_User_UserStatusId FOREIGN KEY (UserStatusId) REFERENCES UserStatus(Id),
		CONSTRAINT FK_User_UserTypeId FOREIGN KEY (UserTypeId) REFERENCES UserType(Id)
    );
	CREATE UNIQUE INDEX UX_User_Email ON [User] (Email);
	CREATE UNIQUE INDEX UX_User_UniqueId ON [User] (UniqueId);
	CREATE INDEX IX_User_UserStatusId ON [User] (UserStatusId);
	CREATE INDEX IX_User_UserTypeId ON [User] (UserTypeId);
	CREATE INDEX IX_User_CreateDate ON [User] (CreateDate);
	INSERT INTO [User] (Id, UserStatusId, UserTypeId, IsEmailConfirmed, Email, Password, RoleIds, GaSecretKey, UniqueId) VALUES (0, 0, 0, 0, N'', N'', 0, N'', newid());
	INSERT INTO [User] (Id, UserStatusId, UserTypeId, IsEmailConfirmed, Email, Password, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 11, 1, N'Admin', N'07', '1001', N'', newid());
	INSERT INTO [User] (Id, UserStatusId, UserTypeId, IsEmailConfirmed, Email, Password, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 11, 1, N'Developer1', N'07', '1001', N'', newid());
	INSERT INTO [User] (Id, UserStatusId, UserTypeId, IsEmailConfirmed, Email, Password, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 11, 1, N'Developer2', N'07', '1001', N'', newid());
	INSERT INTO [User] (Id, UserStatusId, UserTypeId, IsEmailConfirmed, Email, Password, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 21, 1, N'Person1', N'07', '2001', N'', newid());

	/*CustomerType*/
	CREATE TABLE dbo.CustomerType (
		Id			INT NOT NULL,
		
		Name		NVARCHAR(50),

		CONSTRAINT PK_CustomerType PRIMARY KEY (Id)
	);
	INSERT INTO CustomerType (Id, Name) VALUES (0, N'');
	INSERT INTO CustomerType (Id, Name) VALUES (11, N'Standart');
	INSERT INTO CustomerType (Id, Name) VALUES (12, N'Influencer');

	/*Cutomer - Müşteri*/
	CREATE SEQUENCE dbo.sqCutomer AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Customer(
		Id					INT NOT NULL,

		UserStatusId		INT NOT NULL,
		CustomerTypeId		INT NOT NULL,

		IsEmailConfirmed	BIT NOT NULL,
		NameSurname			NVARCHAR(100), 
		ResidenceAddress	NVARCHAR(50), /*ikamet adresi*/ 
		Avatar				VARCHAR(MAX), /*vesikalık resim*/
		GeoLocation			GEOGRAPHY, /*Üye Konum bilgisi -  automatik de alınabilir, şart değil*/

		Email				NVARCHAR(100),
		Password			NVARCHAR(100),	/*Bu alan her update olduğunda, KullaniciSifre tablosuna insert edilecek, history için*/
		RoleIds				NVARCHAR(MAX),
		
		GaSecretKey			NVARCHAR(100),	/*Google Authenticator Secret Key*/
		SessionGuid			NVARCHAR(100),	/*Login olan kullanıcıya verilen unique orturum id*/
		ValidityDate		DATE,			/* Şifre geçerlilik tarihi*/

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		CreateDate			DATETIME,
		CreatedUserId		INT,
		UpdateDate			DATETIME,
		UpdatedUserId		INT,

		CONSTRAINT PK_Customer PRIMARY KEY  (Id),
		CONSTRAINT FK_Customer_UserStatusId FOREIGN KEY (UserStatusId) REFERENCES UserStatus(Id),
		CONSTRAINT FK_Customer_CustomerTypeId FOREIGN KEY (CustomerTypeId) REFERENCES CustomerType(Id)
    );
	CREATE UNIQUE INDEX UX_Customer_Email ON Customer (Email);
	CREATE UNIQUE INDEX UX_Customer_UniqueId ON Customer (UniqueId);
	CREATE INDEX IX_Customer_UserStatusId ON Customer (UserStatusId);
	CREATE INDEX IX_Customer_NameSurname ON Customer (NameSurname);
	INSERT INTO Customer (Id, UserStatusId, CustomerTypeId, IsEmailConfirmed, NameSurname, Email, UniqueId) VALUES (0, 0, 0, 0, N'', N'', newid());

	
	/*Product Category*/
	CREATE SEQUENCE dbo.sqProductCategory AS INT START WITH 1001 INCREMENT BY 1;
	CREATE TABLE dbo.ProductCategory (
		Id				INT NOT NULL,
		
		ParentId		INT,
		IsActive		BIT NOT NULL,
		Name			NVARCHAR(50),

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_ProductCategory PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_ProductCategory_UniqueId ON ProductCategory (UniqueId);
	CREATE UNIQUE INDEX UX_ProductCategory_Name ON ProductCategory (Name);
	INSERT INTO ProductCategory (Id, IsActive, Name, UniqueId) VALUES (0, 0, N'', newid());
	INSERT INTO ProductCategory (Id, IsActive, Name, UniqueId) VALUES (Next Value For dbo.sqProductCategory, 1, N'Game', newid());
	INSERT INTO ProductCategory (Id, IsActive, Name, UniqueId) VALUES (Next Value For dbo.sqProductCategory, 1, N'Sotfware', newid());

	/*Product*/
	CREATE SEQUENCE dbo.sqProduct AS INT START WITH 10001 INCREMENT BY 1;
	CREATE TABLE dbo.Product (
		Id					INT NOT NULL,
		
		ProductCategoryId	INT NOT NULL,
		IsActive			BIT NOT NULL,
		Name				NVARCHAR(150),

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_Product PRIMARY KEY (Id),
		CONSTRAINT FK_Product_ProductCategoryId FOREIGN KEY (ProductCategoryId) REFERENCES ProductCategory(Id)
	);
	CREATE UNIQUE INDEX UX_Product_UniqueId ON Product (UniqueId);
	CREATE UNIQUE INDEX UX_Product_Name ON Product (Name);
	CREATE INDEX IX_Product_ProductCategoryId ON Product (ProductCategoryId);
	INSERT INTO Product (Id, ProductCategoryId, IsActive, Name, UniqueId) VALUES (0, 0, 0, N'', newid());
	INSERT INTO Product (Id, ProductCategoryId, IsActive, Name, UniqueId) VALUES (Next Value For dbo.sqProduct, 1001, 1, N'Tetris', newid());
	INSERT INTO Product (Id, ProductCategoryId, IsActive, Name, UniqueId) VALUES (Next Value For dbo.sqProduct, 1001, 1, N'Game2', newid());


	/* X1  M O D U L  için gerekenler */

	/*CustomerWallet*/
	CREATE SEQUENCE dbo.sqCustomerWallet AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.CustomerWallet(
		Id				INT NOT NULL,

		IsActive		BIT NOT NULL,
		CustomerId		INT NOT NULL,
		CurrencyId		INT NOT NULL,

		WalletNumber	VARCHAR(200), 

		UniqueId		UNIQUEIDENTIFIER NOT NULL,
		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_CustomerWallet PRIMARY KEY  (Id),
		CONSTRAINT FK_CustomerWallet_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
    );
	CREATE UNIQUE INDEX UX_CustomerWallet_CurrencyId_WalletNumber ON CustomerWallet (CurrencyId, WalletNumber);
	CREATE UNIQUE INDEX UX_CustomerWallet_UniqueId ON CustomerWallet (UniqueId);
	CREATE INDEX IX_CustomerWallet_CustomerId ON CustomerWallet (CustomerId);
	INSERT INTO CustomerWallet (Id, IsActive, CustomerId, CurrencyId, UniqueId) VALUES (0, 0, 0, 0, newid());

	/*Cüzdan Hareket türleri*/
	CREATE TABLE dbo.TransactionType (
		Id			INT NOT NULL,
		
		Name			NVARCHAR(50),

		CONSTRAINT PK_TransactionType PRIMARY KEY (Id)
	);
	INSERT INTO TransactionType (Id, Name) VALUES (1, N'Incoming Amount, '); --Dışardan Gelen Tutar
	INSERT INTO TransactionType (Id, Name) VALUES (2, N'Outgoing Amount'); -- Dışarı Giden Tutar
	INSERT INTO TransactionType (Id, Name) VALUES (3, N'Game Credit'); --Cüzdandan ödeme yapılır alacak hanesine yazılır
	INSERT INTO TransactionType (Id, Name) VALUES (4, N'Game Debit'); --Karta iadesi yapılırsa alacak alanına yazılır


	/*CutomerTransaction*/
	CREATE SEQUENCE dbo.sqCustomerTransaction AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.CustomerTransaction(
		Id					INT NOT NULL,

		CustomerId			INT NOT NULL,
		CustomerWalletId	INT NOT NULL,
		Debit				DECIMAL(18,6), 
		Credit				DECIMAL(18,6), 
 
		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		CreateDate			DATETIME,
		CreatedUserId		INT,
		UpdateDate			DATETIME,
		UpdatedUserId		INT,

		CONSTRAINT PK_CustomerTransaction PRIMARY KEY  (Id),
		CONSTRAINT FK_CustomerTransaction_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
		CONSTRAINT FK_CustomerTransaction_CustomerWalletId FOREIGN KEY (CustomerWalletId) REFERENCES CustomerWallet(Id)
    );
	CREATE UNIQUE INDEX UX_CustomerTransaction_UniqueId ON CustomerTransaction (UniqueId);
	CREATE INDEX IX_CustomerTransaction_CustomerId ON CustomerTransaction (CustomerId);






