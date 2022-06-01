using System;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Entities
{
    public class WeeklyParkingSpotTests
    {
        private readonly WeeklyParkingSpot _weeklyParkingSpot;
        private readonly Date _now;
        public WeeklyParkingSpotTests()
        {
            _now = new Date(DateTime.Parse("2022-02-25"));
            _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
        }

        [Theory]
        [InlineData("2020-02-02")]
        [InlineData("2025-02-02")]
        [InlineData("2025-02-24")]
        public void given_invalid_date_add_reservation_should_fail(string dateString)
        {
            var invalidDate = DateTime.Parse(dateString);

            //Arrange
            var reservation = new Reservation(Guid.NewGuid(), "Joe Doe", "XYZ123", new Date(invalidDate));

            //Act
            var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, _now));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidReservationDateException>(exception);
        }

        [Fact]
        public void given_reservation_for_existing_date_add_reservation_should_fail()
        {
            //Arrange
            var reservationDate = _now.AddDays(1);
            var reservation = new Reservation(Guid.NewGuid(), "Joe Doe", "XYZ123", new Date(reservationDate));
            _weeklyParkingSpot.AddReservation(reservation, reservationDate);

            //Act
            var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, reservationDate));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ParkingSpotAlreadyReservedException>(exception);
        }


        [Fact]
        public void given_reservation_for_not_taken_date_add_reservation_success()
        {
            //Arrange
            var reservationDate = _now.AddDays(1);
            var reservation = new Reservation(Guid.NewGuid(), "Joe Doe", "XYZ123", new Date(reservationDate));
            _weeklyParkingSpot.AddReservation(reservation, reservationDate);

            //Act
            _weeklyParkingSpot.AddReservation(reservation, reservationDate);

            //Assert
            _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
            _weeklyParkingSpot.Reservations.ShouldContain(reservation);
        }
    }
}
