using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using Config4Net.Core;
using NUnit.Framework;

namespace Config4Net.Tests
{
    [TestFixture]
    public class ConfigUtilsTest
    {
        [Test]
        public void BackupToZipAsync_NoConfigFile_ShouldGenerateAnEmptyZipFile()
        {
            var config = Utils.CreateConfig();
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            config.BackupToZipAsync(zipFilePath).Wait();
            Assert.AreEqual(0, ComputeZipEntryCount(zipFilePath));
            File.Delete(zipFilePath);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void BackupToZipAsync_HasConfigFile_ShouldGenerateAnNotEmptyZipFile()
        {
            var config = Utils.CreateConfig();
            config.Register<TestConfig>();
            config.Register<AnotherTestConfig>();
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            config.BackupToZipAsync(zipFilePath).Wait();
            Assert.AreEqual(2, ComputeZipEntryCount(zipFilePath));
            File.Delete(zipFilePath);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void BackupToZipAsync_ZipFileAlreadyExists_ShouldOverwrite()
        {
            var config = Utils.CreateConfig();
            config.Register<TestConfig>();
            config.Register<AnotherTestConfig>();
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllText(zipFilePath, @"Existing file");
            config.BackupToZipAsync(zipFilePath).Wait();
            Assert.AreEqual(2, ComputeZipEntryCount(zipFilePath));
            File.Delete(zipFilePath);
            Utils.ReleaseConfig(config);
        }

        [Test]
        public void RestoreFromZipAsync_NotEmptyZipFile_ShouldAppendNewConfig()
        {
            var backupConfig = Utils.CreateConfig();
            backupConfig.Get<TestConfig>().Name = "sontx";
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            backupConfig.BackupToZipAsync(zipFilePath).Wait();

            var restoreConfig = Utils.CreateConfig();
            restoreConfig.Get<AnotherTestConfig>().Skin = Color.Red;
            restoreConfig.RestoreFromZipAsync(zipFilePath).Wait();
            Assert.AreEqual(Color.Red, restoreConfig.Get<AnotherTestConfig>().Skin);
            Assert.AreEqual("sontx", restoreConfig.Get<TestConfig>().Name);
            Utils.ReleaseConfig(restoreConfig);

            File.Delete(zipFilePath);
            Utils.ReleaseConfig(backupConfig);
        }

        [Test]
        public void RestoreFromZipAsync_EmptyZipFile_ShouldChangeNothing()
        {
            var backupConfig = Utils.CreateConfig();
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            backupConfig.BackupToZipAsync(zipFilePath).Wait();

            var restoreConfig = Utils.CreateConfig();
            restoreConfig.Get<AnotherTestConfig>().Skin = Color.Red;
            restoreConfig.RestoreFromZipAsync(zipFilePath).Wait();
            var configMap = Utils.GetFieldValueByPath<Dictionary<string, object>>(restoreConfig, "_configDataManager._configMap");
            Assert.AreEqual(1, configMap.Count);
            Assert.AreEqual(Color.Red, restoreConfig.Get<AnotherTestConfig>().Skin);
            Utils.ReleaseConfig(restoreConfig);

            File.Delete(zipFilePath);
            Utils.ReleaseConfig(backupConfig);
        }

        [Test]
        public void RestoreFromZipAsync_CorruptedZipFile_ShouldThrowExceptionAndChangeNothing()
        {
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllText(zipFilePath, @"Corrupted zip file");

            var restoreConfig = Utils.CreateConfig();
            restoreConfig.Get<AnotherTestConfig>().Skin = Color.Red;
            Assert.ThrowsAsync<InvalidDataException>(async () => { await restoreConfig.RestoreFromZipAsync(zipFilePath); });
            var configMap = Utils.GetFieldValueByPath<Dictionary<string, object>>(restoreConfig, "_configDataManager._configMap");
            Assert.AreEqual(1, configMap.Count);
            Utils.ReleaseConfig(restoreConfig);

            File.Delete(zipFilePath);
        }

        [Test]
        public void RestoreFromZipAsync_FileDoesNotExist_ShouldThrowExceptionAndChangeNothing()
        {
            var zipFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);

            var restoreConfig = Utils.CreateConfig();
            restoreConfig.Get<AnotherTestConfig>().Skin = Color.Red;
            Assert.ThrowsAsync<FileNotFoundException>(async () => { await restoreConfig.RestoreFromZipAsync(zipFilePath); });
            var configMap = Utils.GetFieldValueByPath<Dictionary<string, object>>(restoreConfig, "_configDataManager._configMap");
            Assert.AreEqual(1, configMap.Count);
            Utils.ReleaseConfig(restoreConfig);
        }

        private int ComputeZipEntryCount(string zipFilePath)
        {
            if (!File.Exists(zipFilePath))
                return -1;

            using (var zipFile = ZipFile.OpenRead(zipFilePath))
            {
                return zipFile.Entries.Count;
            }
        }
    }
}