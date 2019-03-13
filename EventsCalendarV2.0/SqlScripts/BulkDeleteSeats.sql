USE [EventsCalendar]
GO

/****** Object:  StoredProcedure [dbo].[BulkDeleteSeats]    Script Date: 3/12/2019 10:15:18 PM ******/
DROP PROCEDURE [dbo].[BulkDeleteSeats]
GO

/****** Object:  StoredProcedure [dbo].[BulkDeleteSeats]    Script Date: 3/12/2019 10:15:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkDeleteSeats] 
	@numberOfSeats int = 0,
	@seatTypeId int = 0,
	@venueId int = 0
AS
BEGIN
	WHILE (1=1)
	BEGIN
		DELETE TOP(@numberOfSeats) FROM dbo.Seats 
			WHERE VenueId = @venueId
			AND SeatTypeId = @seatTypeId
		BREAK
END
END
GO


