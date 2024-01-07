using DVT.Elevator.Challenge.Domain.Enums;

namespace DVT.Elevator.Challenge.Domain.Models
{
    public class Person
    {
        public string? UserId { get; set; } // Incase of keycard or access to floors
        public MovementEnum Movement { get; set; }
        public required int DesignatedFloor { get; set; }
        public required int CurrentFloor { get; set; }
    }
}
