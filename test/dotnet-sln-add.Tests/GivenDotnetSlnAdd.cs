// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Build.Construction;
using Microsoft.DotNet.Cli.Sln.Internal;
using Microsoft.DotNet.Tools;
using Microsoft.DotNet.Tools.Test.Utilities;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Cli.Sln.Add.Tests
{
    public class GivenDotnetSlnAdd : TestBase
    {
        private string HelpText = @".NET Add project(s) to a solution file Command

Usage: dotnet sln <SLN_FILE> add [options] <args>

Arguments:
  <SLN_FILE>   Solution file to operate on. If not specified, the command will search the current directory for one.
  <args>       Add one or more specified projects to the solution.

Options:
  -h, --help   Show help information.
";

        private const string SlnCommandHelpText = @".NET modify solution file command

Usage: dotnet sln [options] <SLN_FILE> [command]

Arguments:
  <SLN_FILE>   Solution file to operate on. If not specified, the command will search the current directory for one.

Options:
  -h, --help   Show help information.

Commands:
  add <args>      .NET Add project(s) to a solution file Command
  list            .NET List project(s) in a solution file Command
  remove <args>   .NET Remove project(s) from a solution file Command
";

        private ITestOutputHelper _output;

        public GivenDotnetSlnAdd(ITestOutputHelper output)
        {
            _output = output;
        }

        private const string ExpectedSlnFileAfterAddingLibProj = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.26006.2
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""App"", ""App\App.csproj"", ""{7072A694-548F-4CAE-A58F-12D257D5F486}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Lib"", ""Lib\Lib.csproj"", ""__LIB_PROJECT_GUID__""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Debug|x64 = Debug|x64
		Debug|x86 = Debug|x86
		Release|Any CPU = Release|Any CPU
		Release|x64 = Release|x64
		Release|x86 = Release|x86
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x64.ActiveCfg = Debug|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x64.Build.0 = Debug|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x86.ActiveCfg = Debug|x86
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x86.Build.0 = Debug|x86
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|Any CPU.Build.0 = Release|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x64.ActiveCfg = Release|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x64.Build.0 = Release|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x86.ActiveCfg = Release|x86
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x86.Build.0 = Release|x86
		__LIB_PROJECT_GUID__.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		__LIB_PROJECT_GUID__.Debug|Any CPU.Build.0 = Debug|Any CPU
		__LIB_PROJECT_GUID__.Debug|x64.ActiveCfg = Debug|x64
		__LIB_PROJECT_GUID__.Debug|x64.Build.0 = Debug|x64
		__LIB_PROJECT_GUID__.Debug|x86.ActiveCfg = Debug|x86
		__LIB_PROJECT_GUID__.Debug|x86.Build.0 = Debug|x86
		__LIB_PROJECT_GUID__.Release|Any CPU.ActiveCfg = Release|Any CPU
		__LIB_PROJECT_GUID__.Release|Any CPU.Build.0 = Release|Any CPU
		__LIB_PROJECT_GUID__.Release|x64.ActiveCfg = Release|x64
		__LIB_PROJECT_GUID__.Release|x64.Build.0 = Release|x64
		__LIB_PROJECT_GUID__.Release|x86.ActiveCfg = Release|x86
		__LIB_PROJECT_GUID__.Release|x86.Build.0 = Release|x86
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
";

        private const string ExpectedSlnFileAfterAddingLibProjToEmptySln = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.26006.2
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Lib"", ""Lib\Lib.csproj"", ""__LIB_PROJECT_GUID__""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Debug|x64 = Debug|x64
		Debug|x86 = Debug|x86
		Release|Any CPU = Release|Any CPU
		Release|x64 = Release|x64
		Release|x86 = Release|x86
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		__LIB_PROJECT_GUID__.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		__LIB_PROJECT_GUID__.Debug|Any CPU.Build.0 = Debug|Any CPU
		__LIB_PROJECT_GUID__.Debug|x64.ActiveCfg = Debug|x64
		__LIB_PROJECT_GUID__.Debug|x64.Build.0 = Debug|x64
		__LIB_PROJECT_GUID__.Debug|x86.ActiveCfg = Debug|x86
		__LIB_PROJECT_GUID__.Debug|x86.Build.0 = Debug|x86
		__LIB_PROJECT_GUID__.Release|Any CPU.ActiveCfg = Release|Any CPU
		__LIB_PROJECT_GUID__.Release|Any CPU.Build.0 = Release|Any CPU
		__LIB_PROJECT_GUID__.Release|x64.ActiveCfg = Release|x64
		__LIB_PROJECT_GUID__.Release|x64.Build.0 = Release|x64
		__LIB_PROJECT_GUID__.Release|x86.ActiveCfg = Release|x86
		__LIB_PROJECT_GUID__.Release|x86.Build.0 = Release|x86
	EndGlobalSection
EndGlobal
";

        private const string ExpectedSlnFileAfterAddingNestedProj = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.26006.2
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""App"", ""App.csproj"", ""{7072A694-548F-4CAE-A58F-12D257D5F486}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""src"", ""src"", ""__SRC_FOLDER_GUID__""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Lib"", ""src\Lib\Lib.csproj"", ""__LIB_PROJECT_GUID__""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Debug|x64 = Debug|x64
		Debug|x86 = Debug|x86
		Release|Any CPU = Release|Any CPU
		Release|x64 = Release|x64
		Release|x86 = Release|x86
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x64.ActiveCfg = Debug|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x64.Build.0 = Debug|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x86.ActiveCfg = Debug|x86
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Debug|x86.Build.0 = Debug|x86
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|Any CPU.Build.0 = Release|Any CPU
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x64.ActiveCfg = Release|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x64.Build.0 = Release|x64
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x86.ActiveCfg = Release|x86
		{7072A694-548F-4CAE-A58F-12D257D5F486}.Release|x86.Build.0 = Release|x86
		__LIB_PROJECT_GUID__.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		__LIB_PROJECT_GUID__.Debug|Any CPU.Build.0 = Debug|Any CPU
		__LIB_PROJECT_GUID__.Debug|x64.ActiveCfg = Debug|x64
		__LIB_PROJECT_GUID__.Debug|x64.Build.0 = Debug|x64
		__LIB_PROJECT_GUID__.Debug|x86.ActiveCfg = Debug|x86
		__LIB_PROJECT_GUID__.Debug|x86.Build.0 = Debug|x86
		__LIB_PROJECT_GUID__.Release|Any CPU.ActiveCfg = Release|Any CPU
		__LIB_PROJECT_GUID__.Release|Any CPU.Build.0 = Release|Any CPU
		__LIB_PROJECT_GUID__.Release|x64.ActiveCfg = Release|x64
		__LIB_PROJECT_GUID__.Release|x64.Build.0 = Release|x64
		__LIB_PROJECT_GUID__.Release|x86.ActiveCfg = Release|x86
		__LIB_PROJECT_GUID__.Release|x86.Build.0 = Release|x86
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
		__LIB_PROJECT_GUID__ = __SRC_FOLDER_GUID__
	EndGlobalSection
EndGlobal
";

        [Theory]
        [InlineData("--help")]
        [InlineData("-h")]
        [InlineData("-?")]
        [InlineData("/?")]
        public void WhenHelpOptionIsPassedItPrintsUsage(string helpArg)
        {
            var cmd = new DotnetCommand()
                .ExecuteWithCapturedOutput($"sln add {helpArg}");
            cmd.Should().Pass();
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Theory]
        [InlineData("")]
        [InlineData("unknownCommandName")]
        public void WhenNoCommandIsPassedItPrintsError(string commandName)
        {
            var cmd = new DotnetCommand()
                .ExecuteWithCapturedOutput($"sln {commandName}");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(CommonLocalizableStrings.RequiredCommandNotPassed);
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(SlnCommandHelpText);
        }

        [Fact]
        public void WhenTooManyArgumentsArePassedItPrintsError()
        {
            var cmd = new DotnetCommand()
                .ExecuteWithCapturedOutput("sln one.sln two.sln three.sln add");
            cmd.Should().Fail();
            cmd.StdErr.Should().BeVisuallyEquivalentTo($"Unrecognized command or argument 'two.sln'\r\nUnrecognized command or argument 'three.sln'\r\n{CommonLocalizableStrings.SpecifyAtLeastOneProjectToAdd}");
        }

        [Theory]
        [InlineData("idontexist.sln")]
        [InlineData("ihave?invalidcharacters")]
        [InlineData("ihaveinv@lidcharacters")]
        [InlineData("ihaveinvalid/characters")]
        [InlineData("ihaveinvalidchar\\acters")]
        public void WhenNonExistingSolutionIsPassedItPrintsErrorAndUsage(string solutionName)
        {
            var cmd = new DotnetCommand()
                .ExecuteWithCapturedOutput($"sln {solutionName} add p.csproj");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(string.Format(CommonLocalizableStrings.CouldNotFindSolutionOrDirectory, solutionName));
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Fact]
        public void WhenInvalidSolutionIsPassedItPrintsErrorAndUsage()
        {
            var projectDirectory = TestAssets
                .Get("InvalidSolution")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln InvalidSolution.sln add {projectToAdd}");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(string.Format(CommonLocalizableStrings.InvalidSolutionFormatString, "InvalidSolution.sln", LocalizableStrings.FileHeaderMissingError));
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Fact]
        public void WhenInvalidSolutionIsFoundItPrintsErrorAndUsage()
        {
            var projectDirectory = TestAssets
                .Get("InvalidSolution")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var solutionPath = Path.Combine(projectDirectory, "InvalidSolution.sln");
            var projectToAdd = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln add {projectToAdd}");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(string.Format(CommonLocalizableStrings.InvalidSolutionFormatString, solutionPath, LocalizableStrings.FileHeaderMissingError));
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Fact]
        public void WhenNoProjectIsPassedItPrintsErrorAndUsage()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndCsprojFiles")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput(@"sln App.sln add");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(CommonLocalizableStrings.SpecifyAtLeastOneProjectToAdd);

            _output.WriteLine("[STD OUT]");
            _output.WriteLine(cmd.StdOut);
            _output.WriteLine("[HelpText]");
            _output.WriteLine(HelpText);

            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Fact]
        public void WhenNoSolutionExistsInTheDirectoryItPrintsErrorAndUsage()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndCsprojFiles")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var solutionPath = Path.Combine(projectDirectory, "App");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(solutionPath)
                .ExecuteWithCapturedOutput(@"sln add App.csproj");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(string.Format(CommonLocalizableStrings.SolutionDoesNotExist, solutionPath + Path.DirectorySeparatorChar));
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Fact]
        public void WhenMoreThanOneSolutionExistsInTheDirectoryItPrintsErrorAndUsage()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithMultipleSlnFiles")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln add {projectToAdd}");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(string.Format(CommonLocalizableStrings.MoreThanOneSolutionInDirectory, projectDirectory + Path.DirectorySeparatorChar));
            cmd.StdOut.Should().BeVisuallyEquivalentToIfNotLocalized(HelpText);
        }

        [Fact]
        public void WhenNestedProjectIsAddedSolutionFoldersAreCreated()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndCsprojInSubDir")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine("src", "Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();

            var slnPath = Path.Combine(projectDirectory, "App.sln");
            var expectedSlnContents = GetExpectedSlnContents(slnPath, ExpectedSlnFileAfterAddingNestedProj);
            File.ReadAllText(slnPath)
                .Should().BeVisuallyEquivalentTo(expectedSlnContents);
        }

        [Fact]
        public void WhenProjectDirectoryIsAddedSolutionFoldersAreNotCreated()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndCsprojFiles")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();

            var slnFile = SlnFile.Read(Path.Combine(projectDirectory, "App.sln"));
            var solutionFolderProjects = slnFile.Projects.Where(
                p => p.TypeGuid == ProjectTypeGuids.SolutionFolderGuid);
            solutionFolderProjects.Count().Should().Be(0);
            slnFile.Sections.GetSection("NestedProjects").Should().BeNull();
        }

        [Theory]
        [InlineData(".")]
        [InlineData("")]
        public void WhenSolutionFolderExistsItDoesNotGetAdded(string firstComponent)
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndSolutionFolders")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine($"{firstComponent}", "src", "src", "Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();

            var slnFile = SlnFile.Read(Path.Combine(projectDirectory, "App.sln"));
            slnFile.Projects.Count().Should().Be(4);

            var solutionFolderProjects = slnFile.Projects.Where(
                p => p.TypeGuid == ProjectTypeGuids.SolutionFolderGuid);
            solutionFolderProjects.Count().Should().Be(2);

            var solutionFolders = slnFile.Sections.GetSection("NestedProjects").Properties;
            solutionFolders.Count.Should().Be(3);

            solutionFolders["{DDF3765C-59FB-4AA6-BE83-779ED13AA64A}"]
                .Should().Be("{72BFCA87-B033-4721-8712-4D12166B4A39}");

            var newlyAddedSrcFolder = solutionFolderProjects.Where(
                p => p.Id != "{72BFCA87-B033-4721-8712-4D12166B4A39}").Single();
            solutionFolders[newlyAddedSrcFolder.Id]
                .Should().Be("{72BFCA87-B033-4721-8712-4D12166B4A39}");

            var libProject = slnFile.Projects.Where(p => p.Name == "Lib").Single();
            solutionFolders[libProject.Id]
                .Should().Be(newlyAddedSrcFolder.Id);
        }

        [Theory]
        [InlineData("TestAppWithSlnAndCsprojFiles", ExpectedSlnFileAfterAddingLibProj, "")]
        [InlineData("TestAppWithSlnAndCsprojProjectGuidFiles", ExpectedSlnFileAfterAddingLibProj, "{84A45D44-B677-492D-A6DA-B3A71135AB8E}")]
        [InlineData("TestAppWithEmptySln", ExpectedSlnFileAfterAddingLibProjToEmptySln, "")]
        public void WhenValidProjectIsPassedBuildConfigsAreAdded(
            string testAsset,
            string expectedSlnContentsTemplate,
            string expectedProjectGuid)
        {
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = "Lib/Lib.csproj";
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();

            var slnPath = Path.Combine(projectDirectory, "App.sln");

            var expectedSlnContents = GetExpectedSlnContents(
                slnPath,
                expectedSlnContentsTemplate,
                expectedProjectGuid);

            File.ReadAllText(slnPath)
                .Should().BeVisuallyEquivalentTo(expectedSlnContents);
        }

        [Theory]
        [InlineData("TestAppWithSlnAndCsprojFiles")]
        [InlineData("TestAppWithSlnAndCsprojProjectGuidFiles")]
        [InlineData("TestAppWithEmptySln")]
        public void WhenValidProjectIsPassedItGetsAdded(string testAsset)
        {
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = "Lib/Lib.csproj";
            var projectPath = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();
            cmd.StdOut.Should().Be(string.Format(CommonLocalizableStrings.ProjectAddedToTheSolution, projectPath));
            cmd.StdErr.Should().BeEmpty();
        }

        [Theory]
        [InlineData("TestAppWithSlnAndCsprojFiles")]
        [InlineData("TestAppWithSlnAndCsprojProjectGuidFiles")]
        [InlineData("TestAppWithEmptySln")]
        public void WhenInvalidProjectIsPassedItDoesNotGetAdded(string testAsset)
        {
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = "Lib/Library.cs";
            var projectPath = Path.Combine("Lib", "Library.cs");
            var slnFile = SlnFile.Read(Path.Combine(projectDirectory, "App.sln"));
            var expectedNumberOfProjects = slnFile.Projects.Count();

            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();
            cmd.StdOut.Should().BeEmpty();
            cmd.StdErr.Should().Match(string.Format(CommonLocalizableStrings.InvalidProjectWithExceptionMessage, '*', '*'));

            slnFile = SlnFile.Read(Path.Combine(projectDirectory, "App.sln"));
            slnFile.Projects.Count().Should().Be(expectedNumberOfProjects);
        }

        [Theory]
        [InlineData("TestAppWithSlnAndCsprojFiles")]
        [InlineData("TestAppWithSlnAndCsprojProjectGuidFiles")]
        [InlineData("TestAppWithEmptySln")]
        public void WhenValidProjectIsPassedTheSlnBuilds(string testAsset)
        {
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput(@"sln App.sln add App/App.csproj Lib/Lib.csproj");
            cmd.Should().Pass();

            var slnPath = Path.Combine(projectDirectory, "App.sln");

            new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .Execute($"restore App.sln")
                .Should().Pass();

            new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .Execute("build App.sln --configuration Release")
                .Should().Pass();

            var reasonString = "should be built in release mode, otherwise it means build configurations are missing from the sln file";

            var appReleaseDirectory = Directory.EnumerateDirectories(
                Path.Combine(projectDirectory, "App", "bin"),
                "Release",
                SearchOption.AllDirectories);
            appReleaseDirectory.Count().Should().Be(1, $"App {reasonString}");
            Directory.EnumerateFiles(appReleaseDirectory.Single(), "App.dll", SearchOption.AllDirectories)
                .Count().Should().Be(1, $"App {reasonString}");

            var libReleaseDirectory = Directory.EnumerateDirectories(
                Path.Combine(projectDirectory, "Lib", "bin"),
                "Release",
                SearchOption.AllDirectories);
            libReleaseDirectory.Count().Should().Be(1, $"Lib {reasonString}");
            Directory.EnumerateFiles(libReleaseDirectory.Single(), "Lib.dll", SearchOption.AllDirectories)
                .Count().Should().Be(1, $"Lib {reasonString}");
        }

        [Theory]
        [InlineData("TestAppWithSlnAndExistingCsprojReferences")]
        [InlineData("TestAppWithSlnAndExistingCsprojReferencesWithEscapedDirSep")]
        public void WhenSolutionAlreadyContainsProjectItDoesntDuplicate(string testAsset)
        {
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var solutionPath = Path.Combine(projectDirectory, "App.sln");
            var projectToAdd = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();
            cmd.StdOut.Should().Be(string.Format(CommonLocalizableStrings.SolutionAlreadyContainsProject, solutionPath, projectToAdd));
            cmd.StdErr.Should().BeEmpty();
        }

        [Fact]
        public void WhenPassedMultipleProjectsAndOneOfthemDoesNotExistItCancelsWholeOperation()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndCsprojFiles")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var slnFullPath = Path.Combine(projectDirectory, "App.sln");
            var contentBefore = File.ReadAllText(slnFullPath);

            var projectToAdd = Path.Combine("Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd} idonotexist.csproj");
            cmd.Should().Fail();
            cmd.StdErr.Should().Be(string.Format(CommonLocalizableStrings.ProjectDoesNotExist, "idonotexist.csproj"));

            File.ReadAllText(slnFullPath)
                .Should().BeVisuallyEquivalentTo(contentBefore);
        }

        //ISSUE: https://github.com/dotnet/sdk/issues/522
        //[Fact]
        public void WhenPassedAnUnknownProjectTypeItFails()
        {
            var projectDirectory = TestAssets
                .Get("SlnFileWithNoProjectReferencesAndUnknownProject")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var slnFullPath = Path.Combine(projectDirectory, "App.sln");
            var contentBefore = File.ReadAllText(slnFullPath);

            var projectToAdd = Path.Combine("UnknownProject", "UnknownProject.unknownproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Fail();
            cmd.StdErr.Should().BeVisuallyEquivalentTo("Unsupported project type. Please check with your sdk provider.");

            File.ReadAllText(slnFullPath)
                .Should().BeVisuallyEquivalentTo(contentBefore);
        }

        [Theory]
        //ISSUE: https://github.com/dotnet/sdk/issues/522
        //[InlineData("SlnFileWithNoProjectReferencesAndCSharpProject", "CSharpProject", "CSharpProject.csproj", ProjectTypeGuids.CSharpProjectTypeGuid)]
        //[InlineData("SlnFileWithNoProjectReferencesAndFSharpProject", "FSharpProject", "FSharpProject.fsproj", "{F2A71F9B-5D33-465A-A702-920D77279786}")]
        //[InlineData("SlnFileWithNoProjectReferencesAndVBProject", "VBProject", "VBProject.vbproj", "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}")]
        [InlineData("SlnFileWithNoProjectReferencesAndUnknownProjectWithSingleProjectTypeGuid", "UnknownProject", "UnknownProject.unknownproj", "{130159A9-F047-44B3-88CF-0CF7F02ED50F}")]
        [InlineData("SlnFileWithNoProjectReferencesAndUnknownProjectWithMultipleProjectTypeGuids", "UnknownProject", "UnknownProject.unknownproj", "{130159A9-F047-44B3-88CF-0CF7F02ED50F}")]
        public void WhenPassedAProjectItAddsCorrectProjectTypeGuid(
            string testAsset,
            string projectDir,
            string projectName,
            string expectedTypeGuid)
        {
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine(projectDir, projectName);
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();
            cmd.StdOut.Should().Be(string.Format(CommonLocalizableStrings.ProjectAddedToTheSolution, projectToAdd));
            cmd.StdErr.Should().BeEmpty();

            var slnFile = SlnFile.Read(Path.Combine(projectDirectory, "App.sln"));
            var nonSolutionFolderProjects = slnFile.Projects.Where(
                p => p.TypeGuid != ProjectTypeGuids.SolutionFolderGuid);
            nonSolutionFolderProjects.Count().Should().Be(1);
            nonSolutionFolderProjects.Single().TypeGuid.Should().Be(expectedTypeGuid);
        }

        [Fact]
        private void WhenSlnContainsSolutionFolderWithDifferentCasingItDoesNotCreateDuplicate()
        {
            var projectDirectory = TestAssets
                .Get("TestAppWithSlnAndCaseSensitiveSolutionFolders")
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var projectToAdd = Path.Combine("src", "Lib", "Lib.csproj");
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .Execute($"sln App.sln add {projectToAdd}");
            cmd.Should().Pass();

            var slnFile = SlnFile.Read(Path.Combine(projectDirectory, "App.sln"));
            var solutionFolderProjects = slnFile.Projects.Where(
                p => p.TypeGuid == ProjectTypeGuids.SolutionFolderGuid);
            solutionFolderProjects.Count().Should().Be(1);
        }

        private string GetExpectedSlnContents(
            string slnPath,
            string slnTemplate,
            string expectedLibProjectGuid = null)
        {
            var slnFile = SlnFile.Read(slnPath);

            if (string.IsNullOrEmpty(expectedLibProjectGuid))
            {
                var matchingProjects = slnFile.Projects
                    .Where((p) => p.FilePath.EndsWith("Lib.csproj"))
                    .ToList();

                matchingProjects.Count.Should().Be(1);
                var slnProject = matchingProjects[0];
                expectedLibProjectGuid = slnProject.Id;
            }
            var slnContents = slnTemplate.Replace("__LIB_PROJECT_GUID__", expectedLibProjectGuid);

            var matchingSrcFolder = slnFile.Projects
                    .Where((p) => p.FilePath == "src")
                    .ToList();
            if (matchingSrcFolder.Count == 1)
            {
                slnContents = slnContents.Replace("__SRC_FOLDER_GUID__", matchingSrcFolder[0].Id);
            }

            return slnContents;
        }
    }
}
