using DVT.Elevator.Challenge.Domain.Enums;
using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.DomainLogic.Service;
using DVT.Elevator.Challenge.Tests.FakeData;

namespace DVT.Elevator.Challenge.Tests.UnitTests
{
    public class ElevatorClientUnitTests
    {
        private readonly int _nonFunctionalZone = -100;
        private readonly string _nonFunctionalElevatorDesignation = "This elevator should not exists";

        [Fact(DisplayName = "Elevator Request Up - Success")]
        [Trait("Client", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Request_Up_Success()
        {
            var service = GetClient();
            Assert.NotNull(service);

            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            var requestResult = await service.RequestElevator(elevator.LocationZone, 1, 5);

            Assert.NotNull(requestResult);
            Assert.Equal(MovementEnum.Up, requestResult.Movement);
            Assert.Contains("IS GOING UP FROM FLOOR", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Elevator Request Down - Success")]
        [Trait("Client", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Request_Down_Success()
        {
            var service = GetClient();
            Assert.NotNull(service);

            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            var requestResult = await service.RequestElevator(elevator.LocationZone, 5, 1);

            Assert.NotNull(requestResult);
            Assert.Equal(MovementEnum.Down, requestResult.Movement);
            Assert.Contains("IS GOING DOWN TO FLOOR", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Elevator Status - Success")]
        [Trait("Client", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Status_Success()
        {
            var service = GetClient();
            Assert.NotNull(service);

            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            var requestResult = await service.RequestElevatorStatus(elevator.LocationZone, elevator.ElevatorDesignation);

            Assert.NotNull(requestResult);
            Assert.NotEqual(MovementEnum.Unknown, requestResult.Movement);
        }

        [Fact(DisplayName = "Elevator Status - Failure (invalid elevator)")]
        [Trait("Client", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Status_Failure_Invalid()
        {
            var service = GetClient();
            Assert.NotNull(service);

            var requestResult = await service.RequestElevatorStatus(_nonFunctionalZone, _nonFunctionalElevatorDesignation);

            Assert.NotNull(requestResult);
            Assert.Equal(MovementEnum.Unknown, requestResult.Movement);
            Assert.Equal($"No Elevator {_nonFunctionalElevatorDesignation} Available in Zone {_nonFunctionalZone}", requestResult.Message);
        }

        [Fact(DisplayName = "Elevator Status - Failure (Elevator Under Maintenance)")]
        [Trait("Client", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Status_Failure_Elevator_Under_Maintenance()
        {
            var service = GetClient();
            Assert.NotNull(service);

            var elevator = AppConfigurationFakeData.GetElevatorConfig().FirstOrDefault(elevator => !elevator.IsEnabled);
            Assert.NotNull(elevator);
            var requestResult = await service.RequestElevatorStatus(elevator.LocationZone, elevator.ElevatorDesignation);

            Assert.NotNull(requestResult);
            Assert.Equal(MovementEnum.Unknown, requestResult.Movement);
            Assert.Equal($"Elevator {elevator.ElevatorDesignation} is currently under maintenance!", requestResult.Message);
        }

        private IElevatorService GetService() => new ElevatorService(AppConfigurationFakeData.GetConfig(new TimeSpan(0, 0, 5)));

        private IElevatorClient GetClient()
        {
            var service = GetService();
            service.Setup();
            return new ElevatorClient(service);
        }
    }
}
