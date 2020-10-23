using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    [Table("Note")]

    class NoteDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Content { get; set; } = "";

        public int FolderId { get; set; }

        public int UserId { get; set; }
    }
}
