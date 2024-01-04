using DVT.Elevator.Challenge.DomainLogic.Interface;

namespace DVT.Elevator.Challenge.DomainLogic.Service
{
    public class App(ICentralCommand centralCommand) : IApplication
    {
        private ICentralCommand _command = centralCommand;

        public async Task Run()
        {
            // Run Setup 
            await _command.Setup();

            // Setup Task to Update on elevator movement

            // Setup Person inputs randomly (TASK)

        }

        static void RunBackgroundTask()
        {

        }
    }
}
