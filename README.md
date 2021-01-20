# Microcharts

Customized version of Microcharts from [dotnet-ad/Microcharts](https://github.com/dotnet-ad/Microcharts)

## Packages

For the moment, we only need Microcharts.Forms package.

[Microcharts.Forms](http://nuget.axiocode.net/?specialType=singlePackage&id=Microcharts.Forms&version=1.0.0)

## How to publish a new version

For `Microcharts.Forms` :

- Change version number in `Microcharts.Forms.csproj`
- Change version number in `NuGet\Package.Forms.nuspec`
- Build `Microcharts.Forms` Project in `Release`
- run `nuget pack path\to\NuGet\Package.Forms.nuspec`
- Upload `Microcharts.Forms.x.x.x.nupkg` on [AxioCode NuGet server](http://nuget.axiocode.net)
