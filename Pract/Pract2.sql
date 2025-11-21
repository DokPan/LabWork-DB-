--5.1
CREATE FUNCTION dbo.GetVisitorPoints(@VisitorID INT)
RETURNS INT
AS
BEGIN
    DECLARE @TotalMinutes INT;

    SELECT @TotalMinutes = ISNULL(SUM(f.Duration), 0)
    FROM Ticket t
    INNER JOIN Session s ON t.SessionID = s.SessionID
    INNER JOIN Film f ON s.FilmID = f.FilmID
    WHERE t.VisitorID = @VisitorID;

    RETURN @TotalMinutes;
END;

SELECT
    VisitorID,
    [Name],
    dbo.GetVisitorPoints(VisitorID) AS AccumulatedMinutes
FROM Visitor;

--5.2
ALTER FUNCTION dbo.GetFilmsByGenre(@GenreName NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    SELECT
        f.FilmID,
        f.Title AS FilmTitle,
        STRING_AGG(g2.Title, ', ') WITHIN GROUP (ORDER BY g2.Title) AS GenresList
    FROM Film f
    INNER JOIN FilmGenre fg ON f.FilmID = fg.FilmID
    INNER JOIN Genre g ON fg.GenerId = g.GenreID
    INNER JOIN FilmGenre fg2 ON fg2.FilmID = f.FilmID
    INNER JOIN Genre g2 ON fg2.GenerId = g2.GenreID
    WHERE g.Title = @GenreName
    GROUP BY f.FilmID, f.Title
);

SELECT * FROM dbo.GetFilmsByGenre('Романтика');

--5.3
ALTER PROCEDURE dbo.AddNewTicket
    @PhoneNumber NVARCHAR(20),
    @SessionId INT,
    @Row INT,
    @Seat INT,
    @OutTicketId INT OUTPUT
AS
BEGIN
	INSERT INTO dbo.Ticket (VisitorId, SessionId, [Row], [Seat])
    SELECT v.VisitorId, @SessionId, @Row, @Seat
    FROM dbo.Visitor v
    WHERE v.Phone = @PhoneNumber;

    SET @OutTicketId = CAST(SCOPE_IDENTITY() AS INT);
END;

DECLARE @NewTicketId INT;

EXEC dbo.AddNewTicket
    @PhoneNumber = '9762913',
    @SessionId = 1,
    @Row = 8,
    @Seat = 8,
    @OutTicketId = @NewTicketId OUTPUT;

PRINT 'Билет успешно создан! Номер билета: ' + CAST(@NewTicketId AS NVARCHAR(10));

--5.4
ALTER PROCEDURE dbo.UpsertCinemaHall
    @Cinema NVARCHAR(100),
    @HallNumber INT,
    @RowsCount INT,
    @SeatsCount INT
AS
BEGIN
    UPDATE Hall
    SET RowsCount = @RowsCount,
        SeatsCount = @SeatsCount
    WHERE Cinema = @Cinema
      AND HallNumber = @HallNumber;

    IF @@ROWCOUNT = 0
    BEGIN
        INSERT INTO Hall (Cinema, HallNumber, RowsCount, SeatsCount)
        VALUES (@Cinema, @HallNumber, @RowsCount, @SeatsCount);
    END
END;

EXEC dbo.UpsertCinemaHall
    @Cinema  = 'Титан-Арена',
    @HallNumber = 2,
    @RowsCount =15,
    @SeatsCount = 20;


--5.5
ALTER FUNCTION dbo.GetTodayFilmsByTheater(@TheaterName NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    SELECT
        f.FilmID,
        f.Title,
        s.StartTime,
        h.HallNumber
    FROM Session s
    INNER JOIN Film f ON s.FilmID = f.FilmID
    INNER JOIN Hall h ON s.HallId= h.HallId AND h.Cinema = @TheaterName
    WHERE CAST(s.StartTime AS DATE) = CAST(GETDATE() AS DATE)
);

SELECT * FROM dbo.GetTodayFilmsByTheater('Русь');
