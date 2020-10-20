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
        public UserDTO Get()
        {
            return JsonConvert.DeserializeObject<UserDTO>(File.ReadAllText("User.json"));
        }

        public void Save(UserDTO dto)
        {
            File.WriteAllText("User.json", JsonConvert.SerializeObject(dto, 
                Formatting.Indented));
        }
    }
}
