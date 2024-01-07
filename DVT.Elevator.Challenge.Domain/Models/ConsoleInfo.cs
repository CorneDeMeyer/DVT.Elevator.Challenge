using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT.Elevator.Challenge.Domain.Models
{
    public class ConsoleInfo
    {
        public bool commandReaty { get; set; }
        public StringBuilder sbRead { get; set; }
        public List<string> outputBuffer { get; set; }
        public string lastCommand { get; set; }
        public StringBuilder lastResult { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ConsoleInfo()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            sbRead = new StringBuilder();
            outputBuffer = new List<string>();
            commandReaty = false;
            lastResult = new StringBuilder();
        }
    }
}
