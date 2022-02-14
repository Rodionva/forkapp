using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form3 : Form
    {
        public static string path;
        public Form3()
        {
            InitializeComponent();
            path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\projects\";
            textBox2.Text = path;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox2.Text = fbd.SelectedPath + @"\";
                }
                else
                {
                    textBox2.Text = fbd.SelectedPath + @"\" + textBox1.Text;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Directory.CreateDirectory(textBox2.Text);
                File.WriteAllText(textBox2.Text + @"\" + textBox1.Text + ".fork", "функция установки()\n{\n\t\n}\n\nфункция повторять()\n{\n\t\n}\n");
                Program.form2.Hide();
                Form1 form = new Form1(new string[] { textBox2.Text + @"\" + textBox1.Text + ".fork" });
                form.Closed += (s, args) => Program.form2.Close();
                form.Show();
                this.Close();
            }
            else
            {
                Directory.CreateDirectory(textBox2.Text);
                File.WriteAllText(textBox2.Text + @"\" + textBox1.Text + ".ino", "void setup()\n{\n\t\n}\n\nvoid loop()\n{\n\t\n}\n");
                string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                Process.Start(folderpath + @"\Arduino\arduino.exe", textBox2.Text + @"\" + textBox1.Text + ".ino");
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox2.Text = path + textBox1.Text;
            }
        }
    }
}
