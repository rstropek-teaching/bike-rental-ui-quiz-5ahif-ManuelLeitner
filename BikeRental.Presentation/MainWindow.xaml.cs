using BikeRental.Persistence;
using BikeRental.Presentation.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BikeRental.Presentation {

    public partial class MainWindow : Window {

        public BikeRentalViewModel Model { get; } = new BikeRentalViewModel(new ApiClient(Settings.Default.ServiceURL));

        public MainWindow() {
            InitializeComponent();
            GenderColumn.ItemsSource = Enum.GetValues(typeof(Gender));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            var customerViewSource = ((CollectionViewSource)(this.FindResource("CustomerViewSource")));
            customerViewSource.Source = Model.Customers;

            var bikeViewSource = ((CollectionViewSource)(this.FindResource("BikeViewSource")));
            bikeViewSource.Source = Model.Bikes;

            var categoryViewSource = ((CollectionViewSource)(this.FindResource("CategoryViewSource")));
            categoryViewSource.Source = Model.Categories;

            try {
                await Model.RefreshAllAsync();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            CollectionViewSource rentalViewSource = ((CollectionViewSource)(this.FindResource("RentalViewSource")));
            rentalViewSource.Source = Model.UnpaiedRentals;
        }

        private bool locker = true;
        private async void StoreBikes(object sender, DataGridRowEditEndingEventArgs e) {
            if (locker) {
                try {
                    locker = false;
                    (sender as DataGrid).CommitEdit(DataGridEditingUnit.Row, false);
                    await Model.StoreBikeAsync((Bike)BikeDataGrid.SelectedItem);
                } finally {
                    locker = true;
                }
            }
        }

        private async void StoreCustomers(object sender, DataGridCellEditEndingEventArgs e) {
            if (locker) {
                try {
                    locker = false;
                    (sender as DataGrid).CommitEdit(DataGridEditingUnit.Row, false);
                    await Model.StoreCustomerAsync((Customer)CustomerDataGrid.SelectedItem);
                } finally {
                    locker = true;
                }
            }
        }

        private async void ApplyCustomerFilter(object sender, TextChangedEventArgs e) {
            await Model.RefreshCustomersAsync();
        }

        private async void Refresh(object sender, RoutedEventArgs e) {
            await Model.RefreshAllAsync();
        }

        private async void DeleteBike(object sender, KeyEventArgs e) {
            if (e.Key == Key.Delete) {
                await Model.DeleteBikeAsync((Bike)BikeDataGrid.SelectedItem);
            }
        }
    }
}
