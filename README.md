# Config4Net
``` cs
var awesomeAppConfig = Config.Default.Get<AwesomeAppConfig>();
UiManager.Default.Build<IWindowContainer>(awesomeAppConfig).Show();
```
**Config4Net** manages multiple configurations (saves, loads, builds UI automatically and synchronizes from UI to underlying data).

![](https://2.bp.blogspot.com/-qFPl7LZA9wk/Wj4dzwdnslI/AAAAAAAATNc/rm9oBO1VWMchEGar_EmANktq2is82FHhACLcBGAs/s1600/Untitled+Diagram.png)

## Getting Started
### Define a config class.
```cs
class Person
{
    [Showable]
    public string Name { get; set; }

    [Showable]
    public int Age { get; set; }

    [Showable("Where are you now:")]
    public Address Address { get; set; }
}
```

> [Showable attribute](https://github.com/sontx/config4net/blob/master/Config4Net.UI/ShowableAttribute.cs):  Annotates a property is a visible component that will be shown in the UI. A property without Showable attribute will be invisible in the UI.

### Load config
Loads configuration from file if it already exists or creates new one.
```cs
var person = Config.Default.Get<Person>();
```

> [Config](https://github.com/sontx/config4net/blob/master/Config4Net.Core/Config.cs) saves the configurations data to files when app's closing automatically and loads these data again when the app starts.
> If the configuration type that is demanding but it does not exist, an instance of this configuration type will be created and registered to the [Config](https://github.com/sontx/config4net/blob/master/Config4Net.Core/Config.cs) .

### Show config to UI
Loads built-in components that were implemented for native-winform.
``` cs
new WinFormFlatformLoader().Load();
```

> The Config4Net.UI just is an abstract layer, but there is an implementation is native-winform that uses built-in controls from WinForm to build the UI.

Show config to UI and enjoy ;)
``` cs
UiManager.Default.Build<IWindowContainer>(person).Show();
```

> The UI builds based on configuration type (class structure), each property in the config will be bound to a component and type of component depends on property type.
> The data from the configuration will be synchronized from/to the UI.

-------------
Full example: [Program.cs](https://github.com/sontx/config4net/blob/master/ExampleApp/Program.cs)
## Install

#### NuGet
[Config4Net.Core](https://github.com/sontx/config4net/tree/master/Config4Net.Core) a lightweight version without UI components.
See [Nuget package](https://www.nuget.org/packages/Config4Net.Core/)

``` bash
Install-Package Config4Net.Core -Version 1.0.1
```

[Config4net.UI.WinForm](https://github.com/sontx/config4net/tree/master/Config4Net.UI.WinForms) a UI implementation for WinForm.
See [Nuget package](https://www.nuget.org/packages/Config4Net.UI.WinForm/)

``` bash
Install-Package Config4Net.UI.WinForm -Version 1.0.0
```
## Authors

* **Tran Xuan Son** - *main developer* - [sontx](https://github.com/sontx)

See also the list of [contributors](https://github.com/sontx/config4net/contributors) who participated in this project.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details
