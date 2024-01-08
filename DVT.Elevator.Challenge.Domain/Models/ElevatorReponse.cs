using DVT.Elevator.Challenge.Domain.Enums;

namespace DVT.Elevator.Challenge.Domain.Models
{
    public class ElevatorReponse
    {
        public MovementEnum Movement { get; set; }
        public ElevatorRequestAcceptanceEnum ElevatorRequestAccepted { get; set; }
        public required int Zone { get; set; }
        public required int Floor { get; set; }  
        public required string Message { get; set; }
    }
}
