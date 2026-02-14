# central package management (cpm)

this solution uses [central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management) to ensure consistent package versions across all projects.

## overview

instead of specifying versions in each `.csproj` file, all package versions are defined centrally in `Directory.Packages.props`. this prevents version mismatches and makes updates easier.

## how it works

### central file: `Directory.Packages.props`

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
  </ItemGroup>
</Project>
```

### project file: `Cutline.Api.csproj`

```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" />
</ItemGroup>
```

**notice:** no `version` attribute in the project file!

## adding a new package

### step 1: add version to central file

edit `Directory.Packages.props`:

```xml
<ItemGroup>
  <!-- existing packages... -->
  <PackageVersion Include="YourPackageName" Version="1.2.3" />
</ItemGroup>
```

### step 2: reference in project

edit `Cutline.Api.csproj` (or any project):

```xml
<ItemGroup>
  <PackageReference Include="YourPackageName" />
</ItemGroup>
```

### step 3: restore

```bash
dotnet restore
```

## current packages

### code quality
- **csharpier.msbuild** v0.30.0 - code formatting

### api / web
- **microsoft.aspnetcore.openapi** v10.0.3 - openapi document generation
- **scalar.aspnetcore** v2.2.0 - interactive api documentation

## shared package references

some packages are referenced automatically for all projects via `Directory.Build.props`:

```xml
<ItemGroup>
  <!-- all projects get csharpier -->
  <PackageReference Include="CSharpier.MsBuild" />
  
  <!-- only web projects get these -->
  <PackageReference Include="Microsoft.AspNetCore.OpenApi" 
                    Condition="'$(UsingMicrosoftNETSdkWeb)' == 'true'" />
  <PackageReference Include="Scalar.AspNetCore" 
                    Condition="'$(UsingMicrosoftNETSdkWeb)' == 'true'" />
</ItemGroup>
```

this means new web projects automatically get openapi and scalar support without modifying their `.csproj`!

## updating package versions

simply change the version in `Directory.Packages.props`:

```xml
<!-- before -->
<PackageVersion Include="Newtonsoft.Json" Version="13.0.1" />

<!-- after -->
<PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
```

all projects using that package will use the new version after `dotnet restore`.

## build errors

### "package version not found"

**error:**
```
error NU1008: projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items
```

**solution:**
1. add the package version to `Directory.Packages.props`
2. remove the `version` attribute from the project file

### "duplicate package reference"

**error:**
```
error NU1009: the packages Microsoft.AspNetCore.OpenApi are implicitly referenced. you do not typically need to reference them from your project
```

**solution:**
remove the duplicate reference from the project file - it's already in `Directory.Build.props`.

## benefits

1. **consistency** - same package version across all projects
2. **single source of truth** - one file controls all versions
3. **easy updates** - change version in one place
4. **enforced** - build fails if version not defined centrally
5. **shared references** - common packages auto-applied to all projects

## migration from standard package references

if you have existing projects with inline versions:

1. move all `<PackageVersion>` entries to `Directory.Packages.props`
2. remove `version` attributes from all `<PackageReference>` in project files
3. add `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>` to `Directory.Packages.props`
4. run `dotnet restore` to verify
