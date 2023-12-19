using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Author? SelectedAuthor { get; set; }

        public Author? SelectedAuthorListView { get; set; }

        public Book? SelectedBook { get; set; }


        public AuthorBookView()
        {
            InitializeComponent();

            using var db = new BookstoreContext();

            LoadBooks();
            LoadAuthors();
            LoadPublisher();
        }

        public void LoadBooks()
        {
            EditBooksListView.Items.Clear();
            var db = new BookstoreContext();
            var loadBooks = db.Books.Include(b=> b.Authors).ToList();
            foreach (var book in loadBooks)
            {
                EditBooksListView.Items.Add(book);
            }

        }

        public void LoadAuthors()
        {
            EditAuthorListView.Items.Clear();
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
                if (PublisherCmb.SelectedItem is Publisher publisherItem)
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

            if (SelectedAuthor == null)
            {
                AuthorFirstNameTbx.Text = null;
                AuthorLastNameTbx.Text = null;
                AuthorBirthDate.Text = null;
            }
            else
            {
                AuthorFirstNameTbx.Text = SelectedAuthor.FirstName;
                AuthorLastNameTbx.Text = SelectedAuthor.LastName;
                AuthorBirthDate.Text = SelectedAuthor.BirthDate.ToString();
            }
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

                if (db.Books.Any(b => b.Isbn13 == BookIdTbx.Text))
                {
                    MessageBox.Show("Book with the same ISBN13 already exists");
                }
                else
                {
                    if (AuthorListView.Items.Count > 0 && PublisherCmb.SelectedItem != null)
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

                        BookIdTbx.Text = null;
                        BookTitleTbx.Text = null;
                        BookLanguageTbx.Text = null;
                        BookPriceTbx.Text = null;
                        BookReleaseDate.Text = null;
                        AuthorListView.ItemsSource = null;
                        AuthorListView.Items.Clear();
                    }
                    else
                    {
                        MessageBox.Show("You need to add both Author and Publisher");
                    }
                }
                
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
                        Id = db.Authors.Count() + 1,
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

                var authorToUpdate = db.Authors.Find(SelectedAuthor.Id);
                if (authorToUpdate != null && DateOnly.TryParse(AuthorBirthDate.Text, out var isAuthorBirthDate))
                {

                    authorToUpdate.FirstName = AuthorFirstNameTbx.Text;
                    authorToUpdate.LastName = AuthorLastNameTbx.Text;
                    authorToUpdate.BirthDate = isAuthorBirthDate;

                }
            }
            else
            {
                MessageBox.Show("Author is not selected");
            }
            db.SaveChanges();
            LoadAuthors();

        }

        private void UpdateBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedBook = EditBooksListView.SelectedItem as Book;
            var newAuthors = GetAuthor(db);
            var findPublisher = GetPublisher(db);

            if (SelectedBook != null)
            {
                var bookToUpdate = db.Books
                    .Include(b => b.Authors)
                    .FirstOrDefault(b => b.Isbn13 == SelectedBook.Isbn13);

                if (bookToUpdate != null && DateOnly.TryParse(BookReleaseDate.Text, out var isBookReleaseDate) && int.TryParse(BookPriceTbx.Text, out var isBookPrice))
                {
                    bookToUpdate.Title = BookTitleTbx.Text;
                    bookToUpdate.Language = BookLanguageTbx.Text;
                    bookToUpdate.Price = isBookPrice;
                    bookToUpdate.ReleaseDate = isBookReleaseDate;
                    bookToUpdate.Publishers = findPublisher;

                     
                    bookToUpdate.Authors.Clear(); 
                    foreach (var newAuthor in newAuthors)
                    {
                        bookToUpdate.Authors.Add(newAuthor); 
                    }

                    db.SaveChanges();
                    LoadBooks();

                   
                }
                else
                {
                    MessageBox.Show("Book is not Selected");
                }
               
            }
            else
            {
                MessageBox.Show("No book is selected");

            }
        }

        private void EditBooksListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedBook = EditBooksListView.SelectedItem as Book;

            if (SelectedBook == null)
            {
                BookIdTbx.Text = null;
                BookTitleTbx.Text = null;
                BookLanguageTbx.Text = null;
                BookPriceTbx.Text = null;
                BookReleaseDate.Text = null;
                AuthorListView.ItemsSource = null;
                AuthorListView.Items.Clear();
            }
            else
            {
                BookIdTbx.Text = SelectedBook.Isbn13;
                BookTitleTbx.Text = SelectedBook.Title;
                BookLanguageTbx.Text = SelectedBook.Language;
                BookPriceTbx.Text = SelectedBook.Price.ToString();
                BookReleaseDate.Text = SelectedBook.ReleaseDate.ToString();

                //Publisher
                var publisherName = db.Books
                    .Include(b => b.Publishers)
                    .Where(b => b.Isbn13 == SelectedBook.Isbn13)
                    .SelectMany(b => b.Publishers) // Flatten the Publishers collection
                    .Select(p => p.Name)
                    .FirstOrDefault();

                PublisherCmb.Text = publisherName;

                //Author
                var isbn13 = SelectedBook.Isbn13;
                var authorsForBook = db.Books
                    .Include(b => b.Authors)
                    .FirstOrDefault(b => b.Isbn13 == isbn13)?
                    .Authors
                    .ToList();

                AuthorListView.Items.Clear();
                if (authorsForBook != null)
                {
                    foreach (var author in authorsForBook)
                    {
                        AuthorListView.Items.Add(author);
                    }
                }

            }
        }

        private void RemoveBookStockBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedBook = EditBooksListView.SelectedItem as Book;


            if (SelectedBook != null)
            {
               
                SelectedBook = db.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Publishers)
                    .FirstOrDefault(b => b.Isbn13 == SelectedBook.Isbn13);

                if (SelectedBook != null)
                {
                    foreach (var author in SelectedBook.Authors.ToList())
                    {
                        author.Books.Remove(SelectedBook);
                    }

                    foreach (var publisher in SelectedBook.Publishers.ToList())
                    {
                        publisher.Books.Remove(SelectedBook);
                    }

                    
                    db.SaveChanges();
                    db.Books.Remove(SelectedBook);
                    db.SaveChanges();
                    SelectedBook = null;
                    LoadBooks();
                }
            }
            else
            {
                MessageBox.Show("No book is selected");

            }
            
        }

        private void RemoveAuthorStockBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedAuthor = EditAuthorListView.SelectedItem as Author;

            if (SelectedAuthor != null)
            {
                db.Authors.Remove(SelectedAuthor);
                db.SaveChanges();
                LoadAuthors();
            }
            else
            {
                MessageBox.Show("No author is selected");
            }
        }

        private void AuthorListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedAuthorListView = AuthorListView.SelectedItem as Author;

            
        }

        private void AuthorToBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (SelectedAuthor != null)
            {
                AuthorListView.Items.Add(SelectedAuthor);
            }
            else
            {
                MessageBox.Show("No Author Selected");
            }
           
        }

        private void DeleteAuthorBook_OnClick(object sender, RoutedEventArgs e)
        {

            var removeAuthor = AuthorListView.SelectedItem;

            if (AuthorListView.SelectedItem != null)
            {
                AuthorListView.Items.Remove(removeAuthor);
            }
            else
            {
                MessageBox.Show($"Select an author");
            }

        }
    }
}
