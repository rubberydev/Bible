﻿namespace Bible.ViewModels
{
    using Bible.Helpers;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SearchAdvancedViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private Bible bible;
        private Book bookName;
        private string versesParameter;
        private bool isRunnning;        
        private ContentResponse contentResponse;
        private ObservableCollection<Verse> verses;
        private BookResponse bookResponse;
        private ObservableCollection<BookItemViewModel> books;
        #endregion

        #region Properties
        public string VersesParameter
        {
            get { return this.versesParameter; }
            set { SetValue(ref this.versesParameter, value); }
        }

        public bool IsRunnning
        {
            get { return this.isRunnning; }
            set { SetValue(ref this.isRunnning, value); }
        }

        public ObservableCollection<Verse> Verses
        {
            get { return this.verses; }
            set { SetValue(ref this.verses, value); }
        }

        public Book SelectedItemBook
        {
            get { return this.bookName; }
            set { SetValue(ref this.bookName, value); }
        }

        public ObservableCollection<BookItemViewModel> Books
        {
            get { return this.books; }
            set { SetValue(ref this.books, value); }
        }
        #endregion


        #region Commands
        public ICommand SearchVersesCommand
        {
            get
            {
                return new RelayCommand(LoadVersesAndChapters);
            }
        } 
        #endregion

        #region Constructor
        public SearchAdvancedViewModel(Bible bible)
        {
            this.bible = bible;
            this.apiService = new ApiService();
            this.LoadBooks();
        } 
        #endregion

        #region Methods
        private async void LoadBooks()
        {         

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {               
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var apiBibles = Application.Current.Resources["APIbibles"].ToString();
            var response = await this.apiService.Get<BookResponse>(
                apiBibles,
                "/api",
                string.Format("/books?language={0}", bible.LangShort));

            if (!response.IsSuccess)
            {               
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            this.bookResponse = (BookResponse)response.Result;

            if (bible.LangShort != "en")
            {
                var response2 = await this.apiService.Get<BookResponse>(
                    apiBibles,
                    "/api",
                    "/books?language=en");

                if (!response2.IsSuccess)
                {                    
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response2.Message,
                        Languages.Accept);
                    return;
                }

                var booksResult2 = (BookResponse)response2.Result;

                for (int i = 0; i < this.bookResponse.Books.Count; i++)
                {
                    this.bookResponse.Books[i].Shortname = booksResult2.Books[i].Shortname;
                }
            }

            this.Books = new ObservableCollection<BookItemViewModel>(
                this.ToBookItemViewModel());           
        }

        private IEnumerable<BookItemViewModel> ToBookItemViewModel()
        {
            return this.bookResponse.Books.Select(b => new BookItemViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Shortname = b.Shortname,
            });
        }

        private async void LoadVersesAndChapters()
        {
                       
            this.IsRunnning = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunnning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            try
            {

                if(string.IsNullOrEmpty(this.VersesParameter))
                {
                    IsRunnning = false;

                    await Application.Current.MainPage.DisplayAlert(
                      Languages.Error,
                      Languages.ValidFormat,
                      Languages.Accept);
                    return;

                }

                if(RegexUtilities.Reg_exp(this.VersesParameter))
                {                 
                    IsRunnning = false;

                    await Application.Current.MainPage.DisplayAlert(
                      Languages.Error,
                      Languages.ValidVerses,
                      Languages.Accept);
                    return;
                }

                var apiBibles = Application.Current.Resources["APIbibles"].ToString();
                var response = await this.apiService.Get<ContentResponse>(
                               apiBibles,
                               "/api",
                               string.Format(
                                   "?bible={0}&reference={1}",
                                   MainViewModel.GetInstance().SelectedModule,
                                    this.SelectedItemBook.Name + " " + this.VersesParameter));

                if (!response.IsSuccess)
                {
                    this.IsRunnning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Accept);
                    return;
                }

                this.contentResponse = (ContentResponse)response.Result;
                this.IsRunnning = false;


                var contentResult = contentResponse.Contents[0];


                var type = typeof(Verses);

                var properties = type.GetRuntimeFields();
                Bible bible = null;

                foreach (var property in properties)
                {
                    bible = (Bible)property.GetValue(contentResult.Verses);
                    if (bible != null)
                    {
                        break;
                    }
                }

                if (bible == null)
                {
                    return;
                }

                type = typeof(Bible);
                properties = type.GetRuntimeFields();
                Dictionary<string, Verse> chapter = null;

                foreach (var property in properties)
                {
                    if (property.Name.StartsWith("<Chapter"))
                    {
                        chapter = (Dictionary<string, Verse>)property.GetValue(bible);

                        if (chapter != null)
                        {
                            break;
                        }
                    }
                }

                var myVerses = chapter.Select(v => new Verse
                {
                    Book = v.Value.Book,
                    Chapter = v.Value.Chapter,
                    Id = v.Value.Id,
                    Italics = v.Value.Italics,
                    Text = v.Value.Text,
                    VerseNumber = v.Value.VerseNumber,
                });

                this.Verses = new ObservableCollection<Verse>(myVerses);

            }
            catch (Exception)
            {

                this.IsRunnning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SelectBook,
                    Languages.Accept);
                return;
            }            
        }         
        #endregion
    }
}
