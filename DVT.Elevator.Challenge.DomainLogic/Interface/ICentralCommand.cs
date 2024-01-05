using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.Domain.Models.Base;
using DVT.Elevator.Challenge.Domain.Models;

namespace DVT.Elevator.Challenge.DomainLogic.Interface
{
    public interface ICentralCommand
    {
        Task Setup();
        Task MoveElevator(BaseElevator elevator);
        Task PersonRequest(Person person);
        Task CheckElevators();
        Task DisplayElevatorPosition();
    }
}
