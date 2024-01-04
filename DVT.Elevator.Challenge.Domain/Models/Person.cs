using DVT.Elevator.Challenge.Domain.Enums;

namespace DVT.Elevator.Challenge.Domain.Models
{
    public class Person
    {
        public required string Name { get; set; } // Incase of keycard or access to floors
        public required decimal Weight { get; set; }
        public required MovementEnum Movement { get; set; }
        public int DesignatedFloor { get; set; }
    }
}
