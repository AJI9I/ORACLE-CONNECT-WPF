using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class QuerySql
    {
        public QuerySql(){
            //this.SqlAddFirmTable = String.Format("declare " +
            //        " a int; " +
            //        " b int; " +
            //        " BEGIN" +
            //        " SELECT city.city_id into a FROM city WHERE city.name = '{0}'; " +
            //        " SELECT city.city_id into b FROM city WHERE city.name = '{1}' " +
            //        " INSERT INTO FIRM(FIRM_ID, NAME, POST_CITY_ID, JUR_CITY_ID) VALUES(supplier_seq.NEXTVAL, '{2}', a, b); " +
            //        " COMMIT; " +
            //        " end;", JUR_CITY,);
        }

        public string sqlAddFirmTable(string NAME, string JUR_CITY, string POST_CITY) {

            string query = "";
            if (POST_CITY.Replace(" ", "") == "")
            {
                query = String.Format("declare" +
                " a int;" +
                " BEGIN" +
               " SELECT city.city_id into a FROM city WHERE city.name = '{0};'" +
                " if a = 0 THEN " +
                " insert into city(CITY_ID, NAME) values(supplier_seq.NEXTVAL, '{0}');" +
                " SELECT city.city_id into a FROM city WHERE city.name = '{0}';" +
                 " end if;" +
                " INSERT INTO FIRM(FIRM_ID, NAME, JUR_CITY_ID) VALUES(supplier_seq.NEXTVAL, '{1}', a);" +
                " COMMIT;" +
                " end;", JUR_CITY, NAME);
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
                " end; ",JUR_CITY,POST_CITY,NAME);
            }
            return query;
        }
        public string sqlFindFirm(string NAME, string CITY) {
            string query = String.Format("select firm.name as НазваниеФирмы, j.name as ЮрАддрес, p.name as ПочтАддрес "+
                "from firm " +
                "left outer " +
                "join city j " +
                "on firm.jur_city_id = j.city_id " +
                "left outer join city p " +
                "on firm.post_city_id = p.city_id " +
                "where upper(firm.name) = upper('{0}') or upper(j.name) = upper('{1}') " +
                "or upper(p.name) = upper('{1}') " +
                "group by firm.name, j.name, p.name", NAME, CITY);
                return query;
        }


        

        //public string SqlAddFirmTable { get; set; }
    }
}
