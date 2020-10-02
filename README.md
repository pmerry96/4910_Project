Usage standards within this repo

1 - .gitignore
	you should have ANY compiled object, binaries, or .dll files ignored by placing them in the .gitignore
	Basically, if you generated it - it can be put in the repo
	If a compiler generated it - it stays off of the repo.

2 - The new project strucutre
	Absolutely no code will go in the "Progress" folder - This is only for screenshots or write-ups of your progress
	The project code should exist in the top level of the directory - as it is now 

3 - Pushing to the repo
	If you have code that does not run - DO NOT PUSH IT TO THE REPO
	The common saying is "Dont break the build"


Setting up your development Environment

1 - We are using Microsoft Visual Studio 2019, where this project is using
	a WPF App (with .NET Core)
	  Stylet

	Both of these are available in MVS2019, and you can get them as follows
	WPF - Ensure you have the correct prerequisites downloaded. It should be Desktop Development With c#
	Stylet - Achieved through the package manager on the Nuget Command Line
		Tools > NuGet Package Manager > Package manager console
		and run this command

		dotnet new -i Stylet.Templates

		This will install the Stylet.Templates package.