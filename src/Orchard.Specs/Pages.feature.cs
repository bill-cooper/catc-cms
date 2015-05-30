﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34014
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Orchard.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Pages")]
    public partial class PagesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Pages.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Pages", "  In order to add content pages to my site\r\n  As an author\r\n  I want to create, p" +
                    "ublish and edit pages", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("In the admin (menu) there is a link to create a Page")]
        public virtual void InTheAdminMenuThereIsALinkToCreateAPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("In the admin (menu) there is a link to create a Page", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
    testRunner.Given("I have installed Orchard", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
    testRunner.When("I go to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
    testRunner.Then("I should see \"<a href=\"/Admin/Contents/Create/Page\"[^>]*>Page</a>\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
    testRunner.When("I go to \"Admin/Contents/Create/Page\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table1.AddRow(new string[] {
                        "Title.Title",
                        "Super Duper"});
            table1.AddRow(new string[] {
                        "LayoutPart.State",
                        "{ \"elements\": [ { \"typeName\": \"Orchard.Layouts.Elements.Text\", \"state\": \"Content=" +
                            "This+is+super.\"} ] }"});
#line 14
        testRunner.And("I fill in", ((string)(null)), table1, "And ");
#line 18
        testRunner.And("I hit \"Publish Now\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
        testRunner.And("I go to \"super-duper\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.Then("I should see \"<h1[^>]*>.*?Super Duper.*?</h1>\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 21
        testRunner.And("I should see \"This is super.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
    testRunner.When("I go to \"Admin/Contents/Create/Page\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table2.AddRow(new string[] {
                        "Title.Title",
                        "Super Duper"});
            table2.AddRow(new string[] {
                        "LayoutPart.State",
                        "{ \"elements\": [ { \"typeName\": \"Orchard.Layouts.Elements.Text\", \"state\": \"Content=" +
                            "This+is+super+number+two.\"} ] }"});
#line 25
        testRunner.And("I fill in", ((string)(null)), table2, "And ");
#line 29
        testRunner.And("I hit \"Publish Now\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
        testRunner.And("I go to \"super-duper-2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
    testRunner.Then("I should see \"<h1[^>]*>.*?Super Duper.*?</h1>\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
        testRunner.And("I should see \"This is super number two.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
    testRunner.When("I go to \"Admin/Contents/Create/Page\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table3.AddRow(new string[] {
                        "Title.Title",
                        "Another"});
            table3.AddRow(new string[] {
                        "LayoutPart.State",
                        "{ \"elements\": [ { \"typeName\": \"Orchard.Layouts.Elements.Text\", \"state\": \"Content=" +
                            "This+is+the+draft+of+a+new+homepage.\"} ] }"});
            table3.AddRow(new string[] {
                        "Autoroute.PromoteToHomePage",
                        "true"});
#line 36
        testRunner.And("I fill in", ((string)(null)), table3, "And ");
#line 41
        testRunner.And("I hit \"Publish Now\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
        testRunner.And("I go to \"/\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.Then("I should see \"<h1>Another</h1>\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
    testRunner.When("I go to \"another\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
    testRunner.Then("the status should be 404 \"Not Found\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 48
    testRunner.When("I go to \"Admin/Contents/Create/Page\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table4.AddRow(new string[] {
                        "Title.Title",
                        "Drafty"});
            table4.AddRow(new string[] {
                        "LayoutPart.State",
                        "{ \"elements\": [ { \"typeName\": \"Orchard.Layouts.Elements.Text\", \"state\": \"Content=" +
                            "This+is+the+draft+of+a+new+homepage.\"} ] }"});
            table4.AddRow(new string[] {
                        "Autoroute.PromoteToHomePage",
                        "true"});
#line 49
        testRunner.And("I fill in", ((string)(null)), table4, "And ");
#line 54
        testRunner.And("I hit \"Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
        testRunner.And("I go to \"/\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
    testRunner.Then("I should see \"<h1>Another</h1>\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
