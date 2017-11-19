using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Config4Net.Core;

namespace Config4Net.Tests
{
    [TestFixture]
    public class ConfigPoolTest
    {
        private static readonly string ConfigDir = Path.GetTempPath();

        [OneTimeSetUp]
        public void TestSetup()
        {
            ConfigPool.ConfigDir = ConfigDir;
        }

        [Test]
        public void get_app_config_that_already_exists_should_return_config_object()
        {
            ConfigPool.RegisterConfigType<MyConfig>();
            var config = ConfigPool.App<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void get_calling_config_that_already_exists_should_return_config_object()
        {
            ConfigPool.RegisterConfigType<MyConfig>();
            var config = ConfigPool.Calling<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void get_config_by_key_that_already_exists_should_return_config_object()
        {
            ConfigPool.RegisterConfigType<MySubConfig1>();
            var config = ConfigPool.Get<MySubConfig1>("subConfig1");
            Assert.IsInstanceOf<MySubConfig1>(config);
        }

        [Test]
        public void get_config_by_config_type_should_return_config_object()
        {
            ConfigPool.RegisterConfigType<MySubConfig1>();
            var config = ConfigPool.Get<MySubConfig1>();
            Assert.IsInstanceOf<MySubConfig1>(config);
        }

        [Test]
        public void get_config_that_does_not_exist_should_return_null_if_AutoRegisterConfigType_is_false()
        {
            ConfigPool.AutoRegisterConfigType = false;
            ConfigPool.UnregisterConfigType<MyConfig>();
            var config = ConfigPool.App<MyConfig>();
            Assert.IsNull(config);
        }

        [Test]
        public void get_config_that_does_not_exist_should_return_config_object_if_AutoRegisterConfigType_is_true()
        {
            ConfigPool.AutoRegisterConfigType = true;
            ConfigPool.UnregisterConfigType<MyConfig>();
            var config = ConfigPool.App<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void register_config_type_should_return_config_object_if_success()
        {
            ConfigPool.UnregisterConfigType<MyConfig>();
            var config = ConfigPool.RegisterConfigType<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void register_config_type_should_throw_exception_if_registered_invalid_config_class()
        {
            Assert.Throws<InvalidConfigTypeException>(() => ConfigPool.RegisterConfigType<NotConfigClass>());
        }

        [Test]
        public void register_config_type_should_ignore_the_one_that_already_exists()
        {
            ConfigPool.UnregisterConfigType<MyConfig>();
            var config1 = ConfigPool.RegisterConfigType<MyConfig>();
            var config2 = ConfigPool.RegisterConfigType<MyConfig>();
            Assert.AreEqual(config1, config2);
        }

        [Test]
        public void register_config_type_with_IsAppConfig_attribute_should_be_app_config()
        {
            ConfigPool.UnregisterConfigType<MyConfig>();
            ConfigPool.RegisterConfigType<MyConfig>();
            var config = ConfigPool.Get<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void register_config_type_with_IsAppConfig_attribute_should_ignore_Key_attribute()
        {
            ConfigPool.AutoRegisterConfigType = false;
            ConfigPool.UnregisterConfigType<MyConfig>();
            ConfigPool.RegisterConfigType<MyConfig>();
            var config = ConfigPool.Get<MyConfig>("myConfig");
            Assert.IsNull(config);
        }

        [Test]
        public void register_config_type_should_take_Key_attribute_as_config_key()
        {
            ConfigPool.AutoRegisterConfigType = false;
            ConfigPool.UnregisterConfigType<MySubConfig1>();
            ConfigPool.RegisterConfigType<MySubConfig1>();
            var config = ConfigPool.Get<MySubConfig1>("subConfig1");
            Assert.IsInstanceOf<MySubConfig1>(config);
        }

        [Test]
        public void register_config_type_should_auto_detect_key_by_calling_assembly_when_Key_and_IsAppConfig_attributes_are_blank()
        {
            ConfigPool.AutoRegisterConfigType = false;
            ConfigPool.UnregisterConfigType<MySubConfig2>();
            ConfigPool.RegisterConfigType<MySubConfig2>();
            var keyByAssembly = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            var config = ConfigPool.Get<MySubConfig2>(keyByAssembly);
            Assert.IsInstanceOf<MySubConfig2>(config);
        }

        [Test]
        public void register_config_type_should_init_with_given_config_object()
        {
            ConfigPool.AutoRegisterConfigType = false;
            ConfigPool.UnregisterConfigType<MySubConfig1>();
            var initConfig = new MySubConfig1 { Config1 = 1 };
            ConfigPool.RegisterConfigType(initConfig);
            var config = ConfigPool.Get<MySubConfig1>("subConfig1");
            Assert.AreEqual(initConfig, config);
        }

        [Test]
        public void save_config_files_should_place_in_defined_dir()
        {
            var subConfig1FilePath = Path.Combine(ConfigDir, $@"subConfig1.{Constants.ConfigFileExtention}");
            var appConfigFilePath = Path.Combine(ConfigDir, $@"{Constants.ApplicationConfigKey}.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            ConfigPool.RegisterConfigType<MySubConfig1>();
            ConfigPool.RegisterConfigType(new MyConfig { MySubConfig2 = new MySubConfig2() });
            ConfigPool.SaveAsync(ConfigDir).Wait();

            Assert.That(File.Exists(subConfig1FilePath), Is.True);
            Assert.That(File.Exists(appConfigFilePath), Is.True);

            Assert.Greater(File.ReadAllText(subConfig1FilePath).Length, 0);
            Assert.Greater(File.ReadAllText(appConfigFilePath).Length, 0);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }

        [Test]
        public void save_config_files_should_place_in_current_dir_if_did_not_give_config_dir()
        {
            var currentDir = Environment.CurrentDirectory;
            var subConfig1FilePath = Path.Combine(currentDir, $@"subConfig1.{Constants.ConfigFileExtention}");
            var appConfigFilePath = Path.Combine(currentDir, $@"{Constants.ApplicationConfigKey}.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            ConfigPool.ConfigDir = null;

            ConfigPool.RegisterConfigType<MySubConfig1>();
            ConfigPool.RegisterConfigType(new MyConfig { MySubConfig2 = new MySubConfig2() });
            ConfigPool.SaveAsync(null).Wait();

            Assert.That(File.Exists(subConfig1FilePath), Is.True);
            Assert.That(File.Exists(appConfigFilePath), Is.True);

            Assert.Greater(File.ReadAllText(subConfig1FilePath).Length, 0);
            Assert.Greater(File.ReadAllText(appConfigFilePath).Length, 0);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }

        [Test]
        public void save_config_files_should_be_successful_if_not_over_timeout()
        {
            var subConfig1FilePath = Path.Combine(ConfigDir, $@"subConfig1.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            ConfigPool.LoadAsync(ConfigDir).Wait();

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(subConfig1FilePath, FileMode.Create))
                {
                    await Task.Delay(1500);
                }
            });

            Thread.Sleep(200);

            ConfigPool.RegisterConfigType<MySubConfig1>();
            ConfigPool.SaveAsync(ConfigDir, true).Wait();

            Assert.That(File.Exists(subConfig1FilePath), Is.True);
            Assert.Greater(File.ReadAllText(subConfig1FilePath).Length, 0);

            task.Wait();

            FileUtils.EnsureDelete(subConfig1FilePath);
        }

        [Test]
        public void save_config_files_should_throw_exception_if_over_timeout_in_persistant_mode()
        {
            var subConfig1FilePath = Path.Combine(ConfigDir, $@"subConfig1.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            ConfigPool.LoadAsync(ConfigDir).Wait();

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(subConfig1FilePath, FileMode.Create))
                {
                    await Task.Delay(Constants.WriteFileTimeoutInMilliseconds + 1000);
                }
            });

            Thread.Sleep(200);

            ConfigPool.RegisterConfigType<MySubConfig1>();
            Assert.Throws(Is.InstanceOf<Exception>(), () => ConfigPool.SaveAsync(ConfigDir, true).Wait());

            task.Wait();

            FileUtils.EnsureDelete(subConfig1FilePath);
        }

        [Test]
        public void save_config_files_should_not_throw_exception_if_over_timeout_in_normal_mode()
        {
            var subConfig1FilePath = Path.Combine(ConfigDir, $@"subConfig1.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            ConfigPool.LoadAsync(ConfigDir).Wait();

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(subConfig1FilePath, FileMode.Create))
                {
                    await Task.Delay(Constants.WriteFileTimeoutInMilliseconds + 1000);
                }
            });

            Thread.Sleep(200);

            ConfigPool.RegisterConfigType<MySubConfig1>();
            Assert.DoesNotThrow(() => ConfigPool.SaveAsync(ConfigDir, false).Wait());

            task.Wait();

            FileUtils.EnsureDelete(subConfig1FilePath);
        }

        [Test]
        public void save_config_files_should_done_automatically_when_app_closing()
        {
            var subConfig1FilePath = Path.Combine(ConfigDir, $@"subConfig1.{Constants.ConfigFileExtention}");
            var appConfigFilePath = Path.Combine(ConfigDir, $@"{Constants.ApplicationConfigKey}.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            var applicationClosingEvent = new MockApplicationClosingEvent();

            ConfigPool.SetApplicationClosingEvent(applicationClosingEvent);
            ConfigPool.AutoSaveWhenApplicationClosing = true;
            ConfigPool.RegisterConfigType<MySubConfig1>();
            ConfigPool.RegisterConfigType(new MyConfig { MySubConfig2 = new MySubConfig2() });
            applicationClosingEvent.Raise();

            Assert.That(File.Exists(subConfig1FilePath), Is.True);
            Assert.That(File.Exists(appConfigFilePath), Is.True);

            Assert.Greater(File.ReadAllText(subConfig1FilePath).Length, 0);
            Assert.Greater(File.ReadAllText(appConfigFilePath).Length, 0);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }

        [Test]
        public void load_config_files_from_given_config_dir_should_be_successful()
        {
            var subConfig1FilePath = Path.Combine(ConfigDir, $@"subConfig1.{Constants.ConfigFileExtention}");
            var appConfigFilePath = Path.Combine(ConfigDir, $@"{Constants.ApplicationConfigKey}.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            var oldSubConfig1 = new MySubConfig1
            {
                Config3 = DateTime.MaxValue,
                Config1 = 2,
                Config2 = "sontx"
            };
            var oldAppConfig = new MyConfig
            {
                Config3 = TimeSpan.MaxValue
            };

            ConfigPool.ConfigDir = ConfigDir;

            ConfigPool.RegisterConfigType(oldSubConfig1);
            ConfigPool.RegisterConfigType(oldAppConfig);

            ConfigPool.SaveAsync(ConfigDir).Wait();

            ConfigPool.LoadAsync(ConfigDir).Wait();

            var subConfig1 = ConfigPool.Get<MySubConfig1>();
            var appConfig = ConfigPool.Get<MyConfig>();

            AssertEx.PropertyValuesAreEquals(subConfig1, oldSubConfig1);
            AssertEx.PropertyValuesAreEquals(oldAppConfig, appConfig);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }

        [Test]
        public void load_config_files_without_given_config_dir_should_lookup_in_current_dir()
        {
            var configDir = Environment.CurrentDirectory;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{Constants.ConfigFileExtention}");
            var appConfigFilePath = Path.Combine(configDir, $@"{Constants.ApplicationConfigKey}.{Constants.ConfigFileExtention}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            var oldSubConfig1 = new MySubConfig1
            {
                Config3 = DateTime.MaxValue,
                Config1 = 2,
                Config2 = "sontx"
            };
            var oldAppConfig = new MyConfig
            {
                Config3 = TimeSpan.MaxValue
            };

            ConfigPool.ConfigDir = null;

            ConfigPool.RegisterConfigType(oldSubConfig1);
            ConfigPool.RegisterConfigType(oldAppConfig);

            ConfigPool.SaveAsync(null).Wait();

            ConfigPool.LoadAsync(null).Wait();

            var subConfig1 = ConfigPool.Get<MySubConfig1>();
            var appConfig = ConfigPool.Get<MyConfig>();

            AssertEx.PropertyValuesAreEquals(subConfig1, oldSubConfig1);
            AssertEx.PropertyValuesAreEquals(oldAppConfig, appConfig);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }
    }

    [Config(IsAppConfig = true, Key = "myConfig")]
    public class MyConfig
    {
        public MySubConfig1 MySubConfig1 { get; set; }
        public MySubConfig2 MySubConfig2 { get; set; }
        public TimeSpan Config3 { get; set; }
    }

    [Config(Key = "subConfig1")]
    public class MySubConfig1
    {
        public int Config1 { get; set; }
        public string Config2 { get; set; }
        public DateTime Config3 { get; set; }
    }

    [Config]
    public class MySubConfig2
    {
        public float Config1 { get; set; }
        public Color Config2 { get; set; }
        public SomeEnum Config3 { get; set; }
    }

    public class NotConfigClass
    {
    }

    public enum SomeEnum
    {
        Enum1,
        Enum2
    }

    public class MockApplicationClosingEvent : IApplicationClosingEvent
    {
        public event EventHandler AppClosing;

        public void Raise()
        {
            AppClosing?.Invoke(this, EventArgs.Empty);
        }

        public void Register()
        {
        }

        public void Unregister()
        {
        }
    }
}