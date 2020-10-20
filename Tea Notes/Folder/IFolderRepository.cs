using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tea_Notes
{
    interface IFolderRepository
    {
        void Save(FolderDTO dto);

        FolderDTO[] GetAll();

        void Delete(FolderDTO dto);
    }
}
