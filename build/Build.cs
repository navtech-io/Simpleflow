using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;


class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution("Simpleflow.sln")] readonly Solution Solution;
    //[GitRepository] readonly GitRepository GitRepository;
    //[GitVersion] readonly GitVersion GitVersion;

    [Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Parameter][Secret] string NugetApiKey;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "test";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath NugetDirectory => ArtifactsDirectory / "nuget";
    GitHubActions GitHubActions => GitHubActions.Instance;
    

    Target Print => _ => _
        .Executes(() =>
        {
            Log.Information("Branch = {Branch}", GitHubActions.Ref);
            Log.Information("Commit = {Commit}", GitHubActions.Sha);
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {

            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            // Clean 

            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                //.SetAssemblyVersion(GitVersion.AssemblySemVer)
                //.SetFileVersion(GitVersion.AssemblySemFileVer)
                //.SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });


    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject("Simpleflow.Tests"))
                .SetConfiguration(Configuration)
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {

            //int commitNum = 0;
            //string NuGetVersionCustom = GitVersion.NuGetVersionV2;

            ////if it's not a tagged release - append the commit number to the package version
            ////tagged commits on master have versions
            //// - v0.3.0-beta
            ////other commits have
            //// - v0.3.0-beta1

            //if (Int32.TryParse(GitVersion.CommitsSinceVersionSource, out commitNum))
            //    NuGetVersionCustom = commitNum > 0 ? NuGetVersionCustom + $"{commitNum}" : NuGetVersionCustom;

            DotNetPack(s => s
                .SetProject(Solution.GetProject("Simpleflow"))
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetTitle("Simpleflow")
                .SetAuthors("navtech.io")
                .SetCopyright("navtech.io")
                .SetPackageProjectUrl("https://github.com/navtech-io/Simpleflow")
                .AddProperty("PackageLicenseExpression", "Apache-2.0")
                .AddProperty("PackageIcon", @"PackageIcon.png")
                .SetPackageRequireLicenseAcceptance(true)
                .SetIncludeSymbols(true)
                .SetDescription("Build dynamic rules and workflows using script")
                .SetPackageTags("Simpleflow.NET Workflow RuleEngine DynamicExpressionEvaluator")
                .SetNoDependencies(true)
                .SetOutputDirectory(ArtifactsDirectory / "nuget"));
        });

    Target Publish => _ => _
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {

            GlobFiles(NugetDirectory, "*.nupkg")
                //.NotEmpty()
                .Where(x => !x.EndsWith("symbols.nupkg"))
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(NugetApiUrl)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });

    
}
