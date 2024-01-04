using DVT.Elevator.Challenge.Domain.Enums;

namespace DVT.Elevator.Challenge.Domain.Models.Base
{
    public class BaseElevator
    {
        public required int ZoneLocated { get; set; }
        public required string ElevatorDesignation { get; set; }
        public int CurrentLevel { get; set; }
        public int MaxLevel { get; set; }
        public MovementEnum Movement { get; set; }
        public required float WeightCapacity { get; set; }
        public required int PersonCapacity { get; set; }
        public List<Person>? PeopleInLift { get; set; }

        public void ElevatorStatus()
        {
            Console.WriteLine($"Elevator: {ElevatorDesignation}");
            Console.WriteLine($"Currently On Level: {CurrentLevel} out of {MaxLevel}");
            Console.WriteLine($"People currently in Lift: {PeopleInLift?.Count}");
            Console.WriteLine($"Movement Status: ", Movement.GetMovement(PeopleInLift?.Count));
            Console.WriteLine("===============================================================");
        }
    }
}
