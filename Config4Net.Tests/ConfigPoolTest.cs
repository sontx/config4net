using Config4Net.Core;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Config4Net.Tests
{
    [TestFixture]
    public class ConfigPoolTest
    {
        private readonly ConcurrentBag<string> _registeredConfigDirList = new ConcurrentBag<string>();

        private string ConfigDir
        {
            get
            {
                var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(dir);
                _registeredConfigDirList.Add(dir);
                return dir;
            }
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (var dir in _registeredConfigDirList)
            {
                Directory.Delete(dir, true);
            }
        }
            
        [Test]
        public void get_app_config_that_already_exists_should_return_config_object()
        {
            var configPool = ConfigPool.Create();
            configPool.RegisterConfigType<MyConfig>();
            var config = configPool.App<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void get_calling_config_that_already_exists_should_return_config_object()
        {
            var configPool = ConfigPool.Create();
            configPool.RegisterConfigType<MyConfig>();
            var config = configPool.Calling<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void get_config_by_key_that_already_exists_should_return_config_object()
        {
            var configPool = ConfigPool.Create();
            configPool.RegisterConfigType<MySubConfig1>();
            var config = configPool.Get<MySubConfig1>("subConfig1");
            Assert.IsInstanceOf<MySubConfig1>(config);
        }

        [Test]
        public void get_config_by_config_type_should_return_config_object()
        {
            var configPool = ConfigPool.Create();
            configPool.RegisterConfigType<MySubConfig1>();
            var config = configPool.Get<MySubConfig1>();
            Assert.IsInstanceOf<MySubConfig1>(config);
        }

        [Test]
        public void get_config_that_does_not_exist_should_return_null_if_AutoRegisterConfigType_is_false()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.AutoRegisterConfigType = false;
            configPool.UnregisterConfigType<MyConfig>();
            var config = configPool.App<MyConfig>();
            Assert.IsNull(config);
        }

        [Test]
        public void get_config_that_does_not_exist_should_return_config_object_if_AutoRegisterConfigType_is_true()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.AutoRegisterConfigType = true;
            configPool.UnregisterConfigType<MyConfig>();
            var config = configPool.App<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void register_config_type_should_return_config_object_if_success()
        {
            var configPool = ConfigPool.Create();
            configPool.UnregisterConfigType<MyConfig>();
            var config = configPool.RegisterConfigType<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }
        
        [Test]
        public void register_config_type_should_ignore_the_one_that_already_exists()
        {
            var configPool = ConfigPool.Create();
            configPool.UnregisterConfigType<MyConfig>();
            var config1 = configPool.RegisterConfigType<MyConfig>();
            var config2 = configPool.RegisterConfigType<MyConfig>();
            Assert.AreEqual(config1, config2);
        }

        [Test]
        public void register_config_type_with_IsAppConfig_attribute_should_be_app_config()
        {
            var configPool = ConfigPool.Create();
            configPool.UnregisterConfigType<MyConfig>();
            configPool.RegisterConfigType<MyConfig>();
            var config = configPool.Get<MyConfig>();
            Assert.IsInstanceOf<MyConfig>(config);
        }

        [Test]
        public void register_config_type_with_IsAppConfig_attribute_should_ignore_Key_attribute()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.AutoRegisterConfigType = false;
            configPool.UnregisterConfigType<MyConfig>();
            configPool.RegisterConfigType<MyConfig>();
            var config = configPool.Get<MyConfig>("myConfig");
            Assert.IsNull(config);
        }

        [Test]
        public void register_config_type_should_take_Key_attribute_as_config_key()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.AutoRegisterConfigType = false;
            configPool.UnregisterConfigType<MySubConfig1>();
            configPool.RegisterConfigType<MySubConfig1>();
            var config = configPool.Get<MySubConfig1>("subConfig1");
            Assert.IsInstanceOf<MySubConfig1>(config);
        }

        [Test]
        public void register_config_type_should_auto_detect_key_by_calling_assembly_when_Key_and_IsAppConfig_attributes_are_blank()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.AutoRegisterConfigType = false;
            configPool.UnregisterConfigType<MySubConfig2>();
            configPool.RegisterConfigType<MySubConfig2>();
            var keyByAssembly = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            var config = configPool.Get<MySubConfig2>(keyByAssembly);
            Assert.IsInstanceOf<MySubConfig2>(config);
        }

        [Test]
        public void register_config_type_should_init_with_given_config_object()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.AutoRegisterConfigType = false;
            configPool.UnregisterConfigType<MySubConfig1>();
            var initConfig = new MySubConfig1 { Config1 = 1 };
            configPool.RegisterConfigType(initConfig);
            var config = configPool.Get<MySubConfig1>("subConfig1");
            Assert.AreEqual(initConfig, config);
        }

        [Test]
        public void save_config_files_should_place_in_defined_dir()
        {
            var configPool = ConfigPool.Create();
            var configDir = ConfigDir;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");
            var appConfigFilePath = Path.Combine(configDir, $@"{configPool.Settings.AppConfigKey}.{configPool.Settings.ConfigFileExtension}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            configPool.RegisterConfigType<MySubConfig1>();
            configPool.RegisterConfigType(new MyConfig { MySubConfig2 = new MySubConfig2() });
            configPool.SaveAsync(configDir).Wait();

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
            var configPool = ConfigPool.Create();

            var currentDir = Environment.CurrentDirectory;
            var subConfig1FilePath = Path.Combine(currentDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");
            var appConfigFilePath = Path.Combine(currentDir, $@"{configPool.Settings.AppConfigKey}.{configPool.Settings.ConfigFileExtension}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            configPool.Settings.ConfigDir = null;

            configPool.RegisterConfigType<MySubConfig1>();
            configPool.RegisterConfigType(new MyConfig { MySubConfig2 = new MySubConfig2() });
            configPool.SaveAsync(null).Wait();

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
            var configPool = ConfigPool.Create();
            var configDir = ConfigDir;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            configPool.LoadAsync(configDir).Wait();

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(subConfig1FilePath, FileMode.Create))
                {
                    await Task.Delay(1500);
                }
            });

            Thread.Sleep(200);

            configPool.RegisterConfigType<MySubConfig1>();
            configPool.SaveAsync(configDir, true).Wait();

            Assert.That(File.Exists(subConfig1FilePath), Is.True);
            Assert.Greater(File.ReadAllText(subConfig1FilePath).Length, 0);

            task.Wait();

            FileUtils.EnsureDelete(subConfig1FilePath);
        }

        [Test]
        public void save_config_files_should_throw_exception_if_over_timeout_in_persistant_mode()
        {
            var configPool = ConfigPool.Create();
            var configDir = ConfigDir;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            configPool.LoadAsync(configDir).Wait();

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(subConfig1FilePath, FileMode.Create))
                {
                    await Task.Delay(configPool.Settings.WriteFileTimeout + 1000);
                }
            });

            Thread.Sleep(200);

            configPool.RegisterConfigType<MySubConfig1>();
            Assert.Throws(Is.InstanceOf<Exception>(), () => configPool.SaveAsync(configDir, true).Wait());

            task.Wait();

            FileUtils.EnsureDelete(subConfig1FilePath);
        }

        [Test]
        public void save_config_files_should_not_throw_exception_if_over_timeout_in_normal_mode()
        {
            var configPool = ConfigPool.Create();
            var configDir = ConfigDir;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            configPool.LoadAsync(configDir).Wait();

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(subConfig1FilePath, FileMode.Create))
                {
                    await Task.Delay(configPool.Settings.WriteFileTimeout + 1000);
                }
            });

            Thread.Sleep(200);

            configPool.RegisterConfigType<MySubConfig1>();
            Assert.DoesNotThrow(() => configPool.SaveAsync(configDir, false).Wait());

            task.Wait();

            FileUtils.EnsureDelete(subConfig1FilePath);
        }

        [Test]
        public void save_config_files_should_done_automatically_when_app_closing()
        {
            var configPool = ConfigPool.Create();
            var configDir = ConfigDir;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");
            var appConfigFilePath = Path.Combine(configDir, $@"{configPool.Settings.AppConfigKey}.{configPool.Settings.ConfigFileExtension}");

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);

            var applicationClosingEvent = new MockApplicationClosingEvent();

            configPool.Settings.ConfigDir = configDir;
            configPool.Settings.ApplicationClosingEvent = applicationClosingEvent;
            configPool.Settings.AutoSaveWhenApplicationClosing = true;
            configPool.RegisterConfigType<MySubConfig1>();
            configPool.RegisterConfigType(new MyConfig { MySubConfig2 = new MySubConfig2() });
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
            var configPool = ConfigPool.Create();
            var configDir = ConfigDir;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");
            var appConfigFilePath = Path.Combine(configDir, $@"{configPool.Settings.AppConfigKey}.{configPool.Settings.ConfigFileExtension}");

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

            configPool.Settings.ConfigDir = configDir;

            configPool.RegisterConfigType(oldSubConfig1);
            configPool.RegisterConfigType(oldAppConfig);

            configPool.SaveAsync(configDir).Wait();

            configPool.LoadAsync(configDir).Wait();

            var subConfig1 = configPool.Get<MySubConfig1>();
            var appConfig = configPool.Get<MyConfig>();

            AssertEx.PropertyValuesAreEquals(subConfig1, oldSubConfig1);
            AssertEx.PropertyValuesAreEquals(oldAppConfig, appConfig);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }

        [Test]
        public void load_config_files_without_given_config_dir_should_lookup_in_current_dir()
        {
            var configPool = ConfigPool.Create();

            var configDir = Environment.CurrentDirectory;

            var subConfig1FilePath = Path.Combine(configDir, $@"subConfig1.{configPool.Settings.ConfigFileExtension}");
            var appConfigFilePath = Path.Combine(configDir, $@"{configPool.Settings.AppConfigKey}.{configPool.Settings.ConfigFileExtension}");

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

            configPool.Settings.ConfigDir = null;

            configPool.RegisterConfigType(oldSubConfig1);
            configPool.RegisterConfigType(oldAppConfig);

            configPool.SaveAsync(null).Wait();

            configPool.LoadAsync(null).Wait();

            var subConfig1 = configPool.Get<MySubConfig1>();
            var appConfig = configPool.Get<MyConfig>();

            AssertEx.PropertyValuesAreEquals(subConfig1, oldSubConfig1);
            AssertEx.PropertyValuesAreEquals(oldAppConfig, appConfig);

            FileUtils.EnsureDelete(subConfig1FilePath);
            FileUtils.EnsureDelete(appConfigFilePath);
        }

        [Test]
        public void config_object_should_have_non_null_properties__with_PreventNullReference_is_true()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.PreventNullReference = true;
            var config = configPool.Get<MySubConfig1>();
            Assert.NotNull(config.Config2);
        }

        [Test]
        public void config_object_should_have_null_properties_if_they_dont_present__with_PreventNullReference_is_false()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.PreventNullReference = false;
            var config = configPool.Get<MySubConfig1>();
            Assert.Null(config.Config2);
        }

        [Test]
        public void config_properties_should_have_default_value_that_define_by_DefaultAttribute()
        {
            var configPool = ConfigPool.Create();
            configPool.Settings.PreventNullReference = false;
            var config = configPool.Get<DefaultValueConfig>();
            Assert.AreEqual(config.Value, "some default value here");
        }
    }

    [Config]
    public class DefaultValueConfig
    {
        [Default("some default value here")]
        public string Value { get; set; }
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