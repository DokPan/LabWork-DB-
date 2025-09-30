using LabWork7;
using System.Data;

Console.WriteLine("Строка подключения к БД:");
Console.WriteLine(DataAccessLayer.ConnectionString);
Console.WriteLine();

//// 5.1.2 
//DataAccessLayer.SetConnectionSettings(server: @"mssql", database: "ispp34", login: "mssql3407", password: "3407");
//Console.WriteLine("После настройки:");
//Console.WriteLine(DataAccessLayer.ConnectionString);
//Console.WriteLine();

// 5.1.3 
bool canConnect = await DataAccessLayer.CanConnectAsync();
Console.WriteLine($"Возможность подключения: {canConnect}");
Console.WriteLine();

//// 5.2.1 
try
{
    string createSql = @"
    IF OBJECT_ID('dbo.TestTemp', 'U') IS NOT NULL DROP TABLE dbo.TestTemp;
    CREATE TABLE dbo.TestTemp (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(100));
    INSERT INTO dbo.TestTemp (Name) VALUES (N'Alpha'), (N'Beta');";

    int rows = await DataAccessLayer.ExecuteNonQueryAsync(createSql);
    Console.WriteLine($"Количество изменённх строк: {rows}");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
Console.WriteLine();

//// 5.2.2 
//try
//{
//    string scalarSql = "SELECT COUNT(*) FROM dbo.TestTemp";
//    var scalar = await DataAccessLayer.ExecuteScalarAsync(scalarSql);
//    Console.WriteLine($"Результат SQL запроса: {scalar}");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Ошибка: {ex.Message}");
//}
//Console.WriteLine();

//// 5.3.1 
//try
//{
//    int sessionId = 1;
//    decimal newPrice = 320.00m;
//    int updated = await DataAccessLayer.UpdateTicketPriceAsync(sessionId, newPrice);
//    Console.WriteLine($"Обновление строки: {updated}");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Ошибка: {ex.Message}");
//}
//Console.WriteLine();

// 5.4.2 
//try
//{
//    int filmIdToUpload = 1;
//    string localFile = @"C:\Temp\ISPP-34\Coco.jpg";
//    int up = await DataAccessLayer.UploadPosterAsync(filmIdToUpload, localFile);
//    Console.WriteLine($"Обновление строки: {up}");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Ошибка: {ex.Message}");
//}
//Console.WriteLine();

// 5.4.3 
//try
//{
//    int filmIdToSave = 1;
//    string outFile = $@"C:\Temp\ISPP-34\FilmId{filmIdToSave}1111.jpg";
//    bool saved = await DataAccessLayer.SavePosterToFileAsync(filmIdToSave, outFile);
//    Console.WriteLine($"Расположение сохранённого файла: {saved} (to {outFile})");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Ошибка: {ex.Message}");
//}
//Console.WriteLine();

// 5.5.1 
//try
//{
//    DataTable upcoming = await DataAccessLayer.GetUpcomingFilmsAsync();
//    Console.WriteLine($"Количество фильмов в скором прокате: {upcoming.Rows.Count}");
//    foreach (DataRow row in upcoming.Rows)
//    {
//        Console.WriteLine($"FilmId={row["FilmId"]}, Title={row["Title"]}, RentalStart={row["RentalStart"]}");
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Ошибка: {ex.Message}");
//}
