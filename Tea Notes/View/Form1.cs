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

        public List<FolderDTO> Folders { get; set; }

        public NotesPresenter Presenter { get; set; } = new NotesPresenter();

        public event Action<int> DeleteNote;

        public event Action<string, int> AddNote;

        public event Action<string, int> ChangeNote;

        public event Action<string, int> RenameNote;


        public event Action<int> DeleteFolder;

        public event Action<string, int> AddFolder;

        public event Action<string, int> RenameFolder;


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


            var folders = Folders.ToList();

            while(folders.Count > 0)
            {
                var f = folders[0];

                AddFolder(treeView1.Nodes, f);

                folders.Remove(f);
            }

            var notes = Notes.ToList();

            while (notes.Count > 0)
            {
                var f = notes[0];

                AddNote(treeView1.Nodes, f);

                notes.Remove(f);
            }

            treeView1.ExpandAll();


            void AddFolder(TreeNodeCollection collection, FolderDTO f)
            {
                if (f.ParentId == 0)
                {
                    treeView1.Nodes.Add(new TreeNode(f.Name)
                    {
                        Name = f.Id.ToString(),
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        ContextMenuStrip = contextMenuStrip1
                    });
                }

                else
                {
                    foreach (var n in collection)
                    {
                        if ((n as TreeNode).Name == f.ParentId.ToString())
                        {
                            (n as TreeNode).Nodes.Add(new TreeNode(f.Name) {
                                Name = f.Id.ToString(), 
                                ImageIndex = 1, SelectedImageIndex = 1,
                                ContextMenuStrip = contextMenuStrip1
                            });

                            return;
                        }
                    }

                    AddFolder(collection, Folders.Find(x => x.Id == f.ParentId));
                }
            }


            void AddNote(TreeNodeCollection collection, NoteDTO f)
            {
                if (f.FolderId == 0)
                {
                    treeView1.Nodes.Add(new TreeNode(f.Name)
                    {
                        Name = "Note",
                        ImageIndex = 0,
                        SelectedImageIndex = 0,
                        ContextMenuStrip = contextMenuStrip3
                    });
                }

                else
                {
                    foreach (var n in collection)
                    {
                        if ((n as TreeNode).Name == f.FolderId.ToString())
                        {
                            (n as TreeNode).Nodes.Add(new TreeNode(f.Name)
                            {
                                Name = "Note",
                                ImageIndex = 0,
                                SelectedImageIndex = 0,
                                ContextMenuStrip = contextMenuStrip3
                            });

                            return;
                        }

                        AddNote((n as TreeNode).Nodes, f);
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name == "Note")
            {
                var l = GetNotes(treeView1.Nodes);

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
                Delete();
            }
        }

        private static List<TreeNode> GetNotes(TreeNodeCollection collection)
        {
            var l = new List<TreeNode>();

            foreach (var n in collection)
            {
                if ((n as TreeNode).Name != "Note")
                {
                    l.AddRange(GetNotes((n as TreeNode).Nodes));
                }
                else
                {
                    l.Add(n as TreeNode);
                }
            }
            return l;
        }

        private static List<TreeNode> GetFolders(TreeNodeCollection collection)
        {
            var l = new List<TreeNode>();

            foreach (var n in collection)
            {
                if ((n as TreeNode).Name != "Note")
                {
                    l.Add(n as TreeNode);

                    l.AddRange(GetFolders((n as TreeNode).Nodes));
                }
            }

            return l;
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
      
        }

        private void M_OK(string obj)
        {
            if (treeView1.SelectedNode == null)
            {
                AddNote(obj, 0);

                UpdateView();
            }

            else if (treeView1.SelectedNode.Name == "Note")
            {
                AddNote(obj, 0);

                UpdateView();
            }

            else
            {
                AddNote(obj, int.Parse(treeView1.SelectedNode.Name));

                UpdateView();
            }
        }

        private void M_OKF(string obj)
        {
            if (treeView1.SelectedNode == null)
            {
                AddFolder(obj, 0);

                UpdateView();
            }

            else if (treeView1.SelectedNode.Name == "Note")
            {
                AddFolder(obj, 0);

                UpdateView();
            }

            else
            {
                AddFolder(obj, int.Parse(treeView1.SelectedNode.Name));

                UpdateView();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null)
            {
                var i = 0;

                foreach(var n in GetNotes(treeView1.Nodes))
                {
                    if(n == treeView1.SelectedNode)
                    {
                        ChangeNote(richTextBox1.Text, i);
                    }

                    i++;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                var n = new AskNameForm("Переименовать");

                n.Show();

                n.OK += N_OK;
            }
        }

        private void N_OK(string obj)
        {
            if(treeView1.SelectedNode != null)
            {
                var i = 0;

                foreach(var n in GetNotes(treeView1.Nodes))
                {
                    if(n == treeView1.SelectedNode)
                    {
                        RenameNote(obj, i);

                        UpdateView();

                        return;
                    }

                    i++;
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            var m = new AskNameForm("Введите название");
            m.Show();
            m.OK += M_OKF;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            var m = new AskNameForm("Введите название");
            m.Show();
            m.OK += M_OK;
        }

        private void Delete()
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Name == "Note")
                {
                    var l = GetNotes(treeView1.Nodes);

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

                else
                {
                    var l = GetFolders(treeView1.Nodes);

                    if (l.Count > 0 && treeView1.SelectedNode != null)
                    {
                        for (var i = 0; i < l.Count; i++)
                        {
                            if (l[i] == treeView1.SelectedNode)
                            {
                                DeleteFolder(i);

                                UpdateView();
                            }
                        }
                    }
                }
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            var m = new AskNameForm("Введите название");
            m.Show();
            m.OK += q;

            void q(string s)
            {
                AddNote(s, 0);

                UpdateView();
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            var m = new AskNameForm("Введите название");
            m.Show();
            m.OK += q;

            void q(string s)
            {
                AddFolder(s, 0);

                UpdateView();
            }
        }
    }
}
