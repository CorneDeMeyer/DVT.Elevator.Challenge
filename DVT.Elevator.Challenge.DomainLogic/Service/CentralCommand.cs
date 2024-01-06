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

        public Task Start()
        {
            throw new NotImplementedException();
        }

        private static void ConsoleWriter()
        {
            while (true)
            {
                lock (consoleInfo)
                {
                    Console.Clear();
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
                                consoleInfo.outputBuffer = [];
                                break;                                    
                            case "?":
                                consoleInfo.lastResult.AppendLine("Available commands are:");
                                consoleInfo.lastResult.AppendLine("ls       List All Elevators and Details");
                                consoleInfo.lastResult.AppendLine("disable  Disable Elevators");
                                consoleInfo.lastResult.AppendLine("enable   Enable Elevators");
                                consoleInfo.lastResult.AppendLine("cls      Clear");
                                break;
                            default:
                                consoleInfo.lastResult.Append("invalid command, type ? to see command list");
                                break;
                        }
                    }
                    Console.WriteLine(consoleInfo.lastCommand);
                    Console.WriteLine(consoleInfo.lastResult);
                    Console.WriteLine();
                    Console.Write("Command>");
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
