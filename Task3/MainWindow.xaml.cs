using System.Diagnostics;
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

namespace Task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int sharedCounter = 0;
        int IT = 10000;
        object locker = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartDataRace(object sender, RoutedEventArgs e)
        {
            Thread thread1 = new Thread(IncrementCounter);
            Thread thread2 = new Thread(IncrementCounter);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Text.Text = sharedCounter.ToString();
            sharedCounter = 0;

        }
        private void StartDataRaceLock(object sender, RoutedEventArgs e)
        {
            Thread thread1 = new Thread(SyncIncrementCounter);
            Thread thread2 = new Thread(SyncIncrementCounter);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Text.Text = sharedCounter.ToString();
            sharedCounter = 0;
        }
        private void StartMonitorWithTimeout(object sender, RoutedEventArgs e)
        {
            sharedCounter = 0;
            Thread thread1 = new Thread(MonitorIncrementCounter);
            Thread thread2 = new Thread(MonitorIncrementCounter);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Text.Text = sharedCounter.ToString();
        }
        void MonitorIncrementCounter()
        {
            for (int i = 0; i < IT; i++)
            {
                bool lockTaken = false;
                try
                {
                    
                    Monitor.TryEnter(locker, 100, ref lockTaken);

                    if (lockTaken)
                    {
                        sharedCounter++;
                    }
                    else
                    {
                        
                        Debug.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} не получил блокировку вовремя");
                        
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(locker);
                        
                    }
                }
            }
        }
        void IncrementCounter()
        {
            for (int i = 0; i < IT; i++)
            {
                
                sharedCounter++;
            }
        }
        void SyncIncrementCounter()
        {
            lock (locker)
            {
                for (int i = 0; i < IT; i++)
                {

                    sharedCounter++;
                }
            }

            
        }

        
    }

}