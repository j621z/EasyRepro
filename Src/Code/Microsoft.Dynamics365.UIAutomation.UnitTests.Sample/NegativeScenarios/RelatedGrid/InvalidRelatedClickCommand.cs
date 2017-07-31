﻿using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.UnitTests.Sample.NegativeScenarios.RelatedGrid
{
    [TestClass]
public class InvalidRelatedClickCommand
{
    private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
    private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
    private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

    [TestMethod]
    public void TestInvalidRelatedClickCommand()
    {
        using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
        {
            xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

            xrmBrowser.GuidedHelp.CloseGuidedHelp();

            xrmBrowser.ThinkTime(500);
            xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

            xrmBrowser.ThinkTime(3000);
            xrmBrowser.Grid.OpenRecord(0);
            xrmBrowser.Navigation.OpenRelated("Cases");
            xrmBrowser.Related.Sort("createdo1n");

            xrmBrowser.Related.SwitchView("Active Cases");

            xrmBrowser.Related.Search("F");

            xrmBrowser.Related.ClickCommand("ADD");
            xrmBrowser.QuickCreate.Cancel();
            xrmBrowser.ThinkTime(2000);

        }
    }
}
}
