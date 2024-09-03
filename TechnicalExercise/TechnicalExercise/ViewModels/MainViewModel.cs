using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TechnicalExercise.Helpers;
using TechnicalExercise.Models;
using TechnicalExercise.Services;

namespace TechnicalExercise.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private readonly IAirportDatabase _airportDatabase;
        private ObservableCollection<AirportModel> _airports;
        private ObservableCollection<AirportModel> _selectedAirports;
        private AirportModel _selectedAirportLeft;
        private AirportModel _selectedAirportRight;
        private ObservableCollection<ColorModel> _colors;
        private ColorModel _selectedColor;
        private ObservableCollection<AirportModel> _allAirports;
        private string _filterText;

        public ObservableCollection<AirportModel> Airports
        {
            get { return _airports; }
            set { _airports = value; OnPropertyChanged(); }
        }

        public ObservableCollection<AirportModel> SelectedAirports
        {
            get { return _selectedAirports; }
            set { _selectedAirports = value; OnPropertyChanged(); }
        }

        public AirportModel SelectedAirportLeft
        {
            get => _selectedAirportLeft;
            set { _selectedAirportLeft = value; OnPropertyChanged(); }
        }

        public AirportModel SelectedAirportRight
        {
            get => _selectedAirportRight;
            set { _selectedAirportRight = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ColorModel> Colors
        {
            get { return _colors; }
            set { _colors = value; OnPropertyChanged(); }
        }

        public ColorModel SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; OnPropertyChanged(); }
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged();
                    ApplyFilter();  // Automatically apply the filter whenever the text changes
                }
            }
        }

        public MainViewModel(IAirportDatabase airportDatabase)
        {
            _airportDatabase = airportDatabase;
            _selectedAirports = new ObservableCollection<AirportModel>();
            _colors = new ObservableCollection<ColorModel>(ColorHelper.GetKnownColors());
            _selectedColor = _colors[1];
            LoadAirports("GB");
        }

        private async void LoadAirports(string countryCode)
        {
            var airports = await _airportDatabase.GetByCountry(countryCode);
            AllAirports = new ObservableCollection<AirportModel>(airports);  // Unfiltered list
            Airports = new ObservableCollection<AirportModel>(AllAirports);  // Display list

            AttachPropertyChangedHandlers(Airports);
        }

        // Master list of all airports (unfiltered)
        public ObservableCollection<AirportModel> AllAirports
        {
            get { return _allAirports; }
            set { _allAirports = value; OnPropertyChanged(); }
        }

        // Handle when an airport's IsSelected property changes
        private void OnAirportSelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AirportModel.IsSelected))
            {
                var airport = sender as AirportModel;
                if (airport.IsSelected)
                {
                    // Add to SelectedAirports if it is selected and not already added
                    if (!SelectedAirports.Contains(airport))
                    {
                        SelectedAirports.Add(airport);
                    }
                }
                else
                {
                    // Remove from SelectedAirports if it is unselected
                    if (SelectedAirports.Contains(airport))
                    {
                        SelectedAirports.Remove(airport);
                    }
                }
            }
        }

        // Filtering logic based on name
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                // Reset the list to all airports if filter is cleared
                Airports = new ObservableCollection<AirportModel>(AllAirports);
            }
            else
            {
                // Apply filter to the master list (AllAirports)
                Airports = new ObservableCollection<AirportModel>(
                    AllAirports.Where(a => a.name.Contains(FilterText, System.StringComparison.InvariantCultureIgnoreCase)));
            }

            AttachPropertyChangedHandlers(Airports);
        }

        // Attach PropertyChanged event handlers to each airport in the Airports collection
        private void AttachPropertyChangedHandlers(ObservableCollection<AirportModel> airports)
        {
            foreach (var airport in airports)
            {
                // To prevent multiple attachments, first detach the handler
                airport.PropertyChanged -= OnAirportSelectionChanged;
                airport.PropertyChanged += OnAirportSelectionChanged;

                // If an item was already selected before filtering, reflect that in the selection
                if (SelectedAirports.Contains(airport))
                {
                    airport.IsSelected = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
