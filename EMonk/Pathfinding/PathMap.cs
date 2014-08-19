/*
 * PathMap https://github.com/EMonk72/EMonk
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
using System;
using System.Collections.Generic;
using System.Linq;
using EMonk.Collections;

namespace EMonk.Pathfinding
{
	/// <summary>Enumeration of simple movement directions</summary>
	[Flags]
	public enum MapDirection
	{
		Up = 1,
		Right = 2,
		Down = 4,
		Left = 8,

		UpRight = 16,
		DownRight = 32,
		DownLeft = 64,
		UpLeft = 128,

		None = 0,
		Orthogonal = Up | Right | Down | Left,
		Diagonal = UpRight | DownRight | DownLeft | UpLeft
	}

	/// <summary>Extension methods</summary>
	public static partial class tools
	{
		/// <summary>Test if a direction is orthogonal (vertical or horizontal)</summary>
		/// <param name="self">Direction to test</param>
		/// <returns>True if orthogonal, false otherwise</returns>
		public static bool IsOrthogonal(this MapDirection self) { return (self & MapDirection.Orthogonal) != 0; }

		/// <summary>Test if a direction is diagonal</summary>
		/// <param name="self">Direction to test</param>
		/// <returns>True if diagonal, false otherwise</returns>
		public static bool IsDiagonal(this MapDirection self) { return (self & MapDirection.Diagonal) != 0; }
	}

	/// <summary>Map containing possible moves from a specific location</summary>
	public class PathMap<TNode> : SparseMap<MapPosition, PFState<TNode>> 
		where TNode : class, IMapNode, new()
	{
		/// <summary>Generate a map of possible moves for the given map nodes, origin and maximum movement cost</summary>
		/// <param name="nodes">Collection of map nodes describing surrounding movement obstacles and movement costs</param>
		/// <param name="origin">Starting point of path map</param>
		/// <param name="MaxCost">Maximum cost to calculate for</param>
		/// <returns>PathMap instance containing all possible moves</returns>
		public static PathMap<TNode> Generate(IEnumerable<KeyValuePair<MapPosition, TNode>> nodes, MapPosition origin, double MaxCost)
		{
			// setup work-space from supplied nodes
			PathMap<TNode> work = new PathMap<TNode>(nodes);
			work._createonread = true;

			work[origin].IsOrigin = true;

			// init processing queue
			Queue<PFState<TNode>> process = new Queue<PFState<TNode>>();
			foreach (var nxt in work.GetAdjacent(origin, s => s.Passable))
				process.Enqueue(nxt);

			// process nodes
			int iterations = 0;
			while (process.Count > 0)
			{
				PFState<TNode> current = process.Dequeue();
				if (current.IsOrigin)
					continue;

				// get passable adjacent nodes
				PFState<TNode>[] adj = work.GetAdjacent(current, s => s.Passable);

				// find lowest cost movement to this node from adjacent nodes
				double minCost = double.PositiveInfinity;
				PFState<TNode> minNode = null;

				foreach (var nxt in adj)
				{
					double cost = current.CalcMoveCost(nxt);
					if (cost < minCost)
					{
						minCost = cost;
						minNode = nxt;
					}
				}

				// if movement found with less cost, update and re-process adjacent nodes
				if (minCost <= MaxCost && (!current.Visited || minCost < current.TotalCost))
				{
					// set low-cost node as prior node
					current.TotalCost = minCost;
					current.prevPosition = minNode;

					// reprocess all other adjacent nodes
					foreach (var nxt in adj)
					{
						if (nxt != minNode && !process.Contains(nxt))
							process.Enqueue(nxt);
					}
				}
				iterations++;
			}

			// generate results and return
			PathMap<TNode> res = new PathMap<TNode>(work.Filter(s => s.Value.Passable && (s.Value.prevPosition != null || s.Value.IsOrigin) && s.Value.TotalCost <= MaxCost));
			res.Iterations = iterations;
			return res;
		}

		#region Constructors (non public)		
		/// <summary>Copy constructor</summary>
		/// <param name="elements">Position-keyed map of PFState nodes</param>
		protected PathMap(IEnumerable<KeyValuePair<MapPosition, PFState<TNode>>> elements)
			: base(elements)
		{ }

		/// <summary>Initialised constructor</summary>
		/// <param name="elements">List of map nodes describing terrain and obstructions</param>
		protected PathMap(IEnumerable<KeyValuePair<MapPosition, TNode>> elements)
			: base()
		{
			foreach (var nxt in elements)
				this[nxt.Key] = new PFState<TNode>(nxt.Key.X, nxt.Key.Y, nxt.Value);
		}
		#endregion

		/// <summary>Diagnostic: count of nodes examined during generation of PathMap</summary>
		public int Iterations { get; private set; }

		/// <summary>Create a new value at the specified map position</summary>
		/// <param name="idx">Position to create value at</param>
		/// <returns>Created value</returns>
		protected override PFState<TNode> CreateAt(MapPosition idx)
		{
			PFState<TNode> res = new PFState<TNode>(idx);
			this[idx] = res;
			return res;
		}

		/// <summary>Get array of nodes that are adjacent to specified position</summary>
		/// <param name="pos">Position to get adjacent nodes for</param>
		/// <param name="fnFilter">Optional function to test each node with</param>
		/// <returns>Array of adjacent, non-null nodes that pass filtering</returns>
		public PFState<TNode>[] GetAdjacent(MapPosition pos, Func<PFState<TNode>, bool> fnFilter = null)
		{
			List<PFState<TNode>> res = new List<PFState<TNode>>();

			// Orthogonal nodes
			res.Add(this[pos + MapDirection.Up]);
			res.Add(this[pos + MapDirection.Right]);
			res.Add(this[pos + MapDirection.Down]);
			res.Add(this[pos + MapDirection.Left]);

			// Diagonal nodes
			res.Add(this[pos + MapDirection.UpRight]);
			res.Add(this[pos + MapDirection.DownRight]);
			res.Add(this[pos + MapDirection.DownLeft]);
			res.Add(this[pos + MapDirection.UpLeft]);

			// Filer list
			for (int i = res.Count - 1; i >= 0; --i)
			{
				if (null == res[i])
					res.RemoveAt(i);
				else if (null != fnFilter && !fnFilter(res[i]))
					res.RemoveAt(i);
			}

			return res.ToArray();
		}

		/// <summary>Get a list of map positions to reach target position from origin</summary>
		/// <param name="to">Target position of path</param>
		/// <param name="PathCost">Output, total cost of movement</param>
		/// <returns>List of positions in path, or null if no path found</returns>
		public MapPosition[] GetPath(MapPosition to, out double PathCost)
		{
			PathCost = double.PositiveInfinity;

			if (!data.ContainsKey(to))
			//if (!this.ContainsKey(to))
				return null;

			LinkedList<MapPosition> res = new LinkedList<MapPosition>();

			PFState<TNode> curr = this[to];
			PathCost = curr.TotalCost;

			while (curr != null)
			{
				res.AddFirst(new MapPosition(curr));
				curr = (curr.IsOrigin ? null : this[curr.prevPosition]);
			}

			return res.ToArray();
		}
	}
}
