using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPasteBin.DAL
{
    public class SQLDAL
    {
        private string _connectionString;

        #region CTOR
        public SQLDAL()
        {
        }

        #endregion

        public IDataReader ExecuteReader(SqlCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
