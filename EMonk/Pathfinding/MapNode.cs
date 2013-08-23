﻿/*
 * MapNode https://github.com/EMonk72/EMonk
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
	/// <summary>Base implementation of path-finding node</summary>
	public class MapNode : IMapNode
	{
		/// <summary>True if node can be entered</summary>
		public virtual bool Passable { get; private set; }
		/// <summary>Cost of orthogonal movement into this node</summary>
		public virtual double MoveCost { get; private set; }

		/// <summary>Default constructor</summary>
		public MapNode()
			: this(true, 1)
		{ }

		/// <summary>Full constructor</summary>
		/// <param name="passable">True if node can be entered</param>
		/// <param name="movecost">Cost of moving orthogonally into this node</param>
		public MapNode(bool passable, double movecost)
		{
			Passable = passable;
			MoveCost = movecost;
		}
	}
}
