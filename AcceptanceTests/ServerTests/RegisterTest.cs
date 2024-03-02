
using AcceptanceTests.Logic;

namespace AcceptanceTests.ServerTests
{
    [TestFixture]
    public class RegisterTest:AbstractTestCase
    {
        [SetUp]
        public new void BeforeTest()
        {
        }
        [Test]
        public void RegisterUser()
        {
            eds.Register("test@example.com", "John","123123fasf123", "Doe525gsgsd5", "1q2w3eAS!", 1);

        }
    }
}
