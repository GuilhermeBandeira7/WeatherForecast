using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged //Makes the connection with the view through binding
    {
        //query property will call the method to trigger the event OnPropertyChanged
        //a text box from the view as instance can subscribe to this property and upadate the values of the view and model
        private string query;

        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;
                OnPropertyChanged("Query");                
            }
        }

        public ObservableCollection<City> Cities { get; set; }

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set
            {
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
                
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set
            { 
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetCurrentConditions();
            }
        }

        //Property that encapsulates the logic of the Search Button in the UI
        public SearchCommand SearchCommand { get; set; }

        public WeatherVM()
        {
            //checking if the application is running.
            if(DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                selectedCity = new City()
                {
                    LocalizedName = "Paris"
                };

                currentConditions = new CurrentConditions()
                {
                    WeatherText = "Partialy Cloudy",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = 21
                        }
                    }
                };
            }

            //The propertie must be initialized and needs this class as argument 
            SearchCommand = new SearchCommand(this);

            //Cities propertie contains will hold list of cities that updates insertions and deletions within the list
            Cities = new ObservableCollection<City>();
        }

        /// <summary>
        /// Will be executed everytime a selectedCity is changed
        /// </summary>
        private async void GetCurrentConditions()
        {
            Query = string.Empty;
            Cities.Clear();
            CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
        }

        /// <summary>
        /// Get a list of cities from the API
        /// </summary>
        public async void MakeQuery()
        {
            //Query is the prop. containing the text from the TextBox of the UI
            var cities = await AccuWeatherHelper.GetCities(Query);

            Cities.Clear(); 
            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }

        //Implementation of INotifyPropertieChanged that triggers and event when the view or the modelview properties is changed
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invoke PropertyChangedEventHandler and a property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
