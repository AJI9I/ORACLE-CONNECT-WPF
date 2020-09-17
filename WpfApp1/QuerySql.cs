using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class QuerySql
    {

        public string sqlAddFirmTable(string NAME, string JUR_CITY, string POST_CITY) {

            string query = "";
            if (POST_CITY.Replace(" ", "") == "")
            {
                query = String.Format("declare" +
                " a int;" +
                " BEGIN" +
                " SELECT count(city.city_id) into a FROM city WHERE city.name = '{0}';" +
                " if a = 0 THEN" +
                " insert into city(CITY_ID, NAME) values(supplier_seq.NEXTVAL, '{0}');" +
                " end if;" +
                " SELECT city.city_id into a FROM city WHERE city.city_id =(select min(city.city_id) from city where city.name = '{0}');" +
                " INSERT INTO FIRM(FIRM_ID, NAME, JUR_CITY_ID) VALUES(supplier_seq.NEXTVAL, '{2}', a);" +
                " COMMIT;" +
                " end; ", JUR_CITY, POST_CITY, NAME);
            }
            else {

                query = String.Format("declare" +
                " a int;" +
                " b int;" +
                " BEGIN" +
                " SELECT count(city.city_id) into a FROM city WHERE city.name = '{0}';" +
                " SELECT count(city.city_id) into b FROM city WHERE city.name = '{1}';" +
                " if a = 0 THEN" +
                " insert into city(CITY_ID, NAME) values(supplier_seq.NEXTVAL, '{0}');" +
                " end if;" +
                " SELECT city.city_id into a FROM city WHERE city.city_id =(select min(city.city_id) from city where city.name = '{0}');" +
                " if b = 0 then" +
                " insert into city(CITY_ID, NAME) values(supplier_seq.NEXTVAL, '{1}');" +
                " end if;" +
                " SELECT city.city_id into b FROM city WHERE city.city_id =(select min(city.city_id) from city where city.name = '{1}');" +
                " INSERT INTO FIRM(FIRM_ID, NAME, JUR_CITY_ID, POST_CITY_ID) VALUES(supplier_seq.NEXTVAL, '{2}', a, b);" +
                " COMMIT;" +
                " end;", JUR_CITY,POST_CITY,NAME);
            }
            return query;
        }
        public string sqlFindFirm(string NAME, string CITY) {

            return String.Format("select firm.name as НазваниеФирмы, j.name as ЮрАддрес, p.name as ПочтАддрес "+
                "from firm " +
                "left outer " +
                "join city j " +
                "on firm.jur_city_id = j.city_id " +
                "left outer join city p " +
                "on firm.post_city_id = p.city_id " +
                "where upper(firm.name) = upper('{0}') or upper(j.name) = upper('{1}') " +
                "or upper(p.name) = upper('{1}') " +
                "group by firm.name, j.name, p.name", NAME, CITY);
                 

        }

        public string sqlAddCityTable(string NAME) {
            string query = "";

            return String.Format("declare" +
                " a int;" +
                " BEGIN" +
                " SELECT count(city.city_id) into a FROM city WHERE city.name = '{0}';" +
                " if a = 0 THEN" +
                  " insert into city(CITY_ID, NAME) values(supplier_seq.NEXTVAL, '{0}');" +
                " end if;" +
                " COMMIT;" +
                " end; ",NAME);
        }

        public string sqlGetDataSummTable() {
            return String.Format("select extract(year from DOCUMENT.DOC_DATE) as Год,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 1 then DOCUMENT.SUMM else 0 end) as Январь,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 2 then DOCUMENT.SUMM else 0 end) as Февраль,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 3 then DOCUMENT.SUMM else 0 end) as Март,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 4 then DOCUMENT.SUMM else 0 end) as Апрель,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 5 then DOCUMENT.SUMM else 0 end) as Май,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 6 then DOCUMENT.SUMM else 0 end) as Июнь,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 7 then DOCUMENT.SUMM else 0 end) as Июль,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 8 then DOCUMENT.SUMM else 0 end) as Август,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 9 then DOCUMENT.SUMM else 0 end) as Сентябрь,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 10 then DOCUMENT.SUMM else 0 end) as Октябрь,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 11 then DOCUMENT.SUMM else 0 end) as Ноябрь,  " +
                "sum( case  extract(month from DOCUMENT.DOC_DATE) when 12 then DOCUMENT.SUMM else 0 end) as Декабрь " +
                "from DOCUMENT " +
                "group by extract(year from DOCUMENT.DOC_DATE) " +
                "order by extract(year from DOCUMENT.DOC_DATE)");
        }

        public string sqlRandDataSummTable() {
            return String.Format("insert INTO DOCUMENT ( doc_id,doc_date, summ) " +
                "VALUES (supplier_seq.NEXTVAL," +
                         "TO_DATE(" +
                            "TRUNC(" +
                                "DBMS_RANDOM.VALUE(" +
                                    "TO_CHAR(DATE '1993-01-01', 'J')," +
                                    "TO_CHAR(DATE '2003-12-31', 'J')))," +
                             "'J'), " +
                         "400)");
        }

        public string sqlCityFirmsTable() {
            return String.Format("select city.name as Город, firm.name as Фирма, firm.jur_city_id as ЮИД, firm.post_city_id as ПИД " +
                "from city " +
                "inner JOIN firm " +
                "on city.city_id = firm.jur_city_id " +
                "or city.city_id = firm.post_city_id " +
                "ORDER BY city.name");
        }

        public string sqlCityTable(){ 
        return String.Format("select city_id as ИД, name as Город " +
            "from city " +
            "ORDER BY name");
        }

        public string sqlFirmTable() {
            return String.Format("select FIRM_ID as ИД, name as Фирма, jur_city_id as ЮИД, post_city_id as ПИД " +
                "from firm " +
                "ORDER BY name");
        }

        public string sqlUpdateCityTable(string id, string NAME)
        {

            //return String.Format("declare" +
            //    " a int;" +
            //    " b int;" +
            //    " BEGIN" +
            //    "select city.city_id as c into a from city where city.name = '{0}';" +
            //    "update city set city.name='{1}' where city.city_id = a;" +
            //    " COMMIT;" +
            //    " end; ", id, NAME);

            return String.Format(
                
                "update city set city.name='{0}' where city.city_id = " +
                "(select city.city_id from city where city.name = '{1}')",NAME, id );
        }

    }
}
