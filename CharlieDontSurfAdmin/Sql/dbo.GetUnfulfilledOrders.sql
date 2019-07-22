USE [CharlieDontSurf]
GO

/****** Object: SqlProcedure [dbo].[GetUnfulfilledOrders] Script Date: 03/06/2018 17:12:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUnfulfilledOrders]
AS
	SELECT * From [Orders] Where Fulfilled = 0
RETURN 0
