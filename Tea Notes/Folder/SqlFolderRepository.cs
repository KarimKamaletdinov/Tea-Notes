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
    class SqlFolderRepository : IFolderRepository
    {
        private readonly string _ConnectionString;

        private UserDTO User;

        public SqlFolderRepository(UserDTO user)
        {
            User = user;

            var b = new SqlConnectionStringBuilder();
            b.DataSource = "KARIM-NB\\SQLEXPRESS";
            b.InitialCatalog = "Notes";
            b.IntegratedSecurity = true;

            _ConnectionString = b.ToString();
        }

        public void Add(FolderDTO dto)
        {
            dto.UserId = User.Id;
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Insert(dto);
            }
        }

        public void Delete(FolderDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Delete(dto);
            }
        }

        public FolderDTO[] GetAll()
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                return connection.Query<FolderDTO>("SELECT * FROM Folders WHERE UserId=@ui",
                new { ui = User.Id }).ToArray();
            }
        }

        public void Save(FolderDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Update(dto);
            }
        }
    }
}
