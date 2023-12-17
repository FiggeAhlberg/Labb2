
using System.Windows;
using System.Windows.Controls;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labb2.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        public Store? SelectedStore { get; set; }

        public Stock? SelectedBookStore { get; set; }

        public Book? SelectedBookStock { get; set; }

        public StockView()
        {
           
            InitializeComponent();
            LoadBooks();

            using var db = new BookstoreContext();

            var stores = db.Stores.ToList();

            foreach (var store in stores)
            {
                StoreCmb.Items.Add(store);
            }
            

        }

        public void LoadBooks()
        {
            BookListView.Items.Clear();
            using var db = new BookstoreContext();
            var loadBooks = db.Books.ToList();
            //BookListView.ItemsSource = loadBooks;
            foreach (var book in loadBooks)
            {
                BookListView.Items.Add(book);
            }

        }

        public void LoadBooksByStore()
        {

            Store1ListView.Items.Clear();
            using var db = new BookstoreContext();
            var stocksForStore = db.Stocks
                .Include(s => s.Store)
                .Include(s => s.Isbn13Navigation)
                .Where(s => s.StoreId == SelectedStore.Id);


            foreach (var book in stocksForStore)
            {
                Store1ListView.Items.Add(book);
            }

        }


        private void StoreCmb_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStore = StoreCmb.SelectedItem as Store;
            LoadBooksByStore();

            //if (SelectedStore != null)
            //{
            //    Console.WriteLine($"SelectedStore: {SelectedStore.StoreName}");
            //    LoadBooksByStore();
            //}
            //else
            //{
            //    // Log or debug the issue
            //    Console.WriteLine("SelectedStore is null");
            //}

            //Store1ListView.Items.Clear();

            //if (StoreCmb.SelectedItem is Store selectedStore)
            //{
            //    SelectedStore = selectedStore;
            //    LoadBooksByStore();
            //}
            //else
            //{
            //    SelectedStore = null;
            //    Store1ListView.Items.Clear(); 
            //}
        }

        private void BookListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedBookStock = BookListView.SelectedItem as Book;
        }

        private void RemoveBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();
            SelectedBookStore = Store1ListView.SelectedItem as Stock;

            if (SelectedBookStore != null)
            {
                db.Stocks.Remove(SelectedBookStore);
                db.SaveChanges();
                LoadBooksByStore();
            }
        }

        private void AddBookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new BookstoreContext();

            if (int.TryParse(NumberOfBooksTbx.Text, out var numberOfBooksTrue) && SelectedBookStock != null && SelectedStore != null)
            {
                var storeById = db.Stocks
                    .Where(s => s.StoreId == SelectedStore.Id);

                if (storeById.Any(s => s.Isbn13 == SelectedBookStock.Isbn13))
                {
                    var bookExists = db.Stocks.FirstOrDefault(b =>
                        b.StoreId == SelectedStore.Id && b.Isbn13 == SelectedBookStock.Isbn13);
                    if (bookExists != null)
                    {
                        bookExists.Balance += numberOfBooksTrue;
                        db.SaveChanges();
                        LoadBooksByStore();
                    }
                }
                else
                {
                    db.Stocks.Add
                    (
                        new Stock()
                        {
                            Isbn13 = SelectedBookStock.Isbn13,
                            StoreId = SelectedStore.Id,
                            Balance = int.Parse(NumberOfBooksTbx.Text)
                        }
                    );
                    db.SaveChanges();
                    LoadBooksByStore();
                }
            }
        }
    }
}
