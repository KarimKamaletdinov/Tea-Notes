using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    class NotesPresenter
    {
        private INoteRepository Repository;
        private List<NoteDTO> DTOs = new List<NoteDTO>();

        public void Start(IMainView view)
        {
            Repository = new FileNoteRepository(new FileUserRepository().Get());

            DTOs = Repository.GetAll().ToList();

            view.Notes = DTOs;
        }
    }
}
