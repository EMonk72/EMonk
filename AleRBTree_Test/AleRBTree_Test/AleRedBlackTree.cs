using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AleProjects.AleRedBlackTree
{
    class AleRBTreeNode<TKey, TData> where TKey : IComparable
    {
        private bool _IsRed;
        private int _Count;
        private TData _Data;
        private TKey _Key;

        private AleRBTreeNode<TKey, TData> _Left, _Right, _Parent;

        public AleRBTreeNode(TKey NewKey, TData NewData)
        {
            _IsRed = false;
            _Count = 1;
            _Data = NewData;
            _Key = NewKey;
            _Left = null;
            _Right = null;
            _Parent = null;
        }

        public bool IsRed { get { return _IsRed; } set { _IsRed = value; } }
        public bool IsBlack { get { return !_IsRed; } set { _IsRed = !value; } }
        public int Count { get { return _Count; } set { _Count = value; } }
        public TKey Key { get { return _Key; } set { _Key = value; } }
        public TData Data { get { return _Data; } set { _Data = value; } }
        public AleRBTreeNode<TKey, TData> Left { get { return _Left; } set { _Left = value; } }
        public AleRBTreeNode<TKey, TData> Right { get { return _Right; } set { _Right = value; } }
        public AleRBTreeNode<TKey, TData> Parent { get { return _Parent; } set { _Parent = value; } }

        public AleRBTreeNode<TKey, TData> Root
        {
            get
            {
                AleRBTreeNode<TKey, TData> Node;

                Node = this;
                while (Node._Parent != null) Node = Node._Parent;
                return Node;
            }
        }

        public AleRBTreeNode<TKey, TData> MostLeft
        {
            get
            {
                AleRBTreeNode<TKey, TData> Node;

                Node = this;
                while (Node._Left != null) Node = Node._Left;
                return Node;
            }
        }

        public AleRBTreeNode<TKey, TData> MostRight
        {
            get
            {
                AleRBTreeNode<TKey, TData> Node;

                Node = this;
                while (Node._Right != null) Node = Node._Right;
                return Node;
            }
        }

        public AleRBTreeNode<TKey, TData> Next
        {
            get
            {
                AleRBTreeNode<TKey, TData> N;

                if (_Right != null)
                {
                    N = _Right;
                    while (N._Left != null) N = N._Left;
                    return N;
                }
                else
                {
                    N = _Parent;
                    if ((N == null) || (N._Left == this)) return N;
                    while ((N._Parent != null) && (N._Parent._Left != N)) N = N._Parent;
                    N = N._Parent;
                    return N;
                }
            }
        }

        public AleRBTreeNode<TKey, TData> Prev
        {
            get
            {
                AleRBTreeNode<TKey, TData> N;

                if (_Left != null)
                {
                    N = Left;
                    while (N._Right != null) N = N._Right;
                    return N;
                }
                else
                {
                    N = _Parent;
                    if ((N == null) || (N._Right == this)) return N;
                    while ((N._Parent != null) && (N._Parent._Right != N)) N = N._Parent;
                    N = N._Parent;
                    return N;
                }
            }
        }

        private void ToArrayAux(AleRBTreeNode<TKey, TData>[] A, AleRBTreeNode<TKey, TData> node, int Lo, int Hi)
        {
            int i;

            if (node.Left == null) i = Lo;
            else if (node.Right == null) i = Hi;
            else i = Lo + node.Left.Count;

            A[i] = node;
            if (node.Left != null) ToArrayAux(A, node.Left, Lo, i - 1);
            if (node.Right != null) ToArrayAux(A, node.Right, i + 1, Hi);
        }

        public AleRBTreeNode<TKey, TData>[] ToArray()
        {
            AleRBTreeNode<TKey, TData>[] res = new AleRBTreeNode<TKey,TData>[_Count];
            ToArrayAux(res, this, 0, _Count - 1);
            return res;
        }

        public string DebugPrint(int Indent = 0)
        {
            string S, SIndent;

            SIndent = new string(' ', Indent * 11);

            S = "Node(Key=" + _Key.ToString() + "; Color=" + (_IsRed ? "red" : "black") + "; Count=" + _Count.ToString() + "; Data=" + _Data.ToString() + "\u000d\u000a";

            S += SIndent + "     Left =";
            if (_Left != null) S += _Left.DebugPrint(Indent + 1); else S += "null";

            S += "\u000d\u000a" + SIndent + "     Right=";
            if (_Right != null) S += _Right.DebugPrint(Indent + 1) + ")"; else S += "null)";

            return S;
        }

    }



    class AleRBTree<TNode, TKey, TData>
        where TNode : AleRBTreeNode<TKey, TData>
        where TKey : IComparable
    {
        private TNode _Root;

        public AleRBTree()
        {
            _Root = null;
        }

        protected void RotateLeft(TNode Node)
        {
            AleRBTreeNode<TKey, TData> P, N;

            if ((Node == null) || (Node.Right == null)) return;

            P = Node.Parent;
            N = Node.Right;

            if (P != null)
                if (P.Left == Node) P.Left = N; else P.Right = N;
            Node.Right = N.Left;
            if (Node.Right != null) Node.Right.Parent = Node;
            N.Parent = P;
            Node.Parent = N;
            N.Left = Node;

            //correct count property
            Node.Count = 1;
            if (Node.Left != null) Node.Count += Node.Left.Count;
            if (Node.Right != null) Node.Count += Node.Right.Count;
            N.Count = 1;
            if (N.Left != null) N.Count += N.Left.Count;
            if (N.Right != null) N.Count += N.Right.Count;
        }

        protected void RotateRight(TNode Node)
        {
            AleRBTreeNode<TKey, TData> P, N;

            if ((Node == null) || (Node.Left == null)) return;

            P = Node.Parent;
            N = Node.Left;

            if (P != null)
                if (P.Right == Node) P.Right = N; else P.Left = N;
            Node.Left = N.Right;
            if (Node.Left != null) Node.Left.Parent = Node;
            N.Parent = P;
            Node.Parent = N;
            N.Right = Node;

            //correct count property
            Node.Count = 1;
            if (Node.Left != null) Node.Count += Node.Left.Count;
            if (Node.Right != null) Node.Count += Node.Right.Count;
            N.Count = 1;
            if (N.Left != null) N.Count += N.Left.Count;
            if (N.Right != null) N.Count += N.Right.Count;
        }

        public TNode Root
        {
            get 
            { 
                return _Root; 
            }
            set
            {
                if (value == null) _Root = null;
            }
        }

        public int Count
        {
            get
            {
                if (_Root == null) return 0; else return _Root.Count;
            }
        }

        public TNode Find(TKey Key, out int Result)
        {
            int i;
            AleRBTreeNode<TKey, TData> N;

            if (_Root == null)
            {
                Result = 1;
                return null;
            }
            N = _Root;

            while (true)
            {
                i = Key.CompareTo(N.Key);

                if (i > 0)
                {
                    if (N.Right == null)
                    {
                        Result = 1;
                        return (TNode)N;
                    }
                    N = N.Right;
                }
                else if (i < 0)
                {
                    if (N.Left == null)
                    {
                        Result = -1;
                        return (TNode)N;
                    }
                    N = N.Left;
                }
                else
                {
                    Result = 0;
                    return (TNode)N;
                }
            }
        }

        public TNode Find(TKey Key)
        {
            int i;
            AleRBTreeNode<TKey, TData> N;

            N = Find(Key, out i);
            if (i != 0) N = null;
            return (TNode)N;
        }

        public void InsertNode(TNode Node)
        {
            int i;
            AleRBTreeNode<TKey, TData> M, P, C;

            if ((Node == null) || (Node.Left != null) || (Node.Right != null) || (Node.Parent != null)) return;

            if (_Root == null)
            {
                _Root = Node;
                return;
            }

            M = Find(Node.Key, out i);

            if (i == 0) // replace node
            {
                Node.Left = M.Left;
                if (Node.Left != null) Node.Left.Parent = Node;
                Node.Right = M.Right;
                if (Node.Right != null) Node.Right.Parent = Node;
                Node.Parent = M.Parent;
                Node.IsRed = M.IsRed;
                Node.Count = M.Count;
                if (M.Parent != null)
                    if (M.Parent.Left == M) M.Parent.Left = Node; else M.Parent.Right = Node;
                if (_Root == M) _Root = Node;
                M.Left = null;
                M.Right = null;
                M.Parent = null;
                return;
            }

            //correct Count property
            C = M;
            do
            {
                C.Count++;
                C = C.Parent;
            } while (C != null);

            C = M;
            Node.Parent = M;
            Node.IsRed = true;
            if (i > 0) M.Right = Node; else M.Left = Node;

            //modify RBTree on insert
            while ((Node.Parent != null) && (Node.Parent.IsRed))
            {
                P = Node.Parent.Parent;

                if (Node.Parent == P.Left)
                {
                    M = P.Right;
                    if ((M != null) && (M.IsRed))
                    {
                        Node.Parent.IsBlack = true;
                        M.IsBlack = true;
                        P.IsRed = true;
                        Node = (TNode)P;
                    }
                    else
                    {
                        if (Node == Node.Parent.Right)
                        {
                            Node = (TNode)Node.Parent;
                            RotateLeft(Node);
                        }
                        Node.Parent.IsBlack = true;
                        P.IsRed = true;
                        RotateRight((TNode)P);
                    }
                }
                else
                {
                    M = P.Left;
                    if ((M != null) && (M.IsRed))
                    {
                        Node.Parent.IsBlack = true;
                        M.IsBlack = true;
                        P.IsRed = true;
                        Node = (TNode)P;
                    }
                    else
                    {
                        if (Node == Node.Parent.Left)
                        {
                            Node = (TNode)Node.Parent;
                            RotateRight(Node);
                        }
                        Node.Parent.IsBlack = true;
                        P.IsRed = true;
                        RotateLeft((TNode)P);
                    }
                }
            }

            C = C.Root;
            C.IsBlack = true;
            _Root = (TNode)C;
        }

        public void DeleteNode(TNode Node)
        {
            int i;
            AleRBTreeNode<TKey, TData> M, N, P, C, T;

            if (Node.Root != _Root) return; // node doesn't belong to this rb-tree

            M = Node;

            if (M.Left == null) C = M.Right;
            else if (M.Right == null) C = M.Left;
            else
            {
                M = M.Right;
                while (M.Left != null) M = M.Left;
                C = M.Right;
            }

            if (M != Node)
            {
                //Correction of Count property
                M.Count = Node.Count - 1;
                T = M.Parent;
                while (T != null)
                {
                    T.Count--;
                    T = T.Parent;
                }

                Node.Left.Parent = M;
                M.Left = Node.Left;
                if (M != Node.Right)
                {
                    P = M.Parent;
                    if (C != null) C.Parent = P;
                    P.Left = C;
                    M.Right = Node.Right;
                    Node.Right.Parent = M;
                }
                else P = M;

                if (Node.Parent != null)
                    if (Node.Parent.Left == Node) Node.Parent.Left = M; else Node.Parent.Right = M;
                M.Parent = Node.Parent;
                i = (M.IsRed ? 1 : 0);
                M.IsRed = Node.IsRed;
                Node.IsRed = (i != 0);
            }
            else
            {
                //Correction of Count property
                T = Node.Parent;
                while (T != null)
                {
                    T.Count--;
                    T = T.Parent;
                }

                P = M.Parent;
                if (C != null) C.Parent = P;
                if (Node.Parent != null)
                    if (Node.Parent.Left == Node) Node.Parent.Left = C; else Node.Parent.Right = C;
                i = (M.IsRed ? 1 : 0);
            }

            if ((M == Node) && (M.Left == null) && (M.Right == null) && (M.Parent == null))
            {
                _Root = null;
                return;
            }

            Node.Left = null;
            Node.Right = null;
            Node.Parent = null;
            if (P == null) Node = (TNode)C; else Node = (TNode)P;

            //Modify RBTree on delete

            if (i == 0) //black
            {
                while ((P != null) && ((C == null) || (C.IsBlack)))
                {
                    if (C == P.Left)
                    {
                        M = P.Right;
                        if (M.IsRed)
                        {
                            M.IsBlack = true;
                            P.IsRed = true;
                            RotateLeft((TNode)P);
                            M = P.Right;
                        }
                        if (((M.Left == null) || (M.Left.IsBlack)) && ((M.Right == null) || (M.Right.IsBlack)))
                        {
                            M.IsRed = true;
                            C = P;
                            P = P.Parent;
                        }
                        else
                        {
                            if ((M.Right == null) || (M.Right.IsBlack))
                            {
                                M.Left.IsBlack = true;
                                M.IsRed = true;
                                RotateRight((TNode)M);
                                M = P.Right;
                            }
                            M.IsRed = P.IsRed;
                            P.IsBlack = true;
                            if (M.Right != null) M.Right.IsBlack = true;
                            RotateLeft((TNode)P);
                            break;
                        }
                    }
                    else //if C=P.Right
                    {
                        M = P.Left;
                        if (M.IsRed)
                        {
                            M.IsBlack = true;
                            P.IsRed = true;
                            RotateLeft((TNode)P);
                            M = P.Left;
                        }
                        if (((M.Right == null) || (M.Right.IsBlack)) && ((M.Left == null) || (M.Left.IsBlack)))
                        {
                            M.IsRed = true;
                            C = P;
                            P = P.Parent;
                        }
                        else
                        {
                            if ((M.Left == null) || (M.Left.IsBlack))
                            {
                                M.Right.IsBlack = true;
                                M.IsRed = true;
                                RotateLeft((TNode)M);
                                M = P.Left;
                            }
                            M.IsRed = P.IsRed;
                            P.IsBlack = true;
                            if (M.Left != null) M.Left.IsBlack = true;
                            RotateRight((TNode)P);
                            break;
                        }
                    }
                } //while

                if (C != null) C.IsBlack = true;
            } //black

            _Root = (TNode)Node.Root;
        }

        public void DeleteNode(TKey Key)
        {
            int i;
            TNode Node;

            Node = (TNode)Find(Key, out i);
            if (i == 0) DeleteNode(Node);
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            AleRBTreeNode<TKey, TData> node;

            if (_Root != null) node = _Root.MostLeft; else node = null;

            while (node != null)
            {
                yield return (TNode)node;
                node = node.Next;
            }
        }

        public IEnumerable<TNode> Reverse()
        {
            AleRBTreeNode<TKey, TData> node;

            if (_Root != null) node = _Root.MostRight; else node = null;

            while (node != null)
            {
                yield return (TNode)node;
                node = node.Prev;
            }
        }

        public string DebugPrint()
        {
            if (_Root != null) return _Root.DebugPrint(); else return "";
        }
    }

}
