# BackendServiceStarter
Starter for RESTful API service.

## Requirements
* .NET Core (>= 3.0)
* PostgreSQL (>= 12)

## Description
This template has connection to PostgreSQL, model definition, user definition, universal model service (repository), user service (repository), RESTful API for user management and JWT authentication.

## Setup
Change `appsettings.json` file with your settings:
```json
{
  "ConnectionStrings": {
    "ApplicationConnection": "Host=;Port=;Database=;Username=;Password="
  },
  "JwtAuthOptions": {
    "Issuer": "",
    "Audience": "",
    "Key": "",
    "Lifetime": 0
  }
}
```

`ConnectionStrings:ApplicationConnection` - connection string to PostgreSQL database.

`JwtAuthOptions:Issuer` - name of issuer.

`JwtAuthOptions:Audience` - name of client app.

`JwtAuthOptions:Key` - your secret key for encryption JWT.

`JwtAuthOptions:Lifetime` - lifetime for JWT in minutes.

## Models usage
For default model repository add to service container ModelService<>:
```csharp
private void ConfigureModelsServices(IServiceCollection services)
{
    services.AddScoped(typeof(ModelService<>));
    services.AddScoped<UserService>();
}
```
If you need to override default methods or add your methods then create derrived class:
```csharp
class BookService : ModelService<Book>
{
    public override Task Update(Book modelObject)
    {
        // Your realisation
        return base.Update(modelObject);
    }
}
```
