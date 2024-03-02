using AcceptanceTests.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    [TestFixture]
    public abstract class AbstractTestCase
    {
        protected EDS eds;
        private string serverUri = "http://localhost:5001/";
        [SetUp]
        public void TestStarted()
        {
            //need to start the emotion detection server
            eds = new EDS(serverUri, new HttpClient());
        }
    }
}
