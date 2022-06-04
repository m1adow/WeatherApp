using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WeatherApp.Model;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private string? _query;

        public string? Query
        {
            get { return _query; }
            set
            {
                _query = value;
                OnPropertyChanged(nameof(Query));
            }
        }

        public ObservableCollection<City>? Cities { get; set; }

        private CurrentConditions? _currentConditions;

        public CurrentConditions? CurrentConditions
        {
            get { return _currentConditions; }
            set
            {
                _currentConditions = value;
                OnPropertyChanged(nameof(CurrentConditions));
            }
        }

        private City? _selectedCity;

        public City? SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
                GetCurrentCondition();
            }
        }

        public SearchCommand? SearchCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public WeatherViewModel()
        {
            /*if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                SelectedCity = new City()
                {
                    LocalizedName = "New York"
                };

                CurrentConditions = new CurrentConditions()
                {
                    WeatherText = "Partly cloudy",
                    Temperature = new Temperature()
                    {
                        Metric = new Units()
                        {
                            Value = 21
                        }
                    }
                };
            }*/

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }     

        private async void GetCurrentCondition()
        {
            Query = string.Empty;
            Cities?.Clear();

            CurrentConditions = await AccuWeather.GetCurrentConditions(SelectedCity.Key);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void MakeQuery()
        {
            var cities = await AccuWeather.GetCities(Query);

            Cities?.Clear();

            foreach (var city in cities)
                Cities?.Add(city);
        }
    }
}
