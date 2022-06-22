using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpot.Core.Exceptions
{
    public class NoReservationPolicyFoundException: CustomException
    {
        public string JobTitle { get; }

        public NoReservationPolicyFoundException(string jobTitle) : base($"No reservation policy for {jobTitle} found")
        {
            JobTitle = jobTitle;
        }
    }
}
