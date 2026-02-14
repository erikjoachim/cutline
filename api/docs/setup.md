# development setup

detailed setup instructions for the cutline api project.

## prerequisites

- **.net 10.0 sdk** or later
- **ide**: visual studio 2022+, vs code, or jetbrains rider
- **git** for version control

## initial setup

### 1. clone the repository

```bash
git clone <repository-url>
cd api
```

### 2. restore .net tools

the project uses local .net tools (csharpier) defined in `.config/dotnet-tools.json`:

```bash
dotnet tool restore
```

this installs:
- csharpier v0.30.0 (code formatter)

### 3. restore nuget packages

```bash
dotnet restore
```

packages are managed centrally via `Directory.Packages.props`.

### 4. configure ide (optional but recommended)

#### visual studio 2022

1. install [csharpier extension](https://marketplace.visualstudio.com/items?itemName=csharpier.CSharpier)
2. go to `tools` → `options` → `csharpier` → `general`
3. enable **reformat with csharpier on save**
4. set scope to **solution** (not global)

#### vs code

1. install "csharpier - code formatter" extension
2. open settings (ctrl+,)
3. search for "csharpier"
4. enable **editor: format on save**
5. set **csharpier: enable** to `true`

#### jetbrains rider

1. install csharpier plugin from plugins marketplace
2. go to `settings` → `languages & frameworks` → `csharpier`
3. enable **run on save**

### 5. setup pre-commit hook (optional)

to automatically format code before each commit:

**windows (git bash/wsl):**
```bash
cp hooks/pre-commit .git/hooks/
chmod +x .git/hooks/pre-commit
```

**linux/mac:**
```bash
cp hooks/pre-commit .git/hooks/
chmod +x .git/hooks/pre-commit
```

## running the api

### development mode

```bash
dotnet run --project Cutline.Api
```

features in development mode:
- scalar api documentation auto-opens
- hot reload enabled (use `dotnet watch`)
- detailed error pages

### production mode

```bash
dotnet run --project Cutline.Api --configuration release
```

## troubleshooting

### "dotnet-csharpier not found"

run: `dotnet tool restore`

### package restore fails

1. clear nuget cache: `dotnet nuget locals all --clear`
2. restore again: `dotnet restore`

### build fails with "package version not found"

ensure the package version is defined in `Directory.Packages.props`.

see also: [package management](packages.md)
