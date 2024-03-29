﻿namespace Bible.ViewModels
{
    using Helpers;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SearchByKeywordViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private Bible bible;
        private Book bookName;
        private string keyWordParameter;
        private bool isRunnning;        
        private ContentResponse_ contentResponse_;
        private ObservableCollection<Verse> verses;
        private BookResponse bookResponse;
        private ObservableCollection<BookItemViewModel> books;
        #endregion

        #region Properties
        public string KeyWordParameter
        {
            get { return this.keyWordParameter; }
            set { SetValue(ref this.keyWordParameter, value); }
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
        public ICommand SearchByWordCommand
        {
            get
            {
                return new RelayCommand(LoadVersesFoundByWord);
            }
        }       


        #endregion

        #region Constructor
        public SearchByKeywordViewModel(Bible bible)
        {            
            this.Verses = new ObservableCollection<Verse>();
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

        private async void LoadVersesFoundByWord()
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

            if(string.IsNullOrEmpty(this.KeyWordParameter))
            {
                this.IsRunnning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RequiredKeyword,
                    Languages.Accept);
                return;

            }
            
            try
            {
                var apiBibles = Application.Current.Resources["APIbibles"].ToString();
                var response = await this.apiService.Get<ContentResponse_>(
                        apiBibles,
                        "/api",
                        string.Format(
                        "?bible={0}&reference={1}&search={2}",
                        MainViewModel.GetInstance().SelectedModule,
                        this.SelectedItemBook.Shortname,
                        this.KeyWordParameter));

                if (!response.IsSuccess)
                {
                    this.IsRunnning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Accept
                        );
                    return;
                }

                this.contentResponse_ = (ContentResponse_)response.Result;
                this.IsRunnning = false;

                for (int i = 0; i < this.contentResponse_.Contents.Count(); i++)
                {
                    var contentResult = contentResponse_.Contents[i];

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
                                     

                     this.Verses.Add(new ObservableCollection<Verse>
                         (this.ToVersesViewModel(chapter))[0]);
                }

            }
            catch
            {
                var apiBibles = Application.Current.Resources["APIbibles"].ToString();
                var response = await this.apiService.Get<ContentResponse_>(
                             apiBibles,
                             "/api",
                             string.Format(
                                 "?bible={0}&search={1}",
                                 MainViewModel.GetInstance().SelectedModule,
                                 this.KeyWordParameter));

                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Accept
                        );
                    return;
                }

                this.contentResponse_ = (ContentResponse_)response.Result;
                this.IsRunnning = false;

                for (int i = 0; i < this.contentResponse_.Contents.Count(); i++)
                {
                    var contentResult = contentResponse_.Contents[i];

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

                    this.Verses.Add(new ObservableCollection<Verse>
                         (this.ToVersesViewModel(chapter))[0]);
                }
            }  
        }

        private IEnumerable<Verse> ToVersesViewModel(Dictionary<string, Verse> chapter)
        {
            return chapter.Select(v => new Verse
            {
                Book = v.Value.Book,
                Chapter = v.Value.Chapter,
                Id = v.Value.Id,
                Italics = v.Value.Italics,
                Text = v.Value.Text,
                VerseNumber = v.Value.VerseNumber,
            });
        }
        #endregion
    }
}