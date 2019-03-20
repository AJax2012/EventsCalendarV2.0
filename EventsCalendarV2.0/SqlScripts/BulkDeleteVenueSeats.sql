USE [EventCalendarDataContext]
GO

/****** Object:  StoredProcedure [dbo].[BulkDeleteVenueSeats]    Script Date: 3/7/2019 10:58:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkDeleteVenueSeats]
	@venueId int = 0
AS
BEGIN
	WHILE 1=1
	BEGIN
		DELETE TOP(100) FROM dbo.Seats WHERE VenueId = @venueId
		IF @@ROWCOUNT = 0 
			BREAK
		ELSE 
			CONTINUE
END
END
GO

