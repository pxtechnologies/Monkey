﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Monkey.Sql.WebApiHost.AcceptanceTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("PrimitiveInvocations")]
    public partial class PrimitiveInvocationsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PrimitiveInvocations.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PrimitiveInvocations", "In order to avoid silly mistakes\r\nAs a SQL developer\r\nI want to be told how to ex" +
                    "pose simple stored procedures as web-api", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
 testRunner.Given("the \'Test\' database is created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
 testRunner.And("the \'Monkey\' database is created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.And("WebApiHost has started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("Monkey was installed in \'Test\' database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to invoke stored procedure with different primitive parameters")]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "tinyint", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123}", "{\"resultNumber\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "smallint", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123}", "{\"resultNumber\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "int", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123}", "{\"resultNumber\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "bigint", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123}", "{\"resultNumber\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "numeric(10,2)", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123.5}", "{\"resultNumber\":123.50}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "money", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123.5}", "{\"resultNumber\":123.5000}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "smallmoney", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123.5}", "{\"resultNumber\":123.5000}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "real", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123.1}", "{\"resultNumber\":123.1}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "float", "@number", "ResultNumber", "POST", "api/Product/", "{\"number\":123.1}", "{\"resultNumber\":123.1}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "nvarchar(255)", "@name", "ResultName", "POST", "api/Product/", "{\"name\":\"John\"}", "{\"resultName\":\"John\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "varchar(255)", "@name", "ResultName", "POST", "api/Product/", "{\"name\":\"John\"}", "{\"resultName\":\"John\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "char(255)", "@name", "ResultName", "POST", "api/Product/", "{\"name\":\"John\"}", "{\"resultName\":\"John\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "nchar(255)", "@name", "ResultName", "POST", "api/Product/", "{\"name\":\"John\"}", "{\"resultName\":\"John\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "text", "@name", "ResultName", "POST", "api/Product/", "{\"name\":\"John\"}", "{\"resultName\":\"John\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "ntext", "@name", "ResultName", "POST", "api/Product/", "{\"name\":\"John\"}", "{\"resultName\":\"John\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "decimal", "@value", "ResultValue", "POST", "api/Product/", "{\"value\":\"1.0\"}", "{\"resultValue\":1.0}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "uniqueidentifier", "@sku", "ResutSku", "POST", "api/Product/", "{\"sku\":\"B915B92A-8E13-4763-8F4B-2DDF5CE09076\"}", "{\"resutSku\":\"b915b92a-8e13-4763-8f4b-2ddf5ce09076\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "time", "@time", "ResultTime", "POST", "api/Product/", "{\"time\":\"11:22\"}", "{\"resultTime\":\"11:22:00\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "datetime", "@date", "ResultDate", "POST", "api/Product/", "{\"date\":\"2019-04-01 11:22\"}", "{\"resultDate\":\"2019-04-01T11:22:00\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "datetime2", "@date", "ResultDate", "POST", "api/Product/", "{\"date\":\"2019-04-01 11:22\"}", "{\"resultDate\":\"2019-04-01T11:22:00\"}", null)]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "datetimeoffset", "@date", "ResultDate", "POST", "api/Product/", "{\"date\":\"2019-04-01 11:22\"}", "{\"resultDate\":\"2019-04-01T11:22:00+00:00\"}", null)]
        public virtual void IWantToInvokeStoredProcedureWithDifferentPrimitiveParameters(string procedureName, string paramType, string paramName, string resultColumnName, string httpMethod, string url, string requestPayload, string responsePayload, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to invoke stored procedure with different primitive parameters", exampleTags);
#line 12
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table1.AddRow(new string[] {
                        string.Format("CREATE OR ALTER PROC {0} {1} {2}", procedureName, paramName, paramType)});
            table1.AddRow(new string[] {
                        "AS"});
            table1.AddRow(new string[] {
                        "BEGIN"});
            table1.AddRow(new string[] {
                        string.Format("SELECT {0} as {1};", paramName, resultColumnName)});
            table1.AddRow(new string[] {
                        "END"});
#line 13
 testRunner.Given("I executed a script against \'Test\' database:", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table2.AddRow(new string[] {
                        string.Format("EXEC webapi_BindStoredProc \'{0}\',\'Test\';", procedureName)});
#line 21
 testRunner.And("I expose the procedure with sql statement on \'Test\' database:", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table3.AddRow(new string[] {
                        "EXEC webapi_Publish;"});
#line 25
 testRunner.When("I publish WebApi on \'Test\' database with sql statement:", ((string)(null)), table3, "When ");
#line 29
 testRunner.And(string.Format("I invoke WebApi with \'{0}\' request on \'{1}\' with data \'{2}\'", httpMethod, url, requestPayload), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.Then(string.Format("I expect a response from url \'{0}\' with data \'{1}\'", url, responsePayload), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to map stored procedure according to REST conventions")]
        [NUnit.Framework.TestCaseAttribute("AddProduct", "nvarchar(255)", "@name", "@number", "int", "Name", "Number", "POST", "api/Product", "{\"name\":\"pc\",\"number\":123}", "{\"name\":\"pc\",\"number\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("CreateProduct", "nvarchar(255)", "@name", "@number", "int", "Name", "Number", "POST", "api/Product", "{\"name\":\"pc\",\"number\":123}", "{\"name\":\"pc\",\"number\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("InsertProduct", "nvarchar(255)", "@name", "@number", "int", "Name", "Number", "POST", "api/Product", "{\"name\":\"pc\",\"number\":123}", "{\"name\":\"pc\",\"number\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("ModifyProduct", "nvarchar(255)", "@id", "@number", "int", "Name", "Number", "PUT", "api/Product/pc", "{\"number\":123}", "{\"name\":\"pc\",\"number\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("EditProduct", "nvarchar(255)", "@id", "@number", "int", "Name", "Number", "PUT", "api/Product/pc", "{\"number\":123}", "{\"name\":\"pc\",\"number\":123}", null)]
        [NUnit.Framework.TestCaseAttribute("UpdateProduct", "nvarchar(255)", "@id", "@number", "int", "Name", "Number", "PUT", "api/Product/pc", "{\"number\":123}", "{\"name\":\"pc\",\"number\":123}", null)]
        public virtual void IWantToMapStoredProcedureAccordingToRESTConventions(string procedureName, string paramType, string paramName, string paramName2, string paramType2, string resultColumnName, string resultColumnName2, string httpMethod, string url, string requestPayload, string responsePayload, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to map stored procedure according to REST conventions", exampleTags);
#line 56
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table4.AddRow(new string[] {
                        string.Format("CREATE OR ALTER PROC {0} {1} {2}, {3} {4}", procedureName, paramName, paramType, paramName2, paramType2)});
            table4.AddRow(new string[] {
                        "AS"});
            table4.AddRow(new string[] {
                        "BEGIN"});
            table4.AddRow(new string[] {
                        string.Format("SELECT {0} as {1}, {2} as {3};", paramName, resultColumnName, paramName2, resultColumnName2)});
            table4.AddRow(new string[] {
                        "END"});
#line 57
 testRunner.Given("I executed a script against \'Test\' database:", ((string)(null)), table4, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table5.AddRow(new string[] {
                        string.Format("EXEC webapi_BindStoredProc \'{0}\',\'Test\';", procedureName)});
#line 65
 testRunner.And("I expose the procedure with sql statement on \'Test\' database:", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table6.AddRow(new string[] {
                        "EXEC webapi_Publish;"});
#line 69
 testRunner.When("I publish WebApi on \'Test\' database with sql statement:", ((string)(null)), table6, "When ");
#line 73
 testRunner.And(string.Format("I invoke WebApi with \'{0}\' request on \'{1}\' with data \'{2}\'", httpMethod, url, requestPayload), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.Then(string.Format("I expect a response from url \'{0}\' with data \'{1}\'", url, responsePayload), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to pass nulls and retrive nulls")]
        public virtual void IWantToPassNullsAndRetriveNulls()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to pass nulls and retrive nulls", ((string[])(null)));
#line 86
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table7.AddRow(new string[] {
                        "CREATE OR ALTER PROC AddProduct @name nvarchar(255)"});
            table7.AddRow(new string[] {
                        "AS"});
            table7.AddRow(new string[] {
                        "BEGIN"});
            table7.AddRow(new string[] {
                        "if @name is not null throw 51000, \'name is not null\',1;"});
            table7.AddRow(new string[] {
                        "SELECT \'Tv\' as Name, null as Company;"});
            table7.AddRow(new string[] {
                        "END"});
#line 87
 testRunner.Given("I executed a script against \'Test\' database:", ((string)(null)), table7, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table8.AddRow(new string[] {
                        "EXEC webapi_BindStoredProc \'AddProduct\',\'Test\';"});
#line 96
 testRunner.And("I expose the procedure with sql statement on \'Test\' database:", ((string)(null)), table8, "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table9.AddRow(new string[] {
                        "EXEC webapi_Publish;"});
#line 100
 testRunner.When("I publish WebApi on \'Test\' database with sql statement:", ((string)(null)), table9, "When ");
#line 104
 testRunner.And("I invoke WebApi with \'POST\' request on \'api/Product\' with data \'{}\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.Then("I expect a response from url \'api/Product\' with data \'{\"name\":\"Tv\",\"company\":null" +
                    "}\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to retrive many records from procedure execution")]
        public virtual void IWantToRetriveManyRecordsFromProcedureExecution()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to retrive many records from procedure execution", ((string[])(null)));
#line 107
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table10.AddRow(new string[] {
                        "CREATE OR ALTER PROC GetProducts @name nvarchar(255)"});
            table10.AddRow(new string[] {
                        "AS"});
            table10.AddRow(new string[] {
                        "BEGIN"});
            table10.AddRow(new string[] {
                        "SELECT @name as Name"});
            table10.AddRow(new string[] {
                        "UNION ALL SELECT \'Two\'"});
            table10.AddRow(new string[] {
                        "END"});
#line 108
testRunner.Given("I executed a script against \'Test\' database:", ((string)(null)), table10, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table11.AddRow(new string[] {
                        "EXEC webapi_BindStoredProc \'GetProducts\',\'Test\';"});
#line 117
 testRunner.And("I expose the procedure with sql statement on \'Test\' database:", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sql"});
            table12.AddRow(new string[] {
                        "EXEC Publish;"});
#line 121
 testRunner.When("I publish WebApi on \'Monkey\' database with sql statement:", ((string)(null)), table12, "When ");
#line 125
 testRunner.And("I invoke WebApi with \'GET\' request on \'api/Product?name=tv\' without data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
 testRunner.Then("I expect a response from url \'api/Product\' with data \'[{\"name\":\"tv\"},{\"name\":\"Two" +
                    "\"}]\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

