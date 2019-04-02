// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Monkey.Sql.AcceptanceTests.Features.Basic
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Basic")]
    public partial class BasicFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Basic.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Basic", "\tIn order to avoid silly mistakes\r\n\tAs a backend-developer\r\n\tI want to be told ho" +
                    "w to get started with Monkey.WebApi\r\n\tAnd expose my backend as webapi easily", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
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
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 7
#line 8
 testRunner.Given("I have my system configured with SqlServer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("I configured basic WebApi features with swagger", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can expose stored procedure to WebApi")]
        public virtual void ICanExposeStoredProcedureToWebApi()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can expose stored procedure to WebApi", null, ((string[])(null)));
#line 12
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql Line"});
            table1.AddRow(new string[] {
                        "CREATE PROCEDURE AddUser @name nvarchar(255), @id int, @birthdate datetime"});
            table1.AddRow(new string[] {
                        "AS"});
            table1.AddRow(new string[] {
                        "BEGIN"});
            table1.AddRow(new string[] {
                        "SELECT @name + \'!\' as Name, 1 as Id, getdate() as BirthDate"});
            table1.AddRow(new string[] {
                        "END"});
#line 13
 testRunner.Given("I have a stored procedure with name \'AddUser\' in \'Test\' database", ((string)(null)), table1, "Given ");
#line 21
 testRunner.And("I have mapped \'AddUser\' procedure from \'Test\' database in apidatabase in schema \'" +
                    "dbo\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "SqlParameterName",
                        "SqlType",
                        "PropertyName",
                        "PropertyType"});
            table2.AddRow(new string[] {
                        "@id",
                        "int",
                        "Id",
                        "int"});
            table2.AddRow(new string[] {
                        "@name",
                        "nvarchar",
                        "Name",
                        "string"});
            table2.AddRow(new string[] {
                        "@birthdate",
                        "datetime",
                        "BirthDate",
                        "DateTime"});
#line 22
 testRunner.And("I have mapped parameters to command \'AddUserCommand\'", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "SqlColumnName",
                        "SqlColumnType",
                        "PropertyName",
                        "PropertyType"});
            table3.AddRow(new string[] {
                        "Id",
                        "int",
                        "Id",
                        "int"});
            table3.AddRow(new string[] {
                        "Name",
                        "nvarchar",
                        "Name",
                        "string"});
            table3.AddRow(new string[] {
                        "BirthDate",
                        "datetime",
                        "BirthDate",
                        "DateTime"});
#line 28
 testRunner.And("I have mapped resultset \'UserEntity\'", ((string)(null)), table3, "And ");
#line 34
 testRunner.And("I bind that procedure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.When("a commandhandler is generated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 36
 testRunner.And("It is executed with command \'[ \"Id\": 1, \"Name\": \"John\", \"BirthDate\": \"2019-04-01\"" +
                    " ]\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
