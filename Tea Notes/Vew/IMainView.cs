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

        List<FolderDTO> Folders { get; set; }

        NotesPresenter Presenter { get; set; }
    }
}
