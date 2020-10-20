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
                + dto.Content + '¨' + dto.Folder);
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

        private NoteDTO ParseDTO(string f)
        {
            if (File.ReadAllText(f).Split(new char[] { '¨' }).Length == 3)
            {
                return new NoteDTO()
                {
                    Name = File.ReadAllText(f).Split(new char[] { '¨' })[0],
                    Content = File.ReadAllText(f).Split(new char[] { '¨' })[1],
                    Folder = File.ReadAllText(f).Split(new char[] { '¨' })[2],
                    Id = int.Parse(f.Replace(User.Folder + "\\", "").Replace(".nt", ""))
                };
            }

            throw new Exception("Ошибка чтения");
        }
    }
}
