using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.DomainLogic.Interface;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class ElevatorClient(IElevatorService elevatorService): IElevatorClient
    {
        private readonly IElevatorService _service = elevatorService;
        public async Task<ElevatorReponse> RequestElevator(int zone, int floor) => await _service.ElevatorRequest(zone, floor);

        public async Task<ElevatorReponse> RequestElevatorDisable(int zone, string designation) => await _service.ElevatorDisableRequest(zone, designation);

        public async Task<ElevatorReponse> RequestElevatorEnable(int zone, string designation) => await _service.ElevatorEnableRequest(zone, designation);
    }
}
