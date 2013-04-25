using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace tailTool
{
    /// <summary>
    /// tailRenderWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class tailRenderWindow : UserControl
    {
        string _Text = string.Empty;
        bool renderstop = false;
        StreamReader reader;
        FileSystemWatcher fw;
        FileInfo selectedFile;

        public tailRenderWindow()
        {
            InitializeComponent();
        }

        private void setFileWatcher(string filefullName)
        {
            this.label1.Content = filefullName;
            selectedFile = new FileInfo(filefullName);
            fw = new FileSystemWatcher(selectedFile.Directory.FullName, selectedFile.Name);
            fw.Changed += new FileSystemEventHandler(fw_Changed);
            fw.EnableRaisingEvents = true;
            reader = new StreamReader(
            new FileStream(selectedFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 8, true)
            , Encoding.GetEncoding(Convert.ToInt32(this.textBox2.Text)));
            reader.ReadToEnd();
        }

        delegate void formdelegate();
        int TextLength = 0;
        private void fw_Changed(object sender, FileSystemEventArgs e)
        {
            if (!renderstop)
            {
                try
                {
                    Dispatcher.Invoke((formdelegate)delegate()
                    {
                        this.textBox1.AppendText(reader.ReadToEnd());
                    });
                }
                catch (Exception ex) { }
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.renderstop)
            {
                this.scrollViewer1.ScrollToBottom();
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] fileName =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);
            setFileWatcher(fileName[0]);
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            this.renderstop = true;
        }

        private void chkbRender_Unchecked(object sender, RoutedEventArgs e)
        {
            this.renderstop = false;
        }

        private void label1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            op.Multiselect = false;
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.setFileWatcher(op.FileName);
            }
        }

        private void label_ClearText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.textBox1.Text = string.Empty;
        }

        private void label_StopRendering_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.renderstop = !this.renderstop;

            if (this.renderstop)
            {
                this.label_StopRendering.Content = "Stopped...";
            }
            else
            {
                this.label_StopRendering.Content = "Stop";
            }
        }

        private void label_StopRendering_MouseEnter(object sender, MouseEventArgs e)
        {
            this.label_StopRendering.Opacity = 1;
        }

        private void label_StopRendering_MouseLeave(object sender, MouseEventArgs e)
        {
            this.label_StopRendering.Opacity = 0.7;
        }

        private void label_ClearText_MouseEnter(object sender, MouseEventArgs e)
        {
            this.label_ClearText.Opacity = 1;
        }

        private void label_ClearText_MouseLeave(object sender, MouseEventArgs e)
        {
            this.label_ClearText.Opacity = 0.7;
        }
    }
}
