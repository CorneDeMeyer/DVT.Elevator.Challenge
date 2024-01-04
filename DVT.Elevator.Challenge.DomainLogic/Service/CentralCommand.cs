using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.Domain.Models.Base;
using DVT.Elevator.Challenge.Domain.Models;

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
                    _elevators.Add(new FreightElevator
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

        public Task MoveElevator(BaseElevator elevator)
        {
            throw new NotImplementedException();
        }

        public Task PersonRequest(Person person)
        {
            throw new NotImplementedException();
        }

        public Task DisplayElevatorPosition()
        {
            foreach (var elevator in _elevators) 
            {
                elevator.ElevatorStatus();
            }
            Console.WriteLine(
            "=============================================");
            return Task.CompletedTask;
        }
    }
}
