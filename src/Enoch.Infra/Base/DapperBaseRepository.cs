using Dapper;
using Enoch.CrossCutting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Enoch.Infra.Base
{
    public class DapperBaseRepository
    {
        protected SqlConnection Create()
        {
            var parameters = new Parameters();

            return new SqlConnection(Encryption.Decrypt(parameters.Data.ConnectionString));
        }

        public int Execute(string query)
        {
            int ret;
            using (var cn = Create())
            {
                cn.Open();
                ret = cn.Execute(query);
                cn.Close();
                cn.Dispose();
            }

            return ret;
        }

        public IEnumerable<T> Find<T>(string query)
        {
            IEnumerable<T> items;
            using (var cn = Create())
            {
                cn.Open();
                items = cn.Query<T>(query);
                cn.Close();
                cn.Dispose();
            }

            return items;
        }
    }
}
