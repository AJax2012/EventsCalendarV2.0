USE [EventsCalendar]
GO

/****** Object:  StoredProcedure [dbo].[BulkDeleteSeats]    Script Date: 3/7/2019 10:57:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkDeleteSeats] 
	@numberOfSeats int = 0,
	@seatType int,
	@venueId int = 0
AS
BEGIN
	WHILE (1=1)
	BEGIN
		DELETE TOP(@numberOfSeats) FROM dbo.Seats 
			WHERE VenueId = @venueId
			AND SeatType = @seatType
		BREAK
END
END
GO

