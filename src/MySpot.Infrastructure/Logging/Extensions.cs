using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Infrastructure.Logging.Decorators;

namespace MySpot.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
            return services;
        }
    }
}
