using System;
using System.Collections.Generic;
using System.Text;

namespace API.Test.Infrastructure;

public abstract class IntegrationTestFixture(WebAppFactory webAppFactory) : IClassFixture<WebAppFactory>
{
    public HttpClient CreateClient() => webAppFactory.CreateClient();
}
