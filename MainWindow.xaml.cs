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
        Central central = new Central();

        public MainWindow()
        {
            InitializeComponent();

            Consola();

            Rout rout = new Rout(
                new DateTime(2016, 1, 1, 12, 0, 0),
                new Airport(0, 0, "Januszowo"),
                new Airport(20, 20, "Januszowo"),
                new Plane(20, 1000, 10),
                0);
            DateTime date = new DateTime(2016, 1, 1, 12, 0, 1);
            date = new DateTime(2016, 1, 1, 12, 0, 0);
            Customer customer = new Customer("a", "b");
            customer.SetTicket(new Ticket(0, 0, 0));
            Customer customer2 = new Customer("b", "c");
            customer2.SetTicket(new MultiTicket(0, 0, 0, 500));

            try
            {//TODO poprawic bo nie mozna dodac pasazera
                rout.AddPassenger(customer);
                rout.AddPassenger(customer2);
            }catch(System.ApplicationException e)
            {
                Console.WriteLine(e.Message);
            }
          

            DateTime time = new DateTime(2008, 12, 24, 12, 34, 12);
            DateTime time2 = new DateTime(2008, 12, 23, 12, 34, 12);
            Console.WriteLine(time2 == time);
            Console.WriteLine(time2.Ticks + " : " + time.Ticks);
            time2 = time2.AddTicks((long)FlightFrequency.EVERY_DAY);
            Console.WriteLine(time2 == time);
            Console.WriteLine(time2.Ticks + " : " + time.Ticks);


            FillTreeViewWith(customersTreeView, central.customers);
            FillTreeViewWith(planeTreeView, central.planes);
            FillTreeViewWith(routsTreeView, central.routs);
            FillTreeViewWith(airportTreeView, central.airports);
            customersTreeView.MouseLeftButtonUp += treeItemDoubleClick;


            addCustomerButton.Click += CustomerButton_click;
            nameLabel.LostKeyboardFocus += (e, s) =>
            {
                String t = GetSelectedCustomer().Name;
                SetText(ref t, GetTextBoxText(e));
                GetSelectedCustomer().Name = t;
            };
        }

        private void Consola()
        {
            Central central = new Central();

        }

        private void SetText(ref String name, String text)
        {
            if (text != null) 
                name = text;
        }

        private Customer GetSelectedCustomer()
        {
            return (Customer) customersTreeView.SelectedItem;
        }

        private String GetTextBoxText(Object sender)
        {
            TextBox box = (TextBox)sender;
            if (box.Text.Length == 0) return null;
            return box.Text;
        }

        private void treeItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer item = (Customer) ((TreeView) sender).SelectedItem;
            if (item == null) return;
            nameLabel.Text = item.GetName();
            surnameLabel.Text = item.GetSurname();
        }

        private void FillTreeViewWith(TreeView treeView, IEnumerable<Object> list) 
        {
            foreach(Object o in list)
            {
                treeView.Items.Add(o);

            }
        }

        public void CustomerButton_click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer("Name", "Surname");
            central.AddCustomer(customer);
            customersTreeView.Items.Add(customer);
        }
    }
}
