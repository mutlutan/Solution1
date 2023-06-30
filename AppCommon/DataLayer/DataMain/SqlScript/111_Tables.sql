
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
	CREATE SEQUENCE dbo.sqRole AS INT START WITH 2001 INCREMENT BY 13; 
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
	INSERT INTO Role (Id, UniqueId, IsActive, LineNumber, Name) VALUES (1002, newid(), 1, -7, N'Supervisor');
	INSERT INTO Role (Id, UniqueId, IsActive, LineNumber, Name) VALUES (Next Value For dbo.sqRole, newid(), 1, 1, N'User');

	/*Cinsiyet*/
	CREATE TABLE dbo.Cinsiyet(
		Id			INT NOT NULL,

		Durum		BIT NOT NULL,  
		Sira		INT NOT NULL,
		Ad		    NVARCHAR(50) NOT NULL,
		AdEng		NVARCHAR(50) NOT NULL,
		
		CONSTRAINT PK_Cinsiyet PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_Cinsiyet_Ad ON Cinsiyet (Ad);
	CREATE INDEX IX_Cinsiyet_Durum ON Cinsiyet (Durum);
	INSERT INTO Cinsiyet (Id, Durum, Sira, Ad, AdEng) VALUES (0, 1, 0, N'', N'');
	INSERT INTO Cinsiyet (Id, Durum, Sira, Ad, AdEng) VALUES (1, 1, 1, N'Erkek', N'Male');
	INSERT INTO Cinsiyet (Id, Durum, Sira, Ad, AdEng) VALUES (2, 1, 2, N'Kadın', N'Female');
	INSERT INTO Cinsiyet (Id, Durum, Sira, Ad, AdEng) VALUES (3, 1, 3, N'Belirtmek istemiyorum', N'Not want to specify');


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


		UserName			NVARCHAR(100),	/*email adres olabilir, tcno olabilir*/
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
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Admin', N'07', '1001', N'3fed0d4a-6e0a-408f-af50-caced92fafc4', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Developer', N'07', '1001', N'6740c820-5086-49ed-b70a-daec330f3d77', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'rubuplus', N'07', '1001', N'40d6becd-ee0b-4a4c-a96f-8177a2ae4754', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'scepni', N'070A0602030E', '1001', N'fe98b69d-c16a-47b3-9d4f-9b4ba73ee40e', newid());
	INSERT INTO [User] (Id, IsActive, IsEmailConfirmed, UserName, UserPassword, RoleIds, GaSecretKey, UniqueId) VALUES (Next Value For dbo.sqUser, 1, 1, N'Per1', N'07', '2027', N'', newid());


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


	/*   S M A R T    B I K E   */

	/*Kullanıcı(Müşteri) Durumu*/
	CREATE TABLE dbo.UyeDurum (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),

		CONSTRAINT PK_UyeDurum PRIMARY KEY (Id)
	);
	INSERT INTO UyeDurum (Id, Ad) VALUES (0, N'Pasif');
	INSERT INTO UyeDurum (Id, Ad) VALUES (1, N'Aktif');
	INSERT INTO UyeDurum (Id, Ad) VALUES (2, N'Bloke');
	INSERT INTO UyeDurum (Id, Ad) VALUES (3, N'Deleted');

	/*Kullanıcı Grubu (Öğrenci, öğretmen, emekli vb.)*/
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
	INSERT INTO UyeGrup (Id, UniqueId, Durum, Ad) VALUES (Next Value For dbo.sqUyeGrup, newid(), 1, N'Öğretmen');
	INSERT INTO UyeGrup (Id, UniqueId, Durum, Ad) VALUES (Next Value For dbo.sqUyeGrup, newid(), 1, N'Öğrenci');
	INSERT INTO UyeGrup (Id, UniqueId, Durum, Ad) VALUES (Next Value For dbo.sqUyeGrup, newid(), 1, N'Emekli');

	/*Kullanıcı*/
	CREATE SEQUENCE dbo.sqUye AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Uye(
		Id								INT NOT NULL,

		UniqueId						UNIQUEIDENTIFIER NOT NULL,
		--Durum							BIT NOT NULL, iptal
		UyeDurumId						INT NOT NULL,/* 0-Aktif, 1-Pasif, 2-Bloke */
		UyeGrupId						INT NOT NULL,

		UyelikTarihi					DATETIME,
		Email							NVARCHAR(100), /*KullaniciAdi*/
		Sifre							NVARCHAR(100),

		KimlikNumarasi					NVARCHAR(50), /*TCNo*/
		Ad								NVARCHAR(50),
		Soyad							NVARCHAR(50),
		Gsm								NVARCHAR(15),
		DogumTarihi						DATE NOT NULL,
		CinsiyetId						INT NOT NULL,
		Avatar							VARCHAR(MAX), /*vesikalık resim*/

		UyelikDogrulama					BIT NOT NULL,
		SifreSifirlamaKod				NVARCHAR(200),

		KvkkOnayi						BIT NOT NULL,
		UyelikSozlesmeOnayi				BIT NOT NULL,
		AydinlatmaMetniOnayi			BIT NOT NULL,

		CuzdanBakiye					DECIMAL(18,2), --Bu alan değişim olduğunda trigger-procedure ile doldurulur
		MsisdnDogrulama					BIT NOT NULL, --Bu alan masterpass hesabı oluşturulurken uyeye tekrar tekrar sms doğrulaması yapılmaması için. (Kart doğrulama ve ödeme alma)

		FcmRegistrationToken			NVARCHAR(250), -- Firebase notification için device ve üyelik bazında tutulan token. 

		MobileAppState					NVARCHAR(MAX), -- Firebase notification için device ve üyelik bazında tutulan token. 


		CONSTRAINT PK_Uye PRIMARY KEY  (Id),
		CONSTRAINT FK_Uye_UyeDurumId FOREIGN KEY (UyeDurumId) REFERENCES UyeDurum(Id),
		CONSTRAINT FK_Uye_UyeGrupId FOREIGN KEY (UyeGrupId) REFERENCES UyeGrup(Id),
		CONSTRAINT FK_Uye_CinsiyetId FOREIGN KEY (CinsiyetId) REFERENCES Cinsiyet(Id)
    );

	/*Kullanıcı Kara Liste */
	CREATE SEQUENCE dbo.sqUyeKaraListe AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.UyeKaraListe(
		Id								INT NOT NULL,

		UniqueId						UNIQUEIDENTIFIER NOT NULL,
		UyeId							INT NOT NULL,
		BaslangicTarih					DATETIME NOT NULL,
		BitisTarih						DATETIME NOT NULL,
		
		Aciklama						NVARCHAR(300),

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_UyeKaraListe PRIMARY KEY  (Id),
		CONSTRAINT FK_UyeKaraListe_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id)
    );

	/*Mobil Bildirim-Notification */
	CREATE SEQUENCE dbo.sqMobilBildirim AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.MobilBildirim(
		Id								INT NOT NULL,

		UniqueId						UNIQUEIDENTIFIER NOT NULL,
		GonderildiMi					BIT NOT NULL,
		Tarih							DATETIME NOT NULL,
		GonderimTarihi					DATETIME,
		UyeGrupIds						NVARCHAR(MAX),

		Baslik							NVARCHAR(100),
		Mesaj							NVARCHAR(100),
		ResimUrl						NVARCHAR(300),
		Link							NVARCHAR(200),

		CONSTRAINT PK_MobilBildirim PRIMARY KEY  (Id)
    );

	/*Mobil Bildirim-Notification Üyeler */
	CREATE SEQUENCE dbo.sqMobilBildirimUye AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.MobilBildirimUye(
		Id								INT NOT NULL,

		MobilBildirimId					INT NOT NULL,
		UyeId							INT NOT NULL, /*Burada bildirimler tüm kullanıcılar, kullanıcı grubu veya bireysel olarak gönderilebilecek.  */


		CONSTRAINT PK_MobilBildirimUye PRIMARY KEY  (Id),
		CONSTRAINT FK_MobilBildirimUye_MobilBildirimId FOREIGN KEY (MobilBildirimId) REFERENCES MobilBildirim(Id),
		CONSTRAINT FK_MobilBildirimUye_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id),
    );

	/*Sms Bildirim */
	CREATE SEQUENCE dbo.sqSmsBildirim AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.SmsBildirim(
		Id								INT NOT NULL,

		UniqueId						UNIQUEIDENTIFIER NOT NULL,
		GonderildiMi					BIT NOT NULL,
		Tarih							DATETIME NOT NULL,
		GonderimTarihi					DATETIME,
		UyeGrupIds						NVARCHAR(MAX),

		Baslik							NVARCHAR(100),
		Mesaj							NVARCHAR(100),

		CONSTRAINT PK_SmsBildirim PRIMARY KEY  (Id)
    );

	/*SMS Bildirim Üyeler */
	CREATE SEQUENCE dbo.sqSmsBildirimUye AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.SmsBildirimUye(
		Id								INT NOT NULL,

		SmsBildirimId					INT NOT NULL,
		UyeId							INT NOT NULL, /*Burada bildirimler tüm kullanıcılar, kullanıcı grubu veya bireysel olarak gönderilebilecek.  */


		CONSTRAINT PK_SmsBildirimUye PRIMARY KEY  (Id),
		CONSTRAINT FK_SmsBildirimUye_SmsBildirimId FOREIGN KEY (SmsBildirimId) REFERENCES SmsBildirim(Id),
		CONSTRAINT FK_SmsBildirimUye_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id)
    );

	/* Arac Taşıt desekmi acaba */
	CREATE SEQUENCE dbo.sqArac AS INT START WITH 10001 INCREMENT BY 1;
	CREATE TABLE dbo.Arac(
		Id							INT NOT NULL,	
		
		UniqueId					UNIQUEIDENTIFIER NOT NULL,
		Sira						INT NOT NULL,
		Durum						BIT NOT NULL,

		/*Tanım*/
		Ad							NVARCHAR(300),
		Marka						NVARCHAR(50),
		Model						NVARCHAR(50),
		ImeiNo						NVARCHAR(20),
		QrKod						NVARCHAR(20),
		Aciklama					NVARCHAR(MAX),
		Resim						NVARCHAR(300),
		
		/*Son Konum bilgileri*/
		SonKonum					GEOGRAPHY,

		/*KM Sayaç */
		KilometreSayaci				DECIMAL(18,2) NOT NULL,

		/*Pil Şarj Oranı*/
		SarjOrani					DECIMAL(8,2) NOT NULL, 

		/*Araç Şarj Oluyor Mu*/
		SarjOluyorMu				BIT NOT NULL,

		/*Arıza Durumu*/
		ArizaDurumu					BIT NOT NULL,

		/*Kilit Durumu*/
		KilitDurumu					BIT NOT NULL, /* Fiziksel kilit durumudur. Serbest bölge veya Sarj istasyonlarında kilit yapılabilir olacak */

		/*Bloke Durumu*/
		BlokeDurum					BIT NOT NULL,

		/*Bloke Durumu*/
		AcilUyariIstemi				BIT NOT NULL, /* Bunun için alarm tablosu yapılacaktır.*/

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_Arac PRIMARY KEY  (Id)
	);
	INSERT INTO Arac (Id, UniqueId, Sira, Durum, Ad, SonKonum, KilometreSayaci, SarjOrani, SarjOluyorMu, ArizaDurumu, KilitDurumu, BlokeDurum, AcilUyariIstemi) VALUES (Next Value For dbo.sqArac, newid(), 1, 1, 'Velespit bir', 'POINT(39.922985379742435 32.82681762939453)', 0, 0, 0, 0, 0, 0, 0);


	/* Şarj İstasyonları */
	CREATE SEQUENCE dbo.sqSarjIstasyonu AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.SarjIstasyonu(
		Id							INT NOT NULL,	
		
		UniqueId					UNIQUEIDENTIFIER NOT NULL,
		Sira						INT NOT NULL,
		Durum						BIT NOT NULL,

		/*Tanım*/
		Ad							NVARCHAR(300),
		Aciklama					NVARCHAR(MAX),

		/*Şarj için müsaitlik durumu*/
		MusaitlikDurum				BIT NOT NULL, /* Dolu, Boş. Müsaitse boştur harita ve/veya mobil app ekranında göstermek için */
		
		/*Konum Bilgileri*/
		Konum						GEOGRAPHY,

		/*Yazılım Bilgileri*/
		YazilimVersiyon				NVARCHAR(100),
		YazilimVersiyonNo			NVARCHAR(50),
		
		/*Donanım Bilgileri*/
		ModelNo						NVARCHAR(50),
		SeriNo						NVARCHAR(50),

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_SarjIstasyonu PRIMARY KEY  (Id)
	);


	/*Bölge tablosu*/
	CREATE SEQUENCE dbo.sqBolge AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Bolge(
		Id								INT NOT NULL,

		UniqueId						UNIQUEIDENTIFIER NOT NULL,
		Durum							BIT NOT NULL,

		/*Genel ana bölge polygon tanımları tutulur*/ 
		Polygon							GEOGRAPHY, /*Dışı yasaklı bölge ve içi serbest bölgedir*/

		CONSTRAINT PK_Bolge PRIMARY KEY  (Id)
    );
	INSERT INTO Bolge (Id, UniqueId, Durum, Polygon) VALUES (1, newid(), 0, 'POLYGON ((39.98772577274125 32.92844116455078, 39.92140558021372 32.97444641357422, 39.84658671442948 32.725880739746096, 39.98772577274125 32.92844116455078))');
	
	/*Bolge Detay türleri*/
	CREATE TABLE dbo.BolgeDetayTur (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),

		CONSTRAINT PK_BolgeDetayTur PRIMARY KEY (Id)
	);
	INSERT INTO BolgeDetayTur (Id, Ad) VALUES (0, N'Pasif Bölge');
	INSERT INTO BolgeDetayTur (Id, Ad) VALUES (1, N'Yasaklı Bölge');

	/* Harita alanları Güvenli-Pasif */
	CREATE SEQUENCE dbo.sqBolgeDetay AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.BolgeDetay(
		Id							INT NOT NULL,	

		UniqueId					UNIQUEIDENTIFIER NOT NULL,
		Durum						BIT NOT NULL,
		ParkEdilebilirMi			BIT NOT NULL,
		BolgeId						INT NOT NULL,
		BolgeDetayTurId				INT NOT NULL,
		
		Ad							NVARCHAR(300),		

		Polygon						GEOGRAPHY, 

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,	

		CONSTRAINT PK_BolgeDetay PRIMARY KEY  (Id),
		CONSTRAINT FK_BolgeDetay_BolgeId FOREIGN KEY (BolgeId) REFERENCES Bolge(Id),
		CONSTRAINT FK_BolgeDetay_BolgeDetayTurId FOREIGN KEY (BolgeDetayTurId) REFERENCES BolgeDetayTur(Id)
	);


	/*Araç Rezervasyon Hareket Türü*/
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

	/*Araç Rezervasyon*/
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

	/*Arac Hareket*/
	CREATE SEQUENCE dbo.sqAracHareket AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.AracHareket(
		Id						INT NOT NULL,

		UniqueId				UNIQUEIDENTIFIER NOT NULL,
		AracId				INT NOT NULL,
		UyeId					INT NOT NULL,
		AracRezervasyonId		INT NOT NULL,

		BirimFiyat				DECIMAL(18,2) NOT NULL, 
		Mesafe					DECIMAL(18,2) NOT NULL, 
		Tutar					DECIMAL(18,2) NOT NULL, 
					
		BaslangicTarih			DATETIME NOT NULL,
		BitisTarih				DATETIME,	

		BaslangicKonum			GEOGRAPHY, /* Text olarak Point datası tutulacak */
		BitisKonum				GEOGRAPHY, /* Text olarak Point datası tutulacak */

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,


		CONSTRAINT PK_AracHareket PRIMARY KEY  (Id),
		CONSTRAINT FK_AracHareket_AracId FOREIGN KEY (AracId) REFERENCES Arac(Id),
		CONSTRAINT FK_AracHareket_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id)
	);

	/*Arac Hareket Detay*/
	CREATE SEQUENCE dbo.sqAracHareketDetay AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.AracHareketDetay(
		Id						INT NOT NULL,

		AracHareketId		INT NOT NULL,

		Konum					GEOGRAPHY, /* Text olarak Point datası tutulacak */
		Tarih					DATETIME NOT NULL,
		ReportId				INT NOT NULL,

		CreateDate		DATETIME,
		CreatedUserId	INT,
		UpdateDate		DATETIME,
		UpdatedUserId	INT,

		CONSTRAINT PK_AracHareketDetay PRIMARY KEY  (Id),
		CONSTRAINT FK_AracHareketDetay_AracHareketId FOREIGN KEY (AracHareketId) REFERENCES AracHareket(Id)
	);
	CREATE INDEX IX_AracHareketDetay_AracHareketId ON AracHareketDetay (AracHareketId);
	CREATE INDEX IX_AracHareketDetay_Tarih ON AracHareketDetay (Tarih);
	
	/*Arac Hareket (Sürüş) Resim*/
	CREATE SEQUENCE dbo.sqAracHareketResim AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.AracHareketResim(
		Id						INT NOT NULL,

		AracHareketId		INT NOT NULL,
		ResimUrl			NVARCHAR(250),

		CONSTRAINT PK_AracHareketResim PRIMARY KEY  (Id),
		CONSTRAINT FK_AracHareketResim_AracHareketId FOREIGN KEY (AracHareketId) REFERENCES AracHareket(Id)
	);
	CREATE INDEX IX_AracHareketResim_AracHareketId ON AracHareketResim (AracHareketId);

	/*Şarj İstasyonu Arac Hareket (Giriş-Çıkış) Detayı*/
	CREATE SEQUENCE dbo.sqSarjIstasyonuHareket AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.SarjIstasyonuHareket(
		Id						INT NOT NULL,

		UniqueId				UNIQUEIDENTIFIER NOT NULL,
		SarjIstasyonuId			INT NOT NULL,
		AracId				INT NOT NULL,
					
		BaslangicTarih			DATETIME NOT NULL,
		BitisTarih				DATETIME,	

		CreateDate				DATETIME,					

		CONSTRAINT PK_SarjIstasyonuHareket PRIMARY KEY  (Id),
		CONSTRAINT FK_SarjIstasyonuHareket_SarjIstasyonuId FOREIGN KEY (SarjIstasyonuId) REFERENCES SarjIstasyonu(Id),
		CONSTRAINT FK_SarjIstasyonuHareket_AracId FOREIGN KEY (AracId) REFERENCES Arac(Id)
	);


	/*Fiyat Tarife*/
	CREATE SEQUENCE dbo.sqTarife AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Tarife(
		Id							INT NOT NULL,

		UniqueId					UNIQUEIDENTIFIER NOT NULL,
		Durum						BIT NOT NULL,
		BaslangicTarihi				DATE NOT NULL, /* Aktif tarifeyi bulmak için BaslangicTarihi <= Bugün AND Max(BaslangicTarihi) */
		Ad							NVARCHAR(50), /* 2022 yılı tarifesi */

		CONSTRAINT PK_Tarife PRIMARY KEY  (Id)
    );
	INSERT INTO Tarife (Id, UniqueId, Durum, BaslangicTarihi, Ad) VALUES (Next Value For dbo.sqTarife, newid(), 1, getDate(), '2022 Yılı Fiyat Tarifesi');	

	/*Fiyat*/
	CREATE SEQUENCE dbo.sqFiyat AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.Fiyat(
		Id							INT NOT NULL,

		UniqueId					UNIQUEIDENTIFIER NOT NULL,		
		TarifeId					INT NOT NULL,
		UyeGrupId					INT NOT NULL,
		HaftaIciFiyat				DECIMAL(18,2),
		HaftaSonuFiyat				DECIMAL(18,2),
		BayramFiyat					DECIMAL(18,2), /*Bayramların girildiği bir tablo olacak. O tablodaki güne denk gelen ödeme bu fiyattan olacak */

		CONSTRAINT PK_Fiyat PRIMARY KEY  (Id),
		CONSTRAINT FK_Fiyat_UyeGrupId FOREIGN KEY (UyeGrupId) REFERENCES UyeGrup(Id),
		CONSTRAINT FK_Fiyat_TarifeId FOREIGN KEY (TarifeId) REFERENCES Tarife(Id)
    );
	INSERT INTO Fiyat (Id, UniqueId, TarifeId, UyeGrupId, HaftaIciFiyat, HaftaSonuFiyat, BayramFiyat ) VALUES (Next Value For dbo.sqFiyat, newid(), 1, 1, 8,7,6);
	INSERT INTO Fiyat (Id, UniqueId, TarifeId, UyeGrupId, HaftaIciFiyat, HaftaSonuFiyat, BayramFiyat ) VALUES (Next Value For dbo.sqFiyat, newid(), 1, 2, 7,6,5);
	INSERT INTO Fiyat (Id, UniqueId, TarifeId, UyeGrupId, HaftaIciFiyat, HaftaSonuFiyat, BayramFiyat ) VALUES (Next Value For dbo.sqFiyat, newid(), 1, 3, 6,5,4);
	INSERT INTO Fiyat (Id, UniqueId, TarifeId, UyeGrupId, HaftaIciFiyat, HaftaSonuFiyat, BayramFiyat ) VALUES (Next Value For dbo.sqFiyat, newid(), 1, 4, 5,4,3);


	/*Cüzdan Hareket türleri*/
	CREATE TABLE dbo.CuzdanHareketTur (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),

		CONSTRAINT PK_CuzdanHareketTur PRIMARY KEY (Id)
	);
	INSERT INTO CuzdanHareketTur (Id, Ad) VALUES (1, N'Karttan Yükleme'); --Karttan para çekilir cüzdanda borç alanına yazılır
	INSERT INTO CuzdanHareketTur (Id, Ad) VALUES (2, N'Ödeme'); --Cüzdandan ödeme yapılır alacak hanesine yazılır
	INSERT INTO CuzdanHareketTur (Id, Ad) VALUES (3, N'İade'); --Karta iadesi yapılırsa alacak alanına yazılır
	INSERT INTO CuzdanHareketTur (Id, Ad) VALUES (4, N'Cariden İade'); -- Cariden gelen para borç alanına yazılır

	/*Uye Cüzdan Harketleri*/
	CREATE SEQUENCE dbo.sqUyeCuzdanHareket AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.UyeCuzdanHareket(
		Id					INT NOT NULL,

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		UyeId				INT NOT NULL,
		CuzdanHareketTurId	INT NOT NULL, /* 1- Alacak alanına yazılacak, 3- Borç alanına yazılacak */

		Borc				DECIMAL(18,2) NOT NULL, 
		Alacak				DECIMAL(18,2) NOT NULL, 
		Tarih				DATETIME NOT NULL,
		Aciklama			NVARCHAR(MAX),

		CONSTRAINT PK_UyeCuzdanHareket PRIMARY KEY  (Id),
		CONSTRAINT FK_UyeCuzdanHareket_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id),
		CONSTRAINT FK_UyeCuzdanHareket_CuzdanHareketTurId FOREIGN KEY (CuzdanHareketTurId) REFERENCES CuzdanHareketTur(Id)
    );
	--INSERT INTO UyeCuzdanHareket (Id, UniqueId,CuzdanHareketTurId, UyeId, Borc, Alacak,Tarih) VALUES (Next Value For dbo.sqUyeCuzdanHareket, newid(),1, 1,0,0, getDate());	

	/*Cari Hareket türleri*/
	CREATE TABLE dbo.CariHareketTur (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),

		CONSTRAINT PK_CariHareketTur PRIMARY KEY (Id)
	);
	INSERT INTO CariHareketTur (Id, Ad) VALUES (1, N'Kredi/Banka Kartından Ödeme'); -- karttan para çekilir borç alanına yazılır
	INSERT INTO CariHareketTur (Id, Ad) VALUES (2, N'Cüzdandan Ödeme'); -- cüzdandan ödeme borç alanına yazılır
	INSERT INTO CariHareketTur (Id, Ad) VALUES (3, N'Cüzdana İade'); -- cüzdanın alacak alanına yazılır
	INSERT INTO CariHareketTur (Id, Ad) VALUES (4, N'Tahakkuk'); -- Arac hareketten gleen tutarın alacak hanesine yazılmasıdır

	/*Uye Hesap Cari Harketleri*/
	CREATE SEQUENCE dbo.sqUyeCariHareket AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.UyeCariHareket(
		Id					INT NOT NULL,

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		UyeId				INT NOT NULL,
		CariHareketTurId	INT NOT NULL, 

		Borc				DECIMAL(18,2) NOT NULL, 
		Alacak				DECIMAL(18,2) NOT NULL, 
		Tarih				DATETIME NOT NULL,
		Aciklama			NVARCHAR(MAX),

		CONSTRAINT PK_UyeCariHareket PRIMARY KEY  (Id),
		CONSTRAINT FK_UyeCariHareket_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id),
		CONSTRAINT FK_UyeCariHareket_CariHareketTurId FOREIGN KEY (CariHareketTurId) REFERENCES CariHareketTur(Id)
    );
	--INSERT INTO UyeCariHareket (Id, UniqueId,CariHareketTurId, UyeId, Tutar,Tarih) VALUES (Next Value For dbo.sqUyeCariHareket, newid(),1, 1,0, getDate());

	/*AracOzellik :Araç Özelliklerinin Tanımları Yapılır*/
	CREATE SEQUENCE dbo.sqAracOzellik AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.AracOzellik(
		Id					INT NOT NULL,

		UniqueId			UNIQUEIDENTIFIER NOT NULL,
		Durum				BIT NOT NULL,
		Ad					NVARCHAR(50),
		Aciklama			NVARCHAR(200),

		CONSTRAINT PK_AracOzellik PRIMARY KEY  (Id)
    );

	/*AracOzellikDetay :Araç Özelliklerinin ve Araçların Detayı Tanımlanır*/
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

	/*UyeFaturaBilgisi : Üye Faturalarının linki gibi bilgileri tutulur*/
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

	/*Kampanya İndirim Türü*/
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

	/*Kampanya Türü*/
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

	/* Kampanyalar */
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

	/*Kampanya Üyeler */
	CREATE SEQUENCE dbo.sqKampanyaUye AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.KampanyaUye(
		Id								INT NOT NULL,

		KampanyaId						INT NOT NULL,
		UyeId							INT NOT NULL,


		CONSTRAINT PK_KampanyaUye PRIMARY KEY  (Id),
		CONSTRAINT FK_KampanyaUye_KampanyaId FOREIGN KEY (KampanyaId) REFERENCES Kampanya(Id),
		CONSTRAINT FK_KampanyaUye_UyeId FOREIGN KEY (UyeId) REFERENCES Uye(Id),
    );
