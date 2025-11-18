--5.1
ALTER VIEW TodaySessions AS
SELECT
    s.SessionId,
    f.Title AS Title,
    h.Cinema,
    h.HallNumber,
    s.Price,
    FORMAT(s.StartTime, 'HH\:mm') AS RentalStart,
    FORMAT(DATEADD(MINUTE, f.Duration, s.StartTime), 'HH\:mm') AS RentalEnd,
    f.Duration
FROM
    Session s
    INNER JOIN Film f ON s.FilmId = f.FilmId
    INNER JOIN Hall h ON s.HallId = h.HallId
WHERE
    CAST(s.StartTime AS DATE) = CAST(GETDATE() AS DATE)
    AND s.StartTime < GETDATE();

--5.2
ALTER VIEW AllSessionsWithSeats AS
SELECT
    s.SessionId,
    f.Title AS Title,
    h.Cinema,
    h.HallNumber,
    s.Price,
    s.StartTime,

    CAST(h.RowsCount AS INT) * CAST(h.SeatsCount AS INT) AS AllRowSeat
FROM
    Session s
    INNER JOIN Film f ON s.FilmId = f.FilmId
    INNER JOIN Hall h ON s.HallId = h.HallId

--5.3
ALTER VIEW MoviesWithGenres AS
SELECT
    f.FilmId,
    f.Title,
    f.ReleaseYear,
    CONCAT_WS(' ', f.Duration / 60, 'ч', f.Duration % 60, 'м') AS Duration,
    STRING_AGG(g.Title, ', ') AS Жанры,
    f.[Description],
    f.RentalStart
FROM
    Film f
    LEFT JOIN FilmGenre mg ON f.FilmId = mg.FilmId
    LEFT JOIN Genre g ON mg.GenerId = g.GenreId 
GROUP BY
    f.FilmId, f.Title, f.ReleaseYear, f.Duration, f.[Description],f.RentalStart;

--5.4
ALTER VIEW ComingSoonMovies AS
SELECT
    *
FROM
    MoviesWithGenres
WHERE
    RentalStart BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE));

----5.5
ALTER VIEW UpdatableFuture3DSessions AS
SELECT
    s.SessionId,
    s.FilmId,
    s.HallId,
    s.StartTime,
    s.Price,
	s.Is3d
FROM
    Session s
    INNER JOIN Hall h ON s.HallId = h.HallId
WHERE
    s.StartTime > GETDATE()
    AND s.Is3d = 1 
WITH CHECK OPTION;

INSERT INTO UpdatableFuture3DSessions (FilmId, HallId, StartTime, Price, Is3d)
VALUES (1, 3, DATEADD(DAY, 1, GETDATE()), 500, 1);

INSERT INTO UpdatableFuture3DSessions (FilmId, HallId, StartTime, Price, Is3d)
VALUES (4, 2, DATEADD(DAY, -1, GETDATE()), 600, 0);

UPDATE UpdatableFuture3DSessions
SET Price = 550
WHERE SessionId = 16;

UPDATE UpdatableFuture3DSessions
SET Is3d = 0
WHERE SessionId = 16;
