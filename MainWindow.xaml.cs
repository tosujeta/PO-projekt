using System;
using System.Collections.Generic;
using System.IO;
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

            central.AddCustomer(new Customer("Name", "Surname"));

            try
            {
                if (File.Exists("file")) LoeadFromFile(ref central, "file");
            }
            catch
            {
                Console.WriteLine("Some errror");
            }

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
            }
            catch (System.ApplicationException e)
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


            customersTreeView.ItemsSource = central.customers;
            planeTreeView.ItemsSource = central.planes;
            airportTreeView.ItemsSource = central.airports;
            routsTreeView.ItemsSource = central.routs;

            customersTreeView.MouseLeftButtonUp += TreeItemDoubleClick;


            addCustomerButton.Click += CustomerButton_click;
            customerRatioButton.Checked += CustomButtomChecked;
            customerRatioButton.Unchecked += CutomButtonUncheck;
        }

        public void SaveToFile(String f, Central central)
        {
            using (Stream stream = File.Open(f, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, central);
            }
        }

        public void LoeadFromFile(ref Central central, String f)
        {
            using (Stream stream = File.Open(f, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                central = (Central)binaryFormatter.Deserialize(stream);
            }
        }

        private void CutomButtonUncheck(object sender, RoutedEventArgs e)
        {
            nameLabel.IsEnabled = false;
            surnameLabel.IsEnabled = false;
            ticketPriceLabel.IsEnabled = false;
            ticketNumberLabel.IsEnabled = false;

            Customer customer = GetSelectedCustomer();
            int price = customer.Ticket.Price, tickets;
            if (nameLabel.Text.Length > 0) customer.Name = nameLabel.Text;
            if (surnameLabel.Text.Length > 0) customer.Surname = surnameLabel.Text;
            if (Int32.TryParse(ticketPriceLabel.Text, out price) && price > 0) customer.Ticket.Price = price;
            if (Int32.TryParse(ticketNumberLabel.Text, out tickets) && tickets > 0)
                if (!(customer.Ticket.IsSingle && tickets == 0))
                    customer.Ticket = customer.Ticket.Change(price, tickets);
            //TODO Powinno w locie tez zmienic rozna rzeczy

            customersTreeView.Items.Refresh();
        }

        private void CustomButtomChecked(object sender, RoutedEventArgs e)
        {
            nameLabel.IsEnabled = true;
            surnameLabel.IsEnabled = true;
            ticketPriceLabel.IsEnabled = true;
            ticketNumberLabel.IsEnabled = true;
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
            return (Customer)customersTreeView.SelectedItem;
        }

        private String GetTextBoxText(Object sender)
        {
            TextBox box = (TextBox)sender;
            if (box.Text.Length == 0) return null;
            return box.Text;
        }

        private void TreeItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer item = (Customer)((TreeView)sender).SelectedItem;
            if (item == null) return;
            nameLabel.Text = item.GetName();
            surnameLabel.Text = item.GetSurname();
            ticketPriceLabel.Text = item.Ticket.Price.ToString();
            ticketNumberLabel.Text =
                item.Ticket.IsSingle ? "1" : ((MultiTicket)item.Ticket).GetNumberOfTicket().ToString();
            
        }

        public void CustomerButton_click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer("Name", "Surname");
            central.AddCustomer(customer);
            customersTreeView.Items.Refresh();
        }

        private void ClosingWindowEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveToFile("file", central);
        }
    }
}
