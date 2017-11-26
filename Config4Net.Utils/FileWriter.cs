using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Config4Net.Utils
{
    /// <summary>
    /// Provides a saving file hepler that ensure content will be
    /// saved to file, if some exception occurs then try again
    /// until success or reach the timeout.
    /// </summary>
    public class FileWriter
    {
        public string FilePath { get; set; }
        public string Content { get; set; }
        public int Timeout { get; set; } = 1000;// 1s
        public bool ThrowIfFail { get; set; }

        public Task<bool> SaveAsync()
        {
            return Task.Run(async () =>
            {
                var stopwatch = Stopwatch.StartNew();
                var success = false;
                Exception writeFileException = null;

                do
                {
                    try
                    {
                        File.WriteAllText(FilePath, Content);
                        success = true;
                    }
                    catch (IOException ex)
                    {
                        if (stopwatch.ElapsedMilliseconds < Timeout)
                        {
                            await Task.Delay(100);
                        }

                        writeFileException = ex;
                    }
                } while (!success && stopwatch.ElapsedMilliseconds < Timeout);

                if (!success && ThrowIfFail)
                {
                    throw writeFileException;
                }

                return success;
            });
        }
    }
}