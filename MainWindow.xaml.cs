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

            Rout rout = new Rout(
                new DateTime(2016, 1, 1, 12, 0, 0),
                new Airport(0, 0, "Januszowo"),
                new Airport(20, 20, "Januszowo"),
                new Plane(20, 1000, 10),
                0);
            DateTime date = new DateTime(2016, 1, 1, 12, 0, 1);
            Console.WriteLine("Distance: " +rout.GetDistacne());
            Console.WriteLine("IS one rout: " + rout.IsOneRout());
            Console.WriteLine("WillFlight(false): " + rout.WillFlight(date));
            date = new DateTime(2016, 1, 1, 12, 0, 0);
            Console.WriteLine("WillFlight(false): " + rout.WillFlight(date));
            Customer customer = new Customer("a", "b");
            customer.SetTicket(new Ticket(0, 0, 0));
            Customer customer2 = new Customer("b", "c");
            customer.SetTicket(new MultiTicket(0, 0, 0, 500));

            try
            {//TODO poprawic bo nie mozna dodac pasazera
                rout.AddPassenger(customer);
                rout.AddPassenger(customer2);
            }catch
            {
                Console.WriteLine("Wyjatek");
            }
          

            DateTime time = new DateTime(2008, 12, 24, 12, 34, 12);
            DateTime time2 = new DateTime(2008, 12, 23, 12, 34, 12);
            Console.WriteLine(time2 == time);
            Console.WriteLine(time2.Ticks + " : " + time.Ticks);
            time2 = time2.AddTicks((long)FlightFrequency.EVERY_DAY);
            Console.WriteLine(time2 == time);
            Console.WriteLine(time2.Ticks + " : " + time.Ticks);


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
