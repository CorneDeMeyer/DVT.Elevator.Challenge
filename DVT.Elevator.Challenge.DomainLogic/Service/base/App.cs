using DVT.Elevator.Challenge.Domain.Models;
using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.DomainLogic.Interface;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class App(ICentralCommand centralCommand, AppConfiguration configuration) : IApplication
    {
        private readonly ICentralCommand _command = centralCommand;
        private readonly AppConfiguration _configuration = configuration;

        public async Task Run()
        {
            var task = new PeriodicTimer(_configuration.RefreshTime);
            var personTask = new PeriodicTimer(TimeSpan.FromSeconds(20));

            try
            {
                // Run Setup 
                await _command.Setup();

                // Setup Cancelation

                // This is to simulate a elevator moving between Floors 
                // Setup Task to Update on elevator movement
                while (await task.WaitForNextTickAsync())
                {
                    await RunElevatorBackgroundTask();
                }

                // Setup Random User press Button
                while (await personTask.WaitForNextTickAsync())
                {
                    await RunUserCallElevatorTask();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                task.Dispose();
            }
        }

        async Task RunElevatorBackgroundTask()
        {
            await _command.CheckElevators();
        }

        async Task RunUserCallElevatorTask()
        {
            var random = new Random();
            var peopleToAdd = random.Next(0, 10);
           
            for (int i = 0; i < peopleToAdd; i++)
            {
                var personsWeight = random.Next(0, 120);
                var randomFloor = random.Next(0, _configuration.NumberOfFloors);
                var randomCurrentFloor = random.Next(0, _configuration.NumberOfFloors);

                // Just to ensure we not going to the same floor
                while (randomFloor == randomCurrentFloor)
                {
                    randomCurrentFloor = new Random().Next(0, _configuration.NumberOfFloors);
                }

                await _command.PersonRequest(
                    new Person
                    {
                        DesignatedFloor = randomFloor,
                        CurrentFloor = randomCurrentFloor,
                        Weight = personsWeight,
                        Movement = randomFloor > randomCurrentFloor 
                        ? Domain.Enums.MovementEnum.Up
                        : Domain.Enums.MovementEnum.Down
                    });
            }
        }
    }
}
