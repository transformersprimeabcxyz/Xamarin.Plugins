#addin nuget:?package=Cake.AppVeyor
#addin nuget:?package=Cake.Yaml
#addin nuget:?package=Cake.FileHelpers

var TARGET = Argument ("target", Argument ("t", "Default"));

var APPVEYOR_APITOKEN = EnvironmentVariable ("APPVEYOR_APITOKEN") ?? "";
var APPVEYOR_ACCOUNTNAME = EnvironmentVariable ("APPVEYOR_ACCOUNTNAME") ?? "xmendoza";
var APPVEYOR_PROJECTSLUG = EnvironmentVariable ("APPVEYOR_PROJECTSLUG") ?? "xamarin-plugins";
var APPVEYOR_BUILD_NUMBER = EnvironmentVariable ("APPVEYOR_BUILD_NUMBER") ?? "9999";

var COMMIT = EnvironmentVariable ("APPVEYOR_REPO_COMMIT") ?? "";
var GIT_PATH = EnvironmentVariable ("GIT_EXE") ?? (IsRunningOnWindows () ? "git.exe" : "git");

var PROJECTS = DeserializeYamlFromFile<List<Project>> ("./projects.yaml");

Func<FilePath> GetCakeToolPath = () =>
{
	var possibleExe = GetFiles ("../**/tools/Cake/Cake.exe").FirstOrDefault ();

	if (possibleExe != null)
		return possibleExe;
		
	var p = System.Diagnostics.Process.GetCurrentProcess ();	
	return new FilePath (p.Modules[0].FileName);
};

public class Project 
{
	public Project () 
	{
		Name = string.Empty;
		BuildScript = string.Empty;
		TriggerPaths = new List<string> ();
		BuildTargets = new List<string> ();
	}

	public string Name { get; set; }
	public string BuildScript { get; set; }
	public List<string> TriggerPaths { get; set; }
	public List<string> BuildTargets { get; set; }
	public string Version { get; set; }

	public override string ToString ()
	{
		return Name;
	}
}

Task ("Default").Does (() =>
{

	// Now go through all the projects to build and build them
	foreach (var project in PROJECTS) {
		var buildVersion = project.Version.Replace ("{build}", APPVEYOR_BUILD_NUMBER);

		Information ("\tBuilding: {0} ({1})", project.BuildScript, buildVersion);
		// Build each target specified in the manifest
		foreach (var target in project.BuildTargets) {
			var cakeSettings = new CakeSettings { 
				ToolPath = GetCakeToolPath (),
				Arguments = new Dictionary<string, string> { 
					{ "target", target },
					{ "pkgversion", buildVersion },
				},
				Verbosity = Verbosity.Diagnostic
			};

			// Run the script from the subfolder
			CakeExecuteScript (project.BuildScript, cakeSettings);
		}
	}
});

RunTarget (TARGET);