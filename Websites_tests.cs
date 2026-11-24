using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Lab3
{
    public class Websites_tests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private const string BaseUrl = "https://the-internet.herokuapp.com";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        // 1. Forgot Password
        [Test]
        public void ForgotPassword_ShouldSubmitEmail()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/forgot_password");

            var emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("test@example.com");
            driver.FindElement(By.Id("form_submit")).Click();

            Assert.That(driver.PageSource, Is.Not.Empty);
        }

        // 2. Horizontal Slider
        [Test]
        public void HorizontalSlider_ShouldChangeValue()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/horizontal_slider");

            var slider = driver.FindElement(By.CssSelector("input[type='range']"));
            var actions = new Actions(driver);
            actions.ClickAndHold(slider).MoveByOffset(40, 0).Release().Perform();

            var rangeValue = driver.FindElement(By.Id("range")).Text;
            Assert.That(rangeValue, Is.Not.EqualTo("0"));
        }

        // 3. Dropdown
        [Test]
        public void Dropdown_ShouldSelectOption()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/dropdown");

            var selectElement = new SelectElement(driver.FindElement(By.Id("dropdown")));
            selectElement.SelectByValue("2");

            Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 2"));
        }

        // 4. Typos
        [Test]
        public void Typos_ShouldDisplayParagraphText()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/typos");

            var paragraph = driver.FindElement(By.CssSelector(".example p:nth-of-type(2)")).Text;
            Assert.That(paragraph, Does.Contain("Sometimes you'll see a typo"));
        }

        // 5. Entry Ad
        [Test]
        public void EntryAd_ShouldCloseModal()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/entry_ad");

            var closeBtn = wait.Until(d =>
            {
                var el = d.FindElement(By.CssSelector(".modal-footer p"));
                return el.Displayed ? el : null;
            });
            closeBtn!.Click();

            wait.Until(d => !d.FindElement(By.Id("modal")).Displayed);
            Assert.That(driver.FindElement(By.Id("modal")).Displayed, Is.False);
        }

        // 6. File Download
        [Test]
        public void FileDownload_ShouldShowDownloadableFiles()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/download");

            var firstFileLink = wait.Until(d => d.FindElement(By.CssSelector(".example a")));
            Assert.That(firstFileLink.Displayed, Is.True);
            firstFileLink.Click();

            Assert.That(firstFileLink.GetAttribute("href"), Does.Contain("/download/"));
        }

        // 7. Basic Authentication
        [Test]
        public void BasicAuth_ShouldAuthenticateWithValidCredentials()
        {
            driver.Navigate().GoToUrl("https://admin:admin@the-internet.herokuapp.com/basic_auth");

            var message = driver.FindElement(By.CssSelector(".example p")).Text;
            Assert.That(message, Does.Contain("Congratulations"));
        }

        // 8. Dynamic Loading
        [Test]
        public void DynamicLoading_ShouldRevealHiddenText()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/dynamic_loading/1");

            driver.FindElement(By.CssSelector("#start button")).Click();
            var finishText = wait.Until(d =>
            {
                var el = d.FindElement(By.CssSelector("#finish h4"));
                return el.Displayed ? el.Text : null;
            });

            Assert.That(finishText, Is.EqualTo("Hello World!"));
        }

        // 9. Context Menu
        [Test]
        public void ContextMenu_ShouldTriggerAndAcceptAlert()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/context_menu");

            var hotSpot = driver.FindElement(By.Id("hot-spot"));
            var actions = new Actions(driver);
            actions.ContextClick(hotSpot).Perform();

            var alert = wait.Until(d => d.SwitchTo().Alert());
            Assert.That(alert.Text, Does.Contain("context menu"));
            alert.Accept();
        }

        // 10. Redirect Link
        [Test]
        public void RedirectLink_ShouldNavigateToStatusCodes()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/redirector");

            driver.FindElement(By.Id("redirect")).Click();
            wait.Until(d => d.Url.Contains("/status_codes"));

            Assert.That(driver.Url, Does.Contain("/status_codes"));
        }
    }
}