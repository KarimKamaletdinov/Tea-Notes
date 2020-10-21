using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    class FileFolderRepository : IFolderRepository
    {
        private UserDTO User;

        public FileFolderRepository(UserDTO user)
        {
            User = user;
        }

        public void Save(FolderDTO dto)
        {
            File.WriteAllText(User.Folder + "\\" + dto.Id + ".fd", dto.Name +
                "¨" + dto.ParentId);
        }

        public void Delete(FolderDTO dto)
        {
            File.Delete(User.Folder + "\\" + dto.Id + ".fd");
        }

        public FolderDTO[] GetAll()
        {
            var l = new List<FolderDTO>();

            foreach (var f in Directory.GetFiles(User.Folder).ToList().
                Where(x => x.EndsWith(".fd")))
            {
                l.Add(ParseDTO(f));
            }

            return l.ToArray();
        }

        private static FolderDTO ParseDTO(string f)
        {
            return new FolderDTO()
            {
                Name = File.ReadAllText(f).Split(new char[] { '¨' })[0],
                ParentId = int.Parse(File.ReadAllText(f).Split(new char[] { '¨' })[1]),
                Id = int.Parse(f.Replace("Notes\\", "").Replace(".fd", ""))
            };
        }

        public void Add(FolderDTO dto)
        {
            File.WriteAllText(User.Folder + "\\" + GetMaxId() + ".fd", dto.Name +
              "¨" + dto.ParentId);
        }

        private int GetMaxId()
        {
            var id = 0;

            foreach (var n in Directory.GetFiles(User.Folder))
            {
                if (!n.EndsWith(".nt"))
                {
                    if (int.Parse(n.Replace(User.Folder + "\\", "").Replace(".fd", "")) >= id)
                    {
                        id = int.Parse(n.Replace(User.Folder + "\\", "").Replace(".fd", "")) + 1;
                    }
                }
            }

            return id;
        }
    }
}
