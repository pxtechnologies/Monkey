var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var binDirectory = Directory("./../../bin");
var zipTarget = "./../../bundle/Monkey.Sql.WebApiHost.zip";
var webApiHostProj = "./Monkey.Sql.WebApiHost/Monkey.Sql.WebApiHost.csproj";

// Deletes the contents of the Artifacts folder if it contains anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(binDirectory);
    });



// Build using the build configuration specified as an argument.
 Task("Build")
    .Does(() =>
    {
        DotNetCoreBuild(webApiHostProj,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });
Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore(webApiHostProj);
    });

Task("Publish")
    .Does(() =>
    {
        DotNetCorePublish(webApiHostProj,
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                OutputDirectory = binDirectory,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("Zip")
    .Does(() => {
        var files = GetFiles(binDirectory.ToString() + "/**/*.*");
        Zip(binDirectory, zipTarget, files);     
    });

Task("Publish-Zip")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Publish")
    .IsDependentOn("Zip");

RunTarget(target);