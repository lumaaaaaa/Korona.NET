using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;

namespace Korona.NET
{
    public class Statistics // This is a class in which the data from the deserialized JSON is stored
    {
        public int confirmed { get; set; }
        public int deaths { get; set; }
        public int recovered { get; set; }
    }
    public partial class MainWindow : Window
    {
        private static string json;
        Statistics statistics;
        public MainWindow()
        {
            InitializeComponent();
            Update();
            stats.Text = "There are currently " + statistics.confirmed + " cases, " + statistics.deaths + " deaths, and " + statistics.recovered + " recovered as of " + DateTime.Now.ToString("h:mm tt") + ".";
        }
        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            refresh.Content = "Refreshing...";
            stats.Text = "Fetching new data...";
            DoEvents();
            Thread.Sleep(1000);
            Update();
            refresh.Content = "Refresh";
            stats.Text = "There are currently " + statistics.confirmed + " cases, " + statistics.deaths + " deaths, and " + statistics.recovered + " recovered as of " + DateTime.Now.ToString("h:mm tt") + ".";
            DoEvents();
        }
        public void Update()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://wuhan-coronavirus-api.laeyoung.endpoint.ainize.ai/jhu-edu/brief");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            statistics = JsonConvert.DeserializeObject<Statistics>(json);
        }
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
    }
}
