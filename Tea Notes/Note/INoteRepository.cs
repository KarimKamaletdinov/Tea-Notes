using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    interface INoteRepository
    {
        void Save(NoteDTO dto);

        NoteDTO[] GetAll();

        void Delete(NoteDTO dto);
    }
}
