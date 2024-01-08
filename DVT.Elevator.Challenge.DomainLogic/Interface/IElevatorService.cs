using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.Domain.Models.Base;

namespace DVT.Elevator.Challenge.DomainLogic.Interface
{
    public interface IElevatorService
    {
        Task<List<BaseElevator>> GetElevators();
        Task Setup();
        Task DisplayElevatorPosition();
        Task CheckElevators();
        Task<ElevatorReponse> ElevatorRequest(int zone, int currentFloor, int requestFloor, string? userId = null);
        Task<ElevatorReponse> ElevatorDoorsClosed(int zone, string designation, float weight);
        Task<ElevatorReponse> ElevatorDisableRequest(int zone, string designation);
        Task<ElevatorReponse> ElevatorEnableRequest(int zone, string designation);
    }
}
