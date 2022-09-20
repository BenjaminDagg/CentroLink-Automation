﻿using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{
    public class EditLocationTests : BaseTest
    {

        private LoginPage loginPage;
        private LocationSetupPage locationSetup;
        private EditLocationPage editLocation;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            locationSetup = new LocationSetupPage(driver);
            editLocation = new EditLocationPage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();
        }


        [Test]
        public void Test()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterSweepAmount("5000");
            
        }
    }
}
