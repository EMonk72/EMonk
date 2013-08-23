/*
 * ConsoleSample https://github.com/EMonk72/EMonk
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
using System.Text;
using EMonk.Collections;
using EMonk.Pathfinding;

namespace ConsoleSample
{
	public class OpenNode : MapNode
	{
		public OpenNode()
			: base(true, 1.0)
		{ }

		public OpenNode(double cost)
			: base(true, cost)
		{ }
	}

	public class ClosedNode : MapNode
	{
		public ClosedNode()
			: base(false, 1e+99)
		{ }
	}

	class Program
	{
		static void Main(string[] args)
		{
			PathfinderSample();
		}

		static void ShowError(string fmt, params object[] parms)
		{
			Console.WriteLine(fmt, parms);
			Console.Write("Press any key to exit:");
			Console.ReadKey(true);
			Console.WriteLine("\b.");
		}

		/// <summary>Sample invocation of path-finding code in <see cref="EMonk.Pathfinding.PathMap"/>.</summary>
		static void PathfinderSample()
		{
			Console.Clear();
			Console.WriteLine("Pathfinding Sample");
			Console.WriteLine("==================");

			// create a simple set of map nodes
			Console.Write("Generating sample map tiles");
			SparseMap<MapPosition, MapNode> map = new SparseMap<MapPosition, MapNode>();
			
			// add strip of high-cost tiles
			for (int i = 20; i <= 30; ++i)
				map[new MapPosition(i, 15)] = new MapNode(true, 15);

			// create a lower cost hole through strip
			map[new MapPosition(22, 15)] = new MapNode(true, 2);

			// add a medium-cost block in the way of the normal path
			map[new MapPosition(23, 14)] = new MapNode(true, 4);
			map[new MapPosition(22, 14)] = new MapNode(true, 4);
			Console.WriteLine();

			// generate path map for this
			MapPosition origin = new MapPosition(25, 25);
			double maxCost = 25;
			Console.Write("Generating PathMap for Origin={0}, MaxCost={1}", origin, maxCost);
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			PathMap<MapNode> m = PathMap<MapNode>.Generate(map, origin, maxCost);
			sw.Stop();
			if (m == null)
			{
				Console.WriteLine(" failed!");
				Console.WriteLine();
				ShowError("Error occurred while calculating PathMap.");
				return;
			}
			Console.WriteLine(" [{0:0.00}ms]", sw.Elapsed.TotalMilliseconds);
			Console.WriteLine("Statistics:");
			Console.WriteLine("\tInput Nodes     \t{0}", map.Count);
			Console.WriteLine("\tOutput Positions\t{0}", m.Count);
			Console.WriteLine("\tScan Iterations \t{0}", m.Iterations);
			Console.WriteLine();

			// find path to end-point
			MapPosition target = new MapPosition(25, 14);
			Console.Write("Finding path to target position {0}", target);

			if (null == m[target])
			{
				Console.WriteLine(" failed.");
				Console.WriteLine();
				ShowError("Target was not present in PathMap's potential move set.");
				return;
			}

			double pcost;
			sw.Restart();
			MapPosition[] path = m.GetPath(new MapPosition(25, 14), out pcost);
			sw.Stop();
			if (path == null)
			{
				Console.WriteLine(" failed!");
				Console.WriteLine();
				ShowError("Error occurred while calculating path.");
				return;
			}
			Console.WriteLine(" [{0:0.00}ms]", sw.Elapsed.TotalMilliseconds);
			Console.WriteLine();

			// Display generated path
			Console.WriteLine("Generated path from {0}:", path.First());
			Console.WriteLine("\tDirection\tNew Position\tCost\tTotal Cost");
			MapPosition prev = null;
			foreach (var curr in path)
			{
				if (prev != null)
					Console.WriteLine("\t{0,-10}\t{1,-9}\t{2}\t{3}", prev.DirectionTo(curr).ToString(), curr, (m[curr].TotalCost - m[prev].TotalCost), m[curr].TotalCost);
				prev = curr;
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to continue");
			Console.ReadKey();
		}
	}
}
