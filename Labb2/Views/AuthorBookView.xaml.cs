using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labb2.Views
{
    /// <summary>
    /// Interaction logic for AuthorBookView.xaml
    /// </summary>
    public partial class AuthorBookView : UserControl
    {
        public Author SelectedAuthor {get; set; }

        public AuthorBookView()
        {
            InitializeComponent();
            LoadBooks();
            LoadAuthors();
            LoadPublisher();
        }

        public void LoadBooks()
        {
            EditBooksListView.Items.Clear();
            var db = new BookstoreContext();
            var loadBooks = db.Books.ToList();
            foreach (var book in loadBooks)
            {
                EditBooksListView.Items.Add(book);
            }

        }

        public void LoadAuthors()
        {
            var db = new BookstoreContext();
            var loadAuthors = db.Authors.ToList();
            foreach (var author in loadAuthors)
            {
                EditAuthorListView.Items.Add(author);
            }

        }

        public void LoadPublisher()
        {

            using var db = new BookstoreContext();

            var publishers = db.Publishers.ToList();

            foreach (var publisher in publishers)
            {
                PublisherCmb.Items.Add(publisher);
            }
        }
        private List<Publisher> GetPublisher(BookstoreContext db)
        {

            var publisherNewBook = new List<Publisher>();


            foreach (var item in PublisherCmb.Items)
            {
                if (item is Publisher publisherItem)
                {
                    var existingPublisher = db.Publishers.Find(publisherItem.Id);
                    if (existingPublisher != null)
                    {
                      publisherNewBook.Add(existingPublisher); 
                    }
                } 
            }

            return publisherNewBook;
        }

        private void EditAuthorListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAuthor = EditAuthorListView.SelectedItem as Author;
        }

        private void AuthorToBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();

            AuthorListView.Items.Add(SelectedAuthor);
        }

        private List<Author> GetAuthor(BookstoreContext db)
        {
            var authorNewBook = new List<Author>();

            foreach (var item in AuthorListView.Items)
            {
                if (item is Author authorItem)
                {
                    var existingAuthor = db.Authors.Find(authorItem.Id);
                    if (existingAuthor != null)
                    {
                        authorNewBook.Add(existingAuthor);
                    }
                }
            }
            return authorNewBook;
        }

        private void AddStockBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();

            if (DateOnly.TryParse(BookReleaseDate.Text, out var isBookReleaseDate))
            {

                var authorNewBook = GetAuthor(db);
                var publisherNewBook = GetPublisher(db);
                db.Books.Add
                (
                    new Book()
                    {
                        Isbn13 = BookIdTbx.Text,
                        Title = BookTitleTbx.Text,
                        Language = BookLanguageTbx.Text,
                        Price = int.Parse(BookPriceTbx.Text),
                        Publishers = publisherNewBook,
                        ReleaseDate = isBookReleaseDate,
                        Authors = authorNewBook

                    }
                );


                db.SaveChanges();
                LoadBooks();

                
            }
        }

        private void AddAuthorBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (DateOnly.TryParse(AuthorBirthDate.Text, out var isAuthorBirthDate))
            {
                using var db = new BookstoreContext();
                db.Authors.Add
                (
                    new Author()
                    {
                        FirstName = AuthorFirstNameTbx.Text,
                        LastName = AuthorLastNameTbx.Text,
                        BirthDate = isAuthorBirthDate
                    }
                );
                db.SaveChanges();
                LoadAuthors();
            }
        }


        private void UpdateAuthorBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedAuthor = EditAuthorListView.SelectedItem as Author;

            if (SelectedAuthor != null)
            {

            }

        }

        private void UpdateBookBtn_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
