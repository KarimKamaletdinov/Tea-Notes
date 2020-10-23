﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    [Table("Users")]
    class UserDTO
    {
        public int Id { get; set; }

        public string Folder { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }
}
