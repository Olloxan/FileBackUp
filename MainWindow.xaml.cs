using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace GameBackup
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
		XmlStore _store;
		FileMover _fileMover;

		

		bool _running = false;

		public MainWindow()
        {
            InitializeComponent();
			_store = new XmlStore();
			_fileMover = new FileMover();
			_fileMover.OnMessageReceived += MessageReceived;
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
			if (!_running)
			{
				StartTimer();
				StartButtons();
				_running = true;
			}
			else
			{
				StopTimer();
				StopButtons();
				_running = false;
			}
			
		}

		private void StartTimer()
		{
			_fileMover.InitFilemover(txt_FilePathSource.Text, txt_FilePathDestination.Text, 1000);
			_fileMover.Start();
		}

		private void StopTimer()
		{
			_fileMover.Stop();
		}

		private void StartButtons()
		{
			Btn_Start.Content = "Running...";
			Btn_Start.Background = (Brush)new BrushConverter().ConvertFrom("#FFFF3F00");
			btn_OpenFileSource.IsEnabled = false;
			btn_OpenFileDestination.IsEnabled = false;
		}

		private void StopButtons()
		{
			
			Btn_Start.Content = "Start";
			Btn_Start.Background = (Brush)new BrushConverter().ConvertFrom("#FF11FF00");
			btn_OpenFileSource.IsEnabled = true;
			btn_OpenFileDestination.IsEnabled = true;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_fileMover.Stop();
		}

		private void MessageReceived(string message)
		{			
			txt_Info.Dispatcher.Invoke(new Action(()=> txt_Info.Text += message + "\n"));
            txt_Info.Dispatcher.Invoke(new Action(() => txt_Info.ScrollToEnd()));
        }
	}
}
