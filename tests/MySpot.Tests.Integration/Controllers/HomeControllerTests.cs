using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace MySpot.Tests.Integration.Controllers;

public class HomeControllerTests: ControllerTests
{
    [Fact]
    public async Task get_base_endpoint_should_return_200_and_api_name()
    {
        var response = await Client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("MySpot API [test]");
    }

    public HomeControllerTests(OptionsProvider opt) : base(opt)
    {
    }
}