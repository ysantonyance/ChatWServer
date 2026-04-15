using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    private const int DEFAULT_BUFLEN = 512;
    private const string DEFAULT_PORT = "27015";

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "CLIENT SIDE";

        ShowWelcomeMessage();

        try
        {
            var ipAddress = IPAddress.Loopback;
            var remoteEndPoint = new IPEndPoint(ipAddress, int.Parse(DEFAULT_PORT));

            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(remoteEndPoint);
            Console.WriteLine("\nConnected to server successfully!");
            Console.WriteLine("Type your messages (type 'goodbye' or 'bye' to exit)\n");
            Console.WriteLine(new string('-', 50));

            while (true)
            {
                Console.Write("You: ");
                string message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine("Please enter a message.\n");
                    continue;
                }

                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                clientSocket.Send(messageBytes);

                var buffer = new byte[DEFAULT_BUFLEN];
                int bytesReceived = clientSocket.Receive(buffer);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                Console.WriteLine($"Server: {response}\n");

                if (message.Trim().ToLower() == "goodbye" || message.Trim().ToLower() == "bye")
                {
                    Console.WriteLine("Closing connection...");
                    break;
                }
            }

            Thread.Sleep(500);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Console.WriteLine("Connection closed. Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Make sure the server is running first!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void ShowWelcomeMessage()
    {
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("WELCOME TO TCP CHAT CLIENT");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("\nAvailable commands you can send to server:");
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("• hello / hi - Get a greeting");
        Console.WriteLine("• how are you / how are things - Ask how I'm doing");
        Console.WriteLine("• what time is it / time - Get current time");
        Console.WriteLine("• what day is it / day - Get current day");
        Console.WriteLine("• date - Get current date");
        Console.WriteLine("• weather - Get weather info");
        Console.WriteLine("• goodbye / bye - Exit the program");
        Console.WriteLine("• help / commands - Show this menu again");
    }
}