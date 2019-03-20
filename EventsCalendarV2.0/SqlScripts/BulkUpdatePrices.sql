USE [EventCalendarDataContext]
GO

/****** Object:  StoredProcedure [dbo].[BulkUpdatePrices]    Script Date: 3/7/2019 10:58:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkUpdatePrices]
	@price decimal = 0.00,
	@seatType int,
	@performanceId int = 0
AS
BEGIN
	UPDATE dbo.Reservations
		SET Price = @price
		FROM Reservations r
		JOIN Seats s
			ON r.SeatId = s.Id
		WHERE SeatType = @seatType
		AND r.PerformanceId = @performanceId
END
GO

