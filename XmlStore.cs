using System.Configuration;
using System.Reflection;

namespace GameBackup
{
	public class XmlStore
	{		
		
		public string SourcePath
		{
			get
			{				
				return ConfigurationManager.AppSettings.Get("SourcePath");				
			}
			set
			{
				string path = value;				
				string exePath = Assembly.GetExecutingAssembly().Location;
				Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
				config.AppSettings.Settings.Remove("SourcePath");
				config.AppSettings.Settings.Add("SourcePath", path);
				config.Save(ConfigurationSaveMode.Modified);
			}
		}

		public string DestinationPath
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("DestinationPath");				
			}

			set
			{
				string path = value;				
				string exePath = Assembly.GetExecutingAssembly().Location;
				Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
				config.AppSettings.Settings.Remove("DestinationPath");
				config.AppSettings.Settings.Add("DestinationPath", path);
				config.Save(ConfigurationSaveMode.Modified);
			}
		}
	}
}
