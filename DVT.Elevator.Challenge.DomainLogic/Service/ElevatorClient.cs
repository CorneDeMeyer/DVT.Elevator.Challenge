using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models;
using System.Linq;
using DVT.Elevator.Challenge.Domain;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class ElevatorClient(IElevatorService elevatorService): IElevatorClient
    {
        private readonly IElevatorService _service = elevatorService;
        public async Task<ElevatorReponse> RequestElevator(int zone, int currentFloor, int requestedFloor, string? userId = "") => await _service.ElevatorRequest(zone, currentFloor, requestedFloor, userId);

        public async Task<ElevatorReponse> RequestElevatorStatus(int zone, string designation)
        {
            var response = new ElevatorReponse
            { 
                Floor = -100,
                Message = $"No Elevator are currently configured",
                Zone = zone,
                Movement = Domain.Enums.MovementEnum.Unknown,
            };

            var elevators = await _service.GetElevators();
            if (elevators != null && elevators.Count > 0)
            {
                var elevator = elevators.FirstOrDefault(elevator => elevator.ZoneLocated == zone &&
                                                                    elevator.ElevatorDesignation.Equals(designation, StringComparison.OrdinalIgnoreCase));
                if (elevator != null)
                {
                    response.Floor = elevator.CurrentFloor;
                    
                    if (elevator.Enabled)
                    {
                        response.Movement = elevator.Movement;
                        response.Message = $"Elevator {elevator.ElevatorDesignation} is {elevator.Movement.GetMovement()} to/on floor {elevator.Movement.DetermineGoingToFloor(elevator.CurrentFloor, elevator.Requests)}";
                    }
                    else
                    {
                        response.Movement = Domain.Enums.MovementEnum.Unknown;
                        response.Message = $"Elevator {elevator.ElevatorDesignation} is currently under maintenance!";
                    }
                }
                else
                {
                    response.Message = $"No Elevator {designation} Available in Zone {zone}";
                }
            }

            return response;
        }
    }
}
