
/*audit view için */
IF EXISTS(select * FROM sys.views where name = 'VwAuditLog')
BEGIN
	DROP VIEW VwAuditLog;
END
GO
CREATE VIEW dbo.VwAuditLog
	AS 
	SELECT *
	FROM	solution1_log.dbo.AuditLog;
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
	FROM	solution1_log.dbo.UserLog;
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
	FROM	solution1_log.dbo.SystemLog;
GO
