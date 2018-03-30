namespace Bible.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Views;
    using Xamarin.Forms;

    public class BibleItemViewModel : Bible
    {
        #region Commands
        public ICommand SelectBibleCommand
        {
            get
            {
                return new RelayCommand(SelectBible);
            }
        }

        public ICommand AdvancedSearchCommand
        {
            get
            {
                return new RelayCommand(LoadBooks);
            }
        }

        public ICommand SearchByKeyWordCommand
        {
            get
            {
                return new RelayCommand(LoadBooks_);
            }
        }

        #endregion

        #region Methods
        private async void SelectBible()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Bible = new BibleViewModel(this);
            mainViewModel.SelectedModule = Module;
            await App.Navigator.PushAsync(new BiblePage());
            
        }

        private async void LoadBooks()
        {
            var mainViewModel = MainViewModel.GetInstance();            
            mainViewModel.Search = new SearchAdvancedViewModel(this);
            mainViewModel.SelectedModule = Module;
            await App.Navigator.PushAsync(new SearchAdvancedPage());
            
        }

        private async void LoadBooks_()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Search_ = new SearchByKeywordViewModel(this);
            mainViewModel.SelectedModule = Module;
            await App.Navigator.PushAsync(new SearchByKeywordPage());

        }
        #endregion
    }
}