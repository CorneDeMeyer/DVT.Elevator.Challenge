using DVT.Elevator.Challenge.Domain;
using DVT.Elevator.Challenge.Domain.Enums;
using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.Domain.Models.Base;
using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.DomainLogic.Interface;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class ElevatorService(AppConfiguration config) : IElevatorService
    {
        private readonly AppConfiguration _config = config;
        private List<BaseElevator> _elevators = [];
        private Queue<Person> _queue = [];

        public async Task Setup()
        {
            await SetupElevators();
        }

        public Task<List<BaseElevator>> GetElevators() => Task.FromResult(_elevators);

        public Task CheckElevators()
        {
            foreach (var elevator in _elevators.Where(x => x.Enabled && 
                                                           x.Movement != Domain.Enums.MovementEnum.Stationery && 
                                                           x.WeightCapacity <= x.CurrentWeight))
            {
                MoveElevator(elevator);
            }
            return Task.CompletedTask;
        }

        public Task DisplayElevatorPosition()
        {
            foreach (var elevator in _elevators)
            {
                CentralCommand.consoleInfo.lastResult.AppendLine($"Elevator: {elevator.ElevatorDesignation}");
                CentralCommand.consoleInfo.lastResult.AppendLine($"Zone: {elevator.ZoneLocated.ToString()}");
                if (elevator.Enabled)
                {
                    CentralCommand.consoleInfo.lastResult.AppendLine($"Currently On Level: {elevator.CurrentFloor} out of {elevator.MaxLevel}");
                    CentralCommand.consoleInfo.lastResult.AppendLine($"People currently in Lift: {elevator.PeopleInLift?.Count}");
                    CentralCommand.consoleInfo.lastResult.AppendLine($"Movement Status: {elevator.Movement.GetMovement(elevator.PeopleInLift?.Count)}");
                }
                else
                {
                    CentralCommand.consoleInfo.lastResult.AppendLine("Status: Not Available");
                }
                CentralCommand.consoleInfo.lastResult.AppendLine("===============================================================");
            }
            return Task.CompletedTask;
        }

        public Task<ElevatorReponse> ElevatorRequest(int zone, int currentFloor, int requestFloor, string? userId = null)
        {
            var respone = new ElevatorReponse
            { 
                Floor = requestFloor,
                Zone = zone,
                Message = $"No Elevator Available in Zone {zone}",
                Movement = Domain.Enums.MovementEnum.Stationery
            };

            if (_elevators != null && _elevators.Any(elevator => elevator.Enabled && elevator.ZoneLocated == zone))
            {
                if (_elevators.Any(elevator => elevator?.PeopleInLift?.Count < elevator?.PersonCapacity && elevator?.CurrentWeight < elevator?.WeightCapacity))
                {
                    if (currentFloor > requestFloor)
                    {
                        // Elevator Request - Going Down
                        var elevatorOfChoice = _elevators.Where(elevator => elevator.Enabled &&
                                                                        elevator.ZoneLocated == zone &&
                                                                        elevator.PeopleInLift?.Count < elevator.PersonCapacity &&
                                                                        elevator.WeightCapacity < elevator.CurrentWeight &&
                                                                        (elevator.Movement == MovementEnum.Down ||
                                                                         elevator.Movement == MovementEnum.Stationery))
                                                     .OrderByDescending(order => Math.Abs(requestFloor - order.CurrentFloor)) // Find Closest Number to Requested Floor
                                                     .OrderBy(order2 => order2?.CurrentWeight) // Less Crouded List 1st
                                                     .FirstOrDefault();
                        respone.Movement = MovementEnum.Down;
                        respone.Message = $"Elevator {elevatorOfChoice?.ElevatorDesignation} is Going down to Floor {elevatorOfChoice?.CurrentFloor}";
                        elevatorOfChoice?.Requests?.Add(new ElevatorRequest(requestFloor, currentFloor, userId));
                    }
                    else
                    {
                        // Elevator Request - Going Up
                        var elevatorOfChoice = _elevators.Where(elevator => elevator.Enabled &&
                                                                        elevator.ZoneLocated == zone &&
                                                                        elevator.PeopleInLift?.Count < elevator.PersonCapacity &&
                                                                        elevator.CurrentWeight < elevator.WeightCapacity &&
                                                                        (elevator.Movement == MovementEnum.Up ||
                                                                         elevator.Movement == MovementEnum.Stationery))
                                                     .OrderBy(order => Math.Abs(requestFloor - order.CurrentFloor)) // Find Closest Number to Requested Floor
                                                     .OrderBy(order2 => order2?.CurrentWeight) // Less Crouded List 1st
                                                     .FirstOrDefault();
                        respone.Movement = MovementEnum.Up;
                        respone.Message = $"Elevator {elevatorOfChoice?.ElevatorDesignation} is Going up from Floor {elevatorOfChoice?.CurrentFloor}";
                        elevatorOfChoice?.Requests?.Add(new ElevatorRequest(requestFloor, currentFloor, userId));
                    }
                }
                else
                {
                    respone.Message = "All Elevators at capacity, please wait a moment";
                }
            }

            return Task.FromResult(respone);
        }

        public Task<ElevatorReponse> ElevatorDisableRequest(int zone, string designation)
        {
            var response = new ElevatorReponse
            { 
                Floor = 0,
                Zone = zone,
                Message = "Elevator not found"
            };

            var elevator = _elevators.FirstOrDefault(elevator => elevator.ZoneLocated == zone && elevator.ElevatorDesignation.Equals(designation, StringComparison.CurrentCulture));
            if (elevator != null)
            {
                elevator.Enabled = false;
                response.Floor = elevator.CurrentFloor;
                response.Zone = zone;
                if (!elevator.Enabled)
                {
                    response.Message = $"{elevator.ElevatorDesignation} is already out of service";
                }
                else
                {
                    response.Movement = MovementEnum.Stationery;
                    response.Message = $"{elevator.ElevatorDesignation} Elevator has been stopped on floor {elevator.CurrentFloor}";
                }
            }

            return Task.FromResult(response);
        }

        public Task<ElevatorReponse> ElevatorEnableRequest(int zone, string designation)
        {
            var response = new ElevatorReponse
            {
                Floor = 0,
                Zone = zone,
                Message = "Elevator not found"
            };

            var elevator = _elevators.FirstOrDefault(elevator => elevator.ZoneLocated == zone && elevator.ElevatorDesignation.Equals(designation, StringComparison.CurrentCulture));
            if (elevator != null)
            {
                response.Floor = elevator.CurrentFloor;
                response.Zone = zone;

                if (elevator.Enabled)
                {
                    response.Message = $"{elevator.ElevatorDesignation} is already in service";
                }
                else
                {
                    elevator.Enabled = true;
                    response.Message = $"{elevator.ElevatorDesignation} Elevator is placed back into service on floor {elevator.CurrentFloor}";
                }
            }

            return Task.FromResult(response);
        }

        public Task<ElevatorReponse> ElevatorDoorsClosed(int zone, string designation, float weight)
        {
            var elevator = _elevators.FirstOrDefault(elevator => elevator.ElevatorDesignation.Equals(designation, StringComparison.OrdinalIgnoreCase) &&
                                                                 elevator.ZoneLocated == zone);

            if (elevator == null)
                return Task.FromResult(
                    new ElevatorReponse {
                        Floor = 0,
                        Zone = zone,
                        Message = "No Such Elevator Found.",
                        ElevatorRequestAccepted = ElevatorRequestAcceptanceEnum.Declinced
                    });

            var response = new ElevatorReponse { 
                Floor = elevator.CurrentFloor,
                Zone = zone,   
                Message = "Elevator Weight Limit Exceeded, please get off till elevator is under weight limit.",
                ElevatorRequestAccepted = ElevatorRequestAcceptanceEnum.Declinced
            };

            elevator.CurrentWeight = weight;
            if (elevator.CurrentWeight <= elevator.WeightCapacity)
            {
                var goingToFloor = DetermineGoingToFloor(elevator.Movement, elevator.Requests);
                response.ElevatorRequestAccepted = ElevatorRequestAcceptanceEnum.Accepted;
                response.Message = $"Elevator Proceeding to Floor {goingToFloor.ToString()}";
                var removedRequests = elevator?.Requests?.RemoveAll(req => req.RequestedFloor == goingToFloor);
            }

            return Task.FromResult(response);
        }

        private int DetermineGoingToFloor(MovementEnum movement, List<ElevatorRequest>? requests)
        {
            if (movement == MovementEnum.Down)
            {
                return requests?.OrderByDescending(x => x.RequestedFloor)?.FirstOrDefault()?.RequestedFloor ?? 0;
            }
            else if (movement == MovementEnum.Up)
            {
                return requests?.OrderBy(x => x.RequestedFloor)?.FirstOrDefault()?.RequestedFloor ?? 0;
            }

            return 0;
        }

        private Task SetupElevators()
        {
            var rand = new Random();
            if (_config.ElevatorConfig.Length > 0)
            {
                foreach (var elevator in _config.ElevatorConfig)
                {
                    _elevators.Add(new ElevatorModel
                    {
                        ElevatorDesignation = elevator.ElevatorDesignation,
                        WeightCapacity = elevator.WeightCapacity,
                        PersonCapacity = elevator.PersonCapacity,
                        MaxLevel = elevator.MaxLevelReached,
                        CurrentFloor = rand.Next(_config.NumberOfFloors),
                        CurrentWeight = 0.00f,
                        PeopleInLift = [],
                        Requests = [],
                        ZoneLocated = elevator.LocationZone,
                        Movement = Domain.Enums.MovementEnum.Stationery,
                        Enabled = elevator.IsEnabled
                    });
                }
            }
            return Task.CompletedTask;
        }

        private void MoveElevator(BaseElevator elevatorOnTheMove)
        {
            int? peopleGettingOnOff = 0;
            switch (elevatorOnTheMove.Movement)
            {
                case Domain.Enums.MovementEnum.Down:
                    elevatorOnTheMove.CurrentFloor -= 1;
                    if (elevatorOnTheMove.CurrentFloor == 0)
                    {
                        elevatorOnTheMove.Movement = Domain.Enums.MovementEnum.Stationery;
                    }
                    peopleGettingOnOff = elevatorOnTheMove?.PeopleInLift?.RemoveAll(p => p.DesignatedFloor == elevatorOnTheMove.CurrentFloor);
                    break;
                case Domain.Enums.MovementEnum.Up:
                    elevatorOnTheMove.CurrentFloor += 1;
                    if (elevatorOnTheMove.CurrentFloor == elevatorOnTheMove.MaxLevel)
                    {
                        elevatorOnTheMove.Movement = Domain.Enums.MovementEnum.Stationery;
                    }
                    peopleGettingOnOff = elevatorOnTheMove?.PeopleInLift?.RemoveAll(p => p.DesignatedFloor == elevatorOnTheMove.CurrentFloor);
                    break;
                default:
                    break;
            }
            CentralCommand.consoleInfo.outputBuffer.Add($"{peopleGettingOnOff} people are getting off on Floor {elevatorOnTheMove?.CurrentFloor}, {Environment.NewLine}elevator is " +
                        $"{elevatorOnTheMove?.Movement.GetMovement(elevatorOnTheMove?.PeopleInLift?.Count)} with ${elevatorOnTheMove?.PeopleInLift?.Count} still in.");
        }
    }
}
