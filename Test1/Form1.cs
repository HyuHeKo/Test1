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

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeTrayIcon();
            //backgroundWorker.DoWork += backgroundWorker_DoWork;
        }

        //BackgroundWorker backgroundWorker = new BackgroundWorker();

        private bool ctrlPressed = false;
        private bool shiftPressed = false;

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.Test_icon1; // Замініть на власну піктограму
            notifyIcon.Text = "Test_1";
            notifyIcon.Visible = true;
            //notifyIcon.BalloonTipTitle = "Test_1";
            //notifyIcon.BalloonTipTitle = "Згорнуто";

            // Додайте можливість показу контекстного меню при правому кліку
            ContextMenu contextMenu = new ContextMenu();
            MenuItem openMenuItem = new MenuItem("Відкрити");
            openMenuItem.Click += OpenMenuItem_Click;
            contextMenu.MenuItems.Add(openMenuItem);

            MenuItem exitMenuItem = new MenuItem("Вийти");
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenu.MenuItems.Add(exitMenuItem);

            notifyIcon.ContextMenu = contextMenu;

            // Приховати форму після запуску
            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
        }

        public void Win_Show()
        {
            Show();
            ShowInTaskbar = true;
        }
        public void Win_Hide()
        {
            Hide();
            ShowInTaskbar = false;
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            // Відобразити форму при виборі "Відкрити" з контекстного меню
            Win_Show();
        }
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            // Закрити програму при виборі "Вийти" з контекстного меню
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
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Win_Show();
        }



        C_Process process_Rufus = new C_Process(@"\\manager\Downloads\Install\Rufus 2.11\rufus-2.11p.exe");
        C_Process process_Steam = new C_Process(@"C:\Program Files (x86)\Steam\steam.exe");

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            //backgroundWorker.RunWorkerAsync();

            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            process_Rufus.Run();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            process_Steam.Run();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrlPressed = true;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                shiftPressed = true;
            }


            else if (ctrlPressed && shiftPressed && e.KeyCode == Keys.R)
            {
                button1_Click(null, EventArgs.Empty);
            }
            else if (ctrlPressed && shiftPressed && e.KeyCode == Keys.S)
            {
                button2_Click(null, EventArgs.Empty);
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrlPressed = false;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                shiftPressed = false;
            }
        }



        //private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    while (true)
        //    {
        //        if (ctrlPressed && shiftPressed)
        //        {
        //            if (IsKeyCombinationPressed(Keys.R))
        //            {
        //                // Викликати метод для запуску процесу в фоновому режимі
        //                process_Rufus.Run();
        //            }
        //            else if (IsKeyCombinationPressed(Keys.S))
        //            {
        //                // Викликати метод для запуску процесу в фоновому режимі
        //                process_Steam.Run();
        //            }
        //        }

        //        // Затримка, щоб не завантажувати процесор швидким циклом перевірок
        //        Thread.Sleep(100);
        //    }
        //}


        //private bool IsKeyCombinationPressed(Keys key)
        //{
        //    return Keyboard.IsKeyDown(key); // Перевірка, чи клавіша натиснута
        //}

        //public static class Keyboard
        //{
        //    [DllImport("user32.dll")]
        //    private static extern short GetKeyState(int key);

        //    public static bool IsKeyDown(Keys key)
        //    {
        //        return (GetKeyState((int)key) & 0x8000) != 0; // Бітова маска для перевірки стану клавіші
        //    }
        //}



    }
}
