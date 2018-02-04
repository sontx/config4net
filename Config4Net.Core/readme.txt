//-----------------------------------------------------------
// Config4Net.Core manages multiple configurations, an alternative to built-in settings of .Net Framework, it's also simple, portable and customizable.
// A lightweight version of Config4Net without UI components, it can be used in almost .Net projects.
//-----------------------------------------------------------

//-----------------------------------------------------------
// Getting Started
//-----------------------------------------------------------

1. Define a config class.

class Person
{
    public string Name { get; set; }

    [DefaultValue(24)]
    public int Age { get; set; }

    public Address Address { get; set; }
}

2. Load configs from files

Config.Default.LoadAsync();

NOTE: Normally there is no need to call this method as the library will load files automatically when needed.

3.Get configs

var person = Config.Default.Get<Person>();

NOTE: If the configuration does not exist yet, a new instance will be created, registered and then returned.

4. Save configs to files

Config.Default.SaveAsync();

NOTE: Normally there is no need to call this method as the library will save to files automatically when application is closing.

//-----------------------------------------------------------

For more details: https://github.com/sontx/config4net/tree/master/Config4Net.Core
Need a help, contact me via xuanson33bk@gmail.com or https://www.facebook.com/noem.bk