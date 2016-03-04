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
    public class DeployAuditSolutionTest
    {
        [TestMethod]
        public void DeployAuditSolution()
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
                bool isBlocked = (bool)js.ExecuteScript(@"return $('.ctl00_ctl00_Fcph_Mcph_portalBody_multipaneView_divPanes').is(':visible')");
                if (!isBlocked)
                    return driver.FindElement(By.CssSelector(@"div[ui-id='Zap_BI_Portal_OptionItemTypes_New_ResourceBase__ModelWizardResource']"));
                else
                    return null;
            });
            element.Click();

            driver.Manage().Window.Maximize();

            element = wait.Until(d => d.FindElement(By.CssSelector("span[tooltip='Zap.Modeller.Tooltips.Wizard.CreateWizardSolutionTab']")));
            element.Click();

            element = wait.Until(d => d.FindElement(By.XPath("//*[contains(text(), 'Zap BI Auditing')]")));
            element.Click();

            wait.Until(d => d.FindElement(By.CssSelector("div.combobox[name=Server]")));
            driver.FindElement(By.XPath(@"(//input[@ng-model='combo.text'])[1]")).SendKeys(@"zap-farm-03");
            driver.FindElement(By.CssSelector("input[type=text][name=UserName]")).SendKeys(@"zaptesting\simon");
            driver.FindElement(By.CssSelector("input[type=password]")).SendKeys(@"ayDFH1Dx%");
            driver.FindElement(By.XPath(@"(//input[@ng-model='combo.text'])[2]")).SendKeys(@"ZapBI-latest");
            driver.FindElement(By.CssSelector(@"icon-button[ng-click='testConnection.test\(\)']")).Click();

            element = wait.Until(d => d.FindElement(By.CssSelector(@"icon-button[ng-click='modelWizardCtrl.goToNextPage\(\)']")));
            element.Click();

            wait.Until(d => d.FindElement(By.CssSelector("input[type=text][ng-model='createModelScreenCtrl.modelCreationSettings.Name']")));
            driver.FindElement(By.CssSelector("input[type=text][ng-model='createModelScreenCtrl.modelCreationSettings.Name']")).Clear();
            driver.FindElement(By.CssSelector("input[type=text][ng-model='createModelScreenCtrl.modelCreationSettings.Name']")).SendKeys(GenerateName(10));
            driver.FindElement(By.CssSelector("input[type=text][ng-model='createModelScreenCtrl.modelCreationSettings.Name']")).SendKeys(Keys.Tab);
            driver.FindElement(By.CssSelector(@"icon-button[ng-click='modelWizardCtrl.goToNextPage\(\)']")).Click();

            wait.Timeout = TimeSpan.FromMinutes(10); //wait for model creation
            element = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(@"icon-button[ng-click='modelWizardCtrl.finishWizard\(\)']")));
            element.Click();

            wait.Timeout = TimeSpan.FromMinutes(2);

            element = wait.Until((d) =>
            {
                //string isBusyOnClient = @"return window.$ && $('.progress-bar').css('visibility') == 'visible'|| window.$ && $('.loadingOverlay,.block-clicks').length > 0;";
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                bool isPostingBack = (bool)js.ExecuteScript(@" return $('.sneakyModalBackground').is(':visible')");
                bool isVisible = (bool)js.ExecuteScript("return $('icon-button[ng-click=\"processButtonCtrl.showProcessPopup()\"]').is(':visible')");
                bool buttonDisabled = (bool)js.ExecuteScript(@" return $('icon-button[ng-click=""processButtonCtrl.showProcessPopup()""] button').is(':disabled')");
                if (!isPostingBack && isVisible && !buttonDisabled)
                    return driver.FindElement(By.CssSelector(@"icon-button[ng-click='processButtonCtrl.showProcessPopup\(\)']"));
                else
                    return null;
            });
            element.Click();

            element = wait.Until(d => d.FindElement(By.CssSelector(@"icon-button[ng-click='processPopupCtrl.process\(\)']")));
            element.Click();

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
