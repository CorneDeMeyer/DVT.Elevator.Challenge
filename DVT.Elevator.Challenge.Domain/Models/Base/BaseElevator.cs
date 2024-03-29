﻿using DVT.Elevator.Challenge.Domain.Enums;

namespace DVT.Elevator.Challenge.Domain.Models.Base
{
    public class BaseElevator
    {
        public required int ZoneLocated { get; set; }
        public required string ElevatorDesignation { get; set; }
        public int CurrentFloor { get; set; }
        public int MaxLevel { get; set; }
        public MovementEnum Movement { get; set; }
        public required float WeightCapacity { get; set; }
        public required float CurrentWeight { get; set; }
        public required int PersonCapacity { get; set; }
        public List<Person>? PeopleInLift { get; set; }
        public List<ElevatorRequest>? Requests { get; set; }
        public bool Enabled { get; set; }
    }
}
