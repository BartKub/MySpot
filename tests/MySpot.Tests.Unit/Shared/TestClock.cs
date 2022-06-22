using System;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;

namespace MySpot.Tests.Unit.Shared
{
    internal class TestClock: IClock
    {
        public DateTime Current() => new(2022, 2, 1);

    }
}
