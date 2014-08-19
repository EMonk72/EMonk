using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AleProjects.AleRedBlackTree;

namespace AleRBTree_Test
{
    public partial class Form1 : Form
    {
        private AleRBTree<AleRBTreeNode<int, string>, int, string> tree;
        
        public Form1()
        {
            InitializeComponent();
            tree = new AleRBTree<AleRBTreeNode<int, string>, int, string>();
        }

        private void AddToTreeView(AleRBTreeNode<int, string> node, TreeNode tvNode)
        {
            if (node == null) return;

            TreeNode newtvNode;
            int i;

            i=(node.IsBlack ? 0 : 1);
            newtvNode = new TreeNode("Key=" + node.Key.ToString() + "; Data=" + node.Data.ToString() + "; Count=" + node.Count, i, i);
            tvNode.Nodes.Add(newtvNode);
            AddToTreeView(node.Left, newtvNode);
            AddToTreeView(node.Right, newtvNode);
        }

        private void ShowRBTree()
        {
            tvRB.Nodes.Clear();
            if (tree.Count==0) return;

            tvRB.Nodes.Add(tree.Root.Key.ToString(), "Key=" + tree.Root.Key.ToString() + "; Data=" + tree.Root.Data.ToString() + "; Count=" + tree.Root.Count, (tree.Root.IsBlack ? 0 : 1));
            AddToTreeView(tree.Root.Left, tvRB.Nodes[0]);
            AddToTreeView(tree.Root.Right, tvRB.Nodes[0]);

            tvRB.ExpandAll();
        }

        private void btnBuildTree_Click(object sender, EventArgs e)
        {
            AleRBTreeNode<int, string> node;
            int i, k;
            Random rnd = new Random();

            for (i = 1; i <= 5; i++)
            {
                k = rnd.Next(100); 
                //k = i;
                node = new AleRBTreeNode<int, string>(k, "Data" + k.ToString());
                tree.InsertNode(node);
            }

            ShowRBTree();
        }

        private void btnIterate_Click(object sender, EventArgs e)
        {

            tvRB.Nodes.Clear();
            
            foreach (AleRBTreeNode<int, string> node in tree.Reverse())
            {
                tvRB.Nodes.Add(node.Key.ToString(), "Key=" + node.Key.ToString() + "; Data=" + node.Data.ToString() + "; Count=" + node.Count, (node.IsBlack ? 0 : 1));
            }
            
            
            /*
            // another way to iterate tree
            AleRBTreeNode<int, string> node;
            
            if (tree.Count != 0) node = tree.Root.MostLeft; else node = null;
            while (node != null)
            {
                tvRB.Nodes.Add(node.Key.ToString(), "Key=" + node.Key.ToString() + "; Data=" + node.Data.ToString() + "; Count=" + node.Count, (node.IsBlack ? 0 : 1));
                node = node.Next;
            }
            */

            /*
            // yet another way to iterate tree
            AleRBTreeNode<int, string>[] A;
            A = tree.Root.ToArray();

            for (int i = 0; i < A.Length; i++)
            {
                tvRB.Nodes.Add(A[i].Key.ToString(), "Key=" + A[i].Key.ToString() + "; Data=" + A[i].Data.ToString() + "; Count=" + A[i].Count, (A[i].IsBlack ? 0 : 1));
            }
            */

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            AleRBTreeNode<int, string> node;
            int i, k;

            try { i = Convert.ToInt32(txtKey.Text); }
            catch { i = -1; }
            node = tree.Find(i, out k);
            if (k == 0) tree.DeleteNode(node);

            ShowRBTree();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tree.Root = null;
            ShowRBTree();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            AleRBTreeNode<int, string> node;
            int i, k;

            try
            {
                i = Convert.ToInt32(txtKey.Text);
                node = tree.Find(i, out k);
                if (k == 0) lblData.Text = node.Data.ToString(); else lblData.Text = "not found";
            }
            catch
            {
                lblData.Text = "not found";
            }
        }
    }
}
