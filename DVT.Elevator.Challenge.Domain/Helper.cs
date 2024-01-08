using DVT.Elevator.Challenge.Domain.Enums;
using DVT.Elevator.Challenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT.Elevator.Challenge.Domain
{
    public static class Helper
    {
        public static string GetMovement(this MovementEnum movement, int? PersonOnBoard = 0)
        {
            switch (movement)
            {
                case MovementEnum.Down:
                    return "Going Down";
                case MovementEnum.Stationery:
                    return PersonOnBoard.HasValue && PersonOnBoard.Value > 0
                        ? "Offloading"
                        : "Idle";
                case MovementEnum.Up:
                    return "Going Up";
                default:
                    return "Unknown";
            }
        }

        public static int DetermineGoingToFloor(this MovementEnum movement, int currentLevel, List<ElevatorRequest>? requests)
        {
            if (movement == MovementEnum.Down)
            {
                return requests?.OrderByDescending(x => x.RequestedFloor)?.FirstOrDefault()?.RequestedFloor ?? currentLevel;
            }
            else if (movement == MovementEnum.Up)
            {
                return requests?.OrderBy(x => x.RequestedFloor)?.FirstOrDefault()?.RequestedFloor ?? currentLevel;
            }

            return currentLevel;
        }
    }
}
