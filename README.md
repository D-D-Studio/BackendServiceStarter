# BackendServiceStarter
Starter for RESTful API service.

## Requirements
* .NET Core (>= 3.1)
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

## Repository usage
For default model repository add to service container Repository<>:
```csharp
public static class ModelsExtension
{
    public static IServiceCollection AddModelsServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(Repository<>));
        services.AddScoped<UserRepository>();

        return services;
    }
}
```
If you need to override default methods or add your methods then create derrived class:
```csharp
class BookRepository : Repository<Book>
{
    public override Task Update(Book modelObject)
    {
        // Your realisation
        return base.Update(modelObject);
    }
}
```
