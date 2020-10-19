# SecureConnectionString

*This is a nuget package for secure your connection strings in .Net Core project.* 

https://www.nuget.org/packages/SecureConnectionString/1.1.0

# Usage

SecureConnectionString - add as a namespace

```csharp      
using SecureConnectionString;
```

The connectoin string in appsettings.json will be change on first load.

*You should add the VisualStudioHelper Class*

```csharp      

services.AddDbContext<BackendContext>(opt =>
{
    var connstr = configuration.GetConnectionString("ConnStr_DB1");

    if (ConnectionString.IsCrypted(connstr))
    {
        opt.UseSqlServer(ConnectionString.Decrypt(connstr));
    }
    else
    {
        VisualStudioHelper.ChangeAppSettingsJsonFile("ConnStr_DB1",ConnectionString.Encrypt(connstr));       
        opt.UseSqlServer(connstr);
    }

});

```

# VisualStudioHelper Class

It find and changes the appsettings.json

```csharp      
public static class VisualStudioHelper
{
    private static DirectoryInfo GetSolutionDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory;
    }

    private static FileInfo GetAppSettingsJsonDirectory() =>
        GetSolutionDirectory().GetDirectories("*.WebAPI").First().GetFiles("appsettings.json").First();


    public static void ChangeAppSettingsJsonFile(string key, string value)
    {
        string appSettingsJsonFilePath = GetAppSettingsJsonDirectory().FullName;
        string json = File.ReadAllText(appSettingsJsonFilePath);
        var jObject = JObject.Parse(json);
        jObject["ConnectionStrings"][key] = value;
        File.WriteAllText(appSettingsJsonFilePath, jObject.ToString());
    }
}

```

# Licence

MIT Licence
