using System.Net;
using Moq;
using TFA.Domain.Authorization;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.Tests.Authorization;

public class IntentionManagerShould
{
    [Fact]
    public void ReturnFalse_WhenNoMatchingResolver()
    {
        var resolvers = new IIntentionResolver[]
        {
            new Mock<IIntentionResolver>().Object,
            new Mock<IIntentionResolver<HttpStatusCode>>().Object
        };
        var sut = new IntentionManager(Enumerable.Empty<IIntentionResolver>(), new Mock<IIdentityProvider>().Object);
    }
}