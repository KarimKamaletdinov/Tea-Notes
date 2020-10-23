using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tea_Notes
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var f = new Form1();

            var p = new NotesPresenter();

            p.Start(f, new SqlNoteRepository(new SqlUserRepository().GetAll().Find(x => x.Id
                    == int.Parse(File.ReadAllText("CurrentUser.txt")))),
                new SqlFolderRepository(new SqlUserRepository().GetAll().Find(x => x.Id
                    == int.Parse(File.ReadAllText("CurrentUser.txt")))));

            Application.Run(f);
        }
    }
}
