CREATE DATABASE [mojoportal]
GO

USE [mojoportal]
GO
exec sp_dboption N'mojoPortal', N'autoclose', N'false'
GO

exec sp_dboption N'mojoPortal', N'bulkcopy', N'false'
GO

exec sp_dboption N'mojoPortal', N'trunc. log', N'false'
GO

exec sp_dboption N'mojoPortal', N'torn page detection', N'true'
GO

exec sp_dboption N'mojoPortal', N'read only', N'false'
GO

exec sp_dboption N'mojoPortal', N'dbo use', N'false'
GO

exec sp_dboption N'mojoPortal', N'single', N'false'
GO

exec sp_dboption N'mojoPortal', N'autoshrink', N'false'
GO

exec sp_dboption N'mojoPortal', N'ANSI null default', N'false'
GO

exec sp_dboption N'mojoPortal', N'recursive triggers', N'false'
GO

exec sp_dboption N'mojoPortal', N'ANSI nulls', N'false'
GO

exec sp_dboption N'mojoPortal', N'concat null yields null', N'false'
GO

exec sp_dboption N'mojoPortal', N'cursor close on commit', N'false'
GO

exec sp_dboption N'mojoPortal', N'default to local cursor', N'false'
GO

exec sp_dboption N'mojoPortal', N'quoted identifier', N'false'
GO

exec sp_dboption N'mojoPortal', N'ANSI warnings', N'false'
GO

exec sp_dboption N'mojoPortal', N'auto create statistics', N'true'
GO

exec sp_dboption N'mojoPortal', N'auto update statistics', N'true'
GO

if( ( (@@microsoftversion / power(2, 24) = 8) and (@@microsoftversion & 0xffff >= 724) ) or ( (@@microsoftversion / power(2, 24) = 7) and (@@microsoftversion & 0xffff >= 1082) ) )
	exec sp_dboption N'mojoPortal', N'db chaining', N'false'
GO

