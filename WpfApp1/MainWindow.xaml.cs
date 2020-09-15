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
            TexBoxReset();

            CityNameLabel.Content = "Название города";
            CityNameJURLabel.Visibility = Visibility.Hidden;
            CityNameJurTextBox.Visibility = Visibility.Hidden;
            CityNamePostLabel.Visibility = Visibility.Hidden;
            CityNamePostTextBox.Visibility = Visibility.Hidden;

            BtnVisible();

            String sql = querySql.sqlCityTable();
            this.AUD(99, sql);
        }

        
        //клик на кнопку FIRM
        private void FirmBtnTable_Click(object sender, RoutedEventArgs e)
        {
            progVar.TName = "FIRM";
            TexBoxReset();

            VisibleFirmFields();

            String sql = querySql.sqlFirmTable();
            this.AUD(99, sql);
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
            
            

            if (Content == "T FIRM") {

                BtnVisible();
            }
            if (Content == "Find")
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


            String sql = querySql.sqlCityFirmsTable();
            this.AUD(99, sql);
        }

        //клик на кнопку поиск
        private void FindBtn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "";
            this.AUD(2, sql);
        }


        #region сброс значений текстовых полей

        //кнопка сбросить все значения
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            TexBoxReset();
        }
        private void TexBoxReset() {
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
        private void AUD(int state, string sql) {
            

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            string one = CityNameTextBox.Text.ToString();
            string two = CityNameJurTextBox.Text.ToString();
            string tre = CityNamePostTextBox.Text.ToString();



            switch (state) {
                case 0:
                    cmd.CommandText = querySql.sqlAddCityTable(one);
                    break;
                case 1:
                    cmd.CommandText = querySql.sqlAddFirmTable(one, two, tre);
                    break;
                case 2:
                    cmd.CommandText = querySql.sqlFindFirm(one, two);
                    break;

            }

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
            String sql = "";
            switch (progVar.TName) {
                case "CITY":
                    this.AUD(0, sql);
                    break;
                case "FIRM":
                    this.AUD(1, sql);
                    break;
            }
        }
        #endregion

        
        //Выбор вкладки Задание 2
        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            String sql = querySql.sqlGetDataSummTable();
            this.AUD(99, sql);
        }

        //Кнопка добавить рандомное значение
        private void AddRandBtn_Click(object sender, RoutedEventArgs e)
        {
            String sql = querySql.sqlRandDataSummTable();
            this.AUD(99, sql);

            sql = querySql.sqlGetDataSummTable();
            this.AUD(99, sql);
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

    }
}
