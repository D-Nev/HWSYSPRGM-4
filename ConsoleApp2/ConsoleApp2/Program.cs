namespace ConsoleApp2
{
    internal class Program
    {
        static async Task Main()
        {
            List<string> urls = new List<string>
        {
            "https://www.example.com",
            "https://www.microsoft.com",
            "https://www.github.com",
            "https://store.steampowered.com/?l=eng"
        };

            using CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            cts.CancelAfter(TimeSpan.FromSeconds(10));
            Task cancelTask = Task.Run(() =>
            {
                Console.ReadKey();
                cts.Cancel();
            });

            try
            {
                List<Task> downloadTasks = new List<Task>();
                using HttpClient client = new HttpClient();

                foreach (var url in urls)
                {
                    downloadTasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            Console.WriteLine($"Загрузка: {url}");
                            string content = await client.GetStringAsync(url, token);
                            Console.WriteLine($"Успешно загружено: {url} ({content.Length} символов)");
                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine($"Загрузка отменена: {url}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при загрузке {url}: {ex.Message}");
                        }
                    }));
                }
                await Task.WhenAll(downloadTasks);
                Console.WriteLine("Загрузка завершена.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция была отменена пользователем или по тайм-ауту.");
            }
        }
    }
}
