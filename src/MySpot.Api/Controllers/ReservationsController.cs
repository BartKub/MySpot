using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationsController: ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public ActionResult<ReservationDto[]> Get() => Ok(_reservationService.GetAllWeekly());
       
        [HttpGet("{id:guid}")]
        public ActionResult<ReservationDto> Get(Guid id)
        {
            var reservation = _reservationService.Get(id);

            if (reservation is null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPost]
        public ActionResult Post(CreateReservation command)
        {
            var id = _reservationService.Create(command with{ ReservationId = Guid.NewGuid() });

            if (id is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new {Id = id}, default);
        }

        [HttpPut("{id:guid}")]
        public ActionResult Put(Guid id, ChangeReservationLicencePlate command)
        {
            var isSucceeded = _reservationService.Update(command with{ReservationId = id});

            if (!isSucceeded)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var isSucceeded = _reservationService.Delete(new DeleteReservation(id));

            if (!isSucceeded)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
