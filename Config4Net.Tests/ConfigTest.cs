using Config4Net.Core;
using Config4Net.Tests.Mock;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Config4Net.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        private const string ValidConfigSample = "{\r\n  \"Metadata\": {\r\n    \"Key\": \"Config4Net.Tests\",\r\n    \"TypeId\": \"Config4Net.Tests.TestConfig, Config4Net.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n    \"Modified\": \"2018-02-03T16:36:56.2611672+07:00\"\r\n  },\r\n  \"ConfigData\": {\r\n    \"Name\": null,\r\n    \"Age\": 24\r\n  }\r\n}";
        private const string CorruptedConfigSample = "Hello there!";

        [Test]
        public void Create__ShouldCreateNewConfigInstance()
        {
            Assert.AreNotEqual(Config.Create(), Config.Create());
        }

        [Test]
        public void Default__ShouldStayTheSame()
        {
            Assert.NotNull(Config.Default);
            Assert.AreEqual(Config.Default, Config.Default);
        }

        [Test]
        public void LoadAsync_HasNotConfigFileInDisk_ShouldLoadOk()
        {
            var config = Utils.CreateConfig();
            Assert.DoesNotThrowAsync(config.LoadAsync);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void LoadAsync_HasConfigFilesInDisk_ShouldLoadOkWithConfigData()
        {
            var config = Utils.CreateConfig();
            Directory.CreateDirectory(config.Settings.ConfigDir);
            File.WriteAllText(Path.Combine(config.Settings.ConfigDir, "Config4Net.Tests.json"), ValidConfigSample);
            Assert.DoesNotThrowAsync(config.LoadAsync);
            Assert.AreEqual(24, config.Get<TestConfig>().Age);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void LoadAsync_CorruptedConfigFileAndIgnoredLoadingFailure_ShouldLoadOkWithoutConfigData()
        {
            var config = Utils.CreateConfig();
            config.Settings.IgnoreLoadingFailure = true;
            File.WriteAllText(Path.Combine(config.Settings.ConfigDir, "Config4Net.Tests.json"), CorruptedConfigSample);
            Assert.DoesNotThrowAsync(config.LoadAsync);
            Assert.AreNotEqual(24, config.Get<TestConfig>().Age);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void LoadAsync_CorruptedConfigFileAndNotIgnoredLoadingFailure_ShouldThrowException()
        {
            var config = Utils.CreateConfig();
            config.Settings.IgnoreLoadingFailure = false;
            File.WriteAllText(Path.Combine(config.Settings.ConfigDir, "Config4Net.Tests.json"), CorruptedConfigSample);
            Assert.ThrowsAsync<ConfigException>(config.LoadAsync);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void SaveAsync_NoConfigData_ShouldHaveNoFileInDisk()
        {
            var config = Utils.CreateConfig();
            config.SaveAsync().Wait();
            Assert.AreEqual(0, Directory.GetFiles(config.Settings.ConfigDir).Length);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void SaveAsync_HasConfigData_ShouldCreateConfigFilesWithSameCount()
        {
            var config = Utils.CreateConfig();
            config.Register<TestConfig>();
            config.Register<AnotherTestConfig>();
            config.SaveAsync().Wait();
            Assert.AreEqual(Directory.GetFiles(config.Settings.ConfigDir).Length, 2);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void SaveAsync__ShouldHaveContentThatDoesNotChangeAfterSaved()
        {
            var config = Utils.CreateConfig();
            config.Get<TestConfig>().Age = 25;
            config.SaveAsync().Wait();

            var anotherConfig = Utils.CreateConfig();
            anotherConfig.Settings.ConfigDir = config.Settings.ConfigDir;
            anotherConfig.LoadAsync().Wait();

            AssertEx.PropertyValuesAreEquals(config.Get<TestConfig>(), anotherConfig.Get<TestConfig>());

            Utils.ReleaseConfig(config);
            Utils.ReleaseConfig(anotherConfig);
        }

        [Test]
        public void Get_KeyDoesNotExistAndAutoRegisterConfigType_ShouldReturnAnInitialConfigData()
        {
            var config = Utils.CreateConfig();
            config.Settings.AutoRegisterConfigType = true;
            var testConfig = config.Get<TestConfig>();
            Assert.NotNull(testConfig);
            Assert.AreEqual(0, testConfig.Age);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Get_KeyDoesNotExistAndNotAutoRegisterConfigType_ShouldThrowException()
        {
            var config = Utils.CreateConfig();
            config.Settings.AutoRegisterConfigType = false;
            Assert.Throws<ConfigException>(() => config.Get<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Get_KeyExists_ReturnAConfigData()
        {
            var config = Utils.CreateConfig();
            Assert.NotNull(config.Get<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Get_KeyExists_ReturnedConfigDataShouldBeAReference()
        {
            var config = Utils.CreateConfig();
            config.Get<TestConfig>().Age = 25;
            Assert.AreEqual(25, config.Get<TestConfig>().Age);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Get_KeyExists_ShouldReturnAConfigDataBySpecificKey()
        {
            var config = Utils.CreateConfig();
            config.Get<AnotherTestConfig>("anotherConfig").Skin = Color.Red;
            Assert.AreEqual(Color.Red, config.Get<AnotherTestConfig>("anotherConfig").Skin);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Get_KeyDoesNotExistAndAutoRegisterConfigType_GetByKeyShouldRegisterTheGivingKeyAutomatically()
        {
            var config = Utils.CreateConfig();
            config.Settings.AutoRegisterConfigType = true;
            config.Get<AnotherTestConfig>("givingKey").Skin = Color.Red;
            Assert.AreEqual(Color.Red, config.Get<AnotherTestConfig>("givingKey").Skin);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Indexer_KeyDoesNotExist_ShouldThrowAnException()
        {
            var config = Utils.CreateConfig();
            Assert.Throws<ConfigException>(() => { var configData = config["unknownKey"]; });
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Indexer_KeyExists_ShouldReturnDynamicConfigData()
        {
            var config = Utils.CreateConfig();
            config.Settings.PreferAppNameAsKey = false;
            config.Register(new TestConfig{Name = "sontx"});
            Assert.DoesNotThrow(() => { var configData = config["TestConfig"]; });
            Assert.AreEqual("sontx", config["TestConfig"].Name);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Register_KeyDoNotExist_RegisterShouldBeOk()
        {
            var config = Utils.CreateConfig();
            Assert.DoesNotThrow(() => config.Register<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Unregister_KeyDoNotExist_ShouldBeOk()
        {
            var config = Utils.CreateConfig();
            Assert.DoesNotThrow(() => config.Unregister<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Unregister_KeyExists_ShouldBeOk()
        {
            var config = Utils.CreateConfig();
            config.Register<TestConfig>();
            Assert.DoesNotThrow(() => config.Unregister<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void Register_KeyExists_RegisterShouldReplaceTheOldOne()
        {
            var config = Utils.CreateConfig();
            var configData1 = new TestConfig();
            var configData2 = new TestConfig();
            config.Register(configData1);
            config.Register(configData2);
            Assert.AreEqual(configData2, config.Get<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void RegisterFactory_FactoryDoesNotExist_ShouldChangeInitialConfigData()
        {
            var config = Utils.CreateConfig();
            var testConfig = new TestConfig { Age = 25 };
            config.RegisterFactory<TestConfig>(new MockConfigDataFactory(testConfig));
            AssertEx.PropertyValuesAreEquals(testConfig, config.Get<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void RegisterFactory_FactoryExists_ShouldReplaceTheOldOne()
        {
            var config = Utils.CreateConfig();
            var testConfig1 = new TestConfig { Age = 25 };
            config.RegisterFactory<TestConfig>(new MockConfigDataFactory(testConfig1));
            var testConfig2 = new TestConfig { Age = 24 };
            config.RegisterFactory<TestConfig>(new MockConfigDataFactory(testConfig2));
            AssertEx.PropertyValuesAreEquals(testConfig2, config.Get<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void UnregisterFactory_FactoryDoesNotExist_ShouldBeOk()
        {
            var config = Utils.CreateConfig();
            Assert.DoesNotThrow(() => config.UnregisterFactory<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void UnregisterFactory_FactoryExists_ShouldBeOk()
        {
            var config = Utils.CreateConfig();
            var testConfig = new TestConfig { Age = 25 };
            config.RegisterFactory<TestConfig>(new MockConfigDataFactory(testConfig));
            Assert.DoesNotThrow(() => config.UnregisterFactory<TestConfig>());
            AssertEx.PropertyValuesAreNotEquals(testConfig, config.Get<TestConfig>());
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigAttribute__ShouldUseTheDefinedKeyInAttribute()
        {
            var config = Utils.CreateConfig();
            config.Register<AnotherTestConfig>();
            Assert.DoesNotThrow(() => config.Get<AnotherTestConfig>("anotherConfig"));
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void DefaultValueAttribute__ShouldSetDefaultPropertyValueInInitialConfigData()
        {
            var config = Utils.CreateConfig();
            config.Register<AnotherTestConfig>();
            Assert.AreEqual(3393, config.Get<AnotherTestConfig>().Value);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigFileNameFactory__CanChangeSavedFileName()
        {
            var config = Utils.CreateConfig();
            config.Settings.ConfigFileNameFactory = new MockConfigFileNameFactory("config.test");
            config.Register<TestConfig>();
            config.SaveAsync().Wait();
            Assert.IsTrue(File.Exists(Path.Combine(config.Settings.ConfigDir, "config.test")));
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigFileFactory__CanChangeConfigFile()
        {
            var config = Utils.CreateConfig();
            config.Settings.ConfigFileFactory = new MockConfigFileFactory("sontx");
            config.Register<AnotherTestConfig>();
            config.SaveAsync().Wait();
            var configFilePath = Path.Combine(config.Settings.ConfigDir, "anotherConfig.json");
            Assert.Greater(File.ReadAllText(configFilePath).IndexOf("sontx", StringComparison.Ordinal), -1);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigFileAdapter__CanChangeSerializingConfigFileContent()
        {
            var config = Utils.CreateConfig();
            var testConfig = new AnotherTestConfig { Skin = Color.Black };
            config.Settings.ConfigFileAdapter = new MockConfigFileAdapter("anotherKey", "Hi There!", testConfig);
            config.Register<AnotherTestConfig>();
            config.SaveAsync().Wait();
            var configFilePath = Path.Combine(config.Settings.ConfigDir, "anotherConfig.json");
            Assert.AreEqual("Hi There!", File.ReadAllText(configFilePath));
            config.LoadAsync().Wait();
            Assert.AreEqual(testConfig.Skin, config.Get<AnotherTestConfig>("anotherKey").Skin);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void StoreService__CanChangeHowConfigFileSaved()
        {
            var config = Utils.CreateConfig();
            config.Settings.StoreService = new MockStoreService("sontx", ValidConfigSample);
            Assert.AreEqual(24, config.Get<TestConfig>("Config4Net.Tests").Age);
            config.SaveAsync().Wait();
            var configFilePath = Path.Combine(config.Settings.ConfigDir, "Config4Net.Tests.json");
            Assert.AreEqual(0, File.ReadAllText(configFilePath).IndexOf("sontx", StringComparison.Ordinal));
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigDir_DoesNotExist_ShouldCreateNewOneIfNecessary()
        {
            var config = Utils.CreateConfig();
            Directory.Delete(config.Settings.ConfigDir);
            config.Register<TestConfig>();
            config.SaveAsync().Wait();
            Assert.IsTrue(Directory.Exists(config.Settings.ConfigDir));
            Assert.Greater(Directory.GetFiles(config.Settings.ConfigDir).Length, 0);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigDir_IsNullOrEmpty_ShouldThrowException()
        {
            var config = Utils.CreateConfig();
            config.Settings.ConfigDir = null;
            Assert.ThrowsAsync<ConfigException>(config.SaveAsync);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigFileExtension_IsNullOrEmpty_SavedFileShouldHaveNoExtension()
        {
            var config = Utils.CreateConfig();
            config.Settings.ConfigFileExtension = null;
            config.Register<AnotherTestConfig>();
            config.SaveAsync().Wait();
            var savedFilePath = Path.Combine(config.Settings.ConfigDir, "anotherConfig");
            Assert.IsTrue(File.Exists(savedFilePath));
            Assert.Greater(File.ReadAllText(savedFilePath).Length, 0);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ConfigFileExtension__CanChangeFileExtension()
        {
            var config = Utils.CreateConfig();
            config.Settings.ConfigFileExtension = "txt";
            config.Register<AnotherTestConfig>();
            config.SaveAsync().Wait();
            var savedFilePath = Path.Combine(config.Settings.ConfigDir, "anotherConfig.txt");
            Assert.IsTrue(File.Exists(savedFilePath));
            Assert.Greater(File.ReadAllText(savedFilePath).Length, 0);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ApplicationClosingEvent_SaveWhenApplicationClosing_ShouldAutoSaveWhenAppClosing()
        {
            var config = Utils.CreateConfig();
            var @event = new MockApplicationClosingEvent();
            config.Settings.ApplicationClosingEvent = @event;
            config.Register<AnotherTestConfig>();
            @event.SimulateEvent();
            var savedFilePath = Path.Combine(config.Settings.ConfigDir, "anotherConfig.json");
            Assert.IsTrue(File.Exists(savedFilePath));
            Assert.Greater(File.ReadAllText(savedFilePath).Length, 0);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void ApplicationClosingEvent_NotSaveWhenApplicationClosing_ShouldNotAutoSaveWhenAppClosing()
        {
            var config = Utils.CreateConfig();
            var @event = new MockApplicationClosingEvent();
            config.Settings.ApplicationClosingEvent = @event;
            config.Settings.SaveWhenApplicationClosing = false;
            config.Register<AnotherTestConfig>();
            @event.SimulateEvent();
            var savedFilePath = Path.Combine(config.Settings.ConfigDir, "anotherConfig.json");
            Assert.IsFalse(File.Exists(savedFilePath));
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void PreventNullReference_IsTrue_ConfigDataShouldHaveNoNullProperty()
        {
            var config = Utils.CreateConfig();
            Directory.CreateDirectory(config.Settings.ConfigDir);
            File.WriteAllText(Path.Combine(config.Settings.ConfigDir, "Config4Net.Tests.json"), ValidConfigSample);
            config.Settings.PreventNullReference = true;
            Assert.NotNull(config.Get<TestConfig>().Name);
            Assert.NotNull(config.Get<TestConfig>("anotherConfigHere").Name);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void PreventNullReference_IsFalse_ConfigDataShouldHaveNullPropertyIfItIs()
        {
            var config = Utils.CreateConfig();
            Directory.CreateDirectory(config.Settings.ConfigDir);
            File.WriteAllText(Path.Combine(config.Settings.ConfigDir, "Config4Net.Tests.json"), ValidConfigSample);
            config.Settings.PreventNullReference = false;
            Assert.IsNull(config.Get<TestConfig>().Name);
            Assert.IsNull(config.Get<TestConfig>("anotherConfigHere").Name);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void PreferAppNameAsKey_IsTrue_ShouldUseAppNameAsDefaultKey()
        {
            var config = Utils.CreateConfig();
            config.Settings.PreferAppNameAsKey = true;
            config.Register<TestConfig>();
            config.Get<TestConfig>().Age = 25;
            Assert.AreEqual(config.Get<TestConfig>("Config4Net.Tests").Age, 25);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void PreferAppNameAsKey_IsFalse_ShouldUseConfigTypeNameAsDefaultKey()
        {
            var config = Utils.CreateConfig();
            config.Settings.PreferAppNameAsKey = false;
            config.Register<TestConfig>();
            config.Get<TestConfig>().Age = 25;
            Assert.AreEqual(config.Get<TestConfig>("TestConfig").Age, 25);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void AppName_PreferAppNameAsKey_ShouldUseAppNameAsDefaultKey()
        {
            var config = Utils.CreateConfig();
            config.Settings.PreferAppNameAsKey = true;
            config.Settings.AppName = "sontx";
            config.Register<TestConfig>();
            config.Get<TestConfig>().Age = 25;
            Assert.AreEqual(config.Get<TestConfig>(config.Settings.AppName).Age, 25);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void AppName_NotPreferAppNameAsKey_ShouldUseConfigTypeNameAsDefaultKey()
        {
            var config = Utils.CreateConfig();
            config.Settings.PreferAppNameAsKey = false;
            config.Settings.AppName = "sontx";
            config.Register<TestConfig>();
            config.Get<TestConfig>().Age = 25;
            Assert.AreEqual(config.Get<TestConfig>("TestConfig").Age, 25);
            Utils.ReleaseConfig(config);
        }
    }

    internal class TestConfig
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [Config("anotherConfig")]
    internal class AnotherTestConfig
    {
        [DefaultValue(3393)]
        public int Value { get; set; }

        public Color Skin { get; set; }
        public DateTime Created { get; set; }
    }
}