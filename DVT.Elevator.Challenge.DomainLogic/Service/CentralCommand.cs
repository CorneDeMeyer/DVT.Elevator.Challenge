using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class CentralCommand: ICentralCommand
    {
        public static ConsoleInfo consoleInfo = new ConsoleInfo();
        private Thread _consoleWriter = new Thread(new ThreadStart(ConsoleWriter));
        private static IElevatorService? _elevatorService; 

        public CentralCommand(IElevatorService elevatorService)
        { 
            _elevatorService = elevatorService;
            _elevatorService.Setup();
        }

        public void Start()
        {
            _consoleWriter.Start();
            consoleInfo.outputBuffer.Add("Running.");
            _elevatorService?.DisplayElevatorPosition().Wait();
            while (true)
            {
                var key = Console.ReadKey(true);
                lock (consoleInfo)
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        consoleInfo.commandReaty = true;
                    }
                    else
                    {
                        consoleInfo.sbRead.Append(key.KeyChar.ToString());
                    }
                }
            }
        }

        private static void ConsoleWriter()
        {
            while (true)
            {
                lock (consoleInfo)
                {
                    Console.Clear();
                    if (consoleInfo.lastResult.Length > 5000)
                    {
                        consoleInfo.lastResult.Clear();
                        if (!string.IsNullOrEmpty(consoleInfo.lastCommand))
                        {
                            consoleInfo.lastCommand = string.Empty;
                        }
                    }
                    if (consoleInfo.outputBuffer[0].Length > 20)
                    {
                        consoleInfo.outputBuffer[0] = "Currently Running.";
                    }
                    else
                    {
                        consoleInfo.outputBuffer[0] += ".";
                    }
                    foreach (var item in consoleInfo.outputBuffer)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("--------------------------------------------------------------");
                    if (consoleInfo.commandReaty)
                    {
                        consoleInfo.commandReaty = false;
                        consoleInfo.lastCommand = consoleInfo.sbRead.ToString();
                        consoleInfo.sbRead.Clear();
                        consoleInfo.lastResult.Clear();
                        switch (consoleInfo.lastCommand)
                        {
                            case "ls":
                                _elevatorService?.DisplayElevatorPosition().Wait();
                                break;
                            case "disable":
                                consoleInfo.lastResult.Append("Elevator Disabled");
                                break;
                            case "enable":
                                consoleInfo.lastResult.Append("Elevator Enabled");
                                break;
                            case "cls":
                                consoleInfo.outputBuffer.Clear();
                                consoleInfo.outputBuffer.Add("Running.");
                                consoleInfo.lastResult.Clear();
                                break;                                    
                            case "?":
                                consoleInfo.lastResult.AppendLine("Available commands are:");
                                consoleInfo.lastResult.AppendLine("==========================================");
                                consoleInfo.lastResult.AppendLine("ls       - List All Elevators and Details");
                                consoleInfo.lastResult.AppendLine("disable  - Disable Elevators");
                                consoleInfo.lastResult.AppendLine("enable   - Enable Elevators");
                                consoleInfo.lastResult.AppendLine("cls      - Clear Console");
                                consoleInfo.lastResult.AppendLine("==========================================");
                                consoleInfo.lastResult.AppendLine("Note! Logs limit to 5000 characters");
                                break;
                            default:
                                consoleInfo.lastResult.Append("invalid command, type ? to see command list");
                                break;
                        }
                    }
                    Console.WriteLine(consoleInfo.lastCommand);
                    Console.WriteLine(consoleInfo.lastResult);
                    Console.WriteLine();
                    Console.Write(">");
                    Console.WriteLine(consoleInfo.sbRead.ToString());
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
                Thread.Sleep(1000);
            }
        }
    }
}
