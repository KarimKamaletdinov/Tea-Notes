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

            view.DeleteNote += DeleteNote;

            view.AddNote += AddNote;

            view.ChangeNote += ChangeNote;
        }

        private void ChangeNote(string arg1, int arg2)
        {
            DTOs[arg2].Content = arg1;

            Repository.Save(DTOs[arg2]);
        }

        public void UpdateView(IMainView view)
        {
            DTOs = Repository.GetAll().ToList();

            view.Notes = DTOs;
        }

        private void AddNote(string obj)
        {
            Repository.Add(new NoteDTO() { Name = obj });
        }

        private void DeleteNote(int obj)
        {
            Repository.Delete(DTOs[obj]);

            DTOs.RemoveAt(obj);
        }
    }
}
