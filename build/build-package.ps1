#
# This script is designed to modify an existing .nuspec file
# prior to a NuGet pack command to add in all <PackageReference>
# elements found in a co-located csproj file. 
#
# NuGet pack is pretty good but for some reason doesn't support the 
# PackageReference elements (despite them being a native NuGet feature)
#
param ([String] $project, [String] $version)

# Build the project
dotnet pack "../src/$project/$project.csproj" -c Release -p:PackageVersion="$version" -o "./packages/$version"
