# Config4Net
``` cs
var awesomeAppConfig = Config.Default.Get<AwesomeAppConfig>();
```
**Config4Net.Core** manages multiple configurations, an alternative to built-in settings of .Net Framework, it's also simple, portable and customizable.
A lightweight version of Config4Net without UI components, it can be used in almost .Net projects.

![](https://3.bp.blogspot.com/-6tZGLnoSUJ0/Wnaqc2o5J5I/AAAAAAAATqE/bP54ShTPR6MjYTwdMHRZzdArFoFpKVMGQCLcBGAs/s1600/core.png)

## Getting Started
### Define a config class.
```cs
class Person
{
    public string Name { get; set; }

    [DefaultValue(24)]
    public int Age { get; set; }

    public Address Address { get; set; }
}
```

> [DefaultValue attribute](https://msdn.microsoft.com/en-us/library/system.componentmodel.defaultvalueattribute(v=vs.110).aspx):  Specifies the default value for a property.

### Manipulate
Most work is done by [Config](https://github.com/sontx/config4net/blob/master/Config4Net.Core/Config.cs) class that provides a default instance or you can create new one by call `Config.Create()`.
##### Load configs from files
Normally there is no need to call this method as the library will load files automatically when needed.
``` cs
Config.Default.LoadAsync();
```
##### Get configs
```cs
var person = Config.Default.Get<Person>();
```
> If the configuration does not exist yet, a new instance will be created, registered and then returned.

The library also can manage multiple configurations in a same instance.
``` cs
var person = Config.Default.Get<Person>();
var anotherPerson = Config.Default.Get<Person>("differentKey");
var animal = Config.Default.Get<Animal>();
```
##### Save configs to files
Normally there is no need to call this method as the libraray will save to files automatically when application is closing.
``` cs
Config.Default.SaveAsync();
```
> Configurations can be saved to files by default. 
> It also can be saved in [Windows Registry](https://vi.wikipedia.org/wiki/Windows_Registry)
> through `Config.Settings.StoreService`, see the [Wiki](https://github.com/sontx/config4net/wiki) for more details.

## Install
#### NuGet
``` bash
Install-Package Config4Net.Core -Version 1.0.1
```