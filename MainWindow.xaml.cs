using System.Windows;
using System.Windows.Forms;

namespace GameBackup
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
		XmlStore _store;
		public MainWindow()
        {
            InitializeComponent();
			_store = new XmlStore();
			InitPathTestFields();
        }

		private void InitPathTestFields()
		{
			txt_FilePathSource.Text = _store.SourcePath;
			txt_FilePathDestination.Text = _store.DestinationPath;
		}

		private void Btn_OpenFileSource_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fbdOpenFolder = new FolderBrowserDialog();
			if (fbdOpenFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txt_FilePathSource.Text = fbdOpenFolder.SelectedPath;
				_store.SourcePath = fbdOpenFolder.SelectedPath;
			}
		}

		private void Btn_OpenFileDestination_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fbdOpenFolder = new FolderBrowserDialog();
			if (fbdOpenFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txt_FilePathDestination.Text = fbdOpenFolder.SelectedPath;
				_store.DestinationPath = fbdOpenFolder.SelectedPath;
			}
		}		

		private void Btn_Start_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
