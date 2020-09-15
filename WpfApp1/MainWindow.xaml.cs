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
        ProgVar progVar;
        OracleConnection con = null;
        public MainWindow()
        {
            this.setConnection();
            progVar = new ProgVar();

            querySql = new QuerySql();

            InitializeComponent();

            //Надо переделать выгрузку кнопок более приятно
            //VisibleFirmFields("");


            //this.TabGrid.Background = 
        }

        #region Работа с базой
        private void setConnection()
        {

            OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();
            sb.DataSource = "localhost:1521/orcl";
            sb.DBAPrivilege = "SYSDBA";
            sb.UserID = "SYS";
            sb.Password = "3aPa3a19892811";

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

        private string conStr()
        {

            OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();
            sb.DataSource = "localhost:1521/orcl";
            sb.DBAPrivilege = "SYSDBA";
            sb.UserID = "SYS";
            sb.Password = "3aPa3a19892811";
            // {    DATA SOURCE=localhost:1521/orcl;USER ID = SYS;PASSWORD = 3aPa3a19892811;DBA PRIVILEGE = SYSDBA;}

            OracleConnection conn = new OracleConnection(sb.ToString());
            conn.Open();
            return sb.ToString();
        }

        private void updateMyDataGrid() {


            OracleCommand cmd = con.CreateCommand();
            //cmd.CommandText = "select city.name as Город, firm.name as Фирма, firm.jur_city_id as сИД, firm.post_city_id as фИД from city inner JOIN firm on city.city_id = firm.jur_city_id or city.city_id = firm.post_city_id WHERE UPPER(city.name) = UPPER('') or UPPER(firm.name) = UPPER('')";
            cmd.CommandText = "select * from document";

            //cmd.CommandText = d;
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();

        }
        #endregion

        #region
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
            VisibleCityFields();
        }

        private void TabItem_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //String sql = "select FIRM_ID as ИД, name as Фирма, jur_city_id as сИД, post_city_id as фИД from firm ORDER BY name";
            //this.AUD(99, sql);
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void CityBtnTable_Click(object sender, RoutedEventArgs e)
        {
            VisibleCityFields();

        }
        #endregion

        #region Кнопочное представление
        private void VisibleCityFields() {

            CityNameLabel.Content = "Название города";
            CityNameJURLabel.Visibility = Visibility.Hidden;
            CityNameJurTextBox.Visibility = Visibility.Hidden;
            CityNamePostLabel.Visibility = Visibility.Hidden;
            CityNamePostTextBox.Visibility = Visibility.Hidden;

            BtnVisible();

            progVar.TName = "CITY";

            String sql = "select city_id as ИД, name as Город from city ORDER BY name";
            this.AUD(99, sql);
        }

        //клик на кнопку FIRM
        private void FirmBtnTable_Click(object sender, RoutedEventArgs e)
        {
            progVar.TName = "FIRM";

            Button btn = (Button)sender;
            VisibleFirmFields(btn.Content.ToString());

            

            String sql = "select FIRM_ID as ИД, name as Фирма, jur_city_id as сИД, post_city_id as фИД from firm ORDER BY name";
            this.AUD(99, sql);
        }

        
        private void VisibleFirmFields(string Content)
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

        //клик на кнопку FIND
        private void CityBtnFindTable_Click(object sender, RoutedEventArgs e)
        {
            progVar.TName = "FIND";

            Button btn = (Button)sender;
            VisibleFirmFields(btn.Content.ToString());
            

            String sql = "select city.name as Город, firm.name as Фирма, firm.jur_city_id as сИД, firm.post_city_id as фИД from city inner JOIN firm on city.city_id = firm.jur_city_id or city.city_id = firm.post_city_id ORDER BY city.name";
            this.AUD(99, sql);

        }

        //клик на кнопку поиск
        private void FindBtn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select city.name as Город, firm.name as Фирма, firm.jur_city_id as сИД, firm.post_city_id as фИД from city inner JOIN firm on city.city_id = firm.jur_city_id or city.city_id = firm.post_city_id WHERE UPPER(firm.name) = UPPER(:NAME) or UPPER(city.name) = UPPER(:POST_CITY_ID) ORDER BY city.name";
            sql = "select firm.name as НазваниеФирмы, j.name as ЮрАддрес, p.name as ПочтАддрес from firm left outer join city j on firm.jur_city_id = j.city_id left outer join city p on firm.post_city_id = p.city_id where upper(firm.name) = upper(:NAME) or upper(j.name) = upper(:POST_CITY_ID) or upper(p.name) = upper(:JUR_CITY_ID) group by ";
            this.AUD(2, sql);
        }

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
                    cmd.Parameters.Add("NAME", OracleDbType.Varchar2, 25).Value = CityNameTextBox.Text.ToString();
                    break;
                case 1:
                    cmd.CommandText = querySql.sqlAddFirmTable(one, two, tre);
                    //cmd.Parameters.Add("NAME", OracleDbType.Varchar2, 25).Value = CityNameTextBox.Text.ToString();
                    //cmd.Parameters.Add("POST_CITY_ID", OracleDbType.Decimal).Value = CityNameJurTextBox.Text;
                    //cmd.Parameters.Add("JUR_CITY_ID", OracleDbType.Decimal).Value = CityNamePostTextBox.Text;
                    break;
                case 2:
                    cmd.CommandText = querySql.sqlFindFirm(one, two);
                    //cmd.Parameters.Add("NAME", OracleDbType.Varchar2, 25).Value = CityNameTextBox.Text.ToString();
                    //cmd.Parameters.Add("POST_CITY_ID", OracleDbType.Varchar2, 25).Value = CityNameJurTextBox.Text.ToString();
                    //cmd.Parameters.Add("JUR_CITY_ID", OracleDbType.Varchar2, 25).Value = CityNamePostTextBox.Text.ToString();
                    break;
                case 3:
                    
                    break;
                case 4:
                    break;
                default:
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

        private void DataOne() { 
        
        }

        #region Клик на кнопку добавить запись
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "";
            switch (progVar.TName) {
                case "CITY":
                    sql = "INSERT INTO CITY(CITY_ID, NAME) " +
                "VALUES(supplier_seq.NEXTVAL, :NAME)";
                    this.AUD(0, sql);
                    break;
                case "FIRM":
                    //sql = querySql.SqlAddFirmTable;
                //    sql = "INSERT INTO FIRM(FIRM_ID, NAME, POST_CITY_ID, JUR_CITY_ID) " +
                //"VALUES(supplier_seq.NEXTVAL, :NAME, :POST_CITY_ID, :JUR_CITY_ID)";
                    this.AUD(1, sql);
                    break;
            }
        }
        #endregion

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
        
        //Выбор вкладки Задание 2
        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            String sql = "select extract(year from DOCUMENT.DOC_DATE) as Год,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 1 then DOCUMENT.SUMM else 0 end) as Январь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 2 then DOCUMENT.SUMM else 0 end) as Февраль,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 3 then DOCUMENT.SUMM else 0 end) as Март,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 4 then DOCUMENT.SUMM else 0 end) as Апрель,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 5 then DOCUMENT.SUMM else 0 end) as Май,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 6 then DOCUMENT.SUMM else 0 end) as Июнь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 7 then DOCUMENT.SUMM else 0 end) as Июль,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 8 then DOCUMENT.SUMM else 0 end) as Август,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 9 then DOCUMENT.SUMM else 0 end) as Сентябрь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 10 then DOCUMENT.SUMM else 0 end) as Октябрь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 11 then DOCUMENT.SUMM else 0 end) as Ноябрь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 12 then DOCUMENT.SUMM else 0 end) as Декабрь from DOCUMENT group by extract(year from DOCUMENT.DOC_DATE) order by extract(year from DOCUMENT.DOC_DATE)";
            this.AUD(99, sql);
        }

        //Кнопка добавить рандомное значение
        private void AddRandBtn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "insert INTO DOCUMENT ( doc_id,doc_date, summ) VALUES (supplier_seq.NEXTVAL,TO_DATE(TRUNC(" +
            "DBMS_RANDOM.VALUE(TO_CHAR(DATE '1993-01-01', 'J'),"+
                                "TO_CHAR(DATE '2003-12-31', 'J')))"+
                                ",'J'), 400)";
            this.AUD(99, sql);

            sql = "select extract(year from DOCUMENT.DOC_DATE) as Год,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 1 then DOCUMENT.SUMM else 0 end) as Январь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 2 then DOCUMENT.SUMM else 0 end) as Февраль,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 3 then DOCUMENT.SUMM else 0 end) as Март,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 4 then DOCUMENT.SUMM else 0 end) as Апрель,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 5 then DOCUMENT.SUMM else 0 end) as Май,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 6 then DOCUMENT.SUMM else 0 end) as Июнь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 7 then DOCUMENT.SUMM else 0 end) as Июль,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 8 then DOCUMENT.SUMM else 0 end) as Август,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 9 then DOCUMENT.SUMM else 0 end) as Сентябрь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 10 then DOCUMENT.SUMM else 0 end) as Октябрь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 11 then DOCUMENT.SUMM else 0 end) as Ноябрь,  sum( case  extract(month from DOCUMENT.DOC_DATE) when 12 then DOCUMENT.SUMM else 0 end) as Декабрь from DOCUMENT group by extract(year from DOCUMENT.DOC_DATE) order by extract(year from DOCUMENT.DOC_DATE)";
            this.AUD(99, sql);
        }
    }
}
