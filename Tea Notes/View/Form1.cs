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

        public event Action<int> DeleteNote;

        public event Action<string, int> AddNote;

        public event Action<string, int> ChangeNote;

        public event Action<string, int> RenameNote;


        public event Action<int> DeleteFolder;

        public event Action<string, int> AddFolder;

        public event Action<string, int> RenameFolder;

        public event Action<IMainView> UpdateView;

        private int? _SelectedFolderId = null;

        private int? _SelectedNoteId = null;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateNotes();
        }

        private void UpdateNotes()
        {
            Notes.Clear();

            treeView1.Nodes.Clear();

            UpdateView(this);


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
                    var m = new FolderContextMenuStrip();
                    m.ParentId = f.Id;
                    m.CreateFolder += AddFolderF;
                    m.CreateNote += AddNoteF;
                    m.Delete += DeleteF;
                    m.Rename += RenameF;

                    treeView1.Nodes.Add(new TreeNode(f.Name)
                    {
                        Name = f.Id.ToString(),
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        ContextMenuStrip = m.contextMenuStrip1
                    });
                }

                else
                {
                    foreach (var n in collection)
                    {
                        if ((n as TreeNode).Name == f.ParentId.ToString())
                        {
                            var m = new FolderContextMenuStrip();
                            m.ParentId = f.Id;
                            m.CreateFolder += AddFolderF;
                            m.CreateNote += AddNoteF;
                            m.Delete += DeleteF;
                            m.Rename += RenameF;

                            (n as TreeNode).Nodes.Add(new TreeNode(f.Name) {
                                Name = f.Id.ToString(), 
                                ImageIndex = 1, SelectedImageIndex = 1,
                                ContextMenuStrip = m.contextMenuStrip1
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
                    var m = new NoteMenuStrip();
                    m.ParentId = f.Id;
                    m.Delete += DeleteN;
                    m.Rename += RenameN;

                    treeView1.Nodes.Add(new TreeNode(f.Name)
                    {
                        Name = "Note",
                        ImageIndex = 0,
                        SelectedImageIndex = 0,
                        ContextMenuStrip = m.contextMenuStrip3
                    });
                }

                else
                {
                    foreach (var n in collection)
                    {
                        if ((n as TreeNode).Name == f.FolderId.ToString())
                        {
                            var m = new NoteMenuStrip();
                            m.ParentId = f.Id;
                            m.Delete += DeleteN;
                            m.Rename += RenameN;

                            (n as TreeNode).Nodes.Add(new TreeNode(f.Name)
                            {
                                Name = "Note",
                                ImageIndex = 0,
                                SelectedImageIndex = 0,
                                ContextMenuStrip = m.contextMenuStrip3
                            });

                            return;
                        }

                        AddNote((n as TreeNode).Nodes, f);
                    }
                }
            }
        }

        private void RenameN(object sender, EventArgs e)
        {
            _SelectedFolderId = null;

            _SelectedNoteId = (int)sender;

            var n = new AskNameForm();

            n.Question = "Переименовать";

            n.Show();

            n.OK += RenameNF;
        }

        private void DeleteN(object sender, EventArgs e)
        {
            DeleteNote((int)sender);

            UpdateNotes();
        }

        private void RenameF(object sender, EventArgs e)
        {
            _SelectedNoteId = null;

            _SelectedFolderId = (int)sender;

            var n = new AskNameForm();

            n.Question = "Переименовать";

            n.Show();

            n.OK += RenameNF;
        }

        private void DeleteF(object sender, EventArgs e)
        {
            DeleteFolder((int)sender);

            UpdateNotes();
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
                        richTextBox1.Enabled = true;
                        richTextBox1.Text = Notes[i].Content;
                    }
                }
            }

            else
            {
                richTextBox1.Enabled = false;
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

        private void AddNoteF_OK(string obj)
        {
            if (_SelectedFolderId == null)
            {
                AddNote(obj, 0);

                UpdateNotes();
            }

            else
            {
                AddNote(obj, (int)_SelectedFolderId);

                UpdateNotes();
            }
        }

        private void AddFolderF_OK(string obj)
        {
            if (_SelectedFolderId == null)
            {
                AddFolder(obj, 0);

                UpdateNotes();
            }

            else
            {
                AddFolder(obj, (int)_SelectedFolderId);

                UpdateNotes();
            }
        }

        private void NoteChanged(object sender, EventArgs e)
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

        private void Rename(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                var n = new AskNameForm();

                n.Question = "Переименовать";

                n.Show();

                n.OK += RenameNF;
            }
        }

        private void RenameNF(string obj)
        {
            if(_SelectedFolderId != null)
            {
                RenameFolder(obj, (int)_SelectedFolderId);

                UpdateNotes();
            }

            if(_SelectedNoteId != null)
            {
                RenameNote(obj, (int)_SelectedNoteId);

                UpdateNotes();
            }
        }

        private void DeleteNF(object sender, EventArgs e)
        {
            Delete();
        }

        private void AddFolderF(object sender, EventArgs e)
        {
            _SelectedFolderId = (int)sender;
            var m = new AskNameForm();
            m.Question = "Введите название";
            m.Show();
            m.OK += AddFolderF_OK;
        }

        private void AddNoteF(object sender, EventArgs e)
        {
            _SelectedFolderId = (int)sender;
            var m = new AskNameForm();
            m.Question = "Введите название";
            m.Show();
            m.OK += AddNoteF_OK;
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

                                UpdateNotes();
                            }
                        }
                    }
                }

                else
                {
                    var l = GetFolders(treeView1.Nodes);

                    if (treeView1.SelectedNode != null)
                    {
                        DeleteFolder(int.Parse(treeView1.SelectedNode.Name));

                        UpdateNotes();
                    }
                }
            }
        }

        private void AddNoteP(object sender, EventArgs e)
        {
            var m = new AskNameForm();
            m.Question = "Введите название";
            m.Show();
            m.OK += q;

            void q(string s)
            {
                AddNote(s, 0);

                UpdateNotes();
            }
        }

        private void AddFolderP(object sender, EventArgs e)
        {
            var m = new AskNameForm();
            m.Question = "Введите название";
            m.Show();
            m.OK += q;

            void q(string s)
            {
                AddFolder(s, 0);

                UpdateNotes();
            }
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
