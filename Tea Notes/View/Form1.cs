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

        public event Action<int> DeleteNote;

        public event Action<string> AddNote;

        public event Action<string, int> ChangeNote;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Presenter.Start(this);

            UpdateView();
        }

        private void UpdateView()
        {
            Notes.Clear();

            treeView1.Nodes.Clear();

            Presenter.UpdateView(this);

            foreach (var note in Notes)
            {
                if (note.Folder != "")
                {
                    if (FolderCreated(note.Folder, out var t))
                    {
                        t.Nodes.Add(new TreeNode(note.Name));
                    }

                    else
                    {
                        var f = new TreeNode(note.Folder);
                        f.Name = "Folder";
                        f.Nodes.Add(note.Name);
                        treeView1.Nodes.Add(f);
                    }
                }
                else
                {
                    treeView1.Nodes.Add(note.Name);
                }
            }

            bool FolderCreated(string fn, out TreeNode t)
            {
                foreach (var n in treeView1.Nodes)
                {
                    if ((n as TreeNode).Name == "Folder" &&
                        (n as TreeNode).Text == fn)
                    {
                        t = n as TreeNode;

                        return true;
                    }
                }

                t = null;

                return false;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name != "Folder")
            {
                var l = GetNodes(treeView1.Nodes);

                for (var i = 0; i < l.Count; i++)
                {
                    if (l[i] == e.Node)
                    {
                        richTextBox1.Text = Notes[i].Content;
                    }
                }
            }

            else
            {
                richTextBox1.Text = "";
            }    
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                var l = GetNodes(treeView1.Nodes);
                if (l.Count > 0 && treeView1.SelectedNode != null)
                {
                    for (var i = 0; i < l.Count; i++)
                    {
                        if (l[i] == treeView1.SelectedNode)
                        {
                            DeleteNote(i);

                            UpdateView();
                        }
                    }
                }
            }
        }

        private static List<TreeNode> GetNodes(TreeNodeCollection collection)
        {
            var l = new List<TreeNode>();

            foreach (var n in collection)
            {
                if ((n as TreeNode).Name == "Folder")
                {
                    l.AddRange(GetNodes((n as TreeNode).Nodes));
                }
                else
                {
                    l.Add(n as TreeNode);
                }
            }
            return l;
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            var m = new AskNameForm("Введите название");
            m.Show();
            m.OK += M_OK;
        }

        private void M_OK(string obj)
        {
            AddNote(obj);

            UpdateView();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null)
            {
                var i = 0;

                foreach(var n in GetNodes(treeView1.Nodes))
                {
                    if(n == treeView1.SelectedNode)
                    {
                        ChangeNote(richTextBox1.Text, i);
                    }

                    i++;
                }
            }
        }
    }
}
