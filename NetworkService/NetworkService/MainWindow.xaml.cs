using NetworkService.Model;
using NetworkService.ViewModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ((MainWindowViewModel)this.DataContext).networkEntitiesViewModel.Entities.CollectionChanged += OnCollectionChanged1;
            Term.Entities.CollectionChanged += OnCollectionChanged2;
        }

        private void OnCollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Entity newEntity in e.NewItems)
                {
                    if (!Term.Entities.Contains(newEntity))
                    {
                        Term.Entities.Add(newEntity);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Entity oldEntity in e.OldItems)
                {
                    if (Term.Entities.Contains(oldEntity))
                    {
                        Term.Entities.Remove(oldEntity);
                    }
                }
            }
        }

        private void OnCollectionChanged2(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Entity newEntity in e.NewItems)
                {
                    if (!((MainWindowViewModel)this.DataContext).networkEntitiesViewModel.Entities.Contains(newEntity))
                    {
                        ((MainWindowViewModel)this.DataContext).networkEntitiesViewModel.Entities.Add(newEntity);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Entity oldEntity in e.OldItems)
                {
                    if (((MainWindowViewModel)this.DataContext).networkEntitiesViewModel.Entities.Contains(oldEntity))
                    {
                        ((MainWindowViewModel)this.DataContext).networkEntitiesViewModel.Entities.Remove(oldEntity);
                    }
                }
            }
        }
    }
}
