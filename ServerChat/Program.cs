using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private const int DEFAULT_BUFLEN = 512;
    private const string DEFAULT_PORT = "27015";
    private const int PAUSE = 1000;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "SERVER SIDE";
        Console.WriteLine("=== SERVER STARTED ===");
        Thread.Sleep(PAUSE);

        try
        {
            var ipAddress = IPAddress.Any;
            var localEndPoint = new IPEndPoint(ipAddress, int.Parse(DEFAULT_PORT));

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);

            Console.WriteLine("Server bound to address and port!");
            Thread.Sleep(PAUSE);

            listener.Listen(10);
            Console.WriteLine("Waiting for client connection...");

            var clientSocket = listener.Accept();
            Console.WriteLine("Client connected!");

            while (true)
            {
                var buffer = new byte[DEFAULT_BUFLEN];
                int bytesReceived = clientSocket.Receive(buffer);

                if (bytesReceived > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    Console.WriteLine($"\nClient: {message}");

                    string response = ProcessCommand(message);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);
                    Console.WriteLine($"Server: {response}");
                }
                else if (bytesReceived == 0)
                {
                    Console.WriteLine("Client disconnected.");
                    break;
                }
                else
                {
                    Console.WriteLine("Error receiving data.");
                    break;
                }
            }

            clientSocket.Shutdown(SocketShutdown.Send);
            clientSocket.Close();
            Console.WriteLine("Server shutting down.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string ProcessCommand(string command)
    {
        command = command.Trim().ToLower();

        switch (command)
        {
            case "hello":
            case "hi":
                return "Hello! Nice to see you!";
            case "how are you":
            case "how are things":
                return "Great! How about you?";
            case "what time is it":
            case "time":
                return DateTime.Now.ToString("HH:mm:ss");
            case "what day is it":
            case "day":
                return DateTime.Now.DayOfWeek.ToString();
            case "date":
                return DateTime.Now.ToString("dd.MM.yyyy");
            case "weather":
                return "Beautiful sunny weather today!";
            case "help":
            case "commands":
                return ShowAvailableCommands();
            case "goodbye":
            case "bye":
                return "Goodbye! Nice talking to you!";
            default:
                return $"Unknown command: '{command}'. Type 'help' or 'commands' for available commands.";
        }
    }

    static string ShowAvailableCommands()
    {
        return "\n=== AVAILABLE COMMANDS ===\n" +
               "• hello / hi - Greeting\n" +
               "• how are you / how are things - Ask how I'm doing\n" +
               "• what time is it / time - Show current time\n" +
               "• what day is it / day - Show current day\n" +
               "• date - Show current date\n" +
               "• weather - Get weather info\n" +
               "• goodbye / bye - End conversation\n" +
               "• help / commands - Show this menu\n" +
               "===========================\n";
    }
}