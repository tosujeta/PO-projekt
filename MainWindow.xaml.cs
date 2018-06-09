using System;
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

namespace po_proj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Central central = new Central();
            central.Customers.Add(new Customer("Janek", "Bar"));
            central.Customers.Add(new Customer("Radek", "Rok"));
            central.Customers.Add(new Customer("Bartek", "Jar"));


            FillTreeViewWith(costumersTreeView, central.Customers);
            costumersTreeView.MouseLeftButtonUp += treeItemDoubleClick;

        }

        private void treeItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer item = (Customer) ((TreeView) sender).SelectedItem;
            nameLabel.Content = item.GetName();
            surnameLabel.Content = item.GetSurname();
            Console.Out.WriteLine("Test");
        }

        private void FillTreeViewWith(TreeView costumersTreeView, List<Customer> customers)
        {
            TreeViewItem menu = new TreeViewItem() { Header = "Klienci" };
            menu.IsExpanded = true;
            customers.ForEach(c =>
           {
               menu.Items.Add(c);
           });

            costumersTreeView.Items.Add(menu);
        }

        public void MyButton_click(object sender, RoutedEventArgs e)
        {
        }
    }
}
