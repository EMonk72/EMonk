/*
 * PFState https://github.com/EMonk72/EMonk
 * 
 * Copyright (c) 2013 EMonk
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 */

namespace EMonk.Pathfinding
{
	/// <summary>Maintains state of a node in the path-finding algorithm</summary>
	/// <typeparam name="TNode">Type of map tile contained</typeparam>
	public class PFState<TNode> : MapPosition
		where TNode : class, IMapNode, new()
	{
		/// <summary>Contained map node, may be empty</summary>
		public readonly TNode Content;

		/// <summary>True if this represents the origin of the path-finding search</summary>
		public bool IsOrigin;

		/// <summary>True if node may be entered</summary>
		public bool Passable { get { return null == Content  ? true : Content.Passable; } }

		/// <summary>Cost of orthogonal movement into this node</summary>
		public double MoveCost { get { return null == Content ? 1 : Content.MoveCost; } }

		/// <summary>Best position to move here from</summary>
		public MapPosition prevPosition;
		/// <summary>Total cost of moving to this node from path origin</summary>
		public double TotalCost;

		/// <summary>True if this is the origin node or has a path to the origin node</summary>
		public bool Visited { get { return IsOrigin || prevPosition != null; } }

		/// <summary>Value constructor</summary>
		/// <param name="x">Horizontal part of position</param>
		/// <param name="y">Vertical part of position</param>
		/// <param name="content">Node at position</param>
		public PFState(int x, int y, TNode content)
			: base(x, y)
		{
			Content = content;
		}

		/// <summary>Position-only constructor</summary>
		/// <param name="x">Horizontal part of position</param>
		/// <param name="y">Vertical part of position</param>
		public PFState(int x, int y)
			: this(x, y, null)
		{ }

		/// <summary>Position-only constructor</summary>
		/// <param name="p">Position of node</param>
		public PFState(MapPosition p)
			: this(p.X, p.Y, null)
		{ }

		/// <summary>Default constructor</summary>
		public PFState()
			: this(int.MaxValue, int.MaxValue, null)
		{ }

		/// <summary>Get cost of moving to this node from the supplied other node</summary>
		/// <typeparam name="T">Content type of other node, must derive from IMapNode</typeparam>
		/// <param name="from">Other node to move from</param>
		/// <returns>Total cost of movement, or +Infinity on invalid move</returns>
		public double CalcMoveCost<T>(PFState<T> from) 
			where T: class, IMapNode, new()
		{
			if (!from.Visited)
				return double.PositiveInfinity;
			MapDirection dir = DirectionFrom(from);
			double mult = dir.IsOrthogonal() ? 1 : 1.5;

			return from.TotalCost + MoveCost * mult;
		}
	}
}
