using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using winetranet_application.Core;
using winetranet_application.Model;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
    class SiteViewModel : ObservableObject
    {

        private ObservableCollection<Model.Site> _sites;
        public ObservableCollection<Model.Site> Sites
        {
            get { return _sites; }
            set { _sites = value; OnPropertyChanged(nameof(Sites)); }
        }
        public ICollectionView SitesView
        {
            get
            {
                var SitesView = CollectionViewSource.GetDefaultView(Sites);
                SitesView.Filter = new Predicate<object>(o => Filter(o as Model.Site));
                return SitesView;
            }
        }
        private bool Filter(Model.Site site)
        {
            return Search == null
                || site.Ville.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private string search;

        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                OnPropertyChanged("Search");
                SitesView.Refresh();
            }
        }
        public RelayCommand CreateSiteViewCommand { get; set; }
        public RelayCommand EditSiteViewCommand { get; set; }
        public RelayCommand DeleteSiteViewCommand { get; set; }

        public SiteViewModel()
        {
            Sites = GetSites();

            CreateSiteViewCommand = new RelayCommand(o =>
            {
                ModalWindow modal = new ModalWindow();
                modal.Content = new CreateSiteView();
                modal.DataContext = new CreateSiteViewModel();
                modal.ShowDialog();
                if (!modal.IsVisible)
                {
                    Sites = GetSites();
                    SitesView.Refresh();
                }
            });

            EditSiteViewCommand = new RelayCommand(o =>
            {
                ModalWindow modal = new ModalWindow();
                modal.Content = new CreateSiteView();
                modal.DataContext = new CreateSiteViewModel((Model.Site)o);
                modal.ShowDialog();
                if (!modal.IsVisible)
                {
                    Sites = GetSites();
                    SitesView.Refresh();
                }
            });

            DeleteSiteViewCommand = new RelayCommand(o =>
            {
                DeleteSite((Model.Site)o);
                Sites = GetSites();
            });
        }

        private void DeleteSite(Model.Site o)
        {
            using (var context = new WinetranetContext())
            {
                var Users = context.Users.Where(users=> users.Site == o.Id);
                if (!Users.Any())
                {

                    context.Sites.Remove(o);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Some users are affected to this Town");
                }
                
                
            }
        }

        public static ObservableCollection<Model.Site> GetSites()
        {
            using (var context = new WinetranetContext())
            {
                List<Model.Site> sites = context.Sites.ToList();

                if (sites != null)
                {
                    return new ObservableCollection<Model.Site>(sites);
                }
                else
                {
                    return new ObservableCollection<Model.Site>(new List<Model.Site> { });
                }
            }
        }
    }
}
