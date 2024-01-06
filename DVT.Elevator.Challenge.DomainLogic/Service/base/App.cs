using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.DomainLogic.Interface;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class App(ICentralCommand centralCommand) : IApplication
    {
        private readonly ICentralCommand _command = centralCommand;

        public void Run()
        {
            try
            {
                // Run Setup 
                _command.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //async Task RunUserCallElevatorTask()
        //{
        //    var random = new Random();
        //    var peopleToAdd = random.Next(0, 10);
           
        //    for (int i = 0; i < peopleToAdd; i++)
        //    {
        //        var personsWeight = random.Next(0, 120);
        //        var randomFloor = random.Next(0, _configuration.NumberOfFloors);
        //        var randomCurrentFloor = random.Next(0, _configuration.NumberOfFloors);

        //        // Just to ensure we not going to the same floor
        //        while (randomFloor == randomCurrentFloor)
        //        {
        //            randomCurrentFloor = new Random().Next(0, _configuration.NumberOfFloors);
        //        }

        //        await _command.PersonRequest(
        //            new Person
        //            {
        //                DesignatedFloor = randomFloor,
        //                CurrentFloor = randomCurrentFloor,
        //                Weight = personsWeight,
        //                Movement = randomFloor > randomCurrentFloor 
        //                ? Domain.Enums.MovementEnum.Up
        //                : Domain.Enums.MovementEnum.Down
        //            });
        //    }
        //}
    }
}
