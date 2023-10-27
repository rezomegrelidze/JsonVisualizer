﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private JObject Obj;

        public MainWindow()
        {
            InitializeComponent();

            string json = @"{
                'people' : [
                    { 'name' : {'rezo' : [{'s' : 1},{'z' : 2}] } },
                    { 'name' : 'rezo2' },
                    { 'name' : 'rez03' },
                    {
                    'ages' : [
                        { 'val' : 1 },
                        { 'val' : 2 }
                    ]
                    }
                ]
                }
            ";

            Obj = JObject.Parse(json);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mainMenuItem = new TreeViewItem(){Header = "json"};

            PopulateTree(Obj,mainMenuItem);
            RootTree.Items.Add(mainMenuItem);

        }

        private void PopulateTree(JObject json, TreeViewItem treeViewItem)
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
    }
}
