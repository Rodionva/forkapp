using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Форк-проект| *.fork| Скетч Arduino| *.ino";
            ofd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\projects";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Split('.')[1] == "ino")
                {
                    string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    Process.Start(folderpath + @"\Arduino\arduino.exe", ofd.FileName);
                    this.Close();
                }
                else if (ofd.FileName.Split('.')[1] == "fork")
                {
                    this.Hide();
                    Form1 form = new Form1(new string[] { ofd.FileName });
                    form.Closed += (s, args) => this.Close();
                    form.Show();
                }
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
        }

        //private void Form2_Shown(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Properties.Settings.Default.key))
        //    {
        //        Form4 form4 = new Form4();
        //        form4.ShowDialog();
        //        MessageBox.Show(Properties.Settings.Default.key);
        //        if (string.IsNullOrEmpty(Properties.Settings.Default.key))
        //        {
        //            this.Close();
        //        }
        //    }
        //    else
        //    {
        //        foreach (var conf in File.ReadAllLines("https://rodionva.github.io/fork/CWEXFYH2J3.data"))
        //        {
        //            if (Properties.Settings.Default.key == conf)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
