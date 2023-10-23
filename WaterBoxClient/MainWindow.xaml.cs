using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace WaterBoxClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>    

    public record WaterBoxResponse(string v1, string b1);

    public partial class MainWindow : Window
    {

        private readonly HttpClient client = new();

        public MainWindow()
        {
            InitializeComponent();

            List<string> cbxS1List = new() { "Com água", "Sem água" };
            cbxS1.ItemsSource = cbxS1List;            
            cbxS2.ItemsSource = cbxS1List;
            cbxS3.ItemsSource = cbxS1List;
            cbxS4.ItemsSource = cbxS1List;
            client.BaseAddress = new Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Loaded += MainWindow_Loaded;
        }

        async private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var objectRequest = new { s1 = 1, s2 = 1, s3 = 1, s4 = 1 };
            HttpResponseMessage response = await client.PostAsJsonAsync("/water-box", objectRequest);                
            
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
            
            var data = await response.Content.ReadFromJsonAsync<WaterBoxResponse>();                
            listView.ItemsSource = new List<string>() { $"V1: {data.v1}", $"B1: {data.b1}" };                       
        }

        async private void btnSend_Click (object sender, RoutedEventArgs e)
        {            
            var s1 = GetSelectedValue(cbxS1.SelectedItem.ToString());
            var s2 = GetSelectedValue(cbxS2.SelectedItem.ToString());
            var s3 = GetSelectedValue(cbxS3.SelectedItem.ToString());
            var s4 = GetSelectedValue(cbxS4.SelectedItem.ToString());

            var objectRequest = new { s1, s2, s3, s4 };
                
            HttpResponseMessage response = await client.PostAsJsonAsync("/water-box", objectRequest);
                
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
                return;
            }
            
            var data = await response.Content.ReadFromJsonAsync<WaterBoxResponse>();
            listView.ItemsSource = new List<string>() { $"V1: {data.v1}", $"B1: {data.b1}" };
        }

        private static int GetSelectedValue(string value)
        {
            if (value.ToLower() == "com água")
                return 1;
            else
                return 0;
        }
    }
}
