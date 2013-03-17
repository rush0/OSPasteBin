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
        public SQLDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        public IDataReader ExecuteQuery(string procedureName, List<SqlParameter> procedureParameters)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand procedure = new SqlCommand(procedureName, connection);
            procedure.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter procParam in procedureParameters)
                procedure.Parameters.Add(procParam);

            try
            {
                connection.Open();
                IDataReader reader = procedure.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                // Close open resources
                if (procedure != null) procedure.Dispose();
                if (connection != null) connection.Close();
                throw;
            }
        }

        public IDataReader ExecuteQuery(string procedureName)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand procedure = new SqlCommand(procedureName, connection);
            procedure.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                IDataReader reader = procedure.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                // Close open resources
                if (procedure != null) procedure.Dispose();
                if (connection != null) connection.Close();
                throw;
            }
        }

        public bool ExecuteBulkCopy(DataTable valueTable, string tableName)
        {
            bool success = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add(0, 0);
                        bulkCopy.ColumnMappings.Add(1, 1);

                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(valueTable);

                        success = true;
                    }
                }
            }
            catch (Exception)
            {   
                throw;
            }


            return success;
        }
    }
}
