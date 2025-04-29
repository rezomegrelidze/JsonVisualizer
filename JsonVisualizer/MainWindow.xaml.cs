using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JToken Obj;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private async Task<string> GetHugeJson()
        {
            var url = "https://raw.githubusercontent.com/json-iterator/test-data/master/large-file.json";

            HttpClient client = new HttpClient();
            return await client.GetStringAsync(url);
        }


        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void PopulateTree(JToken data, TreeViewItem treeViewItem)
        {

            if (data is JObject json)
            {
                foreach (var property in json.Properties())
                {
                    var val = property.Value;

                    if (val is JArray array)
                    {
                        var arrayTree = new TreeViewItem()
                        {
                            Header = $"[] {property.Name} ({array.Count})",
                        };
                        treeViewItem.Items.Add(arrayTree);
                        foreach (JObject obj in array)
                        {
                            PopulateTree(obj, arrayTree);
                        }
                    }
                    else
                    {
                        if (val is JObject)
                        {
                            var item = new TreeViewItem()
                            {
                                Header = property.Name,
                            };
                            PopulateTree(val as JObject, item);
                            treeViewItem.Items.Add(item);
                        }
                        else
                        {

                            var terminalTreeViewItem = new TreeViewItem()
                            {
                                Header = $"{{}} {property.Name}"
                            };
                            terminalTreeViewItem.Items.Add($"'{property.Name}' : '{val}'");
                            treeViewItem.Items.Add(terminalTreeViewItem);
                        }
                    }
                }
            }
            else if(data is JArray array)
            {
                var arrayTree = new TreeViewItem()
                {
                    Header = $"[] ({array.Count})",
                };
                treeViewItem.Items.Add(arrayTree);
                foreach (JToken obj in array)
                {
                    PopulateTree(obj, arrayTree);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var json = JsonInput.Text;
            Obj = JToken.Parse(json);


            Stopwatch stopwatch = Stopwatch.StartNew();

            RootTree.Items.Clear();

            if (Obj is JArray)
            {
                int index = 0;
                foreach (var item in Obj)
                {
                    var mainMenuItem = new TreeViewItem() { Header = $"[{index++}]" };
                    PopulateTree(item, mainMenuItem);
                    RootTree.Items.Add(mainMenuItem);
                }
            }
            else
            {
                var mainMenuItem = new TreeViewItem() { Header = "{}" };
                PopulateTree(Obj, mainMenuItem);
                RootTree.Items.Add(mainMenuItem);
            }


            MessageBox.Show(stopwatch.Elapsed.ToString());
        }
    }
}
