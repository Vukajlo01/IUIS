using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetworkService.ViewModel
{
    public class MeasurementGraphViewModel : BindableBase
    {
        public ObservableCollection<CircleMarker> CircleMarkers { get; set; }

        public List<string> ComboBoxItems { get; set; } = Data.ComboBoxItemsData.entityTypes.Keys.ToList();

        public List<Entity> Entities { get; set; } = new List<Entity>();

        private Entity selectedEntity;

        public static bool ShowTooltips { get; set; } = true;

        public MyICommand OpenHelpTab { get; set; }
        public MyICommand ToggleTooltips { get; set; }
        public MyICommand ShowEntitiesCommand { get; set; }

        private string errorMessage;

        public MeasurementGraphViewModel()
        {
            ShowEntitiesCommand = new MyICommand(OnShow);
            ToggleTooltips = new MyICommand(OnToggleTooltips);

            CircleMarkers = new ObservableCollection<CircleMarker>();
            for (int i = 0; i <= 4; i++)
            {
                CircleMarker marker = new CircleMarker();
                CircleMarkers.Add(marker);
            }

            if (Entities.Count > 0)
            {
                SelectedEntity = Entities.First();
                SetValuesToCircleMarkers(LoadLastFiveUpdates());
            }
        }

        public void OnToggleTooltips()
        {
            bool newState = !ShowTooltips;
            ShowTooltips = newState;
            OnPropertyChanged("ShowTooltips");
        }

       

        private List<CircleMarker> LoadLastFiveUpdates()
        {
            if (!File.Exists("Log.txt"))
            {
                ErrorMessage = "Log file doesn't exist.";
                return null;
            }

            string[] lines = File.ReadAllLines("Log.txt");

            List<CircleMarker> lastFiveUpdates = new List<CircleMarker>();

            for (int i = lines.Count() - 1; i >= 0; i--)
            {
                string line = lines[i];

                string date = line.Substring(0, line.IndexOf(" "));
                line = line.Substring(line.IndexOf(" ") + 1);
                string time = line.Substring(0, line.IndexOf(" ") - 1);
                line = line.Substring(line.IndexOf(" ") + 1);
                int id = int.Parse(line.Substring(0, line.IndexOf(',')));
                string val = line.Substring(line.IndexOf(',') + 2);

                if ((id == SelectedEntity.Id) && (lastFiveUpdates.Count < 5))
                {
                    lastFiveUpdates.Add(new CircleMarker(id, int.Parse(val), date, time));
                }
            }

            return (lastFiveUpdates.Count > 0) ? lastFiveUpdates : null;
        }

        private void SetDefaultValuesToCircleMarkers()
        {
            for (int i = 0; i <= 4; i++)
            {
                CircleMarkers[i].CmId = 0;
                CircleMarkers[i].CmValue = 0;
                CircleMarkers[i].CmDate = "Date";
                CircleMarkers[i].CmTime = "Time";
            }
        }

        private void SetValuesToCircleMarkers(List<CircleMarker> markers)
        {
            if (markers != null)
            {
                for (int i = 0; i < markers.Count; i++)
                {
                    CircleMarkers[i].CmId = markers[markers.Count - i - 1].CmId;
                    CircleMarkers[i].CmValue = markers[markers.Count - i - 1].CmValue;
                    CircleMarkers[i].CmDate = markers[markers.Count - i - 1].CmDate;
                    CircleMarkers[i].CmTime = markers[markers.Count - i - 1].CmTime;
                }
            }
            else
            {
                // Ako se u log fajlu nalazi manje od 5 promena vrednosti
                // Na kruzice se postavljaju default vrednosti
                SetDefaultValuesToCircleMarkers();
            }
        }

        public void OnShow()
        {
            if (SelectedEntity != null)
            {
                ErrorMessage = String.Empty;

                List<CircleMarker> markers = LoadLastFiveUpdates();

                SetValuesToCircleMarkers(markers);
            }
            else
            {
                errorMessage = "Select an Entity.";
            }
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public Entity SelectedEntity
        {
            get
            {
                return selectedEntity;
            }

            set
            {
                selectedEntity = value;
                OnPropertyChanged("SelectedEntity");
            }

        }
    }
}
