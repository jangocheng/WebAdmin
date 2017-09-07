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


		public static void Write(string filePath, string row,bool append=true)
		{
			var file = new FileInfo(filePath);
			if (!file.Directory.Exists)
			{
				file.Directory.Create();
			}
			Lock.EnterWriteLock();
            if (!append)
            {
                using (StreamWriter stream = file.CreateText())
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
            else
            {
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
}
