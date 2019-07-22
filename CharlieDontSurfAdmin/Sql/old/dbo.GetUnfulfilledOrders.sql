CREATE PROCEDURE [dbo].[GetUnfulfilledOrders]
AS
	SELECT * From [Orders] Where Fulfilled = 0
RETURN 0
