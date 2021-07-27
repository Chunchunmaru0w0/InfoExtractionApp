using System;
using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace InfoExtractionApp
{
    class InfoExtraction
    {
        private static IWebDriver Driver;
        private static WebDriverWait wait;
        private static string spidy = "Spider-Man";
        private static IWebElement firstPara;
        private static IWebElement Image;
         
        public static void Main()
        {
            EnterGoogle();
            var Peppa = SearchAndEnter();
            Peppa.Click();
            firstPara = Driver.FindElement(By.CssSelector(".mw-parser-output > p:nth-child(6)"));
            wait.Until(Driver => Driver.FindElement(By.CssSelector("html.client-js.ve-available body.mediawiki.ltr.sitedir-ltr.mw-hide-empty-elt.ns-0.ns-subject.mw-editable.page-Spider-Man.rootpage-Spider-Man.skin-vector.action-view.skin-vector-legacy div#content.mw-body div#bodyContent.vector-body div#mw-content-text.mw-body-content.mw-content-ltr div.mw-parser-output table.infobox tbody tr td a.image img")).Displayed);
            Image = Driver.FindElement(By.CssSelector("html.client-js.ve-available body.mediawiki.ltr.sitedir-ltr.mw-hide-empty-elt.ns-0.ns-subject.mw-editable.page-Spider-Man.rootpage-Spider-Man.skin-vector.action-view.skin-vector-legacy div#content.mw-body div#bodyContent.vector-body div#mw-content-text.mw-body-content.mw-content-ltr div.mw-parser-output table.infobox tbody tr td a.image img"));
            Console.WriteLine(firstPara.Text);
            Console.WriteLine(Image.GetAttribute("src"));
            ConexionMongoDB();
        }

        public static void EnterGoogle()
        {
            Driver = new FirefoxDriver();
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(17));
            Driver.Navigate().GoToUrl("http://www.google.com");
        }

        public static IWebElement SearchAndEnter()
        {
            Driver.FindElement(By.Name("q")).SendKeys(spidy + Keys.Enter);
            wait.Until(Driver => Driver.FindElement(By.CssSelector("h3")).Displayed);
            IWebElement firstResult = Driver.FindElement(By.CssSelector("h3"));
            return firstResult;
        }

        public static void ConexionMongoDB()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://Admin:confusedghost09.@infoextractiondb.agn1w.mongodb.net/");
            var database = dbClient.GetDatabase("InfoExtractionApp");
            var collection = database.GetCollection<Info>("Info");
            var info = new Info() { Title = spidy, Img = Image.GetAttribute("src"), Paragrah = firstPara.Text };
            collection.InsertOne(info);
        }
    }
}
