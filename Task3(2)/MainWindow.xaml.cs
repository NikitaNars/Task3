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

namespace Task3_2_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> list = new List<int>();
        object lockobj = new object();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartBtn(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(AddList);
            Thread thread2 = new Thread(Processing);
            thread.Start();
            thread2.Start();
        }
        private void AddList()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(500);
                    lock (lockobj)
                    {
                        list.Add(list.Count);
                        Monitor.Pulse(lockobj);
                        Dispatcher.Invoke(() =>
                        {
                            Info.Text = "Объект добавлен!";
                        });
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
        private void Processing()
        {
            while (true)
            {
                try
                {
                    lock (lockobj)
                    {

                        Monitor.Wait(lockobj, 1000);
                        list.Remove(list.Count - 1);
                        Dispatcher.Invoke(() =>
                        {
                            Info.Text = "Список изменен";
                        });

                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

            }
        }

    }
}