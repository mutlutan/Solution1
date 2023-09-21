
	/*Database Schema Version */
	CREATE TABLE dbo.Version(
		Id				INT NOT NULL, /*Id => Veritabanı sema versiyonudur*/

		CreateDate		DATETIME,
		Description		NVARCHAR(MAX),  /*işlemin adı*/
		CommandText		NVARCHAR(MAX),  /*işlemin sql komutları*/
		      	
		CONSTRAINT PK_Version PRIMARY KEY (Id)
	);
	INSERT Version (Id, Description, CommandText) VALUES (3, '', '');


	/*SystemLog*/
	CREATE TABLE dbo.SystemLog(
		Id				UNIQUEIDENTIFIER NOT NULL,

		UserId			INT NOT NULL,
		UserType		VARCHAR(50), /*User tablosunun adı olabilir, birden çok usr tablosundan gelebilir*/
		UserName		NVARCHAR(50),
		UserIp			VARCHAR(50),
		UserBrowser		VARCHAR(250), /*Tarayici ve versiyonu*/
		UserSessionGuid	VARCHAR(50), /*Session guid*/

		ProcessTypeId	INT NOT NULL, /*11:Hata,12:Genel,13:Istek,14:Middleware,15:Uyari*/
		ProcessDate		DATETIME,
		ProcessName		NVARCHAR(100),
		ProcessContent	NVARCHAR(MAX), /*log içeriği veya istek-sonuc içeriği*/
		ProcessingTime	TIME,

		/*istek-sonuc*/
		--RequestJson		NVARCHAR(MAX),
		--ResponseJson	NVARCHAR(MAX),
		
		CONSTRAINT PK_SystemLog PRIMARY KEY (Id)
	);
	CREATE INDEX IX_SystemLog_UserId ON SystemLog (UserId);
	CREATE INDEX IX_SystemLog_UserType ON SystemLog (UserType);
	CREATE INDEX IX_SystemLog_UserName ON SystemLog (UserName);
	CREATE INDEX IX_SystemLog_UserIp ON SystemLog (UserIp);
	CREATE INDEX IX_SystemLog_UserBrowser ON SystemLog (UserBrowser);
	CREATE INDEX IX_SystemLog_UserSessionGuid ON SystemLog (UserSessionGuid);

	CREATE INDEX IX_SystemLog_ProcessTypeId ON SystemLog (ProcessTypeId);
	CREATE INDEX IX_SystemLog_ProcessDate ON SystemLog (ProcessDate);
	CREATE INDEX IX_SystemLog_ProcessName ON SystemLog (ProcessName);
		
	/*Audit*/
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
	CREATE INDEX IX_AuditLog_UserId ON AuditLog (UserId);
	CREATE INDEX IX_AuditLog_UserType ON AuditLog (UserType);
	CREATE INDEX IX_AuditLog_UserName ON AuditLog (UserName);
	CREATE INDEX IX_AuditLog_UserIp ON AuditLog (UserIp);
	CREATE INDEX IX_AuditLog_UserBrowser ON AuditLog (UserBrowser);
	CREATE INDEX IX_AuditLog_UserSessionGuid ON AuditLog (UserSessionGuid);
	CREATE INDEX IX_AuditLog_OperationDate ON AuditLog (OperationDate);
	CREATE INDEX IX_AuditLog_TableName ON AuditLog (TableName);
	CREATE INDEX IX_AuditLog_OperationType ON AuditLog (OperationType);
	CREATE INDEX IX_AuditLog_PrimaryKeyField ON AuditLog (PrimaryKeyField);
	CREATE INDEX IX_AuditLog_PrimaryKeyValue ON AuditLog (PrimaryKeyValue);

	/* UserLog - User Oturum Log*/
    CREATE TABLE dbo.UserLog(
		Id				UNIQUEIDENTIFIER NOT NULL,

		TableName		VARCHAR(50), /*User tablosunun adı olabilir, birden çok usr tablosundan gelebilir*/
		UserId			INT NOT NULL,
		
		UserName		NVARCHAR(50),
		UserIp			VARCHAR(50),
		UserBrowser		VARCHAR(250), /*Tarayici ve versiyonu*/
		SessionGuid		VARCHAR(50), /*Session guid*/

		LoginDate		DATETIME,
		LogoutDate		DATETIME,

		ExtraSpace1		NVARCHAR(100), /*ek1*/
		ExtraSpace2		NVARCHAR(100), /*ek2*/
		ExtraSpace3		NVARCHAR(100), /*ek3*/

		CONSTRAINT PK_UserLog PRIMARY KEY (Id)
	);
	CREATE INDEX IX_UserLog_TableName ON UserLog (TableName);
	CREATE INDEX IX_UserLog_UserId ON UserLog (UserId);
	CREATE INDEX IX_UserLog_UserName ON UserLog (UserName);
	CREATE INDEX IX_UserLog_UserIp ON UserLog (UserIp);
	CREATE INDEX IX_UserLog_UserBrowser ON UserLog (UserBrowser);
	CREATE INDEX IX_UserLog_SessionGuid ON UserLog (SessionGuid);