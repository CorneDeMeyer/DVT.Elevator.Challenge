using DVT.Elevator.Challenge.Domain.Models;

namespace DVT.Elevator.Challenge.DomainLogic.Interface
{
    public interface IElevatorClient
    {
        Task<ElevatorReponse> RequestElevator(int zone, int floor);
        Task<ElevatorReponse> RequestElevatorDisable(int zone, string designation);
        Task<ElevatorReponse> RequestElevatorEnable(int zone, string designation);
    }
}
