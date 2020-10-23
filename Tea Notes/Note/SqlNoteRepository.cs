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
    class SqlNoteRepository : INoteRepository
    {
        private readonly string _ConnectionString;

        private UserDTO User;

        public SqlNoteRepository(UserDTO user)
        {
            User = user;

            var b = new SqlConnectionStringBuilder();
            b.DataSource = "KARIM-NB\\SQLEXPRESS";
            b.InitialCatalog = "Notes";
            b.IntegratedSecurity = true;

            _ConnectionString = b.ToString();
        }


        public void Add(NoteDTO dto)
        {
            dto.UserId = User.Id;

            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Insert(dto);
            }
        }

        public void Delete(NoteDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Delete(dto);
            }
        }

        public NoteDTO[] GetAll()
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                var a = connection.Query<NoteDTO>("SELECT * FROM Note WHERE UserId=@n1",
                new { n1 = User.Id}).ToArray();

                return a;
            }
        }

        public void Save(NoteDTO dto)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Update(dto);
            }
        }
    }
}
