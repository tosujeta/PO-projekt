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

        GroupBox selectedGroupBox = null;

        public MainWindow()
        {
            InitializeComponent();



            //DO USUNIECIA
            routGroup.Visibility = Visibility.Hidden;
            customerGroup.Visibility = Visibility.Hidden;
            planeGroup.Visibility = Visibility.Hidden;
            airportGroup.Visibility = Visibility.Hidden;


            central.AddCustomer(new Customer("Name", "Surname", null));

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
                new Plane("J", 20, 1000, 10),
                0);
            DateTime date = new DateTime(2016, 1, 1, 12, 0, 1);
            date = new DateTime(2016, 1, 1, 12, 0, 0);
            Customer customer = new Customer("a", "b", null);
            customer.SetTicket(new Ticket(0, 0, 0));
            Customer customer2 = new Customer("b", "c", null);
            customer2.SetTicket(new MultiTicket(0, 0, 0, 500));
            rout.SetUpFlight();

            try
            {//TODO poprawic bo nie mozna dodac pasazera
                rout.AddPassenger(customer);
                rout.AddPassenger(customer2);
            }
            catch (MaxPassengersReached e)
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

            airportFromBox.ItemsSource = central.airports;
            airportToBox.ItemsSource = central.airports;
            planeBox.ItemsSource = central.planes;
            flighFreqBox.ItemsSource = Enum.GetValues(typeof(FlightFrequency));
            customerFlighComboBox.ItemsSource = central.routs;

            //customersTreeView.MouseLeftButtonUp += CustomersItemClick; object sender, MouseButtonEventArgs e
            airportTreeView.MouseLeftButtonUp += AiportItemClick;
            planeTreeView.MouseLeftButtonUp += PlaneItemClick;
            routsTreeView.MouseLeftButtonUp += RoutsItemClick;
            customersTreeView.MouseLeftButtonUp += CustomersItemClick;
            //object sender, RoutedPropertyChangedEventArgs<object> e

            previousSchedule.Click += ShowPreviousFlightSchedule;
            nextSchedule.Click += ShowNextFlightSchedule;


            customerRatioButton.Checked += CustomButtomChecked;
            customerRatioButton.Unchecked += CutomButtonUncheck;
            aiportCheckBox.Checked += AiportButtonChecked;
            aiportCheckBox.Unchecked += AiportButtonUncheck;
            planeCheckBox.Checked += PlaneButtonChecked;
            planeCheckBox.Unchecked += PlaneButtonUnChecked;
            routsCheckBox.Checked += RoutsButtonChecked;
            routsCheckBox.Unchecked += RoutsButtonUnchecked;

            //Combobox eventy
            customerFlighComboBox.SelectionChanged += CustomerFlightChangedEvent;
            schedulesComboBox.SelectionChanged += SchedulesChangeEvent;
        }

        private void SchedulesChangeEvent(object sender, SelectionChangedEventArgs e)
        {
            Schedule schedule = (Schedule)((ComboBox)sender).SelectedItem;
            if (schedule == null) return;
            seatsAndSeatsLimitLabel.Content = schedule.NumberOfTicketsBought + "/" + schedule.GetFlightID().GetPlain().NumberOfTickets;
            passengersTreeView.ItemsSource = schedule.GetPassengersList();
        }

        private void AiportItemClick2(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowNextFlightSchedule(object sender, RoutedEventArgs e)
        {
            Rout rout = (Rout)customerFlighComboBox.SelectedItem;
            if (rout == null) return;
            //customerFlightSchedule.DataContext = rout.NextSchedule((Schedule)customerFlightSchedule.DataContext);
            try
            {
                SetTextBoxDataContextAndText(customerFlightSchedule, rout.NextSchedule((Schedule)customerFlightSchedule.DataContext));
            }
            catch (FlightFrequencyException ex)
            {
                PushError(ex.Message);
            }

        }

        private void ShowPreviousFlightSchedule(object sender, RoutedEventArgs e)
        {
            Rout rout = (Rout)customerFlighComboBox.SelectedItem;
            if (rout == null) return;
            //customerFlightSchedule.DataContext = rout.PreviousSchedule((Schedule)customerFlightSchedule.DataContext);
            try
            {
                SetTextBoxDataContextAndText(customerFlightSchedule, rout.PreviousSchedule((Schedule)customerFlightSchedule.DataContext));
            }
            catch (FlightFrequencyException ex)
            {
                PushError(ex.Message);
            }
        }

        private void CustomerFlightChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            Rout selectedRout = (Rout)((ComboBox)sender).SelectedItem;
            if (selectedRout == null) return;
            if (!selectedRout.IsSetUp())
            {
                customerFlightSchedule.Text = "";
                return;
            }
            SetTextBoxDataContextAndText(customerFlightSchedule, selectedRout.GetSchedules().ElementAt(0));
        }

        private void SetTextBoxDataContextAndText(TextBox box, object obj)
        {
            customerFlightSchedule.DataContext = obj;
            customerFlightSchedule.Text = obj == null ? "" : obj.ToString();
        }

        private void RoutsItemClick(object sender, MouseButtonEventArgs e)
        {
            Rout routs = (Rout)GetSelectedItem(routsTreeView);
            if (routs == null) return;
            ShowGroupBox(routGroup);

            flighFreqBox.SelectedItem = routs.FlightFrequency;
            airportToBox.SelectedItem = routs.ToAirport;
            airportFromBox.SelectedItem = routs.FromAirport;
            planeBox.SelectedItem = routs.Plane;
            routDatePicker.SelectedDate = routs.FirstDeparturTime;
            schedulesComboBox.ItemsSource = routs.GetSchedules();
            routTimerLabel.Text = routs.FirstDeparturTime.ToString("HH:mm:ss");
            schedulesComboBox.SelectedItem = null;
            seatsAndSeatsLimitLabel.Content = "--/--";
        }

        private void PlaneItemClick(object sender, MouseButtonEventArgs e)
        {
            Plane plane = (Plane)GetSelectedItem(planeTreeView);
            if (plane == null) return;
            ShowGroupBox(planeGroup);

            planeNameLabel.Text = plane.Name;
            numberOfSeatsLabel.Text = plane.NumberOfTickets.ToString();
            rangeLabel.Text = plane.Range.ToString();
            speedLabel.Text = plane.Speed.ToString();
            isFree.Content = plane.IsFree ? "Wolny" : "Zajęty";
        }

        private void AiportItemClick(object sender, MouseButtonEventArgs e)
        {
            Airport airport = (Airport)GetSelectedItem(airportTreeView);
            if (airport == null) return;
            ShowGroupBox(airportGroup);

            cityLabel.Text = airport.City;
            xLabel.Text = airport.X.ToString();
            yLabel.Text = airport.Y.ToString();
        }

        private void CustomersItemClick(object sender, MouseButtonEventArgs e)
        {
            Customer Customer = (Customer)GetSelectedItem(customersTreeView);
            if (Customer == null) return;
            ShowGroupBox(customerGroup);

            nameLabel.Text = Customer.GetName();
            surnameLabel.Text = Customer.GetSurname();
            ticketPriceLabel.Text = Customer.Ticket.Price.ToString();
            customerFlighComboBox.SelectedItem = Customer.GetFlight();
            SetTextBoxDataContextAndText(customerFlightSchedule, Customer.FlightSchedule);
            ticketNumberLabel.Text =
                Customer.Ticket.IsSingle ? "1" : ((MultiTicket)Customer.Ticket).GetNumberOfTicket().ToString();

        }

        public void ShowGroupBox(GroupBox box)
        {
            if (selectedGroupBox != null) selectedGroupBox.Visibility = Visibility.Hidden;

            box.Visibility = Visibility.Visible;
            selectedGroupBox = box;
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

        private void RoutsButtonChecked(object sender, RoutedEventArgs e)
        {
            Rout rout = (Rout)GetSelectedItem(routsTreeView);

            if (airportFromBox.SelectedItem == null) airportFromBox.IsEnabled = true;
            if (airportToBox.SelectedItem == null) airportToBox.IsEnabled = true;

            routDatePicker.IsEnabled = true;
            planeBox.IsEnabled = true;
            flighFreqBox.IsEnabled = true;
            routTimerLabel.IsEnabled = true;
        }

        private void RoutsButtonUnchecked(object sender, RoutedEventArgs e)
        {
            airportToBox.IsEnabled = false;
            airportFromBox.IsEnabled = false;
            planeBox.IsEnabled = false;
            flighFreqBox.IsEnabled = false;
            routDatePicker.IsEnabled = false;
            routTimerLabel.IsEnabled = false;

            Rout rout = (Rout)GetSelectedItem(routsTreeView);
            Plane plane = (Plane)planeBox.SelectedItem;
            Airport fromAirport = (Airport)airportFromBox.SelectedItem;
            Airport toAirport = (Airport)airportToBox.SelectedItem;
            DateTime date = (DateTime)routDatePicker.SelectedDate;

            try
            {
                float distance;
                if (IsInPlaneRange(plane, fromAirport, toAirport, out distance))
                {
                    rout.SetPlain(plane);
                    rout.SetToAirport(toAirport);
                    rout.SetFromAirport(fromAirport);
                }
                else throw new Exception("Samolot posiada za mały zasięg! Zasięg samolotu = " + plane.GetRange() + " Odległość: " + distance);
                DateTime time;
                if (DateTime.TryParse(routTimerLabel.Text, out time))
                {
                    date = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                    rout.ChangeDepartureTime(date);
                    if (!rout.IsSetUp()) rout.SetUpFlight();
                }
                else throw new Exception("Podana data jest nieodpowiednia");

                rout.SetFlightFrequency((FlightFrequency)flighFreqBox.SelectedItem);
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }

            RefreshTreeView();
        }

        private bool IsInPlaneRange(Plane plane, Airport airport1, Airport airport2, out float distance)
        {
            distance = airport1.GetDistance(airport2);
            if (distance <= plane.GetRange()) return true;
            return false;
        }

        private void PlaneButtonChecked(object sender, RoutedEventArgs e)
        {
            numberOfSeatsLabel.IsEnabled = true;
            planeNameLabel.IsEnabled = true;
            rangeLabel.IsEnabled = true;
            speedLabel.IsEnabled = true;
        }

        private void PlaneButtonUnChecked(object sender, RoutedEventArgs e)
        {
            numberOfSeatsLabel.IsEnabled = false;
            rangeLabel.IsEnabled = false;
            speedLabel.IsEnabled = false;
            planeNameLabel.IsEnabled = false;

            Plane plane = (Plane)GetSelectedItem(planeTreeView);

            int number;
            try
            {
                if (planeNameLabel.Text.Length > 0) plane.Name = planeNameLabel.Text;
                else throw new Exception("Nazwa samolotu powinna być dłuższa niż 0 znaków");
                if (Int32.TryParse(numberOfSeatsLabel.Text, out number)) plane.NumberOfTickets = number;
                else throw new Exception("Liczba biletów jest nieodpowiednia");
                if (Int32.TryParse(rangeLabel.Text, out number)) plane.Range = number;
                else throw new Exception("Zasięg jest nieodpowiedni");
                if (Int32.TryParse(speedLabel.Text, out number)) plane.Speed = number;
                else throw new Exception("Prędkość samolotu jest nieodpowiednia");
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }

            RefreshTreeView();
        }

        private void AiportButtonUncheck(object sender, RoutedEventArgs e)
        {
            Airport airport = (Airport)GetSelectedItem(airportTreeView);
            try
            {
                int number;
                if (cityLabel.Text.Length > 0) airport.City = cityLabel.Text;
                else throw new Exception("Nazwa miasta powinna być dłuższa niż 0 znaków");
                if (Int32.TryParse(xLabel.Text, out number)) airport.X = number;
                else throw new Exception(("Zła pozycja Y"));
                if (Int32.TryParse(yLabel.Text, out number)) airport.Y = number;
                else throw new Exception("Zła pozycja X");

                cityLabel.IsEnabled = false;
                xLabel.IsEnabled = false;
                yLabel.IsEnabled = false;
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }

            RefreshTreeView();
        }

        private void AiportButtonChecked(object sender, RoutedEventArgs e)
        {
            cityLabel.IsEnabled = true;
            xLabel.IsEnabled = true;
            yLabel.IsEnabled = true;
        }

        private void CutomButtonUncheck(object sender, RoutedEventArgs e)
        {
            nameLabel.IsEnabled = false;
            surnameLabel.IsEnabled = false;
            ticketPriceLabel.IsEnabled = false;
            ticketNumberLabel.IsEnabled = false;
            customerFlighComboBox.IsEnabled = false;
            customerFlightSchedule.IsEnabled = false;
            previousSchedule.IsEnabled = false;
            nextSchedule.IsEnabled = false;

            Customer customer = (Customer)GetSelectedItem(customersTreeView);
            int price = customer.Ticket.Price, tickets;
            try
            {
                //((Rout)(customerFlighComboBox.SelectedItem)).AddPassanger(customer, (Schedule)customerFlightSchedule.DataContext);
                customer.SetFlightSchedule((Schedule)customerFlightSchedule.DataContext);
                if (nameLabel.Text.Length > 0) customer.Name = nameLabel.Text;
                else throw new Exception("Imię powinno miec więcej niż 0 znaków");
                if (surnameLabel.Text.Length > 0) customer.Surname = surnameLabel.Text;
                else throw new Exception("Imię powinno miec więcej niż 0 znaków");
                if (Int32.TryParse(ticketPriceLabel.Text, out price) && price > 0) customer.Ticket.Price = price;
                else throw new Exception("Cena powinna być większa od 0");
                if (Int32.TryParse(ticketNumberLabel.Text, out tickets) && tickets > 0)
                {
                    if (!(customer.Ticket.IsSingle && tickets == 0))
                        customer.SetTicket(customer.Ticket.Change(price, tickets));
                }
                else throw new Exception("Zła liczba biletów");
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }

            RefreshTreeView();
        }

        private void CustomButtomChecked(object sender, RoutedEventArgs e)
        {
            nameLabel.IsEnabled = true;
            surnameLabel.IsEnabled = true;
            ticketPriceLabel.IsEnabled = true;
            ticketNumberLabel.IsEnabled = true;
            customerFlighComboBox.IsEnabled = true;
            previousSchedule.IsEnabled = true;
            nextSchedule.IsEnabled = true;
        }

        private object GetSelectedItem(TreeView view)
        {
            return view.SelectedItem;
        }

        private String GetTextBoxText(Object sender)
        {
            TextBox box = (TextBox)sender;
            if (box.Text.Length == 0) return null;
            return box.Text;
        }

        public void AddCustomerButtonClick(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer("Name", "Surname", null);
            central.AddCustomer(customer);
            RefreshTreeView();
        }

        public void AddPlaneButtonClick(object sender, RoutedEventArgs e)
        {
            Plane plane = new Plane("Bez nazwy", 0, 0, 0);
            central.AddPlain(plane);
            RefreshTreeView();
        }
        public void AddAirportButtonClick(object sender, RoutedEventArgs e)
        {
            Airport airport = new Airport(0, 0, "Nigdzie");
            central.AddAirport(airport);
            RefreshTreeView();
        }
        public void AddRoutButtonClick(object sender, RoutedEventArgs e)
        {
            Rout rout = new Rout(DateTime.Now, null, null, null, FlightFrequency.ONE_FLIGHT);
            central.AddRout(rout);
            RefreshTreeView();
        }

        private void ClosingWindowEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveToFile("file", central);
        }

        public void WindowMouseButton(object sender, MouseButtonEventArgs e)
        {
            RemoveError();
        }

        public void PushError(String text)
        {
            errorLabel.Foreground = Brushes.Red;
            errorLabel.Content = text;
        }

        public void RemoveError()
        {
            errorLabel.Foreground = Brushes.Black;
            errorLabel.Content = "Wszystko OK!";
        }

        public void RefreshTreeView()
        {
            routsTreeView.Items.Refresh();
            customersTreeView.Items.Refresh();
            planeTreeView.Items.Refresh();
            airportTreeView.Items.Refresh();
            customerFlighComboBox.Items.Refresh();
            schedulesComboBox.Items.Refresh();
            passengersTreeView.Items.Refresh();
        }
    }
}
