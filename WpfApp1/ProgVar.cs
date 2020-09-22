using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class ProgVar
    {

        public string TName { get; set; }

        public string DbName { get; set; }

        public string id { get; set; }

        public OracleConnection con = null;

        public ProgVar()
        {

            this.TName = "CITY";

            //this.DbName = "SQLITE";
            this.DbName = "ORACLE";
        }

        public ProgVar(string Tname)
        {
            this.TName = Tname;
        }


        public void DbNameSet(string DbName)
        {
            this.DbName = DbName;
        }

        private static ProgVar instance;

        public static ProgVar getInstance(string name)
        {
            if (instance == null)
                instance = new ProgVar(name);
            return instance;
        }
    }
}
