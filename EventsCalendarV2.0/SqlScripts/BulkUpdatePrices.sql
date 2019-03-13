USE [EventsCalendar]
GO

/****** Object:  StoredProcedure [dbo].[BulkUpdatePrices]    Script Date: 3/13/2019 11:07:23 AM ******/
DROP PROCEDURE [dbo].[BulkUpdatePrices]
GO

/****** Object:  StoredProcedure [dbo].[BulkUpdatePrices]    Script Date: 3/13/2019 11:07:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[BulkUpdatePrices]
	@price decimal = 0.00,
	@seatTypeId int,
	@performanceId int = 0
AS
BEGIN
	UPDATE dbo.Reservations
		SET Price = @price
		FROM Reservations r
		JOIN Seats s
			ON r.SeatId = s.Id
		WHERE SeatTypeId = @seatTypeId
		AND r.PerformanceId = @performanceId
END
GO


