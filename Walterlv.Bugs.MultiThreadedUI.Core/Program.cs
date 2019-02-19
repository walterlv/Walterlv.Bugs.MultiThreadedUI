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
            for (var i = 0; i < 50; i++)
            {
                RunSplashWindow(i);
            }

            var app = new App();
            app.InitializeComponent();
            app.Dispatcher.UnhandledException += OnUnhandledException;
            app.Run();
        }

        private static void RunSplashWindow(int index)
        {
            var thread = new Thread(() =>
            {
                //SynchronizationContext.SetSynchronizationContext(
                //    new DispatcherSynchronizationContext(
                //        Dispatcher.CurrentDispatcher));

                try
                {
                    var window = new SplashWindow
                    {
                        Title = $"SplashWindow {index.ToString().PadLeft(2, ' ')}",
                    };
                    window.Show();
                }
                catch (Exception ex)
                {
                    Log(ex);
                }

                Dispatcher.CurrentDispatcher.UnhandledException += OnUnhandledException;
                Dispatcher.Run();
            })
            {
                IsBackground = true,
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
            => Log(e.Exception);

        private static void Log(Exception exception) => Console.WriteLine(exception);
    }
}