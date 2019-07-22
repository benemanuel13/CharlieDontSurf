USE [CharlieDontSurf]
GO

/****** Object: SqlProcedure [dbo].[SaveOrder] Script Date: 03/06/2018 17:12:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveOrder]
	@id int,
	@userId nchar(128),
	@recipient nchar(50),
	@addressLine1 nchar(50),
	@addressLine2 nchar(50),
	@city nchar(30),
	@county nchar(30),
	@postcode nchar(15),
	@country nchar(30)
AS
	INSERT into [Orders]
	(Id, UserId, Recipient, AddressLine1, AddressLine2,
	City, County, Postcode, Country) values
	(@id, @userId, @recipient, @addressLine1, @addressLine2,
	@city, @county, @postcode, @country)
RETURN 0
