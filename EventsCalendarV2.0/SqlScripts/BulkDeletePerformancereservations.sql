USE [EventsCalendar]
GO

/****** Object:  StoredProcedure [dbo].[BulkDeletePerformanceReservations]    Script Date: 3/7/2019 10:56:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Adam Gardner
-- Create date: 3/4/19
-- Description:	Delete all reservations for performance
-- =============================================
CREATE PROCEDURE [dbo].[BulkDeletePerformanceReservations]
	@performanceId int = 0
AS
BEGIN
	WHILE 1=1
	BEGIN
		DELETE TOP(100) FROM dbo.Reservations WHERE PerformanceId = @performanceId
		IF @@ROWCOUNT = 0 
			BREAK
		ELSE 
			CONTINUE
END
END
GO


