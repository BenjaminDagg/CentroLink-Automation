using NUnit.Framework;
using System.Threading;

namespace CentroLink_Automation
{
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            loginPage = new LoginPage(driver);
        }

        [Test]
        public void Succussful_Login()
        {
            loginPage.Login("user1", "Diamond1#");
            
        }
    }
}