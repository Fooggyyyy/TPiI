using System.Net.Http.Json;
using System.Text.Json;

class Test
{
    class Answer<T>
    {
        public T? x { get; set; } = default(T?);
        public T? y { get; set; } = default(T?);
        public string? message { get; set; } = null;
    }

    public static string OK = "OK";
    public static string NOK = "NOK";

    HttpClient client = new HttpClient();

    public async Task ExecuteGET<T>(string path, Func<T?, T?, int, string> result)
    {
        await ResultPrint<T>("GET", path, await client.GetAsync(path), result);
    }

    public async Task ExecutePOST<T>(string path, Func<T?, T?, int, string> result)
    {
        await ResultPrint<T>("POST", path, await client.PostAsync(path, null), result);
    }

    public async Task ExecutePUT<T>(string path, Func<T?, T?, int, string> result)
    {
        await ResultPrint<T>("PUT", path, await client.PutAsync(path, null), result);
    }

    public async Task ExecuteDELETE<T>(string path, Func<T?, T?, int, string> result)
    {
        await ResultPrint<T>("DELETE", path, await client.DeleteAsync(path), result);
    }

    private async Task ResultPrint<T>(string method, string path, HttpResponseMessage response, Func<T?, T?, int, string> result)
    {
        int status = (int)response.StatusCode;
        try
        {
            Answer<T>? answer = await response.Content.ReadFromJsonAsync<Answer<T>>() ?? default(Answer<T>);
            string r = result(default(T), default(T), status);
            T? x = default(T);
            T? y = default(T);


            if (answer != null)
            {
                r = result(answer.x, answer.y, status);
                x = answer.x;
                y = answer.y;
            }

            Console.WriteLine($"[{r}]: {method} {path}, status = {status}, x = {x}, y = {y}, message = {answer?.message}");
        }
        catch (JsonException ex)
        {
            string r = result(default(T), default(T), status);
            Console.WriteLine($"[{r}]: {method} {path}, status = {status}, x = {null}, y = {null}, message = {ex.Message}");
        }
    }
}