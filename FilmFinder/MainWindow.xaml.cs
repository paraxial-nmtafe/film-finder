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
    //class Film
    //{
    //    private UInt16 _releaseYear;
    //    private string _name;
    //    private string _description;
    //}

    public partial class MainWindow : Window
    {
        private readonly MySqlConnection connection;
        private readonly String dbServer = "127.0.0.1";
        private readonly String dbPort = "3306";
        private readonly String dbName = "sakila";

        private readonly String dbUser = "sakila_app_user"; 
        private readonly String dbPassword = "Hunter2!";

        public MainWindow()
        {
            InitializeComponent();
            connection = new MySql.Data.MySqlClient.MySqlConnection();
            connection.ConnectionString = $"server={dbServer};port={dbPort};database={dbName};uid={dbUser};pwd={dbPassword}";
        }

        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            ErrorBox.Visibility = Visibility.Hidden;
            try
            {
                connection.Open();
                string commandString = "insert into film (release_year, title, description, language_id) values (@ryear, @title, @description, 1)";
                MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ryear", 1979);
                command.Parameters.AddWithValue("@title", "The Lion of Dessert");
                command.Parameters.AddWithValue("@description", "A pastry chef hunts down a mysterious confection addicted lion");
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorBox.Text = ex.Message;
                ErrorBox.Visibility = Visibility.Visible;
            } finally
            {
                connection.Close();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FilmResults.Items.Clear();
            ErrorBox.Visibility = Visibility.Collapsed;
            string query = SearchBar.Text;
            //SearchBarText.Text = "";
            string defaultQuery = $"select release_year, title, description from film where description like '%{query}%'";


            try {
                connection.Open();
                MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(defaultQuery, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //FilmResults.Items.Add($"{reader[0]}, {reader[1]}: {reader[2]}");
                    FilmResults.Items.Add($"{reader.GetUInt16("release_year")}, {reader.GetString("title")}: {reader.GetString("description")}");
                }
            } catch (Exception ex) {
                ErrorBox.Text = ex.Message;
                ErrorBox.Visibility = Visibility.Visible;
            } finally {
                connection.Close();
            }
        }

        private void SortingMode_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}