using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace ModellerUITest.DeploySolution
{
    [TestClass]
    public class DeployAX2012R3Solution
    {
        [TestMethod]
        public void DeployAX2012R3SolutionTest()
        {

            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://zapbi.zaptesting.com/latest/design");
            Process.Start("C:\\Users\\simon.shen\\Box Sync\\work\\UI Test Projects\\ModellerUITest\\ModellerUITest\\autoitscript\\HandleAuth.exe");

            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromMinutes(2);
            wait.PollingInterval = TimeSpan.FromMilliseconds(500);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            IWebElement element;

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[ui-id='Zap_BI_Portal_OptionItemTypes_New_ResourceBase__ModelWizardResource']")));
            element = wait.Until((d) =>
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                bool isBusy = (bool)js.ExecuteScript(@"return window.$ && $('.progress-bar').css('visibility') == 'visible' || window.$ && $('.loadingOverlay,.block-clicks').length > 0;");
                if (!isBusy)
                    return driver.FindElement(By.CssSelector(@"div[ui-id='Zap_BI_Portal_OptionItemTypes_New_ResourceBase__ModelWizardResource']"));
                else
                    return null;
            });
            int tries = 0;
            while (tries < 100)
                try
                {
                    element.Click();
                }
                catch
                { }
                finally { tries++; };

            Console.WriteLine(tries);

            driver.Manage().Window.Maximize();

            //Select AX 2012R3 solution
            element = wait.Until(d => d.FindElement(By.CssSelector("span[tooltip='Zap.Modeller.Tooltips.Wizard.CreateWizardSolutionTab']")));
            tries = 0;
            while (tries < 100)
                try
                {
                    element.Click();
                }
                catch
                { }
                finally { tries++; };
            element.Click();

            element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(("(//*[contains(text(), 'AX 2012 R3')])"))));
            element.Click();

            //Ax Service connection
            element = wait.Until(d => d.FindElement(By.CssSelector(@"input[type=text][name=AxServer]")));
            element.SendKeys("zap-app-04");
            element = wait.Until(d => d.FindElement(By.CssSelector(@"input[type=text][name=TcpPort]")));
            element = wait.Until(d => d.FindElement(By.CssSelector(@"input[type=text][ng-model=""axController.settings.UserName""]")));
            element.SendKeys(@"zaptesting\simon");
            element = wait.Until(d => d.FindElement(By.CssSelector(@"input[type=password][ng-model=""axController.settings.Password""]")));
            element.SendKeys(@"ayDFH1Dx%");

            //AX Source connection

            element = driver.FindElement(By.XPath(@"(//input[@ng-model='combo.text'])[2]"));
            element.SendKeys(@"Zap-sqldb-01\sql2014");
            element = driver.FindElement(By.XPath(@"(//input[@ng-model='combo.text'])[3]"));
            element.SendKeys(@"MicrosoftDynamicsAX2012R3");
            element = driver.FindElement(By.CssSelector(@"input[type=text][ng-model='sqlServerConnectionInfoCtrl.connectionInfo.UserName']"));
            element.SendKeys(@"zaptesting\simon");
            element = driver.FindElement(By.CssSelector(@"input[type=password][ng-model='sqlServerConnectionInfoCtrl.connectionInfo.Password']"));
            element.SendKeys(@"ayDFH1Dx%");

            //Test Connection
            wait.Timeout = TimeSpan.FromMinutes(5);//AX connection validation can take quite long 
            driver.FindElement(By.CssSelector(@"icon-button[ng-click='testConnection.test\(\)']")).Click();

            element = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector((@"icon-button[ng-click='modelWizardCtrl.goToNextPage\(\)']"))));
            element.Click();

            //Partition selection
            element = wait.Until(d => d.FindElement(By.CssSelector(@"input[ng-model='combo.text']")));
            IJavaScriptExecutor j = (IJavaScriptExecutor) driver;
            j.ExecuteScript(@"$(""input[ng-model='combo.text']"").removeAttr('readonly')");
            //j.ExecuteScript("arguments.removeAttribute('readonly','readonly')",element);
            element = wait.Until(d => d.FindElement(By.CssSelector(@"input[ng-model='combo.text']")));
            element.SendKeys(@"Initial Partition (initial)");

        }
        public static string GenerateName(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
