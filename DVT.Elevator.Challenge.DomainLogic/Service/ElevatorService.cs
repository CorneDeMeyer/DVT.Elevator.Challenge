using DVT.Elevator.Challenge.Domain;
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

        public async Task Setup()
        {
            await SetupElevators();
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
                        CurrentLevel = rand.Next(_config.NumberOfFloors),
                        PeopleInLift = new List<Person>(),
                        ZoneLocated = elevator.LocationZone,
                        Movement = Domain.Enums.MovementEnum.Stationery,
                        Enabled = elevator.IsEnabled
                    });
                }
            }
            return Task.CompletedTask;
        }

        public Task CheckElevators()
        {
            foreach (var elevator in _elevators.Where(x => x.Movement != Domain.Enums.MovementEnum.Stationery))
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
                    CentralCommand.consoleInfo.lastResult.AppendLine($"Currently On Level: {elevator.CurrentLevel} out of {elevator.MaxLevel}");
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

        public void MoveElevator(BaseElevator elevatorOnTheMove)
        {
            int? peopleGettingOff = 0;
            switch (elevatorOnTheMove.Movement)
            {
                case Domain.Enums.MovementEnum.Down:
                    elevatorOnTheMove.CurrentLevel -= 1;
                    if (elevatorOnTheMove.CurrentLevel == 0)
                    {
                        elevatorOnTheMove.Movement = Domain.Enums.MovementEnum.Stationery;
                    }
                    peopleGettingOff = elevatorOnTheMove?.PeopleInLift?.RemoveAll(p => p.DesignatedFloor == elevatorOnTheMove.CurrentLevel);
                    break;
                case Domain.Enums.MovementEnum.Up:
                    elevatorOnTheMove.CurrentLevel -= 1;
                    if (elevatorOnTheMove.CurrentLevel == elevatorOnTheMove.MaxLevel)
                    {
                        elevatorOnTheMove.Movement = Domain.Enums.MovementEnum.Stationery;
                    }
                    peopleGettingOff = elevatorOnTheMove?.PeopleInLift?.RemoveAll(p => p.DesignatedFloor == elevatorOnTheMove.CurrentLevel);
                    break;
                default:
                    break;
            }
            CentralCommand.consoleInfo.outputBuffer.Add($"{peopleGettingOff} people are getting off on Floor {elevatorOnTheMove?.CurrentLevel}, elevator is " +
                        $"{elevatorOnTheMove?.Movement.GetMovement(elevatorOnTheMove?.PeopleInLift?.Count)} with ${elevatorOnTheMove?.PeopleInLift?.Count} still on");
        }

        public Task<ElevatorReponse> ElevatorRequest(int zone, int floor)
        {
            var respone = new ElevatorReponse
            { 
                Floor = floor,
                Zone = zone,
                Message = "No Elevator Available",
                Movement = Domain.Enums.MovementEnum.Stationery
            };

            if (_elevators != null && _elevators.Any(elevator => elevator.Enabled))
            {
                if (_elevators.Any(elevator => elevator?.PeopleInLift?.Count < elevator?.PersonCapacity))
                {

                }
                else
                { }
            }

            return Task.FromResult(respone);
        }

        public Task<ElevatorReponse> ElevatorDisableRequest(int zone, string designation)
        {
            throw new NotImplementedException();
        }

        public Task<ElevatorReponse> ElevatorEnableRequest(int zone, string designation)
        {
            throw new NotImplementedException();
        }

        public Task<ElevatorReponse> ElevatorDoorsClosed(decimal weight)
        {
            throw new NotImplementedException();
        }
    }
}
