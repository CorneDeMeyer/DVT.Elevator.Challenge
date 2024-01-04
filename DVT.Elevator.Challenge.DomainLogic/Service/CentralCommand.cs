using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.Domain.Models.Base;
using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.Domain;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class CentralCommand : ICentralCommand
    {
        private readonly AppConfiguration _config;

        private List<BaseElevator> _elevators;

        public CentralCommand(AppConfiguration config)
        {
            _config = config;
            _elevators = [];
        }

        // Setup Elevators and People to use elevators
        public async Task Setup()
        {
            await SetupElevators();
            await DisplayElevatorPosition();
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
                        Movement = Domain.Enums.MovementEnum.Stationery
                    });
                }
            }
            return Task.CompletedTask;
        }

        public async Task MoveElevator(BaseElevator elevatorOnTheMove)
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
            await Console.Out.WriteLineAsync($"{peopleGettingOff} people are getting off on Floor {elevatorOnTheMove?.CurrentLevel}, elevator is " +
                        $"{elevatorOnTheMove?.Movement.GetMovement(elevatorOnTheMove?.PeopleInLift?.Count)} with ${elevatorOnTheMove?.PeopleInLift?.Count} still on");
        }

        public async Task PersonRequest(Person person)
        {

            await Console.Out.WriteLineAsync();
        }

        public Task DisplayElevatorPosition()
        {
            foreach (var elevator in _elevators) 
            {
                elevator.ElevatorStatus();
            }
            return Task.CompletedTask;
        }

        public async Task CheckElevators()
        {
            foreach (var elevator in _elevators.Where(x => x.Movement != Domain.Enums.MovementEnum.Stationery))
            {
                await MoveElevator(elevator);
            }
        }
    }
}
