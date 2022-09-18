// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using System.IO;
using System;
using System.Text;
using System.Diagnostics;
namespace scl;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var numberOption = new Option<int?>(
            name: "--number",
            description: "The number of calling the api"

            );
        var hostOption = new Option<string?>(
            name: "--host",
            description: "The Host Address"

            );

        var rootCommand = new RootCommand("Sample test client for API");
        rootCommand.AddOption(numberOption);
        rootCommand.AddOption(hostOption);

        rootCommand.SetHandler((number, host) =>
            {
                ExecuteTests(number!, host!);
            },
            numberOption, hostOption);

        return await rootCommand.InvokeAsync(args);
    }

    static void ExecuteTests(int? number = 2, string? host = "127.0.0.1:8083")
    {
        var timer = new Stopwatch();
        var httpClient = new HttpClient();
        Random rnd = new Random();
        for (int i = 0; i < number; i++)
        {
            Console.WriteLine($"---------------------------");
            Console.WriteLine($"----------{i}----------------");
            Console.WriteLine($"---------------------------");


            var length = (int)rnd.NextInt64(20, 60);
            var diffSize = (int)rnd.NextInt64(10, 20);
            var encoded_data = GetRandomPair(length, diffSize);
            var ID = GetRandomString(20);
            Console.WriteLine($"ID = \"{ID}\"");
            timer.Start();
            send_req(httpClient,host, ID, encoded_data.Item1, encoded_data.Item2);
            timer.Stop();

        }
        Console.WriteLine();
        Console.WriteLine($"-----------------------------------------------");
        Console.WriteLine($"---------------------Results-------------------");
        Console.WriteLine($"-----------------------------------------------");
        Console.WriteLine($"took {timer.ElapsedMilliseconds / number} milliseconds per left+right+diff API calls.");
        Console.WriteLine($"{timer.ElapsedMilliseconds} milliseconds total reqs");

    }
    static void send_req(HttpClient httpClient,string? host, string ID, string data = "eyJpbnB1dCI6InRlc3RWYWx1ZSJ9", string data2 = "eyJpbnB1dCI6InRlc3RWYWx1ZSJ9")
    {
Console.WriteLine( $"http://{host}/v1/diff/{ID}/left");
        using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"http://{host}/v1/diff/{ID}/left"))
        {
            request.Headers.TryAddWithoutValidation("Content-Type", "application/custom");
            request.Content = new StringContent(data, Encoding.UTF8, "application/custom");
            var response = httpClient.Send(request);
            //Console.WriteLine($"left result >{response.StatusCode}");
        }
        using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"http://{host}/v1/diff/{ID}/right"))
        {
            request.Headers.TryAddWithoutValidation("Content-Type", "application/custom");
            request.Content = new StringContent(data2, Encoding.UTF8, "application/custom");
            var response = httpClient.Send(request);
            //Console.WriteLine($"right result >{response.StatusCode}");

        }
        Console.WriteLine();
        Console.WriteLine(":::Diff API Result:::");
        var diffResult=httpClient.GetStringAsync($"http://{host}/v1/diff/{ID}").Result;
        Console.WriteLine(diffResult);


    }
    static string GetRandomString(int length)
    {
        Random rnd = new Random();
        List<char> res = new List<char>();
        string refrence = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var refrence_cahrs = refrence.ToCharArray();
        for (int i = 0; i < length; i++)
        {
            var rnd_int = rnd.NextInt64(refrence.Length);
            res.Add(refrence_cahrs[rnd_int]);
        }
        return new string(res.ToArray());
    }
    public static string EncodeBase64(string value)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(valueBytes);
    }
    static (string, string, string) GetRandomPair(int length, int diffSize)
    {
        Random rnd = new Random();
        var st1 = GetRandomString(length).ToCharArray();
        var st2 = new List<char>(st1);
        var st3 =Enumerable.Repeat('-', st1.Length).ToArray(); 
        diffSize = Math.Min(length, diffSize);
        var muted = GetRandomString(diffSize).ToCharArray();

        var muted_positions = Enumerable.Range(0, st1.Length).OrderBy(x => rnd.NextDouble()).Take(diffSize).OrderBy(x => x).ToArray();
        var j = 0;
        foreach (int x in muted_positions)
        {
            st2[x] = muted[j++];
            st3[x]='x';
        }
        var string1 = $"{{\"input\":\"{string.Join("", st1)}\"}}";
        var string2 = $"{{\"input\":\"{string.Join("", st2)}\"}}";
        var string3 = $"{{\"input\":\"{string.Join("", st3)}\"}}";
        Console.WriteLine($"{string1}\n{string2}\n{string3}");

        return (EncodeBase64(string1),
        EncodeBase64(string2),
        string.Join(",", muted_positions));

    }
}

