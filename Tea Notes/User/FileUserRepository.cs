using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    class FileUserRepository : IUserRepository
    {
        public List<UserDTO> GetAll()
        {
            var l = new List<UserDTO>();
            
            foreach(var d in Directory.GetFiles("Users"))
            {
                l.Add(JsonConvert.DeserializeObject<UserDTO>(File.ReadAllText(d)));
            }

            return l;
        }

        public void Save(UserDTO dto)
        {
            File.WriteAllText("User.json", JsonConvert.SerializeObject(dto, 
                Formatting.Indented));
        }
    }
}
