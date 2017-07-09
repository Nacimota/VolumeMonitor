using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace VolumeMonitor
{
    class VMonApplicationContext : ApplicationContext
    {
        public CoreAudioController Controller { get; private set; }
        NotifyIcon icon;

        public VMonApplicationContext()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            icon = new NotifyIcon();
            UpdateIconText("!", Color.Tomato, 10);
            icon.Visible = true;

            ContextMenu menu = new ContextMenu();

            MenuItem exit = new MenuItem("Exit");
            exit.Click += Exit_Click;

            menu.MenuItems.Add(exit);
            icon.ContextMenu = menu;

            Controller = new CoreAudioController();

            var volObserver = new VolumeObserver(this);
            volObserver.Subscribe(Controller.DefaultPlaybackDevice.VolumeChanged);
            var muteObserver = new MuteObserver(this);
            muteObserver.Subscribe(Controller.DefaultPlaybackDevice.MuteChanged);

            var devObserver = new DeviceObserver(volObserver, muteObserver, this);
            devObserver.Subscribe(Controller.AudioDeviceChanged);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("fatal.log", true))
            {
                if (e.IsTerminating)
                {
                    var ex = (Exception)e.ExceptionObject;
                    sw.WriteLine($"{DateTime.Now.ToString("yyyy/mm/dd hh:MM:ss")} VolumeMonitor {Assembly.GetExecutingAssembly().GetName().Version.ToString()} encountered an unhandled {ex.GetType().Name} and terminated:\r\n");
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace + Environment.NewLine);
                }
            }

            Exit(); // attempt clean exit
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        public void UpdateIconText(string text) => UpdateIconText(text, Color.White);
        public void UpdateIconText(string text, Color colour, int size = 7)
        {
            int yOffset = 2;
            if (size > 8)
            {
                yOffset = -2;
            }
            var iconBitmap = new Bitmap(16, 16);
            var canvas = Graphics.FromImage(iconBitmap);
            var format = new StringFormat() { Alignment = StringAlignment.Center };
            canvas.DrawString(text, new Font("Segoe UI", size, FontStyle.Bold), new SolidBrush(colour), new RectangleF(-4, yOffset, 20, 16), format);
            icon.Icon = Icon.FromHandle(iconBitmap.GetHicon());
        }

        public void Exit()
        {
            if (icon != null)
            {
                icon.Visible = false;
            }

            Application.Exit();
        }
    }
}
