using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tea_Notes
{
    public partial class FolderContextMenuStrip : UserControl
    {
        public int ParentId;

        public event EventHandler CreateNote;

        public event EventHandler CreateFolder;

        public event EventHandler Rename;

        public event EventHandler Delete;

        public FolderContextMenuStrip()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Rename(ParentId, e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Delete(ParentId, e);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CreateNote(ParentId, e);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CreateFolder(ParentId, e);
        }
    }
}
