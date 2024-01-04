using DVT.Elevator.Challenge.Domain.Models.Base;

namespace DVT.Elevator.Challenge.Domain.Models.Config
{
    public class AppConfiguration
    {
        public int NumberOfFloors { get; private set; }
        public int NumberOfPeople { get; private set; }
        public TimeSpan RefreshTime { get; private set; }

        public BaseElevatorConfig[] ElevatorConfig { get; set; }

        public AppConfiguration(TimeSpan refreshTime, int numberOfFloors, int? NumberOfPeople, BaseElevatorConfig[] elevatorConfig)
        {
            this.NumberOfPeople = NumberOfPeople.HasValue && NumberOfPeople.Value > 0 ? NumberOfPeople.Value : new Random().Next(100);
            this.ElevatorConfig = elevatorConfig;
            this.NumberOfFloors = numberOfFloors;
            this.RefreshTime = refreshTime;
        }
    }
}
