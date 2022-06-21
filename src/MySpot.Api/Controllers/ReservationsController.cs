﻿using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Services;

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
        public async Task<ActionResult<ReservationDto[]>> Get() => Ok(await _reservationService.GetAllWeeklyAsync());
       
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReservationDto>> Get(Guid id)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation is null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateReservation command)
        {
            var id = await _reservationService.CreateAsync(command with{ ReservationId = Guid.NewGuid() });

            if (id is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new {Id = id}, default);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Put(Guid id, ChangeReservationLicencePlate command)
        {
            var isSucceeded = await _reservationService.UpdateAsync(command with{ReservationId = id});

            if (!isSucceeded)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var isSucceeded =  await _reservationService.DeleteAsync(new DeleteReservation(id));

            if (!isSucceeded)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
