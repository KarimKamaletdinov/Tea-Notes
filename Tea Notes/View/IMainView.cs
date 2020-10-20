using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    interface IMainView
    {
        List<NoteDTO> Notes { get; set; }

        NotesPresenter Presenter { get; set; }

        event Action<int> DeleteNote;

        event Action<string> AddNote;

        event Action<string, int> ChangeNote;

        event Action<string, int> RenameNote;
    }
}
