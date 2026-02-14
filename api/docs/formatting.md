# code formatting with csharpier

this project uses [csharpier](https://csharpier.com/) for consistent code formatting.

## what is csharpier?

csharpier is an opinionated code formatter for c# that ensures consistent style across the entire codebase. it works similarly to prettier for javascript.

## how it works

### 1. automatic formatting on build

csharpier runs automatically during `dotnet build` via the `CSharpier.MsBuild` package.

**when files need formatting:**
```
formatted 3 files in 450ms
```

**when all files are formatted:**
no output (runs silently for fast builds)

**to verify it's running:**
```bash
dotnet build -v:normal
```
look for `CSharpierFormatInner` in the output.

### 2. ide integration (format on save)

recommended setup for the best developer experience:

#### visual studio 2022

1. install [csharpier extension](https://marketplace.visualstudio.com/items?itemName=csharpier.CSharpier)
2. `tools` → `options` → `csharpier` → `general`
3. ✓ **reformat with csharpier on save**
4. set scope to **solution**

#### vs code

1. install "csharpier - code formatter" extension
2. settings (ctrl+,) → search "csharpier"
3. ✓ **editor: format on save**
4. set **csharpier: enable** to `true`

#### jetbrains rider

1. install csharpier plugin
2. `settings` → `languages & frameworks` → `csharpier`
3. ✓ **run on save**

### 3. pre-commit hook

automatically formats staged files before each commit.

**setup:**
```bash
cp hooks/pre-commit .git/hooks/
chmod +x .git/hooks/pre-commit
```

the hook:
- stashes unstaged changes
- runs csharpier on staged `.cs` files
- re-stages formatted files
- restores unstaged changes

## manual formatting

format all files:
```bash
dotnet csharpier .
```

check if files are formatted (ci/cd):
```bash
dotnet csharpier --check .
```

format specific file:
```bash
dotnet csharpier Path/To/File.cs
```

## configuration

csharpier configuration is in `.csharpierrc.json`:

```json
{
  "printWidth": 100,
  "useTabs": false,
  "indentSize": 4,
  "endOfLine": "auto"
}
```

| option | default | description |
|--------|---------|-------------|
| `printWidth` | 100 | target line width (soft limit) |
| `useTabs` | false | use tabs instead of spaces |
| `indentSize` | 4 | number of spaces per indentation |
| `endOfLine` | auto | line ending style |

## troubleshooting

### csharpier not running on build

- ensure `Directory.Build.props` includes `CSharpier.MsBuild` reference
- verify `Directory.Packages.props` has csharpier version
- run `dotnet tool restore`

### ide not formatting on save

1. check that the extension/plugin is installed
2. verify "format on save" is enabled in ide settings
3. ensure `.csharpierrc.json` exists in the api/ directory

### build warnings about csharpier

the build succeeds even if csharpier isn't working - it's a best-effort formatting step. check the build output for specific error messages.

## ci/cd integration

to enforce formatting in ci/cd pipelines:

```bash
dotnet csharpier --check .
```

this exits with error code 1 if any files need formatting.

to enable this in the project, uncomment in `Directory.Build.props`:
```xml
<CSharpier_Check>true</CSharpier_Check>
```
