using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NetworkService.ViewModel
{
	public class NetworkEntitiesViewModel : BindableBase
	{
		public List<string> ComboBoxItems { get; set; } = Data.ComboBoxItemsData.entityTypes.Keys.ToList();

		public ObservableCollection<Entity> EntitiesToShow { get; set; }
		public ObservableCollection<Entity> Entities { get; set; }
		public ObservableCollection<Entity> FilteredEntities { get; set; }

		public MyICommand AddEntityCommand { get; set; }
		public MyICommand SelectedCriteriaCommand { get; set; }
		public MyICommand DeleteEntityCommand { get; set; }
		public MyICommand FilterEntityCommand { get; set; }
		public MyICommand ResetFilterCommand { get; set; }

		public MyICommand OpenTerminal { get; set; }

		// Unos novog entiteta
		private Entity currentEntity = new Entity();
		private EntityType currentEntityType = new EntityType();

		// Entitet selektovan u datagridu
		private Entity selectedEntity;

		// Filtriranje
		private bool nazivBindRadioButton;
		private bool tipBindRadioButton;
		private string selectedCriteria;
		public string SelectedCriteria {
			get { return selectedCriteria; }
			set {
				selectedCriteria = value;
				OnPropertyChanged("SelectedCriteria");
			}
		}

        public bool NazivBindRadioButton
        {
            get { return nazivBindRadioButton; }
            set
            {
                nazivBindRadioButton = value;
                OnPropertyChanged("SelectedCriteria");
            }
        }

        public bool TipBindRadioButton
        {
            get { return tipBindRadioButton; }
            set
            {
                tipBindRadioButton = value;
                OnPropertyChanged("SelectedCriteria");
            }
        }

        private int ZapreminskiCount = 0;
		private int ElektronskiCount = 0;
		private int TurbinskiCount = 0;

        // Prikaz broja entiteta po tipu
        /*
		private int iaRectangleWidth;
		private string iaPercentage;
		private int ibRectangleWidth;
		private string ibPercentage;
		*/
        public NetworkEntitiesViewModel()
		{
			Entities = new ObservableCollection<Entity>();
			EntitiesToShow = Entities;


			AddEntityCommand = new MyICommand(OnAdd);
			DeleteEntityCommand = new MyICommand(OnDelete, CanDelete);
			FilterEntityCommand = new MyICommand(OnFilter);
			ResetFilterCommand = new MyICommand(OnResetFilter);
		}

		public Entity SelectedEntity
		{
			get { return selectedEntity; }
			set
			{
				selectedEntity = value;
				DeleteEntityCommand.RaiseCanExecuteChanged();
			}
		}

		private void OnResetFilter()
		{
            NazivBindRadioButton = false;
            TipBindRadioButton = false;
            SelectedCriteria = string.Empty;
            EntitiesToShow = Entities;
			FilteredEntities = new ObservableCollection<Entity>();
			OnPropertyChanged("EntitiesToShow");
		}


		private void OnFilter()
		{
			FilteredEntities = new ObservableCollection<Entity>();
			foreach(var entity in Entities)
			{
                if (nazivBindRadioButton) {
					if (entity.Name.Contains(SelectedCriteria)) {
                        FilteredEntities.Add(entity);
					}
				}
                if (tipBindRadioButton)
                {
                    if (entity.Type.Name.Contains(SelectedCriteria))
                    {
                        FilteredEntities.Add(entity);
                    }
                }
			}

			EntitiesToShow = FilteredEntities;
            OnPropertyChanged("EntitiesToShow");
        }


        private bool CanDelete()
        {
        	return SelectedEntity != null;
        }

        private void OnDelete()
        {
            switch (SelectedEntity.Type.Name)
            {
                case "Zapreminski":
                    ZapreminskiCount--;
                    MessageBox.Show("Entity:" + SelectedEntity.Name + " deleted successfully");
                    break;
                case "Elektronski":
                    ElektronskiCount--;
                    MessageBox.Show("Entity:" + SelectedEntity.Name + " deleted successfully");
                    break;
                case "Turbinski":
                    TurbinskiCount--;
                    MessageBox.Show("Entity:" + SelectedEntity.Name + " deleted successfully");
                    break;
            }

            Entities.Remove(SelectedEntity);

        }

        public Entity CurrentEntity
		{
			get { return currentEntity; }
			set
			{
				currentEntity = value;
				OnPropertyChanged("CurrentEntity");
			}
		}

		public EntityType CurrentEntityType
		{
			get { return currentEntityType; }
			set
			{
				currentEntityType = value;
				OnPropertyChanged("CurrentEntityType");
			}
		}

		public void OnAdd()
		{
			try
			{
				int parsedId;
				bool canParse = int.TryParse(CurrentEntity.TextId, out parsedId);
				bool idAlreadyExists = false;

				if (canParse)
				{
					foreach (Entity entity in Entities)
					{
						if (entity.Id == parsedId)
						{
							idAlreadyExists = true;
							break;
						}
					}
				}

				CurrentEntity.DoesIdAlreadyExist = idAlreadyExists;

				CurrentEntity.Validate();
				CurrentEntityType.Validate();

				if (CurrentEntity.IsValid)
				{
					Entities.Add(new Entity()
					{
						Id = int.Parse(CurrentEntity.TextId),
						Name = CurrentEntity.Name,
						Type = new EntityType
						{
							Name = CurrentEntityType.Name,
							ImgSrc = CurrentEntityType.ImgSrc
						},
						Value = 0
					});

                    switch (CurrentEntityType.Name)
                    {
                        case "Zapreminski":
                            ZapreminskiCount++;
                            break;
                        case "Elektronski":
                            ElektronskiCount++;
                            break;
                        case "Turbinski":
                            TurbinskiCount++;
                            break;
                    }
                    CurrentEntity.TextId = String.Empty;
					CurrentEntity.Name = String.Empty;
					MessageBox.Show("New entity added successfully", "New entity", MessageBoxButton.OK);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{DateTime.Now} - {ex.Message}");
			}
		}
	}
}
