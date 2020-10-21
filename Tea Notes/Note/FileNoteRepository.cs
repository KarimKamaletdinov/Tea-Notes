using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    class FileNoteRepository : INoteRepository
    {
        private UserDTO User;

        public FileNoteRepository(UserDTO user)
        {
            User = user;
        }

        public void Save(NoteDTO dto)
        {
            File.WriteAllText(User.Folder + "\\" + dto.Id + ".nt", dto.Name + '¨' 
                + dto.Content + '¨' + dto.FolderId);
        }

        public void Delete(NoteDTO dto)
        {
            File.Delete(User.Folder + "\\" + dto.Id + ".nt");
        }

        public NoteDTO[] GetAll()
        {
            var l = new List<NoteDTO>();

            foreach (var f in Directory.GetFiles(User.Folder).ToList().
                Where(x => x.EndsWith(".nt")))
            {
                l.Add(ParseDTO(f));
            }

            return l.ToArray();
        }

        public void Add(NoteDTO dto)
        {
            File.WriteAllText(User.Folder + "\\" + GetMaxId() + ".nt", dto.Name + '¨'
              + dto.Content + '¨' + dto.FolderId);
        }

        private NoteDTO ParseDTO(string f)
        {
            if (File.ReadAllText(f).Split(new char[] { '¨' }).Length == 3)
            {
                return new NoteDTO()
                {
                    Name = File.ReadAllText(f).Split(new char[] { '¨' })[0],
                    Content = File.ReadAllText(f).Split(new char[] { '¨' })[1],
                    FolderId = int.Parse(File.ReadAllText(f).Split(new char[] { '¨' })[2]),
                    Id = int.Parse(f.Replace(User.Folder + "\\", "").Replace(".nt", ""))
                };
            }

            throw new Exception("Ошибка чтения");
        }

        private int GetMaxId()
        {
            var id = 0;

            foreach(var n in Directory.GetFiles(User.Folder))
            {
                if (!n.EndsWith(".fd"))
                {
                    if (int.Parse(n.Replace(User.Folder + "\\", "").Replace(".nt", "")) >= id)
                    {
                        id = int.Parse(n.Replace(User.Folder + "\\", "").Replace(".nt", "")) + 1;
                    }
                }
            }

            return id;
        }
    }
}
