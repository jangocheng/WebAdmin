using System;
using System.IO;
using System.Threading;

namespace MSDev.Work.Tools
{
    public static class Log
    {
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        public static void Write(string row)
        {
            var file = new FileInfo("logs.txt");
            using (StreamWriter stream = file.AppendText())
            {
                stream.WriteLine(row + "\n");
            }
        }


        public static void Write(string filePath, string row, bool append = true)
        {
            var file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            Lock.EnterWriteLock();
            if (!append)
            {
                try
                {
                    File.WriteAllLines(filePath, new string[] { row });
                }
                catch (Exception)
                {
                    Console.WriteLine("write to file failed");
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
            else
            {
                try
                {
                    File.AppendAllLines(filePath, new string[] { row });
                }
                catch (Exception)
                {
                    Console.WriteLine("append to file failed");
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }
    }
}
