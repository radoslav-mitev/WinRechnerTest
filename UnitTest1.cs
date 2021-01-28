using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace WinRechnerTest
{
    [TestFixture]
    public class Tests
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
        private WindowsDriver<WindowsElement> driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", CalculatorAppId);
            options.AddAdditionalCapability("deviceName", "WindowsPC");
            this.driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
        }

        [OneTimeTearDown]
        public void TearDown() => this.driver.Quit();

        [Test]
        public void Addition()
        {
            // Find the buttons by their names and click them in sequence to perform 1 + 7 = 8
            this.driver.FindElementByName("Eins").Click();
            this.driver.FindElementByName("Plus").Click();
            this.driver.FindElementByName("Sieben").Click();
            this.driver.FindElementByName("Gleich").Click();
            Assert.AreEqual("8.", GetCalculatorResultText());
        }
       
        [Test]
        public void Division()
        {
            // Find the buttons by their accessibility ids and click them in sequence to perform 88 / 11 = 8
            this.driver.FindElementByAccessibilityId("num8Button").Click();
            this.driver.FindElementByAccessibilityId("num8Button").Click();
            this.driver.FindElementByAccessibilityId("divideButton").Click();
            this.driver.FindElementByAccessibilityId("num1Button").Click();
            this.driver.FindElementByAccessibilityId("num1Button").Click();
            this.driver.FindElementByAccessibilityId("equalButton").Click();
            Assert.AreEqual("8.", GetCalculatorResultText());
        }

        [Test]
        public void Subtraction()
        {
            // Find the buttons by their accessibility ids using XPath and click them in sequence to perform 9 - 1 = 8
            this.driver.FindElementByXPath(".//*[@AutomationId=\"num9Button\"]").Click();
            this.driver.FindElementByXPath("//Button[@AutomationId=\"minusButton\"]").Click();
            this.driver.FindElementByXPath("//Button[@AutomationId=\"num1Button\"]").Click();
            this.driver.FindElementByXPath("//Button[@AutomationId=\"equalButton\"]").Click();
            Assert.AreEqual("8.", GetCalculatorResultText());
        }

        [Test]
        [TestCase("Eins", "Plus", "Sieben", "8.")]
        [TestCase("Neun", "Minus", "Eins", "8.")]
        [TestCase("Acht", "Dividieren durch", "Acht", "1.")]
        public void SolvingAnEquation(string input1, string operation, string input2, string expectedResult)
        {
            // Run sequence of button presses specified above and validate the results
            this.driver.FindElementByName(input1).Click();
            this.driver.FindElementByName(operation).Click();
            this.driver.FindElementByName(input2).Click();
            this.driver.FindElementByName("Gleich").Click();
            Assert.AreEqual(expectedResult, GetCalculatorResultText());
        }

        private string GetCalculatorResultText()
        {
            try
            {
                string calcResult = this.driver.FindElementByAccessibilityId("CalculatorResults").Text;
                return calcResult.Replace("Die Anzeige lautet", string.Empty).Trim();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}