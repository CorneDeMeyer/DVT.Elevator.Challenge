using DVT.Elevator.Challenge.DomainLogic.Interface;
using DVT.Elevator.Challenge.Domain.Models.Config;
using DVT.Elevator.Challenge.DomainLogic.Service;
using DVT.Elevator.Challenge.Domain.Models.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

// https://www.tkelevator.com/us-en/company/insights/how-is-elevator-capacity-calculated.html
// https://www.codeproject.com/Questions/5358772/Make-my-elevator-move-as-expected
var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false);

IConfiguration config = builder.Build();

// Service Injection Magic 
var _host = Host.CreateDefaultBuilder().ConfigureServices(services => {
    services.AddSingleton<IApplication, App>();
    services.AddSingleton<ICentralCommand, CentralCommand>();
    services.AddTransient<IElevatorService, ElevatorService>();
    services.AddTransient<IElevatorClient, ElevatorClient>();
    services.AddSingleton(new AppConfiguration(
        config.GetValue<TimeSpan>("ElevatorFloorMove"),
        config.GetValue<int>("NumberOfFloors"),
        config.GetValue<int>("NumberOfPeople"),
        config.GetSection("ElevatorConfig").Get<BaseElevatorConfig[]>() ?? []
        )) ;
}).Build();

// Start the application 
var app = _host.Services.GetRequiredService<IApplication>();
app.Run();