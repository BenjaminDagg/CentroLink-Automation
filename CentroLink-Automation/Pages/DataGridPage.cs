using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;

namespace CentroLink_Automation
{
    public class DataGridPage : BasePage
    {
        public virtual By DataGrid { get; set; }
        public virtual By RowSelector { get; set; }

        public DataGridPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //elements
            DataGrid = By.ClassName("DataGrid");
            RowSelector = By.ClassName("DataGridRow");
        }


        public virtual int RowCount
        {
            get
            {
                WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
                var rows = list.FindElements(RowSelector);

                return rows.Count;
            }
        }


        public virtual void SortGridByHeaderDescending(int headerCol)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                
                var targetHeader = list.FindElement(By.XPath(".//Header/HeaderItem[" + (headerCol + 1) + "]"));
                targetHeader.Click();
                targetHeader.Click();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Header column " + headerCol + " invalid");
            }
        }


        public virtual void SortGridByHeaderAscending(int headerCol)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {

                var targetHeader = list.FindElement(By.XPath(".//Header/HeaderItem[" + (headerCol + 1) + "]"));
                targetHeader.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Header column " + headerCol + " invalid");
            }
        }


        public virtual string GetHeader(int headerCol)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {

                var targetHeader = list.FindElement(By.XPath(".//Header/HeaderItem[" + (headerCol + 1) + "]")).Text;
                return targetHeader.Replace("\r","").Replace("\n", " ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Header column " + headerCol + " invalid");
                return null;
            }
        }


        public void SelectRow(int rowNum)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = list.FindElements(By.ClassName("DataGridRow"));

            try
            {
                var row = list.FindElement(By.XPath("(.//DataItem[@ClassName='DataGridRow'])[" + (rowNum + 1) + "]"));
                row.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMachine: Invalid row " + rowNum);
            }
        }


        public virtual void SelectRows(params int[] rowNums)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = list.FindElements(By.ClassName("DataGridRow"));

            Actions holdKeyAction = new Actions(driver);
            holdKeyAction.KeyDown(Keys.Shift);
            holdKeyAction.Build();

            Actions releaseKeyAction = new Actions(driver);
            holdKeyAction.KeyUp(Keys.Shift);
            holdKeyAction.Build();

            holdKeyAction.Perform();
            foreach(int rowNum in rowNums)
            {
                try
                {
                    var row = list.FindElement(By.XPath("(.//DataItem[@ClassName='DataGridRow'])[" + (rowNum + 1) + "]"));
                    row.Click();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetMachine: Invalid row " + rowNum);
                }
            }

            releaseKeyAction.Perform();
        }


        public virtual List<string> GetValuesForColumn(int colNum)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = list.FindElements(RowSelector);

            var values = new List<string>();

            foreach(var row in rows)
            {
                var colItem = row.FindElement(By.XPath(".//Custom[" + (colNum + 1) + "]/Text"));
                values.Add(colItem.Text);
            }

            return values;
        }
    }
}
