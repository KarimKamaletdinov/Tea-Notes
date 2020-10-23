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
    public partial class NoteMenuStrip : UserControl
    {
        public int ParentId;

        public event EventHandler Rename;

        public event EventHandler Delete;

        public NoteMenuStrip()
        {
            InitializeComponent();
        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Rename(ParentId, e);
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            Delete(ParentId, e);
        }
    }
}
