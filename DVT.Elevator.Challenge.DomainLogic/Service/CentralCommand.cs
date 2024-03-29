﻿using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class CentralCommand: ICentralCommand
    {
        private static bool running = true;
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
            // Keeping Console Ready and waiting for
            while (running)
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

            if (!running) 
            { 
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Console Writing commands and request results
        /// </summary>
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
                        if (!ContrainsCustomCommands(consoleInfo.lastCommand))
                        {
                            switch (consoleInfo.lastCommand)
                            {
                                case "ls":
                                case "list":
                                    _elevatorService?.DisplayElevatorPosition().Wait();
                                    break;
                                case "cls":
                                case "clr":
                                case "clear":
                                    consoleInfo.outputBuffer.Clear();
                                    consoleInfo.outputBuffer.Add("Running.");
                                    consoleInfo.lastResult.Clear();
                                    break;
                                case "close":
                                case "exit":
                                case "stop":
                                    running = false;
                                    break;
                                case "?":
                                    consoleInfo.lastResult.AppendLine("Available commands are:");
                                    consoleInfo.lastResult.AppendLine("=======================================================");
                                    consoleInfo.lastResult.AppendLine("ls | list                          - List All Elevators and Details");
                                    consoleInfo.lastResult.AppendLine("d | disable [zone] [designation]   - Disable Elevators");
                                    consoleInfo.lastResult.AppendLine("e | enable  [zone] [designation]   - Enable Elevators");
                                    consoleInfo.lastResult.AppendLine("clr| cls | clear                   - Clear Console");
                                    consoleInfo.lastResult.AppendLine("=======================================================");
                                    consoleInfo.lastResult.AppendLine("Note! Logs limit to 5000 characters");
                                    break;
                                default:
                                    consoleInfo.lastResult.Append("invalid command, type ? to see command list");
                                    break;
                            }
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

        private static bool ContrainsCustomCommands(string command)
        {
            var commandParts = command.Split(' ');
            if (commandParts.Length > 2)
            {
                var zone = int.TryParse(commandParts[1], out var zoneInt);
                var designation = commandParts[2];
                if (command.Contains("enable", StringComparison.OrdinalIgnoreCase) || command.StartsWith('e'))
                {
                    consoleInfo.lastResult.Append($"Attempting to Enable Elevator: {designation}");
                    var enableResponse = _elevatorService?.ElevatorEnableRequest(zoneInt, designation);
                    enableResponse?.Wait();
                    consoleInfo.lastResult.Append($"Elevator {designation} Message: {enableResponse?.Result.Message}");
                }
                else if (command.Contains("disable", StringComparison.OrdinalIgnoreCase) || command.StartsWith('d'))
                {
                    consoleInfo.lastResult.Append($"Attempting to Disable Elevator: {designation}");
                    var enableResponse = _elevatorService?.ElevatorDisableRequest(zoneInt, designation);
                    enableResponse?.Wait();
                    consoleInfo.lastResult.Append($"Elevator {designation} Message: {enableResponse?.Result.Message}");
                }
                else
                {
                    consoleInfo.lastResult.Append($"{command} is not recognized");
                    consoleInfo.lastResult.Append("invalid command, type ? to see command list");
                }
            }
            return false;
        }
    }
}
