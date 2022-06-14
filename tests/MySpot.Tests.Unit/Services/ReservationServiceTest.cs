using System;
using MySpot.Application.Commands;
using MySpot.Application.Services;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services
{
    public class ReservationServiceTest
    {
        private readonly ReservationService _reservationService;

        public ReservationServiceTest(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Fact]
        public void given_valid_command_create_should_add_reservation()
        {
            //Arrange
            var id = Guid.NewGuid();
            var command = new CreateReservation(id, Guid.NewGuid(), "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));
           
            //Act
            var reservationId = _reservationService.Create(command);

            //Assert
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }
    }
}
