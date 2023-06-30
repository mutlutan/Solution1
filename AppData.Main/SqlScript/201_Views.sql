
/*audit view için */
IF EXISTS(select * FROM sys.views where name = 'VwAuditLog')
BEGIN
	DROP VIEW VwAuditLog;
END
GO

CREATE VIEW dbo.VwAuditLog
	AS 
	SELECT *
	FROM	smart_bike_log.dbo.AuditLog;
GO

/*userlog view için */
IF EXISTS(select * FROM sys.views where name = 'VwUserLog')
BEGIN
	DROP VIEW VwUserLog;
END
GO

CREATE VIEW dbo.VwUserLog
	AS 
	SELECT *
	FROM	smart_bike_log.dbo.UserLog;
GO

/*SystemLog view için */
IF EXISTS(select * FROM sys.views where name = 'VwSystemLog')
BEGIN
	DROP VIEW VwSystemLog;
END
GO

CREATE VIEW dbo.VwSystemLog
	AS 
	SELECT *
	FROM	smart_bike_log.dbo.SystemLog;
GO

/*SmsLog view için */
IF EXISTS(select * FROM sys.views where name = 'VwSmsLog')
BEGIN
	DROP VIEW VwSmsLog;
END
GO

CREATE VIEW dbo.VwSmsLog
	AS 
	SELECT *
	FROM	smart_bike_log.dbo.SmsLog;
GO


/*MobilBildirimLog view için */
IF EXISTS(select * FROM sys.views where name = 'VwMobilBildirimLog')
BEGIN
	DROP VIEW VwMobilBildirimLog;
END
GO

CREATE VIEW dbo.VwMobilBildirimLog
	AS 
	SELECT *
	FROM	smart_bike_log.dbo.MobilBildirimLog;
GO


/*AracStatuLog view için */
IF EXISTS(select * FROM sys.views where name = 'VwAracStatuLog')
BEGIN
	DROP VIEW VwAracStatuLog;
END
GO

CREATE VIEW dbo.VwAracStatuLog
	AS 
	SELECT *
	FROM	smart_bike_log.dbo.AracStatuLog;
GO