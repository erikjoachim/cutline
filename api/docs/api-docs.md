# api documentation with scalar

this project uses [scalar](https://scalar.com/) for interactive api documentation.

## what is scalar?

scalar is a modern, interactive api documentation tool that renders openapi specifications as beautiful, browsable documentation with built-in request testing.

## quick access

when running in **development** mode:

| endpoint | description |
|----------|-------------|
| `/scalar/v1` | **scalar ui** - interactive documentation |
| `/openapi/v1.json` | **openapi json** - raw specification |

the browser automatically opens to scalar when you run the project.

## running the api

```bash
dotnet run --project Cutline.Api
```

your browser will open to: `http://localhost:5037/scalar/v1`

## features

### interactive documentation

- **browse endpoints** - see all available api routes
- **view schemas** - request/response models with examples
- **test requests** - try endpoints directly from the ui
- **authentication** - configure auth headers/tokens
- **code samples** - copy-paste ready code in multiple languages

### openapi specification

the api automatically generates openapi documents at:
- `/openapi/v1.json` - json format

this spec can be used with:
- scalar (built-in)
- swagger ui
- postman (import)
- auto-generated api clients

## configuration

### program.cs

```csharp
using scalar.aspnetcore;

var builder = webapplication.createbuilder(args);

// add openapi document generation
builder.services.addopenapi();

var app = builder.build();

// enable scalar in development
if (app.environment.isdevelopment())
{
    app.mapopenapi();           // serve openapi json
    app.mapscalarapireference(); // serve scalar ui
}
```

### launch settings

configured in `properties/launchsettings.json`:

```json
{
  "profiles": {
    "http": {
      "launchbrowser": true,
      "launchurl": "scalar/v1"
    }
  }
}
```

## customizing scalar

you can customize scalar appearance and behavior:

```csharp
app.mapscalarapireference(options =>
{
    options.title = "cutline api";
    options.theme = scalartheme.mars;
    options.defaulthttpclient = new(scalartarget.http, scalarclient.http11);
});
```

available themes: `default`, `alternate`, `moon`, `purple`, `solarized`, `mars`

## adding xml documentation

for richer api docs, enable xml documentation:

### 1. update project file

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

### 2. add xml comments

```csharp
/// <summary>
/// gets a greeting message
/// </summary>
/// <param name="name">name to greet</param>
/// <returns>a greeting message</returns>
app.mapget("/hello/{name}", (string name) => $"hello {name}!");
```

the comments will appear in scalar as endpoint descriptions.

## multiple api versions

to support multiple api versions:

```csharp
builder.services.addopenapi("v1");
builder.services.addopenapi("v2");

if (app.environment.isdevelopment())
{
    app.mapopenapi("/openapi/{documentname}.json");
    
    app.mapscalarapireference("/scalar/v1", options =>
    {
        options.openapiroutepattern = "/openapi/v1.json";
    });
    
    app.mapscalarapireference("/scalar/v2", options =>
    {
        options.openapiroutepattern = "/openapi/v2.json";
    });
}
```

## authentication in scalar

if your api uses authentication, configure it in scalar:

```csharp
app.mapscalarapireference("/scalar", options =>
{
    options.addpreferredsecurityschemes("bearer");
    
    options.withbearerauth(bearer =>
    {
        bearer.token = "your-token-here";
    });
});
```

## troubleshooting

### scalar not opening

check that you're running in **development** mode:
```bash
aspnetcore_environment=development dotnet run --project Cutline.Api
```

### openapi json not found

ensure `app.mapopenapi()` is called before `app.mapscalarapireference()`.

### browser doesn't open automatically

check `launchsettings.json` has:
```json
"launchbrowser": true,
"launchurl": "scalar/v1"
```

## alternative: swagger ui

if you prefer swagger ui instead of scalar:

1. add package: `swashbuckle.aspnetcore.swaggerui`
2. replace `app.mapscalarapireference()` with:
   ```csharp
   app.useswagger();
   app.useswaggerui();
   ```

but scalar is recommended for its modern ui and better developer experience.

## resources

- [scalar documentation](https://guides.scalar.com/)
- [scalar github](https://github.com/scalar/scalar)
- [openapi specification](https://swagger.io/specification/)
