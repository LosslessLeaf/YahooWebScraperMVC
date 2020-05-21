using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data.SqlClient;

namespace YahooWebScraperMVC_Auth
{
    public class Scrape
    {
        public Scrape()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            //options.AddArgument("--remote-debugging-port=9222");
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://www.yahoo.com";

            driver.FindElement(By.Id("header-signin-link")).Click();

            driver.FindElement(By.Id("login-username")).SendKeys("josephmlopresti@yahoo.com");
            driver.FindElement(By.Id("login-signin")).Click();

            String myWindowHandle = driver.CurrentWindowHandle;

            driver.SwitchTo().Window(myWindowHandle);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            driver.FindElement(By.Name("password")).SendKeys("FranticReactiveBurritoSoup");
            driver.FindElement(By.Name("verifyPassword")).Click();

            driver.FindElement(By.LinkText("Finance")).Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,300)");

            driver.FindElement(By.XPath("//*[@id='data-util-col']/section[2]/header/a")).Click();


            //driver.FindElement(By.LinkText("Watchlist")).Click();

            IWebElement table = driver.FindElement(By.XPath("//*[@id='pf-detail-table']/div[1]/table"));

            List<IWebElement> dataRows = new List<IWebElement>(table.FindElements(By.XPath("//*[@id='pf-detail-table']/div[1]/table/tbody/tr")));


            List<IWebElement> dataCells = new List<IWebElement>(dataRows[0].FindElements(By.XPath("//*[@id='pf-detail-table']/div[1]/table/tbody/tr/td")));


            int i = 0;
            Stock stock = new Stock();
            foreach (var cell in dataCells)
            {
                string temp = cell.Text;
                switch (i)
                {
                    case 0:
                        stock.Symbol = temp;
                        break;
                    case 1:
                        stock.LastPrice = temp;
                        break;
                    case 2:
                        stock.Change = temp;
                        break;
                    case 3:
                        stock.ChangePercentage = temp;
                        break;
                    case 4:
                        stock.Currency = temp;
                        break;
                    case 5:
                        stock.MarketTime = temp;
                        break;
                    case 6:
                        stock.Volume = temp;
                        break;
                    case 8:
                        stock.AvgVolume = temp;
                        break;
                    case 12:
                        stock.MarketCap = temp;
                        break;
                }
                i++;

                if (i == 16)
                {
                    i = 0;
                    SQL SQL = new SQL();
                    SqlConnectionStringBuilder builder = SQL.Connection();
                    SQL.StoreData(stock, builder);
                }
            }

            driver.Quit();
        }
    }

}
