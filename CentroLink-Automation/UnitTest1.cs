using NUnit.Framework;

namespace CentroLink_Automation
{
    public class Tests : BaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            
            System.Threading.Thread.Sleep(5000);
            Assert.Pass();
        }
    }
}