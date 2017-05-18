﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UITests.Api;
using Microsoft.Dynamics365.UITests.Browser;
using System;
using System.Security;
using System.Threading;
using OpenQA.Selenium.Support.Events;
using System.Drawing.Imaging;

namespace Microsoft.Dynamics365.UITests.UnitTests
{
    [TestClass]
    public class RelatedTest
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void AccountRelated()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();
                
                Thread.Sleep(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                             
                Thread.Sleep(3000);
                xrmBrowser.Grid.OpenRecord(0);
                xrmBrowser.Navigation.OpenRelated("Cases");
                xrmBrowser.Related.Sort("createdon"); 

                xrmBrowser.Related.SwitchView("Active Cases");

                xrmBrowser.Related.Search("P");

                //TODO: This is commented out until the ClickCommand is supported for related frame.
                //xrmBrowser.CommandBar.ClickCommand("ADD NEW CASE");
                //xrmBrowser.Related.OpenRecord(0);
            }
        }
    }
}