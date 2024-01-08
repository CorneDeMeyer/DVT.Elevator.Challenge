using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.DomainLogic.Service;
using DVT.Elevator.Challenge.Domain.Models.Base;
using DVT.Elevator.Challenge.Tests.FakeData;
using DVT.Elevator.Challenge.Domain.Enums;

namespace DVT.Elevator.Challenge.Tests.UnitTests
{
    public class ElevatorServiceUnitTests
    {
        private readonly int _nonFunctionalZone = -100;
        private readonly string _nonFunctionalElevatorDesignation = "This elevator should not exists";

        [Fact(DisplayName = "Service Request - Elevator Setup - Success")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task CheckSetup_Succeeded()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevators = await service.GetElevators();

            Assert.NotNull(elevators);
            Assert.IsType<List<BaseElevator>>(elevators);
            Assert.True(elevators.Count == AppConfigurationFakeData.GetElevatorConfig().Length);
        }

        [Fact(DisplayName = "Service Request - Elevator Disable - Success")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Disable_Elevator_Success()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            var requestResult = await service.ElevatorDisableRequest(elevator.LocationZone, elevator.ElevatorDesignation);

            Assert.NotNull(requestResult);
            Assert.Contains(requestResult.Message.ToUpper(), $"{elevator.ElevatorDesignation.ToUpper()} IS ALREADY OUT OF SERVICE");
        }

        [Fact(DisplayName = "Service Request - Elevator Disable - Failure")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Disable_Elevator_Failure()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var requestResult = await service.ElevatorDisableRequest(_nonFunctionalZone, _nonFunctionalElevatorDesignation);

            Assert.NotNull(requestResult);
            Assert.True(requestResult.Message.Contains("ELEVATOR NOT FOUND", StringComparison.OrdinalIgnoreCase));
        }

        [Fact(DisplayName = "Service Request - Elevator Enable - Success")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Enable_Elevator_Success()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            await service.ElevatorDisableRequest(elevator.LocationZone, elevator.ElevatorDesignation);
            var requestResult = await service.ElevatorEnableRequest(elevator.LocationZone, elevator.ElevatorDesignation);

            Assert.NotNull(requestResult);
            Assert.Contains("ELEVATOR IS PLACED BACK INTO SERVICE ON FLOOR", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Service Request - Elevator Enable - Failure")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Enable_Elevator_Failure()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var requestResult = await service.ElevatorEnableRequest(-100, "This elevator should not exists");

            Assert.NotNull(requestResult);
            Assert.True(requestResult.Message.Equals("ELEVATOR NOT FOUND", StringComparison.OrdinalIgnoreCase));
        }

        [Fact(DisplayName = "Service Request - Elevator Request Up - Success")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Request_Up_Success()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            var requestResult = await service.ElevatorRequest(elevator.LocationZone, 1, 5);

            Assert.NotNull(requestResult);
            Assert.Equal(MovementEnum.Up, requestResult.Movement);
            Assert.Contains("IS GOING UP FROM FLOOR", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Service Request - Elevator Request Down - Success")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Request_Down_Success()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            var requestResult = await service.ElevatorRequest(elevator.LocationZone, 5, 1);

            Assert.NotNull(requestResult);
            Assert.Equal(MovementEnum.Down, requestResult.Movement);
            Assert.Contains("IS GOING DOWN TO FLOOR", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Service Request - Elevator Doors Closed - Success")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Doors_Closed_Success()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            await service.ElevatorRequest(elevator.LocationZone, 1, 5);
            var requestResult = await service.ElevatorDoorsClosed(elevator.LocationZone, elevator.ElevatorDesignation, 1);

            Assert.NotNull(requestResult);
            Assert.Contains("ELEVATOR PROCEEDING TO FLOOR", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Service Request - Elevator Doors Closed - Failure (No Elevator Found)")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Doors_Closed_Failure_No_Elevator()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var requestResult = await service.ElevatorDoorsClosed(_nonFunctionalZone, _nonFunctionalElevatorDesignation, 100);

            Assert.NotNull(requestResult);
            Assert.Contains("NO SUCH ELEVATOR FOUND", requestResult.Message.ToUpper());
        }

        [Fact(DisplayName = "Service Request - Elevator Doors Closed - Failure (Overweight)")]
        [Trait("Server", "Unit tests for Elevator calls")]
        public async Task Check_Elevator_Doors_Closed_Failure_Overweight()
        {
            var service = GetService();
            Assert.NotNull(service);

            await service.Setup();
            var elevator = AppConfigurationFakeData.GetElevatorConfig().First();
            await service.ElevatorRequest(elevator.LocationZone, 1, 5);
            var requestResult = await service.ElevatorDoorsClosed(elevator.LocationZone, elevator.ElevatorDesignation, 100000);

            Assert.NotNull(requestResult);
            Assert.Equal("ELEVATOR WEIGHT LIMIT EXCEEDED, PLEASE GET OFF TILL ELEVATOR IS UNDER WEIGHT LIMIT.", requestResult.Message.ToUpper());
        }

        private IElevatorService GetService() => new ElevatorService(AppConfigurationFakeData.GetConfig(new TimeSpan(0, 0, 5)));
    }
}
