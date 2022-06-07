using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Api.Commands;
using Xunit;

namespace MySpot.Tests.Unit.Services
{
    public class ReservationServiceTest
    {
        [Fact]
        public void given_valid_command_create_should_add_reservation()
        {
            var id = Guid.NewGuid();
            var command = new CreateReservation(id, Guid.NewGuid(), "Joe Doe", "XYZ123", DateTime.UtcNow);
        }
    }
}
