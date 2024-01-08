using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.Domain.Models.Base;

namespace DVT.Elevator.Challenge.Tests.FakeData
{
    internal class AppConfigurationFakeData
    {
        public static BaseElevatorConfig[] GetElevatorConfig() =>
        [
            new BaseElevatorConfig
            {
                ElevatorDesignation = "Test 1",
                LocationZone = 1,
                MaxLevelReached = 5,
                PersonCapacity = 5,
                WeightCapacity = 80,
                IsEnabled = true
            },
            new BaseElevatorConfig
            {
                ElevatorDesignation = "Test 2",
                LocationZone = 1,
                MaxLevelReached = 5,
                PersonCapacity = 5,
                WeightCapacity = 80,
                IsEnabled = true
            },
            new BaseElevatorConfig
            {
                ElevatorDesignation = "Test 3",
                LocationZone = 1,
                MaxLevelReached = 5,
                PersonCapacity = 5,
                WeightCapacity = 80,
                IsEnabled = false
            }
        ];

        public static AppConfiguration GetConfig(TimeSpan elevatorFloorChange, int floors = 5, int? people = 10) => new AppConfiguration(elevatorFloorChange, floors, people, GetElevatorConfig());
    }
}
