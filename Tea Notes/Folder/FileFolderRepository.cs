﻿using System;
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
            File.WriteAllText(User.Folder + "\\" + dto.Id + ".fd", dto.Name);
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
                Name = File.ReadAllText(f)
            };
        }
    }
}