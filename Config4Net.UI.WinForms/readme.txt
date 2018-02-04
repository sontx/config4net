//-----------------------------------------------------------
// Builds UI from configuration data and synchronizes UI components value back to configuration data.
//-----------------------------------------------------------

//-----------------------------------------------------------
// Getting Started
//-----------------------------------------------------------

1. Define a config class.

class Person
{
    [Showable]
    public string Name { get; set; }

    public int Age { get; set; }// this property will be invisible in the UI

    [Showable("Where are you now:")]
    public Address Address { get; set; }
}

2.Get configs

var person = Config.Default.Get<Person>();

NOTE: If the configuration does not exist yet, a new instance will be created, registered and then returned.

3. Show config to UI

Loads built-in components that were implemented for native-winform and shows.

new WinFormFlatformLoader().Load();
UiManager.Default.Build<IWindowContainer>(person).Show();

//-----------------------------------------------------------

For more details: https://github.com/sontx/config4net
Need a help, contact me via xuanson33bk@gmail.com or https://www.facebook.com/noem.bk