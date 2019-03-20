USE [EventCalendarDataContext]
GO

/****** Object:  StoredProcedure [dbo].[BulkDeleteReservations]    Script Date: 3/7/2019 10:57:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Adam Gardner
-- Create date: 3/4/19
-- Description:	Delete a given amount of reservations
-- =============================================

CREATE PROCEDURE [dbo].[BulkDeleteReservations] 
	@numberOfReservations int =0,
	@price int,
	@performanceId int = 0
AS
BEGIN
	WHILE (1=1)
	BEGIN
		DELETE TOP(@numberOfReservations) FROM dbo.Reservations 
			WHERE PerformanceId = @performanceId
			AND Price = @price
			AND IsTaken = 0
		BREAK
END
END
GO

