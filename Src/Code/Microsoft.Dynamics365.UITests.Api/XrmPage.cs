﻿using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Dynamics365.UITests.Browser;


namespace Microsoft.Dynamics365.UITests.Api
{
    public class XrmPage : BrowserPage
    {

        private static string relatedId;

        public XrmPage(InteractiveBrowser browser) : base(browser)
        {
        }

        /// <summary>
        /// Sets the value of a Checkbox field.
        /// </summary>
        /// <param name="field">Field name or ID.</param>
        /// <param name="check">If set to <c>true</c> [check].</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(string field, bool check)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, check);
            return this.Execute($"Set Value: {field}", driver =>
            {
                if (driver.HasElement(By.Id("int_" + field)))
                {
                    var input = driver.FindElement(By.Id("int_" + field));

                    if (bool.Parse(input.FindElement(By.TagName("input")).GetAttribute("checked")) && !check)
                        input.FindElement(By.TagName("input")).Click();
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(string field, DateTime date)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, date);
            return this.Execute($"Set Value: {field}", driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var fieldElement = driver.FindElement(By.Id(field));
                    fieldElement.Click();

                    //Check to see if focus is on field already
                    if (fieldElement.FindElement(By.ClassName("ms-crm-Inline-Edit")) != null)
                        fieldElement.FindElement(By.ClassName("ms-crm-Inline-Edit")).Click();
                    else
                        fieldElement.FindElement(By.ClassName("ms-crm-Inline-Value")).Click();

                    var input = fieldElement.FindElement(By.TagName("input"));

                    if (input.GetAttribute("value").Length > 0)
                    {
                        input.Clear();
                        fieldElement.Click();
                        input.Click();
                    }

                    input.SendKeys(date.ToShortDateString());
                    

                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Text/Description field.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(string field, string value)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, value);
            return this.Execute($"Set Value: {field}", driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var fieldElement = driver.FindElement(By.Id(field));
                    fieldElement.Click();

                    //Check to see if focus is on field already
                    if (fieldElement.FindElement(By.ClassName("ms-crm-Inline-Edit")) != null)
                        fieldElement.FindElement(By.ClassName("ms-crm-Inline-Edit")).Click();
                    else
                        fieldElement.FindElement(By.ClassName("ms-crm-Inline-Value")).Click();

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                        fieldElement.FindElement(By.TagName("textarea")).SendKeys(value);
                    }
                    else
                    {
                        fieldElement.FindElement(By.TagName("input")).Clear();
                        fieldElement.FindElement(By.TagName("input")).SendKeys(value);

                    }


                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Field.
        /// </summary>
        /// <param name="field">The field .</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(Field field)
        {
            return this.Execute($"Set Value: {field.Name}", driver =>
            {
                if (driver.HasElement(By.Id(field.Id)))
                {
                    var fieldElement = driver.FindElement(By.Id(field.Id));
                    fieldElement.Click();

                    //Check to see if focus is on field already
                    if (fieldElement.FindElement(By.ClassName("ms-crm-Inline-Edit")) != null)
                        fieldElement.FindElement(By.ClassName("ms-crm-Inline-Edit")).Click();
                    else
                        fieldElement.FindElement(By.ClassName("ms-crm-Inline-Value")).Click();


                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                        fieldElement.FindElement(By.TagName("textarea")).SendKeys(field.Value);
                    }
                    else
                    {
                        fieldElement.FindElement(By.TagName("input")).Clear();
                        fieldElement.FindElement(By.TagName("input")).SendKeys(field.Value);
                    }

                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute($"Set Value: {option.Name}", driver =>
            {
                if (driver.HasElement(By.Id(option.Name)))
                {
                    var input = driver.FindElement(By.Id(option.Name));
                    input.Click();

                    var select = input.FindElement(By.TagName("select"));
                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text == option.Value || op.GetAttribute("value") == option.Value)
                            op.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Composite control.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(CompositeControl control)
        {
            return this.Execute($"Set Conposite Control Value: {control.Id}", driver =>
            {
                if (!driver.HasElement(By.Id(control.Id)))
                    return false;

                driver.FindElement(By.Id(control.Id)).Click();

                if (driver.HasElement(By.Id(control.Id + "_compositionLinkControl_flyoutLoadingArea_flyOut")))
                {
                    var compcntrl =
                        driver.FindElement(By.Id(control.Id + "_compositionLinkControl_flyoutLoadingArea_flyOut"));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id("fullname_compositionLinkControl_" + field.Id)).Click();

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));

                        result?.Clear();
                        result?.SendKeys(field.Value);
                    }

                    compcntrl.FindElement(By.Id(control.Id + "_compositionLinkControl_flyoutLoadingArea-confirm")).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetValue(Lookup control)
        {
            return this.Execute($"Set Lookup Value: {control.Name}", driver =>
            {
                if (driver.HasElement(By.Id(control.Name)))
                {
                    var input = driver.FindElement(By.Id(control.Name));
                    input.Click();

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {control.Name} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    var dialogName = $"Dialog_{control.Name}_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if(control.Value != null)
                    {
                        if (!dialogItems.Keys.Contains(control.Value))
                            throw new InvalidOperationException($"List does not have {control.Value}.");

                        var dialogItem = dialogItems[control.Value];
                        dialogItem.Click();
                    }
                    else
                    { 
                        if (dialogItems.Count < control.Index)
                            throw new InvalidOperationException($"List does not have {control.Index + 1} items.");

                        var dialogItem = dialogItems.Values.ToList()[control.Index];
                        dialogItem.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {control.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Gets the value of a Text/Description field.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        //public BrowserCommandResult<string> GetValue(string field)
        //{
        //    //return this.Execute($"Set Value: {field}", SetValue, field, value);
        //    return this.Execute($"Get Value: {field}", driver =>
        //    {
        //        if (driver.HasElement(By.Id(field)))
        //        {
        //            var fieldElement = driver.FindElement(By.Id(field));
        //            fieldElement.Click();

        //            if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
        //            {
        //                var fieldValue = fieldElement.FindElement(By.TagName("textarea")).Text;
        //            }
        //            else
        //            {
        //                var link = fieldElement.FindElement(By.ClassName("ms-crm-gridurl"));
        //                if (link != null)
        //                {
        //                    var fieldValue = link.Text;
        //                }
        //                else
        //                {
        //                    return fieldElement.FindElement(By.TagName("label")).Text;
        //                }

        //            }

        //           // var text = fieldElement.FindElement(By.ClassName("ms-crm-gridurl")).Text;

        //        }
        //        else
        //            throw new InvalidOperationException($"Field: {field} Does not exist");

        //        return true;
        //    });
        //}

        /// <summary>
        /// Switches to content frame in the CRM application.
        /// </summary>
        /// <returns></returns>
        public bool SwitchToContentFrame()
        {
            return this.Execute("Switch to content frame", driver =>
            {
                driver.SwitchTo().DefaultContent();
                //wait for the content panel to render
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.ContentPanel]));

                //find the crmContentPanel and find out what the current content frame ID is - then navigate to the current content frame
                var currentContentFrame = driver.FindElement(By.XPath(Elements.Xpath[Reference.Frames.ContentPanel]))
                                                .GetAttribute(Elements.ElementId[Reference.Frames.ContentFrameId]);

                driver.SwitchTo().Frame(currentContentFrame);

                return true;
            });
        }

        /// <summary>
        /// Switches to dialog frame in the CRM application.
        /// </summary>
        /// <returns></returns>
        public bool SwitchToDialogFrame()
        {
            return this.Execute("Switch to dialog frame", driver =>
            {
                driver.SwitchTo().DefaultContent();
                //wait for the content panel to render
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame]));

                driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.DialogFrameId]);

                return true;
            });
        }

        /// <summary>
        /// Switches to Quick Find frame in the CRM application.
        /// </summary>
        /// <returns></returns>
        public bool SwitchToQuickFindFrame()
        {
            return this.Execute("Switch to QuickFind Frame", driver =>
            {
                driver.SwitchTo().DefaultContent();
                //wait for the content panel to render
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.QuickFindFrame]));

                driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.QuickFindFrameId]);

                return true;
            });
        }

