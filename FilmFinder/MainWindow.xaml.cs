using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace FilmFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MySql.Data.MySqlClient.MySqlConnection connection;
        private readonly string dbServer = "127.0.0.1";
        private readonly string dbPort = "3306";
        private readonly string dbName = "sakila";
        private readonly string dbUser = "sakila_app_user";
        private readonly string dbPassword = "Hunter2!";
        public MainWindow()
        {
            InitializeComponent();
            connection = new MySql.Data.MySqlClient.MySqlConnection();
            connection.ConnectionString = $"server={dbServer};port={dbPort};database={dbName};uid={dbUser};pwd={dbPassword}";

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorBox.Visibility = Visibility.Collapsed;
            string clause = SearchBar.Text;
            SearchBar.Text = "";

            string query = $"select * from film where description like '%{clause}%'";
            FilmResults.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    FilmResults.Items.Add($"{reader[3]}, {reader[1]}: {reader[2]}");
                    //FilmResults.Items.Add($"{reader.GetString("release_year")}, {reader.GetString("title")}: {reader.GetString("description")}");
                }
            } 
            catch (Exception ex) // This is bad practice! Don't catch generic exceptions!
            {
                ErrorBox.Text = ex.Message;
                ErrorBox.Visibility = Visibility.Visible;
            }
                finally
            {
                connection.Close();
            }

        }
        private void SortingMode_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}