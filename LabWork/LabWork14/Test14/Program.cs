using DatabaseLibrary.Models;

using var httpClient = new HttpClient();
var filmService = new FilmServiceAuto(httpClient);

//GET
Console.WriteLine("1. Получение всех фильмов:");
var films = await filmService.GetFilmsAsync();
Console.WriteLine($"Найдено фильмов: {films.Count}");

//POST 
Console.WriteLine("\n2. Создание нового фильма:");
var newFilm = new Film { Title = "Смешарики" };
var createdFilm = await filmService.CreateFilmAsync(newFilm);
Console.WriteLine($"Создан фильм: {createdFilm.Title} (ID: {createdFilm.FilmId})");

//GET ID
Console.WriteLine("\n3. Получение фильма по ID:");
var filmById = await filmService.GetFilmsByIdAsync(3);
if (filmById != null)
    Console.WriteLine($"Найден фильм: {filmById.Title}");
else
    Console.WriteLine("Фильм не найден");

//PUT
Console.WriteLine("\n4. Обновление фильма:");
createdFilm.Title = "Смешарики";
var updateResult = await filmService.UpdateFilmAsync(createdFilm.FilmId, createdFilm);

////DELETE
//Console.WriteLine("\n5. Удаление фильма:");
//var deleteResult = await filmService.DeleteFilmAsync(createdFilm.FilmId);
//if (deleteResult)
//    Console.WriteLine("Фильм успешно удален");
//else
//    Console.WriteLine("Фильм для удаления не найден");

//using var httpClient = new HttpClient();
//var filmService = new FilmServiceStatusHandling(httpClient);

////GET
//Console.WriteLine("1. Получение всех фильмов:");
//var films = await filmService.GetFilmsWithStatusHandlingAsync();
//Console.WriteLine($"Найдено фильмов: {films.Count}");

////GET ID
//Console.WriteLine("\n3. Получение фильма по ID:");
//var filmById = await filmService.GetFilmByIdAsync(3);
//if (filmById != null)
//    Console.WriteLine($"Найден фильм: {filmById.Title}");
//else
//    Console.WriteLine("Фильм не найден");

Console.ReadLine();