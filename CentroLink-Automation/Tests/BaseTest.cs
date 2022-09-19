using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using EGMSimulator.Core.Settings;
using Framework.Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CentroLink_Automation
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected WindowsDriver<WindowsElement> driver;
        protected NavMenu navMenu;
        protected string ConnectionString;
        protected DatabaseManager LotteryRetailDatabase;
        protected readonly string TestMachineId = "00001";
        protected DealManagerService DealManager;
        protected SqlConnection DbConnection;
        protected ServiceProvider ServiceProvider;
        protected TransactionPortalService TpService;
        protected DealManagerService DealService;
        protected MachineManagerService MachineService;
        protected GameManagerService GameService;
        protected ILogService _logService;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            ConnectionString = $"Server = 10.0.50.186; Database = LotteryRetail; User Id=sa; Password=3m3r@ld!; MultipleActiveResultSets=True";
            DbConnection = new SqlConnection(ConnectionString);
            DbConnection.Open();

            LotteryRetailDatabase = new DatabaseManager(DbConnection);

            var services = new ServiceCollection();
            services.AddSingleton<ISimulatorSettings, SimulatorSettings>();
            

            ServiceProvider = services.BuildServiceProvider();
            _logService = ServiceProvider.GetService<ILogService>();

            

            //Setup TP Service
            DealService = await DealManagerService.BuildDealManagerAsync(TestData.TestDealNumber, DbConnection,_logService);
            MachineService = await MachineManagerService.MachineManagerServiceAsync(int.Parse(TestData.DefaultMachineNumber), DbConnection);
            GameService = await GameManagerService.BuildGameManagerServiceAsync(TestData.TestGameCode, DbConnection);

            TpService = new TransactionPortalService(MachineService, DealService, LotteryRetailDatabase, GameService, _logService);
        }


        [SetUp]
        public virtual async Task Setup()
        {
            
            SessionManager.Init();
            driver = SessionManager.Driver;

            navMenu = new NavMenu(driver);
        }


        [TearDown]
        public virtual async Task EndTest()
        {
            //SessionManager.Close();
        }
    }
}
