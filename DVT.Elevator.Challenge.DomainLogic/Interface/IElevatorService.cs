using DVT.Elevator.Challenge.Domain.Models.Base;

namespace DVT.Elevator.Challenge.DomainLogic.Interface
{
    public interface IElevatorService
    {
        Task Setup();
        Task MoveElevator(BaseElevator elevatorOnTheMove);
        Task DisplayElevatorPosition();
        Task CheckElevators();
    }
}
