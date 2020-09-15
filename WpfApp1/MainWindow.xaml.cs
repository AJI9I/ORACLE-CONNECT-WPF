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
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1
{
    class  ProgVar{
        

        public string TName{ get; set; }

        public ProgVar()
        {
            this.TName = "CITY";
        }
        public ProgVar(string Tname) {
            this.TName = Tname;
        }

        private static ProgVar instance;
        public static ProgVar getInstance(string name)
        {
            if (instance == null)
                instance = new ProgVar(name);
            return instance;
        }
    }


    
        public class MyEntity
        {
            public string MyColumn { get; set; }
        }

        public class MyContext : DbContext
        {
            // This property defines the table
            public DbSet<MyEntity> MyTable { get; set; }

            // This method connects the context with the database
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "test.db" };
                var connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);

                optionsBuilder.UseSqlite(connection);
            }
        }

        /// <summary>
        /// Логика взаимодействия для MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
    {
        QuerySql querySql = null;
        ProgVar progVar = null;
        OracleConnection con = null;

        public MainWindow()
        {
            this.setConnection();
            progVar = new ProgVar();
            querySql = new QuerySql();


            InitializeComponent();

            using (var db = new MyContext())
            {
                db.Database.EnsureCreated();
            }
        }

        #region Работа с базой
        private void setConnection()
        {

            #region Альтернативное создание строки подключения
            OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();
            sb.DataSource = "localhost:1521/orcl";
            sb.DBAPrivilege = "SYSDBA";
            sb.UserID = "SYS";
            sb.Password = "3aPa3a19892811";
            #endregion

            String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp)
            {

            }
        }

        private void updateMyDataGrid() {

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from document";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();

        }
        #endregion

        #region Кнопочное представление
        private void CityBtnTable_Click(object sender, RoutedEventArgs e)
        {
            VisibleCityFields();

        }

        private void VisibleCityFields() {

            progVar.TName = "CITY";
            

            CityNameLabel.Content = "Название города";
            CityNameJURLabel.Visibility = Visibility.Hidden;
            CityNameJurTextBox.Visibility = Visibility.Hidden;
            CityNamePostLabel.Visibility = Visibility.Hidden;
            CityNamePostTextBox.Visibility = Visibility.Hidden;

            BtnVisible();
            TexBoxReset();

            this.AUD(7);
        }

        //клик на кнопку FIRM
        private void FirmBtnTable_Click(object sender, RoutedEventArgs e)
        {
            progVar.TName = "FIRM";
            

            VisibleFirmFields();
            TexBoxReset();

            this.AUD(6);
        }

        private void VisibleFirmFields()
        {
            CityNameLabel.Content = "Название фирмы";
            if (progVar.TName.ToString() == "FIRM") {
                CityNameJURLabel.Content = "Город Юр";
                CityNamePostLabel.Visibility = Visibility.Visible;
                CityNamePostTextBox.Visibility = Visibility.Visible;
            } 
            if (progVar.TName.ToString() == "FIND") {
                CityNameJURLabel.Content = "Город";
                CityNamePostLabel.Visibility = Visibility.Hidden;
                CityNamePostTextBox.Visibility = Visibility.Hidden;
            } 

            CityNameJURLabel.Visibility = Visibility.Visible;
            CityNameJurTextBox.Visibility = Visibility.Visible;
            
            

            if (progVar.TName == "FIRM") {

                BtnVisible();
            }
            if (progVar.TName == "FIND")
            {
                BtnHidden();
            }
        }

        //Переход к заданию 1
        private void TabItem_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            FindMenuOpen();
        }
        
        //клик на кнопку FIND
        private void CityBtnFindTable_Click(object sender, RoutedEventArgs e)
        {

            FindMenuOpen();
        }

        private void FindMenuOpen() {

            progVar.TName = "FIND";
            TexBoxReset();
;
            VisibleFirmFields();

            this.AUD(5);
        }

        //клик на кнопку поиск
        private void FindBtn_Click(object sender, RoutedEventArgs e)
        {
            this.AUD(2);
        }


        #region сброс значений текстовых полей

        //кнопка сбросить все значения
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            TexBoxReset();
        }
        private void TexBoxReset() {

            ResetBtn.IsEnabled = false;
            DeleteBtn.IsEnabled = false;
            UpdateBtn.IsEnabled = false;

            CityNameTextBox.Text = "";
            CityNamePostTextBox.Text = "";
            CityNameJurTextBox.Text = "";

        }
        #endregion

        #region скрытие открытие кнопок управления  НИЖНЯЯЯ
        private void BtnHidden() {
            //Надо проверить момент активности нажать можно нет
            AddBtn.Visibility = Visibility.Hidden;
            UpdateBtn.Visibility = Visibility.Hidden;
            ResetBtn.Visibility = Visibility.Hidden;
            DeleteBtn.Visibility = Visibility.Hidden;
            FindBtn.Visibility = Visibility.Visible;
        }
        private void BtnVisible(){
            AddBtn.Visibility = Visibility.Visible;
            UpdateBtn.Visibility = Visibility.Visible;
            ResetBtn.Visibility = Visibility.Visible;
            DeleteBtn.Visibility = Visibility.Visible;
            FindBtn.Visibility = Visibility.Hidden;
        }
        #endregion

        #endregion

        #region Оправляем запросы через эту функцию и принимаем ответ в датагрид
        private void AUD(int state) {
            

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            string one = CityNameTextBox.Text.ToString();
            string two = CityNameJurTextBox.Text.ToString();
            string tre = CityNamePostTextBox.Text.ToString();

            string sql = "select * from city";

            switch (state) {
                case 0:
                    //добавить город
                    sql = querySql.sqlAddCityTable(one);
                    break;
                case 1:
                    //добавить фирму
                    sql = querySql.sqlAddFirmTable(one, two, tre);
                    break;
                case 2:
                    //найти фирму
                    sql = querySql.sqlFindFirm(one, two);
                    break;
                case 3:
                    //Таблица в шахматном порядке
                    sql = querySql.sqlGetDataSummTable();
                    break;
                case 4:
                    //добавить рандомную запись с датой
                    sql = querySql.sqlRandDataSummTable();
                    break;
                case 5:
                    //вывести фирмыГорода
                    sql = querySql.sqlCityFirmsTable();
                    break;
                case 6:
                    //фирмы
                    sql = querySql.sqlFirmTable();
                    break;
                case 7:
                    //города
                    sql = querySql.sqlCityTable();
                    break;

            }
            cmd.CommandText = sql;

            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            var v = dt.HasErrors;
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        #endregion

        #region Клик на кнопку добавить запись
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (progVar.TName) {
                case "CITY":
                    this.AUD(0);
                    break;
                case "FIRM":
                    this.AUD(1);
                    break;
            }
        }
        #endregion

        
        //Выбор вкладки Задание 2
        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            progVar.TName = "TAB2";
            this.AUD(3);
        }

        //Кнопка добавить рандомное значение
        private void AddRandBtn_Click(object sender, RoutedEventArgs e)
        {
            this.AUD(4);

            this.AUD(3);
        }

        #region хлам
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateMyDataGrid();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            con.Close();
        }

        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TabItem_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        #region хлам
        private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void TabItem_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void TabItem_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void TabItem_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        #endregion

        #endregion

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;

            if (dataRowView != null) {
                if (progVar.TName == "CITY")
                {

                    CityNameTextBox.Text = dataRowView["Город"].ToString();
                }
                if (progVar.TName == "FIRM")
                {

                    CityNameTextBox.Text = dataRowView["Фирма"].ToString();

                    //CityNameJurTextBox.Text = GetTableField(dataRowView["ЮИД"].ToString());

                    //CityNamePostTextBox.Text = GetTableField(dataRowView["ПИД"].ToString());
                }
            }

            ResetBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = true;
            UpdateBtn.IsEnabled = true;

        }

        private string GetTableField(string id) {

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = String.Format("select c.city_id as id, c.name as n from city c where c.city_id = {0}", id);

            OracleDataReader dr = cmd.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(dr);
            var v = dataTable.HasErrors;

            DataGrid dg = new DataGrid();
            dg.ItemsSource = dataTable.DefaultView;
            DataRowView dataRowView = dg.SelectedItem as DataRowView;

            dr.Close();

            

            return dataRowView["n"].ToString();
        }

        private void TabGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FindMenuOpen();
        }
    }
}
