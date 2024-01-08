namespace DVT.Elevator.Challenge.Domain.Models
{
    public class ElevatorRequest
    {
        public int RequestedFloor { get; set; }
        public DateTime RequestedTime { get; set; }
        public List<Person> accessUserRequests { get; set; }

        public ElevatorRequest(int requestedFloor, int currentFloor = 0, string? userId = null, DateTime? requestedDateTime = null)
        {
            this.RequestedFloor = requestedFloor;
            this.RequestedTime = requestedDateTime ?? DateTime.UtcNow;
            this.accessUserRequests = [];
            if (!string.IsNullOrEmpty(userId))
            {
                this.accessUserRequests.Add(
                    new Person {
                        CurrentFloor = requestedFloor,
                        DesignatedFloor = requestedFloor, 
                        UserId = userId });
            }
        }
    }
}
