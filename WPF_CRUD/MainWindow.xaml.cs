using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace WPF_CRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=mpfDB;Integrated Security=True");

        public void ClearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            city_txt.Clear();
            search_txt.Clear();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Table_1", connection);
            DataTable datatable = new DataTable();

            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            datatable.Load(sdr);
            connection.Close();

            datagrid.ItemsSource = datatable.DefaultView;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

      

        public bool isValid()
        {
            if (name_txt.Text == String.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == String.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == String.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (city_txt.Text == String.Empty)
            {
                MessageBox.Show("City is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Table_1 VALUES(@Name, @Age, @Gender, @City)", connection);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Age", age_txt.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender_txt.Text);
                    cmd.Parameters.AddWithValue("@City", city_txt.Text);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    LoadGrid();
                    MessageBox.Show("Successfully registered", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Table_1 SET Name = '"+ name_txt.Text +"'," +
                " Age = '"+ age_txt.Text + "', " +
                "Gender = '"+ gender_txt.Text + "'," +
                " City = '"+ city_txt.Text +"'" +
                " WHERE ID = '"+ search_txt.Text +"' ", connection);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been Updated", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);

                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Not Updated", ex.Message);
            }
            finally
            {
                connection.Close();
                ClearData();
                LoadGrid();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Table_1 WHERE ID = " + search_txt.Text + "", connection);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                connection.Close();

                ClearData();
                LoadGrid();
                connection.Close();

            }
            catch(SqlException ex)
            {
                MessageBox.Show("Not Deleted" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
