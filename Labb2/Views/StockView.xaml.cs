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
using DataAccess;

namespace Labb2.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        public StockView()
        {
            InitializeComponent();
            LoadBooks();

        }

        public void LoadBooks()
        {
            using var db = new BookstoreContext();

            var loadBooks = db.Books.ToList();

            foreach (var book in loadBooks)
            {
                Store1ListView.Items.Add(book.Title);
            }
        }
    }
}
