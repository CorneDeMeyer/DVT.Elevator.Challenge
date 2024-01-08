using DVT.Elevator.Challenge.Domain.Models;

namespace DVT.Elevator.Challenge.DomainLogic.Interface
{
    public interface IElevatorClient
    {
        Task<ElevatorReponse> RequestElevator(int zone, int currentFloor, int requestedFloor, string? userId = "");
        Task<ElevatorReponse> RequestElevatorStatus(int zone, string designation);
    }
}
