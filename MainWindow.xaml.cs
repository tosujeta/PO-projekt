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
    public partial class MainWindow : Window
    {
        private string FILE_NAME = "Dane";

        Central central = new Central();

        GroupBox selectedGroupBox = null;

        public MainWindow()
        {
            InitializeComponent();

            //Ukryj wszystkie GroupBox'y
            routGroup.Visibility = Visibility.Hidden;
            customerGroup.Visibility = Visibility.Hidden;
            planeGroup.Visibility = Visibility.Hidden;
            airportGroup.Visibility = Visibility.Hidden;

            try
            {
                if (File.Exists(FILE_NAME)) LoeadFromFile(ref central, FILE_NAME);
            }
            catch
            {
                Console.WriteLine("Błąd podczas wczytywania pliku");
            }

            //Ustaw źródła TreeView
            customersTreeView.ItemsSource = central.Customers;
            planesTreeView.ItemsSource = central.Planes;
            airportsTreeView.ItemsSource = central.Airports;
            routsTreeView.ItemsSource = central.Routs;

            //Ustaw źródła ComboBox'ów
            fromAirportBox.ItemsSource = central.Airports;
            toAirportBox.ItemsSource = central.Airports;
            planeBox.ItemsSource = central.Planes;
            flighFreqBox.ItemsSource = Enum.GetValues(typeof(FlightFrequency));
            customerFlighBox.ItemsSource = central.Routs;

            //Eventu ComboBox'ów
            customerFlighBox.SelectionChanged += CustomerFlightChangedEvent;
            schedulesComboBox.SelectionChanged += SchedulesChangeEvent;
            toAirportBox.SelectionChanged += AirportSelectionBoxChanged;
            fromAirportBox.SelectionChanged += AirportSelectionBoxChanged;

            airportsTreeView.MouseLeftButtonUp += AiportItemClick;
            planesTreeView.MouseLeftButtonUp += PlaneItemClick;
            routsTreeView.MouseLeftButtonUp += RoutsItemClick;
            customersTreeView.MouseLeftButtonUp += CustomersItemClick;

            //Ustaw zdarzenia Check/Uncheck przycisków "edytuj" 
            customerRatioButton.Checked += CustomerButtonChecked;
            customerRatioButton.Unchecked += CustomersButtonUnchecked;
            aiportCheckBox.Checked += AiportButtonChecked;
            aiportCheckBox.Unchecked += AiportButtonUncheck;
            planeCheckBox.Checked += PlaneButtonChecked;
            planeCheckBox.Unchecked += PlaneButtonUnChecked;
            routsCheckBox.Checked += RoutsButtonChecked;
            routsCheckBox.Unchecked += RoutsButtonUnchecked;
            generateRout.Unchecked += GenerateRoutButtonUnchecked;
            generateRout.Checked += GenerateRoutButtonChecked;
        }


        private void SchedulesChangeEvent(object sender, SelectionChangedEventArgs e)
        {
            Schedule schedule = (Schedule)((ComboBox)sender).SelectedItem;
            if (schedule == null) return;
            seatsAndSeatsLimitLabel.Content = schedule.NumberOfTicketsBought + "/" + schedule.Rout.Plane.NumberOfTickets;
            passengersTreeView.ItemsSource = schedule.GetPassengersList();
            flightArriveTime.Content = schedule.GetArrivalTimeAsString();
        }
        private void AirportSelectionBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            Airport toAirport = (Airport)toAirportBox.SelectedItem;
            Airport fromAirport = (Airport)fromAirportBox.SelectedItem;
            if (toAirport == null || fromAirport == null)
            {
                distanceAirportsLabel.Content = "-";
            }
            else
                distanceAirportsLabel.Content = toAirport.GetDistance(fromAirport);
        }

        private void ShowNextFlightSchedule(object sender, RoutedEventArgs e)
        {
            Rout rout = (Rout)customerFlighBox.SelectedItem;
            if (rout == null || !rout.IsSetUp()) return;
            try
            {
                Schedule schedule = rout.NextSchedule((Schedule)customerFlightScheduleLabel.DataContext);
                SetTextBoxDataContextAndText(customerFlightScheduleLabel, schedule);
                customerFlightArriveTime.Content = schedule.GetArrivalTimeAsString();
            }
            catch (FlightFrequencyException ex)
            {
                PushError(ex.Message);
            }
        }

        private void ShowPreviousFlightSchedule(object sender, RoutedEventArgs e)
        {
            Rout rout = (Rout)customerFlighBox.SelectedItem;
            if (rout == null || !rout.IsSetUp()) return;
            try
            {
                Schedule schedule = rout.PreviousSchedule((Schedule)customerFlightScheduleLabel.DataContext);
                SetTextBoxDataContextAndText(customerFlightScheduleLabel, schedule);
                customerFlightArriveTime.Content = schedule.GetArrivalTimeAsString();
            }
            catch (FlightFrequencyException ex)
            {
                PushError(ex.Message);
            }
        }

        private void CustomerFlightChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            Rout rout = (Rout)((ComboBox)sender).SelectedItem;
            if (rout == null) return;
            if (!rout.IsSetUp())
            {
                customerFlightScheduleLabel.Text = "";
                ((ComboBox)sender).SelectedItem = null;
                PushError("Wybrany lot nie jest skonfigurowany!");
                return;
            }
            Schedule schedule = rout.GetSchedules().ElementAt<Schedule>(0);
            SetTextBoxDataContextAndText(customerFlightScheduleLabel, schedule);
            customerFlightArriveTime.Content = schedule.GetArrivalTimeAsString();
        }

        private void SetTextBoxDataContextAndText(TextBox box, object obj)
        {
            customerFlightScheduleLabel.DataContext = obj;
            customerFlightScheduleLabel.Text = obj == null ? "" : obj.ToString();
        }

        private void RoutsItemClick(object sender, object e)
        {
            Rout routs = (Rout)GetSelectedItem(routsTreeView);
            if (routs == null) return;
            ShowGroupBox(routGroup);

            flighFreqBox.SelectedItem = routs.FlightFrequency;
            toAirportBox.SelectedItem = routs.ToAirport;
            fromAirportBox.SelectedItem = routs.FromAirport;
            planeBox.SelectedItem = routs.Plane;
            routDatePicker.SelectedDate = routs.FirstDeparturTime;
            schedulesComboBox.ItemsSource = routs.GetSchedules();
            routTimerLabel.Text = routs.FirstDeparturTime.ToString("HH:mm:ss");
            passengersTreeView.ItemsSource = null;

            schedulesComboBox.SelectedItem = null;
            seatsAndSeatsLimitLabel.Content = "--/--";
            flightArriveTime.Content = "--";

            if (routs.IsSetUp())
            {
                generateRout.IsEnabled = false;
            }
            else
            {
                generateRout.IsEnabled = true;
            }
        }

        private void PlaneItemClick(object sender, object e)
        {
            Plane plane = (Plane)GetSelectedItem(planesTreeView);
            if (plane == null) return;
            ShowGroupBox(planeGroup);

            planeNameLabel.Text = plane.Name;
            numberOfSeatsLabel.Text = plane.NumberOfTickets.ToString();
            rangeLabel.Text = plane.Range.ToString();
            speedLabel.Text = plane.Speed.ToString();
            isFree.Content = plane.IsFree ? "Wolny" : "Zajęty";
        }

        private void AiportItemClick(object sender, object e)
        {
            Airport airport = (Airport)GetSelectedItem(airportsTreeView);
            if (airport == null) return;
            ShowGroupBox(airportGroup);

            cityLabel.Text = airport.City;
            xLabel.Text = airport.X.ToString();
            yLabel.Text = airport.Y.ToString();
        }

        private void CustomersItemClick(object sender, object e)
        {
            Customer customer = (Customer)GetSelectedItem(customersTreeView);
            if (customer == null) return;
            ShowGroupBox(customerGroup);

            nameLabel.Text = customer.Name;
            surnameLabel.Text = customer.Surname;
            ticketPriceLabel.Text = customer.Ticket.Price.ToString();
            customerFlighBox.SelectedItem = customer.GetFlight();
            SetTextBoxDataContextAndText(customerFlightScheduleLabel, customer.FlightSchedule);
            ticketNumberLabel.Text =
                customer.Ticket.IsSingle ? "1" : ((MultiTicket)customer.Ticket).GetNumberOfTicket().ToString();
            if (customer.FlightSchedule != null)
                customerFlightArriveTime.Content = customer.FlightSchedule.GetArrivalTimeAsString();
            else
                customerFlightArriveTime.Content = "";
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

            if (!rout.IsSetUp())
            {
                fromAirportBox.IsEnabled = true;
                toAirportBox.IsEnabled = true;
            }

            generateRout.IsEnabled = false;
            routDatePicker.IsEnabled = true;
            planeBox.IsEnabled = true;
            flighFreqBox.IsEnabled = true;
            routTimerLabel.IsEnabled = true;
        }

        private void RoutsButtonUnchecked(object sender, RoutedEventArgs e)
        {
            toAirportBox.IsEnabled = false;
            fromAirportBox.IsEnabled = false;
            planeBox.IsEnabled = false;
            flighFreqBox.IsEnabled = false;
            routDatePicker.IsEnabled = false;
            routTimerLabel.IsEnabled = false;
            generateRout.IsEnabled = true;

            Rout rout = (Rout)GetSelectedItem(routsTreeView);
            Plane plane = (Plane)planeBox.SelectedItem;
            Airport fromAirport = (Airport)fromAirportBox.SelectedItem;
            Airport toAirport = (Airport)toAirportBox.SelectedItem;
            DateTime date = (DateTime)routDatePicker.SelectedDate;

            try
            {
                float distance;
                DateTime time;

                if (!fromAirport.IsSetUp) throw new Exception("Lotnisko wylotowe nie jest skonfigurowane");
                if (!toAirport.IsSetUp) throw new Exception("Lotnisko docelowe nie jest skonfigurowane");
                if (!plane.IsSetUp) throw new Exception("Samolot nie jest skonfigurowany");
                if (!IsInPlaneRange(plane, fromAirport, toAirport, out distance)) throw new Exception("Samolot posiada za mały zasięg! Zasięg samolotu = " + plane.Range + " Odległość: " + distance);
                if (!DateTime.TryParse(routTimerLabel.Text, out time)) throw new Exception("Podana data jest nieodpowiednia");

                date = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

                rout.SetPlain(plane);
                rout.SetToAirport(toAirport);
                rout.SetFromAirport(fromAirport);
                rout.ChangeDepartureTime(date);
                rout.SetFlightFrequency((FlightFrequency)flighFreqBox.SelectedItem);

                if (!rout.IsSetUp()) rout.SetUpFlight();
            }
            catch (NullReferenceException) { PushError("Ustaw poprawne wartości"); }
            catch (Exception ex) { PushError(ex.Message); }
            RefreshTreeView();
        }

        private bool IsInPlaneRange(Plane plane, Airport airport1, Airport airport2, out float distance)
        {
            distance = airport1.GetDistance(airport2);
            if (distance <= plane.Range) return true;
            return false;
        }

        private void PlaneButtonChecked(object sender, RoutedEventArgs e)
        {
            Plane plane = (Plane)GetSelectedItem(planesTreeView);
            if (!plane.IsSetUp)
            {
                rangeLabel.IsEnabled = true;
                speedLabel.IsEnabled = true;
                numberOfSeatsLabel.IsEnabled = true;
            }
            planeNameLabel.IsEnabled = true;
        }

        private void PlaneButtonUnChecked(object sender, RoutedEventArgs e)
        {
            numberOfSeatsLabel.IsEnabled = false;
            rangeLabel.IsEnabled = false;
            speedLabel.IsEnabled = false;
            planeNameLabel.IsEnabled = false;

            Plane plane = (Plane)GetSelectedItem(planesTreeView);

            try
            {
                int range, speed, numberOfTickets;
                String name;
                if (planeNameLabel.Text.Length > 0) name = planeNameLabel.Text;
                else throw new Exception("Nazwa samolotu powinna być dłuższa niż 0 znaków");
                if (!Int32.TryParse(numberOfSeatsLabel.Text, out numberOfTickets)) throw new Exception("Liczba biletów jest nieodpowiednia");
                if (!Int32.TryParse(rangeLabel.Text, out range)) throw new Exception("Zasięg jest nieodpowiedni");
                if (!Int32.TryParse(speedLabel.Text, out speed)) throw new Exception("Prędkość samolotu jest nieodpowiednia");

                plane.SetUp(name, numberOfTickets, range, speed);
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }

            RefreshTreeView();
        }

        private void AiportButtonUncheck(object sender, RoutedEventArgs e)
        {
            Airport airport = (Airport)GetSelectedItem(airportsTreeView);
            try
            {
                int X, Y;
                string cityName;
                if (cityLabel.Text.Length > 0) cityName = cityLabel.Text;
                else throw new Exception("Nazwa miasta powinna być dłuższa niż 0 znaków");
                if (!Int32.TryParse(xLabel.Text, out X)) throw new Exception("Zła pozycja Y");
                if (!Int32.TryParse(yLabel.Text, out Y)) throw new Exception("Zła pozycja X");

                airport.SetUp(X, Y, cityName);

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
            Airport airport = (Airport)GetSelectedItem(airportsTreeView);

            if (!airport.IsSetUp)
            {
                cityLabel.IsEnabled = true;
                xLabel.IsEnabled = true;
                yLabel.IsEnabled = true;
            }
        }

        private void CustomersButtonUnchecked(object sender, RoutedEventArgs e)
        {
            nameLabel.IsEnabled = false;
            surnameLabel.IsEnabled = false;
            ticketPriceLabel.IsEnabled = false;
            ticketNumberLabel.IsEnabled = false;
            customerFlighBox.IsEnabled = false;
            customerFlightScheduleLabel.IsEnabled = false;
            previousSchedule.IsEnabled = false;
            nextSchedule.IsEnabled = false;

            Customer customer = (Customer)GetSelectedItem(customersTreeView);
            Schedule schedule;
            int price = customer.Ticket.Price, tickets;
            try
            {
                //((Rout)(customerFlighComboBox.SelectedItem)).AddPassanger(customer, (Schedule)customerFlightSchedule.DataContext);
                if (nameLabel.Text.Length > 0) customer.Name = nameLabel.Text;
                else throw new Exception("Imię powinno miec więcej niż 0 znaków");
                if (surnameLabel.Text.Length > 0) customer.Surname = surnameLabel.Text;
                else throw new Exception("Imię powinno miec więcej niż 0 znaków");
                if (Int32.TryParse(ticketPriceLabel.Text, out price) && price >= 0) customer.Ticket.Price = price;
                else throw new Exception("Cena powinna być większa od 0");
                if (Int32.TryParse(ticketNumberLabel.Text, out tickets) && tickets > 0)
                {
                    if (!(customer.Ticket.IsSingle && tickets == 0))
                        customer.SetTicket(customer.Ticket.Change(price, tickets));
                }
                else throw new Exception("Zła liczba biletów");
                schedule = (Schedule)customerFlightScheduleLabel.DataContext;
                if (schedule != null) schedule.Rout.AddPassanger(customer, schedule);
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }

            RefreshTreeView();
        }

        private void CustomerButtonChecked(object sender, RoutedEventArgs e)
        {
            nameLabel.IsEnabled = true;
            surnameLabel.IsEnabled = true;
            ticketPriceLabel.IsEnabled = true;
            ticketNumberLabel.IsEnabled = true;
            customerFlighBox.IsEnabled = true;
            previousSchedule.IsEnabled = true;
            nextSchedule.IsEnabled = true;
        }

        private void GenerateRoutButtonUnchecked(object sender, RoutedEventArgs e)
        {
            routsCheckBox.IsEnabled = true;
            fromAirportBox.IsEnabled = false;
            toAirportBox.IsEnabled = false;
            flighFreqBox.IsEnabled = false;
            seatsLimitLabel.IsEnabled = false;
            routDatePicker.IsEnabled = false;
            routTimerLabel.IsEnabled = false;

            Rout rout = (Rout)GetSelectedItem(routsTreeView);
            Airport fromAirport = (Airport)fromAirportBox.SelectedItem;
            Airport toAirport = (Airport)toAirportBox.SelectedItem;
            DateTime date = (DateTime)routDatePicker.SelectedDate;

            int numberOfSeats;
            DateTime time;
            try
            {
                if (!fromAirport.IsSetUp) throw new Exception("Lotnisko wylotowe nie jest skonfigurowane");
                if (!toAirport.IsSetUp) throw new Exception("Lotnisko docelowe nie jest skonfigurowane");
                if (!Int32.TryParse(seatsLimitLabel.Text, out numberOfSeats)) throw new Exception("Nieprawidłowa liczba miejsc");
                if (DateTime.TryParse(routTimerLabel.Text, out time))
                    date = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                else
                    throw new Exception("Podana data jest nieodpowiednia");

                central.GenerateRout(rout, date, fromAirport, toAirport, numberOfSeats, (FlightFrequency)flighFreqBox.SelectedItem);
                generateRout.IsEnabled = false;
                RoutsItemClick(sender, null);
            }
            catch (NullReferenceException) { PushError("Ustaw poprawne wartości"); }
            catch (Exception ex) { PushError(ex.Message); }
            RefreshTreeView();
        }

        private void GenerateRoutButtonChecked(object sender, RoutedEventArgs e)
        {
            routsCheckBox.IsEnabled = false;
            fromAirportBox.IsEnabled = true;
            toAirportBox.IsEnabled = true;
            flighFreqBox.IsEnabled = true;
            seatsLimitLabel.IsEnabled = true;
            routDatePicker.IsEnabled = true;
            routTimerLabel.IsEnabled = true;
        }

        private object GetSelectedItem(TreeView view)
        {
            return view.SelectedItem;
        }

        public void AddCustomerButtonClick(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer("Name", "Surname");
            central.AddCustomer(customer);
            RefreshTreeView();
        }

        public void AddPlaneButtonClick(object sender, RoutedEventArgs e)
        {
            Plane plane = new Plane();
            central.AddPlain(plane);
            RefreshTreeView();
        }
        public void AddAirportButtonClick(object sender, RoutedEventArgs e)
        {
            Airport airport = new Airport();
            central.AddAirport(airport);
            RefreshTreeView();
        }
        public void AddRoutButtonClick(object sender, RoutedEventArgs e)
        {
            Rout rout = new Rout(DateTime.Now, FlightFrequency.ONE_FLIGHT);
            central.AddRout(rout);
            RefreshTreeView();
        }

        public void RemoveCustomerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                central.RemoveCustomer((Customer)GetSelectedItem(customersTreeView));
                RefreshTreeView();
                CustomersItemClick(customersTreeView.SelectedItem, null);
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }
        }
        public void RemoveAirportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                central.RemoveAiport((Airport)GetSelectedItem(airportsTreeView));
                RefreshTreeView();
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }
        }

        public void RemovePlanerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                central.RemovePlain((Plane)GetSelectedItem(planesTreeView));
                RefreshTreeView();
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }
        }

        public void RemoveRoutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                central.RemoveRout((Rout)GetSelectedItem(routsTreeView));
                RefreshTreeView();
            }
            catch (Exception ex)
            {
                PushError(ex.Message);
            }
        }


        private void ClosingWindowEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveToFile(FILE_NAME, central);
        }

        public void WindowMouseButton(object sender, MouseButtonEventArgs e)
        {
            RemoveError();
        }

        //Wyświetl błąd na dole okna
        public void PushError(String text)
        {
            errorLabel.Foreground = Brushes.Red;
            errorLabel.Content = text;
        }

        //Usuń błąd na dole okna
        public void RemoveError()
        {
            errorLabel.Foreground = Brushes.Black;
            errorLabel.Content = "Wszystko OK!";
        }

        public void RefreshTreeView()
        {
            RefreshView(routsTreeView);
            RefreshView(customersTreeView);
            RefreshView(planesTreeView);
            RefreshView(airportsTreeView);
            RefreshView(passengersTreeView);
            RefreshView(customerFlighBox);
            RefreshView(schedulesComboBox);
            /*

            routsTreeView.Items.Refresh();
            customersTreeView.Items.Refresh();
            planesTreeView.Items.Refresh();
            airportsTreeView.Items.Refresh();
            passengersTreeView.Items.Refresh();
            schedulesComboBox.Items.Refresh();
            customerFlighBox.Items.Refresh();
            */
        }

        private void RefreshView(ComboBox view)
        {
            view.Items.Refresh();
        }

        public void RefreshView(TreeView view)
        {
            object obj = view.SelectedItem;
            view.Items.Refresh();
            TreeViewItem obj2 = (TreeViewItem)view.ItemContainerGenerator.ContainerFromItem(obj);
            if (obj2 != null) obj2.IsSelected = true;

        }
    }
}
