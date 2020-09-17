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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

namespace WpfApp1
{

        #region точка входа
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

        }
        #endregion

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
        //Выбор вкладки Задание 2
        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            progVar.TName = "TAB2";
            this.AUD(3);

            Tab2ModelView();
        }

        private void Tab2ModelView() {

            //UpdateBtn.Visibility = Visibility.Visible;
            //ResetBtn.Visibility = Visibility.Visible;

        }



        //Кнопка добавить рандомное значение
        private void AddRandBtn_Click(object sender, RoutedEventArgs e)
        {
            this.AUD(4);

            this.AUD(3);
        }

        private void TabGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FindMenuOpen();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (progVar.TName)
            {
                case "CITY":
                    this.AUD(8);
                    break;
                case "FIRM":
                    this.AUD(1);
                    break;
            }
        }
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
            if (progVar.TName == "FIRM") {
                this.AUD(2);
            }
            if (progVar.TName == "TAB2")
            {
                this.AUD(10);
            }
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
            DeleteBtn.Visibility = Visibility.Hidden;
            FindBtn.Visibility = Visibility.Hidden;

            if(progVar.TName == "FIRM"){
                UpdateBtn.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #endregion

        #region Оправляем запросы через эту функцию и принимаем ответ в датагрид Oracle
        private void AUD(int state) {

            if (progVar.DbName == "SQLITE")
            {

                SQLAUD(state);

            }
            if (progVar.DbName == "ORACLE") {

                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                string one = CityNameTextBox.Text.ToString();
                string two = CityNameJurTextBox.Text.ToString();
                string tre = CityNamePostTextBox.Text.ToString();

                string four = FirmNameTextBox_Copy.Text.ToString();

                string sql = "select * from city";

                switch (state)
                {
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
                    case 8:
                        sql = querySql.sqlUpdateCityTable(progVar.id, one);
                        break;
                    case 10:

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
        }

        #endregion

        #region Оправляем запросы через эту функцию и принимаем ответ в датагрид SQLite
        
        #region контекст
        ApplicationContext db;

        public class ApplicationContext : System.Data.Entity.DbContext
        {
            public ApplicationContext() : base("DefaultConnection")
            {
            }
            public System.Data.Entity.DbSet<City> Cities { get; set; }
            public System.Data.Entity.DbSet<Firm> Firms { get; set; }
        }
        #endregion


        public void SQLAUD(int state) {

            db = new ApplicationContext();

            string one = CityNameTextBox.Text.ToString();
            string two = CityNameJurTextBox.Text.ToString();
            string tre = CityNamePostTextBox.Text.ToString();

            string sql = "select * from city";



            //db.Cities.FirstOrDefault(p => p.Id == 1);

            List<City> dr = null;

            List<Firm> fr = null;

            switch (state)
            {
                case 0:
                    dr = new List<City>();
                    //добавить город
                    dr.Add(CityAdd(one,db));
                    myDataGrid.ItemsSource = dr;
                    break;
                case 1:
                    //добавить фирму
                    fr = new List<Firm>();
                    fr.Add(FirmAdd(one,two,tre, db));
                    myDataGrid.ItemsSource = fr;
                    break;
                case 2:
                    //найти фирму

                    //sql = querySql.sqlFindFirm(one, two);
                    break;
                case 3:
                    //Таблица в шахматном порядке
                    //sql = querySql.sqlGetDataSummTable();
                    break;
                case 4:
                    //добавить рандомную запись с датой
                    //sql = querySql.sqlRandDataSummTable();
                    break;
                case 5:
                    //вывести фирмыГорода
                    //sql = querySql.sqlCityFirmsTable();
                    break;
                case 6:
                    //фирмы
                    //sql = querySql.sqlFirmTable();
                    GetFirmSqlite();
                    break;
                case 7:
                    dr = new List<City>();
                    dr = (from p in db.Cities
                          select p).ToList();
                    myDataGrid.ItemsSource = dr;
                    break;
                default:
                    dr = new List<City>();
                    dr = (from p in db.Cities
                          select p).ToList();
                    myDataGrid.ItemsSource = dr;
                    break;

            }

            

            // City city = new City();
            // //city.Id = 1;
            // City c1 = db.Cities.FirstOrDefault(p => p.name == "Плесецк");
            // Firm f1 = new Firm();
            // f1.Name = "Рога и копыта";
            // f1.jur_city_id = c1;
            // db.Firms.Add(f1);
            //// Firm c2 = db.Firms.FirstOrDefault(p => p.Name == "Плесецк");
            // // city.Name = "Миасс";
            // //   db.Cities.Add(city);
        }

        private void GetFirmSqlite() {

            string query = String.Format("select cities.name as Город, firms.name as Фирма, firms.citysjurid as ЮИД, firms.cityspostid as ПИД from cities  inner JOIN firms  on cities.Id = firms.citysjurid  or cities.Id = firms.cityspostid   ORDER BY cities.name");

            //string q = String.Format("select cities.name as Город, firms.name as Фирма, firms.citysjurid as ЮИД, firms.cityspostid as ПИД " +
            //    "from cities " +
            //    "inner JOIN firms " +
            //    "on cities.Id = firms.citysjurid " +
            //    "or cities.Id = firms.cityspostid " +
            //    "ORDER BY cities.name");
            //List<Firm> firm = new List<Firm>();
            //firm = db.Database.SqlQuery<Firm>(query).ToList();


            using (SqlConnection conn = new SqlConnection("Data Source = C:\\Users\\yaaal\\OneDrive\\Рабочий стол\\app\\Проект на сдачу\\WpfApp1 — копия\\WpfApp1\\mobile.db;"))
            {
                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(query, conn);


                //Set the SqlDataAdapter object
                SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);

                //define dataset
                DataSet ds = new DataSet();

                //fill dataset with query results
                dAdapter.Fill(ds);

                //set DataGridView control to read-only
                

                //set the DataGridView control's data source/data table
                myDataGrid.ItemsSource = ds.Tables[0].DefaultView;

            
            //close connection
            conn.Close();
            }
            //firm.fi
            //myDataGrid.ItemsSource = firm;
        }

        private City CityAdd(string one, ApplicationContext dbb) {

            City city = dbb.Cities.FirstOrDefault(p => p.name == one);
            if (city == null) {

                city = new City();
                city.name = one;
                dbb.Cities.Add(city);
                dbb.SaveChanges();
            }
            //var c = (City)(from p in db.Cities
            //where p.name == one
            //       select p);

           // return db.Cities.Where(p=>p.name == one).Select(x=>x.name = город);
            return dbb.Cities.FirstOrDefault(p => p.name == one);

            //return (City)(from p in db.Cities
            //              where p.name == one
            //       select p);
        }

        private Firm FirmAdd(string one, string two, string tre, ApplicationContext dbb)
        {
            City cityJur = CityAdd(two, dbb);
            City cityPost = null;

            if (tre.Replace(" ", "") != "")
            {
                cityPost = CityAdd(tre, dbb);
            }

            Firm firmEquals = db.Firms.FirstOrDefault(p=>p.Name == one);

            Firm firm = new Firm();
            firm.Name = one;
            firm.jur_city_id = cityJur;
            if (cityPost != null) {
                firm.post_city_id = cityPost;
            }
            


            if (firmEquals != firm) {
                dbb.Firms.Add(firm);

                dbb.SaveChanges();
            }
            var ff = dbb.Database.SqlQuery<Firm>("select * from firms ");
            return firm = db.Firms.FirstOrDefault(p => p.Name == one);

            //return firm = db.Firms.FirstOrDefault(p => p.Name == one);
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

        #region выбор из таблици элемента
        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;

            if (dataRowView != null)
            {
                if (progVar.TName == "CITY")
                {

                    CityNameTextBox.Text = dataRowView["Город"].ToString();
                    progVar.id = dataRowView["Город"].ToString();
                }
                if (progVar.TName == "FIRM")
                {
                    //обновление информации о фирме, проблема с поиском по ID
                    CityNameTextBox.Text = dataRowView["Фирма"].ToString();

                    progVar.id = dataRowView["Фирма"].ToString();

                    //CityNameJurTextBox.Text = GetTableField(dataRowView["ЮИД"].ToString());

                    //CityNamePostTextBox.Text = GetTableField(dataRowView["ПИД"].ToString());
                }
            }



            ResetBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = true;
            UpdateBtn.IsEnabled = true;

        }

        //не работает
        private string GetTableField(string id)
        {

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
        #endregion

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

    }
}
