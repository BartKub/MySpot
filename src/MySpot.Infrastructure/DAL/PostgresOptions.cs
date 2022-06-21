using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MySpot.Infrastructure.DAL
{
    internal sealed class PostgresOptions
    {
        public string ConnectionString { get; set; }
    }
}
