using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries
{
    public class GetUser: IQuery<UserDto>
    {
        public Guid UserId { get; set; }
    }
}
