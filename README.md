# config4net
Config4net allows you to store and retrieve settings and other information for your app dynamically.
Config4net is an alternative to built-in Settings feature in Visual Studio.
There are three points that make config4net becomes the best choice for manage configuration information.
1. Define configuration from classes and attributes.
2. Manage many configuration instances at the same time.
3. Edit configuration from UI at the runtime as a part of the application.
# Take a look
## Define configuration classes.
```cs
[Showable("Your personal information")]
[Config]
class Person
{
    [Showable("First name:", ComponentType = typeof(ITextEditor))]
    public string FirstName { get; set; }

    [Showable("Last name:", ComponentType = typeof(ITextEditor))]
    public string LastName { get; set; }

    [Showable(ComponentType = typeof(INumberEditor))]
    public int Age { get; set; }

    [Showable("Sex:", ComponentType = typeof(ICheckboxEditor), Description = "You are male?")]
    public bool Male { get; set; }

    [Showable("Where are you now:")]
    public Address Address { get; set; }

    [Showable("Your job:")]
    public Job Job { get; set; }
}

class Address
{
    [Showable(ComponentType = typeof(ITextEditor))]
    public string City { get; set; }

    [Showable(ComponentType = typeof(INumberEditor))]
    public int FaxCode { get; set; }

    public string InvisibleField { get; set; }
}

class Job
{
    [Showable(ComponentType = typeof(ITextEditor))]
    public string Company { get; set; }

    [Showable(ComponentType = typeof(ITextEditor))]
    public string Position { get; set; }

    public int Salary { get; set; }
}
```
## Load configuration and show to UI to edit.
```cs
// Load configuration from file if it already exists or create new one.
var person = ConfigPool.Get<Person>();
// Load built-in components that were implemented for winform.
new WinFormFlatformLoader().Load();
// Show person configuration to UI and enjoy ;)
var window = UiManager.Default.Build<IWindowContainer>(person);
```
Edit your configuration here and that will be updated to your configuration instance and save to disk later.
<img src="https://4.bp.blogspot.com/-cU8v35QDM6E/Wh7rSFS41xI/AAAAAAAAS98/UMAmi6xFORwzcMnUGAQtlu0pWqBhWJsFQCLcBGAs/s1600/Capture.PNG"/>
# Core features
Comming soon...
# Install
Comming soon...
