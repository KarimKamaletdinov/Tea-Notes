using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tea_Notes
{
    partial class Form1 : Form, IMainView
    {
        public List<NoteDTO> Notes { get; set; } = new List<NoteDTO>();

        public NotesPresenter Presenter { get; set; } = new NotesPresenter();

        public List<FolderDTO> Folders { get; set; } = new List<FolderDTO>();

        public Form1()
        {
            InitializeComponent();
        }     

        private void Form1_Load(object sender, EventArgs e)
        {
            Presenter.Start(this);

            foreach(var note in Notes)
            {
                

                treeView1.Nodes.Add(note.Name);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.NodeFont.Size != 20)
            {
                var l = new List<TreeNode>();

                foreach (var n in treeView1.Nodes)
                {
                    if ((n as TreeNode).Nodes.Count == 0)
                    {
                        l.Add(n as TreeNode);
                    }
                }


                for (var i = 0; i < l.Count; i++)
                {
                    if (l[i] == e.Node)
                    {
                        richTextBox1.Text = Notes[i].Content;
                    }
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            
        }
    }
}
