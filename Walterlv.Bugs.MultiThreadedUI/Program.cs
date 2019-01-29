using System;
using System.Threading;
using System.Windows.Threading;

namespace Walterlv.Bugs.MultiThreadedUI
{
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            RunSplashWindow();
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static void RunSplashWindow()
        {
            var thread = new Thread(() =>
            {
                SynchronizationContext.SetSynchronizationContext(
                    new DispatcherSynchronizationContext(
                        Dispatcher.CurrentDispatcher));

                var window = new SplashWindow();
                window.Show();

                Dispatcher.Run();
            })
            {
                IsBackground = true,
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}