#addin "Cake.FileHelpers"
#addin "Cake.Xamarin"

var TARGET = Argument ("target", Argument ("t", "NuGetPack"));

//var version = Argument ("pkgversion", EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? "0.0.9999");
var version = "0.0.9999";


Func<FilePath> GetNuGetToolPath = () =>
{
	var possibleExe = GetFiles ("../**/tools/nuget3.exe").FirstOrDefault ();

	if (possibleExe != null)
		return possibleExe;
		
	var p = System.Diagnostics.Process.GetCurrentProcess ();	
	return new FilePath (p.Modules[0].FileName);
};

Func<FilePath> GetXCToolPath = () =>
{
	var possibleExe = GetFiles ("../tools/xamarin-component.exe").FirstOrDefault ();

	if (possibleExe != null)
		return possibleExe;
		
	var p = System.Diagnostics.Process.GetCurrentProcess ();	
	return new FilePath (p.Modules[0].FileName);
};



Task ("Build").Does (() =>
{

	var sln = GetFiles("./*.sln").FirstOrDefault();
	const string cfg = "Release";

	Information ("Restoring {0}", sln);

	NuGetRestore (sln, new NuGetRestoreSettings {
		ToolPath = GetNuGetToolPath ()
	});

	RestoreComponents(sln, new XamarinComponentRestoreSettings {
		ToolPath = GetXCToolPath (),
		Email = EnvironmentVariable ("XAMARIN_EMAIL"),
		Password = EnvironmentVariable ("XAMARIN_PASSWORD")
	});


    if (!IsRunningOnWindows ())
        DotNetBuild (sln, c => c.Configuration = cfg);
    else
        MSBuild (sln, c => {
            c.Configuration = cfg;
            c.MSBuildPlatform = MSBuildPlatform.x86;
        });
});

Task ("NuGetPack")
	.IsDependentOn ("Build")
	.Does (() =>
{
	NuGetPack ("./Plugin.HockeyApp.nuspec", new NuGetPackSettings {
		ToolPath = GetNuGetToolPath (),
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		BasePath = "./",
	});
});


RunTarget (TARGET);
