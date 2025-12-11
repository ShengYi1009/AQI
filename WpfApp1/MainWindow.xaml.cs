using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String aqiURL = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=4c89a32a-a214-461b-bf29-30ff32a61a8a&limit=1000&sort=ImportDate%20desc&format=JSON";

        AQIData aqiData = new AQIData();
        List<Field> fields = new List<Field>();
        List<Record> records = new List<Record>();

        public MainWindow()
        {
            InitializeComponent();
            URLTextBox.Text = aqiURL;
        }

        private async void GetAqiButton_Click(object sender, RoutedEventArgs e)
        {
            string url = URLTextBox.Text;
            ContentTextBox.Text = "抓取資料中......";

            String data = await GetAQIAync(url);
            if (data != null)
            {
                ContentTextBox.Text = data;
                aqiData = JsonSerializer.Deserialize<AQIData>(data);
                fields = aqiData.fields;
                records = aqiData.records;
                StatusTextBlock.Text = $"取得資料成功，共有 {records.Count} 筆記錄。";
            }

        }

        private async Task<string> GetAQIAync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    StatusTextBlock.Text = "無法取得資料，HTTP Status: " + response.StatusCode;
                    return null;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
        }
    }
}