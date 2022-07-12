using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands
{
    public record SignUp(Guid UserId, string Email, string Username, string Password, string FullName,
        string Role) : ICommand;

}
