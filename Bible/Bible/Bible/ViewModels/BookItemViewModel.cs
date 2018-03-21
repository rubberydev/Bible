namespace Bible.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Views;
    using Xamarin.Forms;

    public class BookItemViewModel : Book
    {
        #region Commands
        public ICommand SelectBookCommand
        {
            get
            {
                return new RelayCommand(SelectBook);
            }
        }
        #endregion


        #region Methods

        private async void SelectBook()
        {
            MainViewModel.GetInstance().Book = new BookViewModel(this);
            await App.Navigator.PushAsync(new BookPage());
            
        }
        #endregion
    }
}
