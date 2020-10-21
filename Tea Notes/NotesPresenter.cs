using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    class NotesPresenter
    {
        private INoteRepository NoteRepository;

        private IFolderRepository FolderRepository;

        private List<NoteDTO> NoteDTOs = new List<NoteDTO>();

        private List<FolderDTO> FolderDTOs = new List<FolderDTO>();

        public void Start(IMainView view)
        {
            NoteRepository = new FileNoteRepository(new FileUserRepository().Get());

            FolderRepository = new FileFolderRepository(new FileUserRepository().Get());

            view.DeleteNote += DeleteNote;

            view.AddNote += AddNote;

            view.ChangeNote += ChangeNote;

            view.RenameNote += RenameNote;

            view.DeleteFolder += DeleteFolder;

            view.AddFolder += AddFolder;

            view.RenameFolder += RenameFolder;
        }

        public void UpdateView(IMainView view)
        {
            NoteDTOs = NoteRepository.GetAll().ToList();

            view.Notes = NoteDTOs;


            FolderDTOs = FolderRepository.GetAll().ToList();

            view.Folders = FolderDTOs;
        }



        private void RenameNote(string arg1, int arg2)
        {
            NoteDTOs[arg2].Name = arg1;

            NoteRepository.Save(NoteDTOs[arg2]);
        }

        private void ChangeNote(string arg1, int arg2)
        {
            NoteDTOs[arg2].Content = arg1;

            NoteRepository.Save(NoteDTOs[arg2]);
        }

        private void AddNote(string obj)
        {
            NoteRepository.Add(new NoteDTO() { Name = obj });
        }

        private void DeleteNote(int obj)
        {
            NoteRepository.Delete(NoteDTOs[obj]);

            NoteDTOs.RemoveAt(obj);
        }


        private void DeleteFolder(int obj)
        {
            
        }

        private void AddFolder(string obj)
        {
            
        }

        private void RenameFolder(string arg1, int arg2)
        {
            throw new NotImplementedException();
        }
    }
}
