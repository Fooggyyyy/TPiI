using NUnit.Framework.Internal;


Test test = new Test();
Console.WriteLine("-- /A");

await test.ExecuteGET<int?>("http://localhost:5293/A/3", (int? x, int? y, int status) =>
    (x == 3 && y == null && status == 200) ? Test.OK : Test.NOK);

await test.ExecuteGET<int?>("http://localhost:5293/A/-3", (int? x, int? y, int status) =>
    (x == -3 && y == null && status == 200) ? Test.OK : Test.NOK);

await test.ExecuteGET<int?>("http://localhost:5293/A/118", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecutePOST<int?>("http://localhost:5293/A/5", (int? x, int? y, int status) =>
    (x == 5 && y == null && status == 200) ? Test.OK : Test.NOK);

await test.ExecutePOST<int?>("http://localhost:5293/A/-5", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecutePOST<int?>("http://localhost:5293/A/118", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecutePUT<int?>("http://localhost:5293/A/2/3", (int? x, int? y, int status) =>
    (x == 2 && y == 3 && status == 200) ? Test.OK : Test.NOK);

await test.ExecutePUT<int?>("http://localhost:5293/A/0/3", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecutePUT<int?>("http://localhost:5293/A/25/-3", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecutePUT<int?>("http://localhost:5293/A/0/-3", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecuteDELETE<int?>("http://localhost:5293/A/1-99", (int? x, int? y, int status) =>
    (x == 1 && y == 99 && status == 200) ? Test.OK : Test.NOK);

await test.ExecuteDELETE<int?>("http://localhost:5293/A/99-1", (int? x, int? y, int status) =>
    (x == 99 && y == 1 && status == 200) ? Test.OK : Test.NOK);

await test.ExecuteDELETE<int?>("http://localhost:5293/A/-1-25", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecuteDELETE<int?>("http://localhost:5293/A/-1--25", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecuteDELETE<int?>("http://localhost:5293/A/25-101", (int? x, int? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
Console.WriteLine("-- /B");
await test.ExecuteGET<float?>("http://localhost:5293/B/2.5", (float? x, float? y, int status) =>
    (x == 2.5 && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteGET<float?>("http://localhost:5293/B/2", (float? x, float? y, int status) =>
    (x == 2.0 && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteGET<float?>("http://localhost:5293/B/2X", (float? x, float? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecutePOST<float?>("http://localhost:5293/B/2.5/3.2", (float? x, float? y, int status) =>
    (x == 2.5 && y == 3.2 && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteDELETE<float?>("http://localhost:5293/B/2.5-3.2", (float? x, float? y, int status) =>
    (x == 2.5 && y == 3.2 && status == 200) ? Test.OK : Test.NOK);

Console.WriteLine("-- /C");
await test.ExecuteGET<bool?>("http://localhost:5293/C/2.5", (bool? x, bool? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecuteGET<bool?>("http://localhost:5293/C/true", (bool? x, bool? y, int status) =>
    (x == true && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecutePOST<bool?>("http://localhost:5293/C/true,false", (bool? x, bool? y, int status) =>
    (x == true && y == false && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteDELETE<bool?>("http://localhost:5293/C/true,false", (bool? x, bool? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

Console.WriteLine("-- /D");
await test.ExecuteGET<DateTime?>("http://localhost:5293/D/2025-02-25", (DateTime? x, DateTime? y, int status) =>
    (x == new DateTime(2025,02,25) && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteGET<DateTime?>("http://localhost:5293/D/2025-02-29", (DateTime? x, DateTime? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecuteGET<DateTime?>("http://localhost:5293/D/2024-02-29", (DateTime? x, DateTime? y, int status) =>
    (x == new DateTime(2024, 02, 29) && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteGET<DateTime?>("http://localhost:5293/D/2025-02-25T19:25", (DateTime? x, DateTime? y, int status) =>
    (x == new DateTime(2025, 02, 25,19,25,0) && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecutePOST<DateTime?>("http://localhost:5293/D/2025-02-25|2025-03-25", (DateTime? x, DateTime? y, int status) =>
    (x == new DateTime(2025, 02, 25) && y == new DateTime(2025, 03, 25) && status == 200) ? Test.OK : Test.NOK);
await test.ExecutePUT<DateTime?>("http://localhost:5293/D/2025-02-25T19:25", (DateTime? x, DateTime? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);


Console.WriteLine("-- /E");

await test.ExecuteGET<string?>("http://localhost:5293/E/12-bis", (string? x, string? y, int status) =>
    (x == "bis" && y == null && status == 200) ? Test.OK : Test.NOK);

await test.ExecuteGET<string?>("http://localhost:5293/E/11-bis", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecuteGET<string?>("http://localhost:5293/E/12-777", (string? x, string? y, int status) =>
    (x == "777" && y == null && status == 200) ? Test.OK : Test.NOK);

await test.ExecuteGET<string?>("http://localhost:5293/E/12-", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

await test.ExecutePUT<string?>("http://localhost:5293/E/abcd", (string? x, string? y, int status) =>
    (x == "abcd" && y == null && status == 200) ? Test.OK : Test.NOK);

await test.ExecutePUT<string?>("http://localhost:5293/E/abcd123", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecutePUT<string?>("http://localhost:5293/E/a", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecutePUT<string?>("http://localhost:5293/E/123456", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecutePUT<string?>("http://localhost:5293/E/aabbccddeeffgghh", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);

Console.WriteLine("-- /F");

await test.ExecuteGET<string?>("http://localhost:5293/F/smw@belstu.by", (string? x, string? y, int status) =>
    (x == "smw@belstu.by" && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteGET<string?>("http://localhost:5293/F/xxx@yyyy.by", (string? x, string? y, int status) =>
    (x == "xxx@yyyy.by" && y == null && status == 200) ? Test.OK : Test.NOK);
await test.ExecuteGET<string?>("http://localhost:5293/F/xxx@yyyy.ru", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecuteGET<string?>("http://localhost:5293/F/xxxyyyy.by", (string? x, string? y, int status) =>
    (x ==null && y == null && status == 404) ? Test.OK : Test.NOK);
await test.ExecuteGET<string?>("http://localhost:5293/F/xxx@yyyy", (string? x, string? y, int status) =>
    (x == null && y == null && status == 404) ? Test.OK : Test.NOK);
