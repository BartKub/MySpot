using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;

namespace MySpot.Api.Controllers
{
    [ApiController]
    [Route("parking-spots")]
    public class ParkingSpotsController: ControllerBase
    {
        //Here we should create Dispatcher instead of injecting handlers

        private readonly ICommandHandler<ReserveParkingSpotForVehicle> _commandHandler;

        public ParkingSpotsController(ICommandHandler<ReserveParkingSpotForVehicle> commandHandler)
        {
            _commandHandler = commandHandler;
        }

    }
}
