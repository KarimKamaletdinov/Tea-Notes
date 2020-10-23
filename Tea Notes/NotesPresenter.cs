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

        public void Start(IMainView view, INoteRepository noteRepository,
            IFolderRepository folderRepository)
        {
            NoteRepository = noteRepository;

            FolderRepository = folderRepository;

            view.DeleteNote += DeleteNote;

            view.AddNote += AddNote;

            view.ChangeNote += ChangeNote;

            view.RenameNote += RenameNote;

            view.DeleteFolder += DeleteFolder;

            view.AddFolder += AddFolder;

            view.RenameFolder += RenameFolder;

            view.UpdateView += UpdateView;
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
            NoteDTOs.Find(x => x.Id == arg2).Name = arg1;

            NoteRepository.Save(NoteDTOs.Find(x => x.Id == arg2));
        }

        private void ChangeNote(string arg1, int arg2)
        {
            NoteDTOs.Find(x => x.Id == arg2).Content = arg1;

            NoteRepository.Save(NoteDTOs.Find(x => x.Id == arg2));
        }

        private void AddNote(string obj, int i)
        {
            NoteRepository.Add(new NoteDTO() { Name = obj , FolderId = i});
        }

        private void DeleteNote(int obj)
        {
            NoteRepository.Delete(NoteDTOs.Find(x => x.Id == obj));

            NoteDTOs.Remove(NoteDTOs.Find(x => x.Id == obj));
        }


        private void DeleteFolder(int obj)
        {
            foreach (var n in NoteDTOs.ToList().Where(x => x.FolderId == obj))
            {
                NoteRepository.Delete(n);
                NoteDTOs.Remove(n);
            }

            foreach (var n in FolderDTOs.ToList().Where(x => x.ParentId == obj))
            {
                FolderRepository.Delete(n);
                FolderDTOs.Remove(n);
            }

            FolderRepository.Delete(FolderDTOs.Find(x => x.Id == obj));

            FolderDTOs.Remove(FolderDTOs.Find(x => x.Id == obj));
        }

        private void AddFolder(string obj, int i)
        {
            FolderRepository.Add(new FolderDTO() { Name = obj, ParentId = i });
        }

        private void RenameFolder(string arg1, int arg2)
        {
            FolderDTOs.Find(x=>x.Id == arg2).Name = arg1;

            FolderRepository.Save(FolderDTOs.Find(x => x.Id == arg2));
        }
    }
}
