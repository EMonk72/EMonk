using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMonk.Collections
{
	public class AleRBTree<TKey, TData> : IEnumerable<KeyValuePair<TKey, TData>>
		where TKey : IComparable
	{
		public class Node
		{
			private bool _IsRed;
			private int _Count;
			private TData _Data;
			private TKey _Key;

			private Node _Left, _Right, _Parent;

			public Node(TKey NewKey, TData NewData)
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
			public Node Left { get { return _Left; } set { _Left = value; } }
			public Node Right { get { return _Right; } set { _Right = value; } }
			public Node Parent { get { return _Parent; } set { _Parent = value; } }

			public Node Root
			{
				get
				{
					Node Node;

					Node = this;
					while (Node._Parent != null) Node = Node._Parent;
					return Node;
				}
			}

			public Node LeftMost
			{
				get
				{
					Node Node;

					Node = this;
					while (Node._Left != null) Node = Node._Left;
					return Node;
				}
			}

			public Node RightMost
			{
				get
				{
					Node Node;

					Node = this;
					while (Node._Right != null) Node = Node._Right;
					return Node;
				}
			}

			public Node Next
			{
				get
				{
					Node N;

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

			public Node Prev
			{
				get
				{
					Node N;

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

			private static void ToArrayAux(Node[] A, Node node, int Lo, int Hi)
			{
				int i;

				if (node.Left == null) i = Lo;
				else if (node.Right == null) i = Hi;
				else i = Lo + node.Left.Count;

				A[i] = node;
				if (node.Left != null) ToArrayAux(A, node.Left, Lo, i - 1);
				if (node.Right != null) ToArrayAux(A, node.Right, i + 1, Hi);
			}

			public Node[] ToArray()
			{
				Node[] res = new Node[_Count];
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

		
		private Node _Root;

		public AleRBTree()
		{
			_Root = null;
		}

		protected void RotateLeft(Node Node)
		{
			Node P, N;

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

		protected void RotateRight(Node Node)
		{
			Node P, N;

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

		public Node Root
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

		public Node Find(TKey Key, out int Result)
		{
			int i;
			Node N;

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
						return (Node)N;
					}
					N = N.Right;
				}
				else if (i < 0)
				{
					if (N.Left == null)
					{
						Result = -1;
						return (Node)N;
					}
					N = N.Left;
				}
				else
				{
					Result = 0;
					return (Node)N;
				}
			}
		}

		public Node Find(TKey Key)
		{
			int i;
			Node N;

			N = Find(Key, out i);
			if (i != 0) N = null;
			return (Node)N;
		}

		public void InsertNode(Node Node)
		{
			int i;
			Node M, P, C;

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
						Node = (Node)P;
					}
					else
					{
						if (Node == Node.Parent.Right)
						{
							Node = (Node)Node.Parent;
							RotateLeft(Node);
						}
						Node.Parent.IsBlack = true;
						P.IsRed = true;
						RotateRight((Node)P);
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
						Node = (Node)P;
					}
					else
					{
						if (Node == Node.Parent.Left)
						{
							Node = (Node)Node.Parent;
							RotateRight(Node);
						}
						Node.Parent.IsBlack = true;
						P.IsRed = true;
						RotateLeft((Node)P);
					}
				}
			}

			C = C.Root;
			C.IsBlack = true;
			_Root = (Node)C;
		}

		public void DeleteNode(Node Node)
		{
			int i;
			Node M, P, C, T;

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
			if (P == null) Node = (Node)C; else Node = (Node)P;

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
							RotateLeft((Node)P);
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
								RotateRight((Node)M);
								M = P.Right;
							}
							M.IsRed = P.IsRed;
							P.IsBlack = true;
							if (M.Right != null) M.Right.IsBlack = true;
							RotateLeft((Node)P);
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
							RotateLeft((Node)P);
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
								RotateLeft((Node)M);
								M = P.Left;
							}
							M.IsRed = P.IsRed;
							P.IsBlack = true;
							if (M.Left != null) M.Left.IsBlack = true;
							RotateRight((Node)P);
							break;
						}
					}
				} //while

				if (C != null) C.IsBlack = true;
			} //black

			_Root = (Node)Node.Root;
		}

		public void DeleteNode(TKey Key)
		{
			int i;
			Node Node;

			Node = (Node)Find(Key, out i);
			if (i == 0) DeleteNode(Node);
		}

		public TData this[TKey key]
		{
			get
			{
				Node res = Find(key);
				if (res == null)
					return default(TData);
				return res.Data;
			}
			set
			{
				InsertNode(new Node(key, value));
			}
		}

		public KeyValuePair<TKey, TData> LeftMost
		{
			get
			{
				if (_Root == null)
					return default(KeyValuePair<TKey, TData>);
				Node n = _Root.LeftMost;
				return new KeyValuePair<TKey, TData>(n.Key, n.Data);
			}
		}

		public bool ContainsKey(TKey Key)
		{
			return Find(Key) != null;
		}

		public KeyValuePair<TKey, TData> RightMost
		{
			get
			{
				if (_Root == null)
					return default(KeyValuePair<TKey, TData>);
				Node n = _Root.RightMost;
				return new KeyValuePair<TKey, TData>(n.Key, n.Data);
			}
		}

		public TKey LeftMostKey
		{
			get
			{
				if (_Root == null)
					return default(TKey);
				return _Root.LeftMost.Key;
			}
		}

		public TKey RightMostKey
		{
			get
			{
				if (_Root == null)
					return default(TKey);
				return _Root.RightMost.Key;
			}
		}

		/*
		public IEnumerator<Node> GetEnumerator()
		{
			Node node;

			if (_Root != null) node = _Root.MostLeft; else node = null;

			while (node != null)
			{
				yield return (Node)node;
				node = node.Next;
			}
		}

		public IEnumerable<Node> Reverse()
		{
			Node node;

			if (_Root != null) node = _Root.MostRight; else node = null;

			while (node != null)
			{
				yield return (Node)node;
				node = node.Prev;
			}
		}
		// */

		public string DebugPrint()
		{
			if (_Root != null) return _Root.DebugPrint(); else return "";
		}

		#region IEnumerable implementation
		public IEnumerator<KeyValuePair<TKey, TData>> GetEnumerator()
		{
			if (_Root == null)
				yield break;

			Node curr = _Root.LeftMost;
			while (curr != null)
			{
				yield return new KeyValuePair<TKey, TData>(curr.Key, curr.Data);
				curr = curr.Next;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TData>>)this).GetEnumerator();
		}

		public IEnumerable<KeyValuePair<TKey, TData>> Reverse()
		{
			if (_Root == null)
				yield break;

			Node curr = _Root.RightMost;
			while (curr != null)
			{
				yield return new KeyValuePair<TKey, TData>(curr.Key, curr.Data);
				curr = curr.Prev;
			}
		}
		#endregion
	}


	public class RBTree<T>
	{
		public class Node
		{
			public Node() { }

			public Node(T value)
				: this(value, true)
			{ }

			public Node(T value, bool isRed)
			{
				Item = value;
				IsRed = isRed;
			}

			public T Item;
			public Node Left;
			public Node Right;
			public Node Parent;
			public bool IsRed;
		}

	
		public RBTree() 
			: this(Comparer<T>.Default)
		{ }

		public RBTree(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		public Node root;
		int count, version;
		IComparer<T> comparer;

		public void Add(T item)
		{
			if (this.root == null)
			{
				this.root = new Node(item, false);
				this.count = 1;
				this.version++;
				return;
			}

			Node root = this.root;
			Node node = null;
			Node grandParent = null;
			Node greatGrandParent = null;
			this.version++;

			int num = 0;
			while (root != null)
			{
				num = this.comparer.Compare(item, root.Item);
				if (num == 0)
				{
					this.root.IsRed = false;
					return;
				}
				if (Is4Node(root))
				{
					Split4Node(root);
					if (IsRed(node))
					{
						this.InsertionBalance(root, ref node, grandParent, greatGrandParent);
					}
				}
				greatGrandParent = grandParent;
				grandParent = node;
				node = root;
				root = (num < 0) ? root.Left : root.Right;
			}
			Node current = new Node(item);
			if (num > 0)
			{
				node.Right = current;
			}
			else
			{
				node.Left = current;
			}
			if (node.IsRed)
			{
				this.InsertionBalance(current, ref node, grandParent, greatGrandParent);
			}
			this.root.IsRed = false;
			this.count++;
		}

		public bool Contains(T item)
		{
			Node curr = root;
			while (curr != null)
			{
				int comp = comparer.Compare(curr.Item, item);
				if (comp == 0)
					return true;
				if (comp < 0)
					curr = curr.Left;
				else
					curr = curr.Right;
			}
			return false;
		}

		private static bool IsRed(Node node)
		{
			return ((node != null) && node.IsRed);
		}

		private static bool Is4Node(Node node)
		{
			return (IsRed(node.Left) && IsRed(node.Right));
		}

		private static void Split4Node(Node node)
		{
			node.IsRed = true;
			node.Left.IsRed = false;
			node.Right.IsRed = false;
		}

		private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
		{
			Node node;
			bool flag = grandParent.Right == parent;
			bool flag2 = parent.Right == current;
			if (flag == flag2)
			{
				node = flag2 ? RotateLeft(grandParent) : RotateRight(grandParent);
			}
			else
			{
				node = flag2 ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent);
				parent = greatGrandParent;
			}
			grandParent.IsRed = true;
			node.IsRed = false;
			ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, node);
		}

		private static Node RotateLeft(Node node)
		{
			Node right = node.Right;
			node.Right = right.Left;
			right.Left = node;
			return right;
		}

		private static Node RotateRight(Node node)
		{
			Node left = node.Left;
			node.Left = left.Right;
			left.Right = node;
			return left;
		}

		private static Node RotateLeftRight(Node node)
		{
			Node left = node.Left;
			Node right = left.Right;
			node.Left = right.Right;
			right.Right = node;
			left.Right = right.Left;
			right.Left = left;
			return right;
		}

		private static Node RotateRightLeft(Node node)
		{
			Node right = node.Right;
			Node left = right.Left;
			node.Right = left.Left;
			left.Left = node;
			right.Left = left.Right;
			left.Right = right;
			return left;
		}

		private void ReplaceChildOfNodeOrRoot(Node parent, Node child, Node newChild)
		{
			if (parent != null)
			{
				if (parent.Left == child)
				{
					parent.Left = newChild;
				}
				else
				{
					parent.Right = newChild;
				}
			}
			else
			{
				this.root = newChild;
			}
		}

		public T First
		{
			get
			{
				if (root == null)
					return default(T);
				Node curr = root;
				while (curr.Left != null)
					curr = curr.Left;
				return curr.Item;
			}
		}

		public T Last
		{
			get
			{
				if (root == null)
					return default(T);
				Node curr = root;
				while (curr.Right != null)
					curr = curr.Right;
				return curr.Item;
			}
		}
	}

	// public class RBDictionary<TKey, TValue> : RBTree<KeyValueP>
}
