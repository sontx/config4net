# Config4Net
``` cs
var awesomeAppSettings = ConfigPool.Default.Get<AwesomeAppSettings>();
UiManager.Default.Build<IWindowContainer>(awesomeAppSettings).Show();
```
**Config4Net** manages user settings (save, load, automatically build UI and synchronize from UI to underlying data).

![](https://2.bp.blogspot.com/-qFPl7LZA9wk/Wj4dzwdnslI/AAAAAAAATNc/rm9oBO1VWMchEGar_EmANktq2is82FHhACLcBGAs/s1600/Untitled+Diagram.png)

## Getting Started
### Define settings classes.
```cs
[Config]
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

> - [Config attribute](https://github.com/sontx/config4net/blob/master/Config4Net.Core/ConfigAttribute.cs): Annotate a class is a config type.
> - [Showable attribute](https://github.com/sontx/config4net/blob/master/Config4Net.UI/ShowableAttribute.cs):  Annotate a property is a showable component that will be shown in the UI.

### Load config
Load configuration from file if it already exists or creates new one.
```cs
var person = ConfigPool.Default.Get<Person>();
```

> [ConfigPool](https://github.com/sontx/config4net/blob/master/Config4Net.Core/ConfigPool.cs) automatically save the configs data to files when app's exiting and load these data again when the app starts.
> If the config type that is demanding but it does not exist, an instance of this config type will be created and registered to the [ConfigPool](https://github.com/sontx/config4net/blob/master/Config4Net.Core/ConfigPool.cs) .

### Show config to UI
Load built-in components that were implemented for native-winform.
``` cs
new WinFormFlatformLoader().Load();
```

> The Config4Net.UI just is an abstract layer, but there is an implementation is native-winform that uses built-in controls from WinForm to build the UI.

Show config to UI and enjoy ;)
``` cs
UiManager.Default.Build<IWindowContainer>(person).Show();
```

> The UI builds based on config type (class structure), each property in the config will be bound to a component and type of component depends on type of the property.
> The data from the config will be synchronized from/to the UI.

-------------
Full example: [Program.cs](https://github.com/sontx/config4net/blob/master/ExampleApp/Program.cs)
## Install
Comming soon...
## Authors

* **Tran Xuan Son** - *main developer* - [sontx](https://github.com/sontx)

See also the list of [contributors](https://github.com/sontx/config4net/contributors) who participated in this project.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details
