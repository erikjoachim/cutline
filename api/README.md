# cutline api

quick start guide for the cutline api project.

## quick start

```bash
# 1. clone the repository
git clone <repo-url>

# 2. restore tools and dependencies
cd api
dotnet tool restore
dotnet restore

# 3. run the api
dotnet run --project Cutline.Api
```

the api will start at:
- http: http://localhost:5037
- https: https://localhost:7081 (if configured)

**browser automatically opens to scalar api documentation at `/scalar/v1`**

## essentials

- **.net 10.0** - target framework
- **code formatting** - csharpier (runs on build + optional format-on-save)
- **api docs** - scalar (auto-opens when running)
- **package management** - central package management (cpm) - versions in `Directory.Packages.props`

## project structure

```
api/
├── Cutline.Api/					# main api project
├── docs/							# documentation
├── hooks/							# git hooks
├── .editorconfig					# editor settings
├── .csharpierrc.json				# csharpier config
├── Directory.Build.props			# shared msbuild config
├── Directory.Packages.props		# central package versions
└── README.md						# this file
```

## documentation

- **[setup guide](docs/setup.md)** - detailed development setup
- **[code formatting](docs/formatting.md)** - csharpier configuration & setup
- **[package management](docs/packages.md)** - how to add packages with cpm
- **[api documentation](docs/api-docs.md)** - scalar setup and usage

## useful commands

```bash
# build
dotnet build

# run with hot reload
dotnet watch --project Cutline.Api

# format code manually
dotnet csharpier .

# check formatting (ci)
dotnet csharpier --check .
```

## key configuration files

| file | purpose |
|------|---------|
| `Directory.Packages.props` | central package versions |
| `Directory.Build.props` | shared build configuration |
| `.csharpierrc.json` | csharpier formatting rules |
| `appsettings.Development.json` | local dev settings (gitignored) |
