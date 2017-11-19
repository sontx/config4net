using System;
using NUnit.Framework;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Config4Net.Tests
{
    [TestFixture]
    public class FileWriterTest
    {
        private static readonly string FileToWrite = Path.GetTempFileName();
        private static readonly string FileContent = "hello world";

        [Test]
        public void save_success_if_file_does_not_exist()
        {
            var fileWriter = new FileWriter
            {
                Content = FileContent,
                FilePath = FileToWrite,
                ThrowIfFail = false,
                Timeout = 0
            };

            File.Delete(FileToWrite);
            fileWriter.SaveAsync().Wait();
            Assert.That(File.Exists(FileToWrite), Is.True);
            Assert.That(File.ReadAllText(FileToWrite) == FileContent, Is.True);
        }

        [Test]
        public void save_success_if_file_already_exists()
        {
            var fileWriter = new FileWriter
            {
                Content = FileContent,
                FilePath = FileToWrite,
                ThrowIfFail = false,
                Timeout = 0
            };

            if (!File.Exists(FileToWrite))
            {
                File.WriteAllText(FileToWrite, FileContent);
            }

            fileWriter.SaveAsync().Wait();
            Assert.That(File.Exists(FileToWrite), Is.True);
            Assert.That(File.ReadAllText(FileToWrite) == FileContent, Is.True);
        }

        [Test]
        public void save_success_if_saving_does_not_over_timeout()
        {
            var fileWriter = new FileWriter
            {
                Content = FileContent,
                FilePath = FileToWrite,
                ThrowIfFail = false,
                Timeout = 4000
            };

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(FileToWrite, FileMode.Create))
                {
                    await Task.Delay(1000);
                }
            });
            
            Thread.Sleep(200);

            fileWriter.SaveAsync().Wait();
            Assert.That(File.Exists(FileToWrite), Is.True);
            Assert.That(File.ReadAllText(FileToWrite) == FileContent, Is.True);

            task.Wait();
        }

        [Test]
        public void save_fail_if_saving_is_over_timeout_without_throw_exception()
        {
            var fileToWrite = Path.GetTempFileName();

            var fileWriter = new FileWriter
            {
                Content = FileContent,
                FilePath = fileToWrite,
                ThrowIfFail = false,
                Timeout = 1000
            };

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(fileToWrite, FileMode.Create))
                {
                    await Task.Delay(2000);
                }
            });

            Thread.Sleep(200);

            var success = fileWriter.SaveAsync().Result;
            Assert.That(success, Is.False);

            task.Wait();

            FileUtils.EnsureDelete(fileToWrite);
        }

        [Test]
        public void save_fail_if_saving_is_over_timeout_with_throw_exception()
        {
            var fileToWrite = Path.GetTempFileName();

            var fileWriter = new FileWriter
            {
                Content = FileContent,
                FilePath = fileToWrite,
                ThrowIfFail = true,
                Timeout = 1000
            };

            var task = Task.Run(async () =>
            {
                using (var fs = new FileStream(fileToWrite, FileMode.Create))
                {
                    await Task.Delay(2000);
                }
            });

            Thread.Sleep(200);

            var exception = Assert.Throws<AggregateException>(() => fileWriter.SaveAsync().Wait());
            Assert.IsInstanceOf<IOException>(exception.InnerException);

            task.Wait();

            FileUtils.EnsureDelete(fileToWrite);
        }

        [OneTimeTearDown]
        public void TestTearDown()
        {
            FileUtils.EnsureDelete(FileToWrite);
        }
    }
}