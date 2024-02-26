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
using WpfAppDemo.AdoModel;
using WpfAppDemo.Classes;

namespace WpfAppDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DefaultOrder = "По умолчанию";
        private const string AscendingOrder = "От А до Я";
        private const string DescendingOrder = "От Я до А";

        private const string FilterKey = "filter";
        private const string SearchKey = "search";
        private const string SortKey = "sort";

        private const string SearchPlaceholder = "Введите для поиска";

        private List<ProductListModel> _products = new List<ProductListModel>();
        private List<ProductListModel> _paginatedProducts = new List<ProductListModel>();

        private Dictionary<string, Action> _listModifications = new Dictionary<string, Action>();

        public MainWindow()
        {
            InitializeComponent();

           // LvProducts.ItemsSource = App.Connection.Product;

            SortComboBox.ItemsSource = new[]
            {
                DefaultOrder,
                AscendingOrder,
                DescendingOrder
            };

            SortComboBox.SelectedItem = DefaultOrder;

            List<ProductType> types = new List<ProductType>();
            
            ProductType UniversalFilter = new ProductType
            {
                Name = "Все"
            };

            types.Add(UniversalFilter);

            foreach (var type in App.Connection.ProductType)
            {
                types.Add(type);
            }
            

            FilterComboBox.ItemsSource = types;

            FilterComboBox.SelectedItem = UniversalFilter;

            LvProducts.ItemsSource = _paginatedProducts;
        }

        private void Tb_Search_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTb = sender as TextBox;
            string text = currentTb.Text;

            _listModifications.Remove(SearchKey);
            if (!String.IsNullOrEmpty(text) && text != SearchPlaceholder)
            {
                _listModifications.Add(SearchKey, () => LvProducts.ItemsSource = ListModifications.Search(LvProducts.ItemsSource.Cast<ProductListModel>(), text));
            }

            ApplyModifications();
        }

        private void Tb_Search_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _listModifications.Remove(SearchKey);
            ApplyModifications();

            TextBox currentTb = sender as TextBox;
            string text = currentTb.Text;

            if (String.IsNullOrEmpty(text))
            {
                SearchTextBox.Text = SearchPlaceholder;
            }
        }

        private void OnSortingChanged(object sender, SelectionChangedEventArgs e)
        {
            string order = (sender as ComboBox).SelectedItem as string;

            _listModifications.Remove(SortKey);

            if (order != DefaultOrder)
            {
                _listModifications.Add(SortKey, () => LvProducts.ItemsSource = ListModifications.Order(LvProducts.ItemsSource.Cast<ProductListModel>(), order));
            }

            ApplyModifications();
        }

        private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            var filter = (sender as ComboBox).SelectedItem as string;

            _listModifications.Remove(FilterKey);

            if (filter != "Все")
            {
                _listModifications.Add(FilterKey, () => LvProducts.ItemsSource = ListModifications.FilterByType(LvProducts.ItemsSource.Cast<ProductListModel>(), filter));
            }

            ApplyModifications();
        }

        private void ApplyModifications()
        {
            if (LvProducts is null)
            {
                return;
            }

            LvProducts.ItemsSource = _paginatedProducts;

            foreach (var key in _listModifications.Keys)
            {
                _listModifications[key]();
            }

            LvProducts.Items.Refresh();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
        }
    }
}