        /// <summary>
        /// Switches to related frame in the CRM application.
        /// </summary>
        /// <returns></returns>
        public bool SwitchToRelatedFrame()
        {

            SwitchToContentFrame();

            return this.Execute("Switch to Related Frame", driver =>
            {
                //wait for the content panel to render
                driver.WaitUntilAvailable(By.Id(relatedId));

                driver.SwitchTo().Frame(relatedId + "Frame");

                return true;
            });
        }

        public bool SwitchToDefaultContent()
        {
            return this.Execute("Switch to Default Content", driver =>
            {
                driver.SwitchTo().DefaultContent();

                return true;
            });
        }

        internal BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                0,
                0,
                null,
                false,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        private BrowserCommandResult<Dictionary<string, IWebElement>> OpenDialog(IWebElement dialog)
        {
            var dictionary = new Dictionary<string, IWebElement>();
            var dialogItems = dialog.FindElements(By.TagName("li"));

            foreach (var dialogItem in dialogItems)
            {
                if (dialogItem.GetAttribute("role") != null && dialogItem.GetAttribute("role") == "menuitem")
                {
                    var links = dialogItem.FindElements(By.TagName("a"));

                    if (links != null && links.Count > 1)
                    {
                        var title = links[1].GetAttribute("title");

                        dictionary.Add(title, links[1]);
                    }
                }
            }



            return dictionary;
        }

    }
}
