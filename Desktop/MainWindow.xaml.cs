using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;
using URLParser;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ParserViewModel(new ParserService());
        }

        private void startButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckInputData()) return;

            parsedLinks.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ParsedLinks.Result"));
            //startButton.Content = "LOADING..";
            //startButton.IsEnabled = false;
        }

        private bool CheckInputData()
        {
            var textBoxColor = ColorConverter.ConvertFromString("#FFABADB3") ?? Brushes.AliceBlue;

            if (!urlTextBox.Text.CheckUrl())
            {
                urlTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Url is not correct", "Validation failed!");
                return false;
            }
            urlTextBox.BorderBrush = new SolidColorBrush((Color)textBoxColor);

            if (!depthTextBox.Text.CheckDepth())
            {
                depthTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Depth value is not correct or not in range of (0;50)", "Validation failed!");
                return false;
            }
            depthTextBox.BorderBrush = new SolidColorBrush((Color)textBoxColor);

            if (!sourceTextBox.Text.CheckSource())
            {
                sourceTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Source path is not correct or directory not exists", "Validation failed!");
                return false;
            }
            sourceTextBox.BorderBrush = new SolidColorBrush((Color)textBoxColor);

            return true;
        }

        private void browseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Folder dialog",
                IsFolderPicker = true,
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
            sourceTextBox.Text = dialog.FileName;
        }
    }
}
