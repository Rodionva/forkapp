using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public static string pathOfProject;
        public static string savedVer;

        public Form1(string[] args)
        {
            InitializeComponent();
            codeBox.TextChanged += CodeBox_TextChanged;
            codeBox.Text += "функция установки()\n{\n\t\n}\n\n";
            CodeBox_TextChanged(this, null);
            codeBox.Text += "функция повторять()\n{\n\t\n}\n";
            CodeBox_TextChanged(this, null);
            KeyPreview = true;

            if (args.Length != 0)
            {
                pathOfProject = args[0];
                codeBox.Text = File.ReadAllText(pathOfProject);
                savedVer = codeBox.Text;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(HandleRef hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        static void EnableRepaint(HandleRef handle, bool enable)
        {
            const int WM_SETREDRAW = 0x000B;
            SendMessage(handle, WM_SETREDRAW, new IntPtr(enable ? 1 : 0), IntPtr.Zero);
        }

        private void CodeBox_TextChanged(object sender, EventArgs e)
        {
            HandleRef gh = new HandleRef(this.codeBox, this.codeBox.Handle);
            EnableRepaint(gh, false);

            try
            {

                var currentSelStart = codeBox.SelectionStart;
                var currentSelLength = codeBox.SelectionLength;
                Font current = codeBox.Font;

                codeBox.SelectAll();
                codeBox.SelectionColor = Color.White;

                foreach (var first in File.ReadAllLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\first.data"))
                {
                    var matches = Regex.Matches(codeBox.Text, first);
                    foreach (var match in matches.Cast<Match>())
                    {
                        codeBox.Select(match.Index, match.Length);
                        codeBox.SelectionColor = Color.FromArgb(86, 156, 214);
                    }
                }

                foreach (var library in Directory.GetDirectories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries"))
                {
                    foreach (var first in File.ReadAllLines(library + @"\first.data"))
                    {
                        var matches = Regex.Matches(codeBox.Text, first);
                        foreach (var match in matches.Cast<Match>())
                        {
                            codeBox.Select(match.Index, match.Length);
                            codeBox.SelectionColor = Color.FromArgb(86, 156, 214);
                        }
                    }
                }

                var matchesText = Regex.Matches(codeBox.Text, "\"(.*?)\"");
                foreach (var match in matchesText.Cast<Match>())
                {
                    codeBox.Select(match.Index, match.Length);
                    codeBox.SelectionColor = Color.FromArgb(214, 157, 133);
                }

                var matchesChar = Regex.Matches(codeBox.Text, "\'(.*?)\'");
                foreach (var match in matchesChar.Cast<Match>())
                {
                    codeBox.Select(match.Index, match.Length);
                    codeBox.SelectionColor = Color.FromArgb(214, 157, 133);
                }

                var matchesNum = Regex.Matches(codeBox.Text, @"([0-9]+)");
                foreach (var match in matchesNum.Cast<Match>())
                {
                    codeBox.Select(match.Index, match.Length);
                    codeBox.SelectionColor = Color.FromArgb(181, 206, 168);
                }

                foreach (var operators in File.ReadAllLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\operators.data"))
                {
                    var matchesOperator = Regex.Matches(codeBox.Text, operators);
                    foreach (var match in matchesOperator.Cast<Match>())
                    {
                        codeBox.Select(match.Index, match.Length);
                        codeBox.SelectionColor = Color.FromArgb(78, 201, 176);
                    }
                }

                foreach (var library in Directory.GetDirectories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries"))
                {
                    foreach (var operators in File.ReadAllLines(library + @"\operators.data"))
                    {
                        var matchesOperator = Regex.Matches(codeBox.Text, operators);
                        foreach (var match in matchesOperator.Cast<Match>())
                        {
                            codeBox.Select(match.Index, match.Length);
                            codeBox.SelectionColor = Color.FromArgb(78, 201, 176);
                        }
                    }
                }

                foreach (var func in File.ReadAllLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\func.data"))
                {
                    var matchesFunc = Regex.Matches(codeBox.Text, func);
                    foreach (var match in matchesFunc.Cast<Match>())
                    {
                        FontStyle newFontStyle;
                        newFontStyle = FontStyle.Bold;
                        codeBox.Select(match.Index, match.Length);
                        codeBox.SelectionFont = new Font(current.FontFamily, current.Size, newFontStyle);
                    }
                }

                foreach (var library in Directory.GetDirectories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries"))
                {
                    foreach (var func in File.ReadAllLines(library + @"\func.data"))
                    {
                        var matchesFunc = Regex.Matches(codeBox.Text, func);
                        foreach (var match in matchesFunc.Cast<Match>())
                        {
                            FontStyle newFontStyle;
                            newFontStyle = FontStyle.Bold;
                            codeBox.Select(match.Index, match.Length);
                            codeBox.SelectionFont = new Font(current.FontFamily, current.Size, newFontStyle);
                        }
                    }
                }

                var matchesComment = Regex.Matches(codeBox.Text, @"//.*");
                foreach (var match in matchesComment.Cast<Match>())
                {
                    codeBox.Select(match.Index, match.Length);
                    codeBox.SelectionColor = Color.FromArgb(87, 166, 74);
                }

                codeBox.Select(currentSelStart, currentSelLength);
                codeBox.SelectionColor = Color.White;
                codeBox.SelectionFont = new Font(current.FontFamily, current.Size, FontStyle.Regular);

            }
            finally
            {
                EnableRepaint(gh, true);
                this.codeBox.Invalidate();
            }
        }

        private static string translite(string str)
        {
            string[] lat_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "", "E", "Yu", "Ya" };
            string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "", "e", "yu", "ya" };
            string[] rus_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            for (int i = 0; i <= 32; i++)
            {
                str = str.Replace(rus_up[i], lat_up[i]);
                str = str.Replace(rus_low[i], lat_low[i]);
            }
            return str;
        }


        private void сохранитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string text = codeBox.Text;
            string result;
            foreach (var library in Directory.GetDirectories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries"))
            {
                try
                {
                    foreach (var line in File.ReadAllLines(library + @"\define.data"))
                    {
                        result = Regex.Replace(text, @"\b" + line.Split(' ')[0] + @"\b", line.Split(' ')[1]);
                        text = result;
                    }
                }
                catch (Exception)
                {
                }
            }
            foreach (var line in File.ReadAllLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\define.data"))
            {
                result = Regex.Replace(text, @"\b" + line.Split(' ')[0] + @"\b", line.Split(' ')[1]);
                text = result;
            }

            try
            {
                Directory.Delete(pathOfProject.Remove(pathOfProject.LastIndexOf('\\') + 1) + @"\sketch");
            }
            catch (Exception)
            {
            }

            string code = translite(text);

            Task.Delay(2000);

            var matchesComment = Regex.Matches(code, @"//.*");
            var matchesCommentOrig = Regex.Matches(text, @"//.*");
            int i = 0;
            foreach (var match in matchesComment.Cast<Match>())
            {
                code = code.Replace(match.Value, matchesCommentOrig.Cast<Match>().ToArray()[i].Value);
                i++;
            }

            var matchesText = Regex.Matches(code, "\"(.*?)\"");
            var matchesTextOrig = Regex.Matches(text, "\"(.*?)\"");
            int a = 0;
            foreach (var match in matchesText.Cast<Match>())
            {
                code = code.Replace(match.Value, matchesTextOrig.Cast<Match>().ToArray()[a].Value);
                a++;
            }

            var matchesText1 = Regex.Matches(code, "(\"(.*?)\")");
            var matchesText1Orig = Regex.Matches(text, "(\"(.*?)\")");
            int b = 0;
            foreach (var match in matchesText1.Cast<Match>())
            {
                code = code.Replace(match.Value, matchesText1Orig.Cast<Match>().ToArray()[b].Value);
                b++;
            }



            var matchesChar = Regex.Matches(code, "\'(.*?)\'");
            var matchesCharOrig = Regex.Matches(text, "\'(.*?)\'");
            int c = 0;
            foreach (var match in matchesChar.Cast<Match>())
            {
                code = code.Replace(match.Value, matchesCharOrig.Cast<Match>().ToArray()[c].Value);
                c++;
            }

            Directory.CreateDirectory(pathOfProject.Remove(pathOfProject.LastIndexOf('\\') + 1) + @"\sketch");
            File.WriteAllText(pathOfProject.Remove(pathOfProject.LastIndexOf('\\') + 1) + @"\sketch\sketch.ino", code);
            string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            Process.Start(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\arduino\arduino.exe", pathOfProject.Remove(pathOfProject.LastIndexOf('\\') + 1) + @"sketch\sketch.ino");
        }

        private void сохранитьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pathOfProject == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Форк-проект|*.fork";
                sfd.DefaultExt = ".fork";
                sfd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\projects";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName.Split('.')[0] + @"\" + sfd.FileName.Split('\\').Last();
                    Directory.CreateDirectory(sfd.FileName.Split('.')[0]);
                    File.WriteAllText(path, codeBox.Text);
                    pathOfProject = path;
                    savedVer = codeBox.Text;
                }
            }
            else
            {
                File.WriteAllText(pathOfProject, codeBox.Text);
                savedVer = codeBox.Text;
            }
        }

        private void установитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Fork library(*.fol)|*.fol";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] directories = Directory.GetDirectories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries");
                ZipFile.ExtractToDirectory(dialog.FileName, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries");
                string[] directoriesafter = Directory.GetDirectories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries");
                var result = new HashSet<string>(directoriesafter);
                result.SymmetricExceptWith(directories);
                foreach (var item in result)
                {
                    Directory.Move(item, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\libraries\" + Path.GetFileName(dialog.FileName).Split('.')[0]);
                }

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (savedVer != codeBox.Text)
            {
                if (MessageBox.Show("Проект содержит несохраненные изменения. Вы уверены что хотите закрыть Fork?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                MatchCollection matchesChar = Regex.Matches(codeBox.Text, "{");
                MatchCollection matchesChar1 = Regex.Matches(codeBox.Text, "}");
                int i = 0;
                int b = 0;
                foreach (Match item in matchesChar)
                {
                    if (codeBox.SelectionStart > item.Index)
                    {
                        try
                        {
                            if (codeBox.SelectionStart < matchesChar1.Cast<Match>().ToArray()[b].Index)
                            {
                                i++;
                            }
                        }
                        catch (Exception)
                        {
                        } 
                    }
                    b++;
                }
                string pressed = "\n";
                while (i > 0)
                {
                    pressed += "\t";
                    i--;
                }
                codeBox.SelectedText = pressed;
                e.Handled = true;
            }
        }
    }
}
