Airlock.Hive
============

_Warning: this project is not yet feature complete and you are likely to
encounter bugs and missing features. Please raise an issue or pull request
for any bugs you encounter._

Airlock.Hive is a set of libraries for accessing Apache Hive from .NET.

- `Airlock.Hive.Database` is a fully-managed ADO.NET provider for Apache Hive.
It aims to provide the same set of features as the official Apache JDBC driver.
- `Airlock.EntityFrameworkCore.Hive` is an Entity Framework Core 2.0 provider.
for Hive

Supported platforms:

- .NET Standard 2.0

## Usage

### ADO.NET

```csharp
using (var connection = new HiveConnection("hive2://hive.example.com:10000?username=username&password=password"))
{
    connection.Open();
    using (var command = connection.CreateCommand())
    {
        command.CommandText = "SELECT id, shortAddress FROM ef.Properties";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var shortAddress = reader.GetString(1);
        }
    }
}
```

### Entity Framework

```csharp
var optionsBuilder = new DbContextOptionsBuilder<EstateAgentContext>()
    .UseHive(connectionString);
 
using (var context = new EstateAgentContext(optionsBuilder.Options))
{
    var properties = from p in context.Properties
                     select new { p.Id, p.ShortAddress };
    Console.WriteLine(properties.First().ShortAddress);
}
```

## Features

### Hive

- Only reading from tables is supported. Modifying and inserting is not supported.
- Only primitive types are currently supported.

### Hive Server Transport Modes

The managed driver supports SASL with username/password authentication. To
connect to Hive Server using a different transport mechanism (e.g. HTTP), you
must use an ODBC driver as described below.

### Native Drivers for Entity Framework

The Entity Framework provider can be used with either The
`Airlock.Hive.Database` managed driver, or a native ODBC driver by supplying
a connection string when initializing the database context. Using a native
driver requires the driver to be installed on your system. It is known to work
with the Hortonworks Hive ODBC driver on Windows.

To use an ODBC driver with Entity Framework, supply an ODBC connection string
instead of a `hive2://...` connection string.
