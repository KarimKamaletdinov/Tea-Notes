using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    class SqlUserRepository : IUserRepository
    {
        private readonly string _ConnectionString;

        public SqlUserRepository()
        {
            var b = new SqlConnectionStringBuilder();
            b.DataSource = "KARIM-NB\\SQLEXPRESS";
            b.InitialCatalog = "Notes";
            b.IntegratedSecurity = true;

            _ConnectionString = b.ToString();
        }


        public void Add(UserDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Insert(dto);
            }
        }

        public void Delete(UserDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Delete(dto);
            }
        }

        public List<UserDTO> GetAll()
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                return connection.Query<UserDTO>("SELECT * FROM Users").ToList();
            }
        }

        public void Save(UserDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Update(dto);
            }
        }
    }
}
