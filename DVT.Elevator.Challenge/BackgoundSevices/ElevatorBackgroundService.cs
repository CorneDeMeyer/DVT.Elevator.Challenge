using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models.Config;
using Microsoft.Extensions.Hosting;

namespace DVT.Elevator.Challenge.BackgoundSevices
{
    public class ElevatorBackgroundService(IElevatorService elevatorService, AppConfiguration config) : BackgroundService, IDisposable
    {
        private readonly IElevatorService _elevatorService = elevatorService;
        private readonly AppConfiguration _config = config;
        private PeriodicTimer? _timer;

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                var _timer = new PeriodicTimer(_config.RefreshTime);
                while (await _timer.WaitForNextTickAsync())
                {
                    await _elevatorService.CheckElevators();
                }
            }
        }
    }
}
