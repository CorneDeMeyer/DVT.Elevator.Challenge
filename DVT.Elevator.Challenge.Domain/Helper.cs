using DVT.Elevator.Challenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT.Elevator.Challenge.Domain
{
    internal static class Helper
    {
        public static string GetMovement(this MovementEnum movement, int? PersonOnBoard)
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
    }
}
