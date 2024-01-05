namespace DVT.Elevator.Challenge.Domain.Models.Base
{
    public class BaseElevatorConfig
    {
        public required string ElevatorDesignation { get; set; }
        public required int LocationZone { get; set; }
        public required float WeightCapacity { get; set; }
        public required int PersonCapacity { get; set; }
        public required int MaxLevelReached { get; set;}
    }
}
