using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GameBackup
{
	class FileMover
	{
		private string _sourcePath;
		private string _destinationPath;
		private Timer _FileCopyTimer;
		private int _SaveInterval;
		private bool _copyLock;

		private int _SaveFileCounter;

		private int _copyCounter_File;		
		private int _deleteCounter_File;
		private int _deleteCounter_Folder;

		public delegate void MessageEventHandler(string message);
		public MessageEventHandler OnMessageReceived;

		public void InitFilemover(string sourcePath, string destinationPath, int saveInterval)
		{
			ThrowIfNotExisting(sourcePath);
			_sourcePath = sourcePath;
			_destinationPath = destinationPath;
			_SaveInterval = saveInterval;
			_SaveFileCounter = 0;
		}
		
		public void Start()
		{
			_FileCopyTimer = new Timer(CopyFiles, new object(), 500, _SaveInterval);			
		}

		private void CopyFiles(object obj)
		{
			string dstPath = _destinationPath + "_" + _SaveFileCounter;			
			_copyCounter_File = 0;			
			_deleteCounter_File = 0;
			_deleteCounter_Folder = 0;
			_copyLock = true;			
			CopyRecursively(_sourcePath, dstPath);
			_copyLock = false;
			ClearFilesInDestiantionDirectory(_sourcePath, dstPath);
			
			OnMessageReceived(createMessage());
			_SaveFileCounter++;
			_SaveFileCounter %= 2;
		}

		string createMessage()
		{
			string time = DateTime.Now.ToString("H:mm:ss");
			return string.Format("{0}: {1} Files or Folders Copied\n{2} Folders and {3} Files deleted in Folder {4}",
				time, 
				_copyCounter_File,
				_deleteCounter_Folder,
				_deleteCounter_File,
				_SaveFileCounter);
		}

		private void ClearFilesInDestiantionDirectory(string sourcepath, string destinationpath)
		{
			DirectoryInfo sourceDir = new DirectoryInfo(sourcepath);
			if (!Directory.Exists(destinationpath))
			{
				return;
			}
			DirectoryInfo destinationDir = new DirectoryInfo(destinationpath);
			

			DirectoryInfo[] sourceSubdirectories = sourceDir.GetDirectories();
			DirectoryInfo[] destinationSubdirectories = destinationDir.GetDirectories();

			FileInfo[] sourceFiles = sourceDir.GetFiles();
			FileInfo[] destinationFiles = destinationDir.GetFiles();

			foreach (FileInfo dstfile in destinationFiles)
			{
				if (!sourceFiles.Select(file => file.Name).Contains(dstfile.Name))
				{					
					File.Delete(dstfile.FullName);
					_deleteCounter_File++;
				}				
			}

			foreach (DirectoryInfo dstDirectory in destinationSubdirectories)
			{
				if (!sourceSubdirectories.Select(dir=>dir.Name).Contains(dstDirectory.Name))
				{
					Directory.Delete(dstDirectory.FullName, true);
					_deleteCounter_Folder++;
				}
			}

			for (int i = 0; i < sourceSubdirectories.Length;i++)
			{
				ClearFilesInDestiantionDirectory(sourceSubdirectories[i].FullName, destinationSubdirectories[i].FullName);
			}

		}

		private void CopyRecursively(string sourcepath, string destinationpath)
		{
			
			DirectoryInfo info = new DirectoryInfo(sourcepath);
			DirectoryInfo[] subdirectories = info.GetDirectories();

			Directory.CreateDirectory(destinationpath);			
			FileInfo[] files = info.GetFiles();
			foreach(FileInfo file in files)
			{
				string tempFilePath = Path.Combine(destinationpath, file.Name);
				File.SetAttributes(file.FullName, FileAttributes.Normal);
				file.CopyTo(tempFilePath, true);
				_copyCounter_File++;
			}

			foreach (DirectoryInfo subDir in subdirectories)
			{
				string tempPath = Path.Combine(destinationpath, subDir.Name);
				CopyRecursively(subDir.FullName, tempPath);
			}
		}

		public void Stop()
		{
            if (_FileCopyTimer != null)
            {
                while (_copyLock)
                {
                    Thread.Sleep(1);
                }
                _FileCopyTimer.Dispose();
            }
		}

		private static void ThrowIfNotExisting(string sourcePath)
		{
			if (DoesNotExist(sourcePath))
			{
				throw new ArgumentException("Source path: {0} does not exist.", sourcePath);
			}			
		}

		private static bool DoesNotExist(string destinationPath)
		{
			return !Directory.Exists(destinationPath);
		}

	}
}
