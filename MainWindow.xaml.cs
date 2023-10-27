using Microsoft.Win32;

using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] _inF;
        private string[] _outF;
        private bool _isOut;
        private bool _isIn;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Read lines from source file.
            _inF = File.ReadAllLines(textBox.Text);
            _outF = File.ReadAllLines(textBox_2.Text);
            File.WriteAllLines(textBox_2.Text + ".back", _outF); // let's make a copy

            const string pattern = @"desc=\""\w+\""\>";
            var sum = 0;
            foreach (var t in _inF)
            {
                var regexIn = new Regex(pattern, RegexOptions.IgnoreCase);
                var match = regexIn.Match(t);

                if (match.Value == "")
                {
                    continue;
                }

                var regexOut = new Regex(match.Value, RegexOptions.IgnoreCase);
                for (var j = 0; j < _outF.Length; j++)
                {
                    if (!regexOut.IsMatch(_outF[j]))
                    {
                        continue;
                    }

                    _outF[j] = t + " <!-- added -->";
                    sum++;
                    if (sum < 2)
                    {
                        continue;
                    }

                    sum = 0;
                    break;
                }
            }
            File.WriteAllLines(textBox_2.Text, _outF); // save the results
            button.IsEnabled = true;
            MessageBox.Show("Opcodes have been replaced!");
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {

            if (OpenFileDialog1())
            {
                textBox.Text = FilePath;
                _isIn = true;
            }
            else
            {
                _isIn = false;
                ShowMessage("For the program to work, you must select a file!");
            }
            if (_isIn && _isOut)
            {
                button.IsEnabled = true;
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFileDialog2())
            {
                textBox_2.Text = FilePath;
                _isOut = true;
            }
            else
            {
                _isOut = false;
                ShowMessage("For the program to work, you must select a file!");
            }
            if (_isIn && _isOut)
            {
                button.IsEnabled = true;
            }
        }
        public string FilePath { get; set; }

        public bool OpenFileDialog1()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File|*.txt";
            openFileDialog.FileName = "New Text Doucment";
            openFileDialog.Title = "Open As Text File";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }
        public bool OpenFileDialog2()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File|*.xml";
            openFileDialog.FileName = "New Text Doucment";
            openFileDialog.Title = "Open As Text File";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
