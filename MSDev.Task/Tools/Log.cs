using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MSDev.Task.Tools
{
	public static class Log
	{
		private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

		public static void Write(string row)
		{
			var file = new FileInfo("c9article.txt");
			using (StreamWriter stream = file.AppendText())
			{
				stream.WriteLine(row + "\n");
			}
		}


		public static void Write(string filePath, string row)
		{
			var file = new FileInfo(filePath);
			if (!file.Directory.Exists)
			{
				file.Directory.Create();
			}
			Lock.EnterWriteLock();
			using (StreamWriter stream = file.AppendText())
			{
				try
				{
					stream.WriteLine(row);
				}
				catch
				{
					Console.WriteLine(row);
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
		}
	}
}
