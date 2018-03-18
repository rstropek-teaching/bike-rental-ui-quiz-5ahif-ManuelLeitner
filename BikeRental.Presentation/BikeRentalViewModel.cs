using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BikeRental.Presentation {
    public class BikeRentalViewModel : INotifyPropertyChanged {
        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
        public ObservableCollection<Bike> Bikes { get; } = new ObservableCollection<Bike>();
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();

        public ApiClient ApiClient { get; private set; }

        public string CustomerFilter { get; set; }

        private string status;
        public string Status {
            get => status; private set {
                status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        public bool IncludeNonAvailableBikes { get; set; }
        public string BikesOrder { get; set; }
        public ObservableCollection<Rental> UnpaiedRentals { get; } = new ObservableCollection<Rental>();

        public BikeRentalViewModel(ApiClient apiClient) {
            ApiClient = apiClient;

            Bikes.CollectionChanged += Bikes_CollectionChangedAsync;
            Customers.CollectionChanged += Customers_CollectionChangedAsync;
        }

        private async void Customers_CollectionChangedAsync(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null)
                foreach (var c in e.OldItems) {
                    await DeleteCustomerAsync((Customer)c);
                }
        }

        private async void Bikes_CollectionChangedAsync(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null)
                foreach (var c in e.OldItems) {
                    await DeleteBikeAsync((Bike)c);
                }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task RefreshCustomersAsync() {
            Customers.Clear();
            Customers.AddRange(await ApiClient.CustomersGetAsync(CustomerFilter));
        }

        public async Task RefreshBikesAsync() {
            Bikes.Clear();
            Bikes.AddRange(await ApiClient.BikesGetAsync(IncludeNonAvailableBikes, BikesOrder));
        }
        public async Task RefreshCategoriesAsync() {
            Categories.Clear();
            Categories.AddRange(await ApiClient.CategoriesGetAsync());
        }


        public async Task RefreshUnpaiedRentalsAsync() {
            UnpaiedRentals.Clear();
            UnpaiedRentals.AddRange(await ApiClient.RentalsGetAsync(true, null));
        }


        public async Task StoreCustomerAsync(Customer c) {
            if (c.Id == null) {
                try {
                    await ApiClient.CustomersPostAsync(c);
                    Status = "Added successfully";
                } catch (SwaggerException e) {
                    Status = "Error during add: " + e.Response;
                } catch (Exception e) {
                    Status = "Error during add: " + e.Message;
                }
            } else {
                try {
                    await ApiClient.CustomersPutAsync((int)c.Id, c);
                    Status = "Updated successfully";
                } catch (SwaggerException e) {
                    Status = "Error during update: " + e.Response;
                } catch (Exception e) {
                    Status = "Error during update: " + e.Message;
                }
            }
            await RefreshCustomersAsync();
        }

        public async Task StoreBikeAsync(Bike bike) {
            if (bike.Id == null) {
                try {
                    await ApiClient.BikesPostAsync(bike);
                    Status = "Added successfully";
                } catch (SwaggerException e) {
                    Status = "Error during add: " + e.Response;
                } catch (Exception e) {
                    Status = "Error during add: " + e.Message;
                }
            } else {
                try {
                    await ApiClient.BikesPutAsync((int)bike.Id, bike);
                    Status = "Updated successfully";
                } catch (SwaggerException e) {
                    Status = "Error during update: " + e.Response;
                } catch (Exception e) {
                    Status = "Error during update: " + e.Message;
                }
            }
            await RefreshBikesAsync();
        }

        public async Task DeleteBikeAsync(Bike b) {
            if (b.Id != null)
                try {
                    await ApiClient.BikesDeleteAsync((int)b.Id);
                    Status = "Deleted successfully";
                } catch (Exception e) {
                    Status = "Error during delete: " + e.Message;
                }
        }

        public async Task DeleteCustomerAsync(Customer c) {
            if (c.Id != null)
                try {
                    await ApiClient.CustomersDeleteAsync((int)c.Id);
                    Status = "Deleted successfully";
                } catch (Exception e) {
                    Status = "Error during delete: " + e.Message;
                }
        }


        public async Task RefreshAllAsync() {
            try {
                Status = ("starting refresh");
                await RefreshBikesAsync();
                Status = ("bikes loaded");
                await RefreshCategoriesAsync();
                Status = ("categories loaded");
                await RefreshCustomersAsync();
                Status = ("customers loaded");
                await RefreshUnpaiedRentalsAsync();
                Status = "unpaied rentals refreshed";
                Status = ("refresh successful");
            } catch (Exception e) {
                Status = "Error during refresh: " + e.Message;
            }

        }
    }
}
