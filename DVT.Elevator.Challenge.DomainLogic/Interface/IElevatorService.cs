using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.Domain.Models.Base;

namespace DVT.Elevator.Challenge.DomainLogic.Interface
{
    public interface IElevatorService
    {
        Task Setup();
        void MoveElevator(BaseElevator elevatorOnTheMove);
        Task DisplayElevatorPosition();
        Task CheckElevators();
        Task<ElevatorReponse> ElevatorRequest(int zone, int floor);
        Task<ElevatorReponse> ElevatorDoorsClosed(decimal weight);
        Task<ElevatorReponse> ElevatorDisableRequest(int zone, string designation);
        Task<ElevatorReponse> ElevatorEnableRequest(int zone, string designation);
    }
}
