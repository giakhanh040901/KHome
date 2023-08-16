using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace EPIC.ClientConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5010/api/real-estate/cms-hub")
                .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}:{message}");
            });
            await connection.StartAsync();

            await connection.InvokeAsync("SendMessage", "user2", "abcd");
        }
    }
}
