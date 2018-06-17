﻿using System;
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

            float expected = 5f;

            Airport airport = new Airport();
            airport.SetUp(2, 5, "1");
            Airport airport2 = new Airport();
            airport2.SetUp(5, 9, "2");
            float actual = airport.GetDistance(airport2);



            //DO USUNIECIA
            routGroup.Visibility = Visibility.Hidden;
            customerGroup.Visibility = Visibility.Hidden;
            planeGroup.Visibility = Visibility.Hidden;
            airportGroup.Visibility = Visibility.Hidden;


            central.AddCustomer(new Customer("Name", "Surname"));

            try
            {
                if (File.Exists("file")) LoeadFromFile(ref central, "file");
            }
            catch
            {
                Console.WriteLine("Some errror");
            }

            customersTreeView.ItemsSource = central.Customers;
            planeTreeView.ItemsSource = central.Planes;
            airportTreeView.ItemsSource = central.Airports;
            routsTreeView.ItemsSource = central.Routs;

            airportFromBox.ItemsSource = central.Airports;
            airportToBox.ItemsSource = central.Airports;
            planeBox.ItemsSource = central.Planes;
            flighFreqBox.ItemsSource = Enum.GetValues(typeof(FlightFrequency));
            customerFlighComboBox.ItemsSource = central.Routs;

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
            generateRout.Unchecked += GenerateRoutButtonUnchecked;
            generateRout.Checked += GenerateRoutButtonChecked;

            //Combobox eventy
            customerFlighComboBox.SelectionChanged += CustomerFlightChangedEvent;
            schedulesComboBox.SelectionChanged += SchedulesChangeEvent;
        }



        private void SchedulesChangeEvent(object sender, SelectionChangedEventArgs e)
        {
            Schedule schedule = (Schedule)((ComboBox)sender).SelectedItem;
            if (schedule == null) return;
            seatsAndSeatsLimitLabel.Content = schedule.NumberOfTicketsBought + "/" + schedule.Rout.Plane.NumberOfTickets;
            passengersTreeView.ItemsSource = schedule.GetPassengersList();
            flightArriveTime.Content = schedule.GetArrivalTimeAsString();
        }

        private void ShowNextFlightSchedule(object sender, RoutedEventArgs e)
        {
            Rout rout = (Rout)customerFlighComboBox.SelectedItem;
            if (rout == null) return;
            //customerFlightSchedule.DataContext = rout.NextSchedule((Schedule)customerFlightSchedule.DataContext);
            try
            {
                Schedule schedule = rout.NextSchedule((Schedule)customerFlightSchedule.DataContext);
                SetTextBoxDataContextAndText(customerFlightSchedule, schedule);
                customerFlightArriveTime.Content = schedule.Arrivaltime;

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
                Schedule schedule = rout.PreviousSchedule((Schedule)customerFlightSchedule.DataContext);
                SetTextBoxDataContextAndText(customerFlightSchedule, schedule);
                customerFlightArriveTime.Content = schedule.Arrivaltime;
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
            Schedule schedule = selectedRout.GetSchedules().ElementAt<Schedule>(0);
            SetTextBoxDataContextAndText(customerFlightSchedule, schedule);
            customerFlightArriveTime.Content = schedule.GetArrivalTimeAsString();
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
            flightArriveTime.Content = "--";
            if (routs.IsSetUp()) generateRout.IsEnabled = false;
            else generateRout.IsEnabled = true;
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
            Customer customer = (Customer)GetSelectedItem(customersTreeView);
            if (customer == null) return;
            ShowGroupBox(customerGroup);

            nameLabel.Text = customer.Name;
            surnameLabel.Text = customer.Surname;
            ticketPriceLabel.Text = customer.Ticket.Price.ToString();
            customerFlighComboBox.SelectedItem = customer.GetFlight();
            SetTextBoxDataContextAndText(customerFlightSchedule, customer.FlightSchedule);
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
                airportFromBox.IsEnabled = true;
                airportToBox.IsEnabled = true;
            }

            generateRout.IsEnabled = false;
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
            generateRout.IsEnabled = true;

            Rout rout = (Rout)GetSelectedItem(routsTreeView);
            Plane plane = (Plane)planeBox.SelectedItem;
            Airport fromAirport = (Airport)airportFromBox.SelectedItem;
            Airport toAirport = (Airport)airportToBox.SelectedItem;
            DateTime date = (DateTime)routDatePicker.SelectedDate;

            try
            {
                if (!fromAirport.IsSetUp) throw new Exception("Lotnisko wylotowe nie jest skonfigurowane");
                if (!toAirport.IsSetUp) throw new Exception("Lotnisko docelowe nie jest skonfigurowane");
                if (!plane.IsSetUp) throw new Exception("Samolot nie jest skonfigurowany");
                float distance;
                if (IsInPlaneRange(plane, fromAirport, toAirport, out distance))
                {
                    rout.SetPlain(plane);
                    rout.SetToAirport(toAirport);
                    rout.SetFromAirport(fromAirport);
                }
                else throw new Exception("Samolot posiada za mały zasięg! Zasięg samolotu = " + plane.Range + " Odległość: " + distance);
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
            if (distance <= plane.Range) return true;
            return false;
        }

        private void PlaneButtonChecked(object sender, RoutedEventArgs e)
        {
            Plane plane = (Plane)GetSelectedItem(planeTreeView);
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

            Plane plane = (Plane)GetSelectedItem(planeTreeView);

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
            Airport airport = (Airport)GetSelectedItem(airportTreeView);
            try
            {
                int X, Y;
                string cityName;
                if (cityLabel.Text.Length > 0) cityName = cityLabel.Text;
                else throw new Exception("Nazwa miasta powinna być dłuższa niż 0 znaków");
                if (Int32.TryParse(xLabel.Text, out X)) ;
                else throw new Exception(("Zła pozycja Y"));
                if (Int32.TryParse(yLabel.Text, out Y)) ;
                else throw new Exception("Zła pozycja X");

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
            Airport airport = (Airport)GetSelectedItem(airportTreeView);

            if (!airport.IsSetUp)
            {
                cityLabel.IsEnabled = true;
                xLabel.IsEnabled = true;
                yLabel.IsEnabled = true;
            }
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
                schedule = (Schedule)customerFlightSchedule.DataContext;
                if (schedule != null) schedule.Rout.AddPassanger(customer, schedule);
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

        private void GenerateRoutButtonUnchecked(object sender, RoutedEventArgs e)
        {
            routsCheckBox.IsEnabled = true;
            airportFromBox.IsEnabled = false;
            airportToBox.IsEnabled = false;
            flighFreqBox.IsEnabled = false;
            seatsLimitLabel.IsEnabled = false;
            routDatePicker.IsEnabled = false;
            routTimerLabel.IsEnabled = false;

            Rout rout = (Rout)GetSelectedItem(routsTreeView);
            Airport fromAirport = (Airport)airportFromBox.SelectedItem;
            Airport toAirport = (Airport)airportToBox.SelectedItem;
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
            catch (Exception ex)
            {
                PushError(ex.Message);
            }
            RefreshTreeView();
        }

        private void GenerateRoutButtonChecked(object sender, RoutedEventArgs e)
        {
            routsCheckBox.IsEnabled = false;
            airportFromBox.IsEnabled = true;
            airportToBox.IsEnabled = true;
            flighFreqBox.IsEnabled = true;
            seatsLimitLabel.IsEnabled = true;
            routDatePicker.IsEnabled = true;
            routTimerLabel.IsEnabled = true;
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
                central.RemoveAiport((Airport)GetSelectedItem(airportTreeView));
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
                central.RemovePlain((Plane)GetSelectedItem(planeTreeView));
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
            passengersTreeView.Items.Refresh();
            schedulesComboBox.Items.Refresh();
        }
    }
}
