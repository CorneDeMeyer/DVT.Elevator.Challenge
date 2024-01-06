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
        public ConsoleInfo()
        {
            sbRead = new StringBuilder();
            outputBuffer = new List<string>();
            commandReaty = false;
            lastResult = new StringBuilder();
        }
    }
}
