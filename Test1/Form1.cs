using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using Microsoft.Win32;
using System.Globalization;
using WindowsInput;

namespace Test1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        //InputLanguage currentInputLanguage = InputLanguage.CurrentInputLanguage;
        InputSimulator simulator = new InputSimulator();

        C_Process process_Rufus = new C_Process(@"\\manager\Downloads\Install\Rufus 2.11\rufus-2.11p.exe");
        C_Process process_Steam = new C_Process(@"C:\Program Files (x86)\Steam\steam.exe");
        C_Process process_Chrome = new C_Process(@"C:\Program Files\Google\Chrome\Application\chrome.exe");
        C_Process process_win_r = new C_Process(@"C:\Users\river\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\System Tools\Run...");
        C_Process process_visual = new C_Process(@"E:\VS\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe");

        public const int MOD_ALT = 0x1;
        public const int MOD_CONTROL = 0x2;
        public const int MOD_SHIFT = 0x4;
        public const int MOD_WIN = 0x8;
        public const int WM_HOTKEY = 0x312;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                textBox1.Text += DateTime.Now.ToString() + ". Hotkey pressed, ID = 0x" + m.WParam.ToString("X");
                textBox1.Text += Environment.NewLine;


                if (m.WParam.ToString("X") == "1")
                    process_Rufus.Run();
                else if (m.WParam.ToString("X") == "2")
                {
                    process_Steam.Run();
                    Cursor.Position = new Point(870, 550);
                    //Thread.Sleep(5000);
                    //simulator.Mouse.LeftButtonClick();
                }
                else if (m.WParam.ToString("X") == "3")
                    process_Chrome.Run();
                else if (m.WParam.ToString("X") == "4")
                {
                    process_win_r.Run();
                    Thread.Sleep(100);
                    SendKeys.SendWait("\\\\manager\\Downloads{ENTER}");
                }
                else if (m.WParam.ToString("X") == "5")
                    process_visual.Run();
                else if (m.WParam.ToString("X") == "6")
                {

                }

                m.Result = (IntPtr)0;
                return;
            }

            base.WndProc(ref m);
        }

        public Form1()
        {
            InitializeComponent();
            InitializeTrayIcon();

            bool res;

            res = RegisterHotKey(this.Handle, 1, MOD_ALT, (uint)Keys.R);
            if (res == false) MessageBox.Show("RegisterHotKey failed");
            res = RegisterHotKey(this.Handle, 2, MOD_ALT, (uint)Keys.S);//регистрируем горячую клавишу
            if (res == false) MessageBox.Show("RegisterHotKey failed");
            res = RegisterHotKey(this.Handle, 3, MOD_ALT, (uint)Keys.C);
            if (res == false) MessageBox.Show("RegisterHotKey failed");
            res = RegisterHotKey(this.Handle, 4, MOD_WIN, (uint)Keys.F2);
            if (res == false) MessageBox.Show("RegisterHotKey failed");
            res = RegisterHotKey(this.Handle, 5, MOD_ALT, (uint)Keys.V);
            if (res == false) MessageBox.Show("RegisterHotKey failed");
            res = RegisterHotKey(this.Handle, 6, MOD_ALT, (uint)Keys.Space);
            if (res == false) MessageBox.Show("RegisterHotKey failed");

        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.Test_icon1; // Замініть на власну піктограму
            notifyIcon.Text = "FastTask";
            notifyIcon.Visible = true;

            // Додайте можливість показу контекстного меню при правому кліку
            ContextMenu contextMenu = new ContextMenu();
            MenuItem openMenuItem = new MenuItem("Відкрити");
            openMenuItem.Click += OpenMenuItem_Click;
            contextMenu.MenuItems.Add(openMenuItem);

            MenuItem exitMenuItem = new MenuItem("Вийти");
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenu.MenuItems.Add(exitMenuItem);

            notifyIcon.ContextMenu = contextMenu;
        }

        public void Win_Show()
        {
            //Show();
            WindowState = FormWindowState.Normal;
        }
        public void Win_Hide()
        {
            //Hide();
            WindowState = FormWindowState.Minimized;
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            Win_Show();
        }
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Перехопити закриття форми і приховати її в трей замість закриття
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Win_Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Win_Hide();
            this.KeyPreview = true;
            SetAutoRunValue(true, Assembly.GetExecutingAssembly().Location);
            //ShowInTaskbar = false;!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        private bool SetAutoRunValue(bool autorun, string path)
        {
            const string name = "FastTask";

            string ExePath = path;

            RegistryKey reg;

            reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\");

            try
            {
                if (autorun)
                {
                    reg.SetValue(name, ExePath);
                }
                else { reg.DeleteValue(name); }

                reg.Flush();

                reg.Close();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
