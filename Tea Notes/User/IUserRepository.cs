using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    interface IUserRepository
    {
        List<UserDTO> GetAll();

        void Save(UserDTO dto);
    }
}
