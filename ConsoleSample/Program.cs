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

	public static class Extensions
	{
		public static double StdDev(this IEnumerable<double> values)
		{
			double ret = 0;
			int count = values.Count();
			if (count > 1)
			{
				//Compute the Average
				double avg = values.Average();

				//Perform the Sum of (value-avg)^2
				double sum = values.Sum(d => (d - avg) * (d - avg));

				//Put it all together
				ret = Math.Sqrt(sum / count);
			}
			return ret;
		}
	}

	class PerfData
	{
		public readonly string Name;

		readonly List<TimeSpan> times = new List<TimeSpan>();

		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


		public PerfData(string name)
		{
			Name = name;
		}

		public void Dump()
		{
			Console.WriteLine("{0}: {1} sample{2}", Name, times.Count, times.Count == 1 ? "" : "s");
			if (times.Count < 1)
				return;

			Console.WriteLine("\tBest:\t{0:0.00}ms", Min);
			Console.WriteLine("\tWorst:\t{0:0.00}ms", Max);
			Console.WriteLine("\tAvg:\t{0:0.00}ms", Avg);
			Console.WriteLine("\tStDev:\t{0:0.00}", StDev);
		}

		public void Clear()
		{
			times.Clear();
		}

		public void Time(Action a)
		{
			sw.Restart();
			a();
			sw.Stop();
			times.Add(sw.Elapsed);
		}

		public double Min { get { return times.Min().TotalMilliseconds; } }
		public double Avg { get { return times.Select(t => t.TotalMilliseconds).Average(); } }
		public double Max { get { return times.Max().TotalMilliseconds; } }

		public double StDev { get { return times.Select(t => t.TotalMilliseconds).StdDev(); } }
	}

	class TimeRun
	{
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		public TimeSpan Time(Action a)
		{
			sw.Restart();
			a();
			sw.Stop();
			return sw.Elapsed;
		}
	}

	class Program
	{
		static TimeRun tmr = new TimeRun();

		class KeyComp<TK, TV> : IComparer<KeyValuePair<TK, TV>>
		{
			static IComparer<TK> c = Comparer<TK>.Default;

			public int Compare(KeyValuePair<TK, TV> x, KeyValuePair<TK, TV> y)
			{
				return c.Compare(x.Key, y.Key);
			}
		}

		static void Main(string[] args)
		{
			Random rnd = new Random();

			PerfData p1 = new PerfData("OrderedSet<KeyValuePair<int, MapNode>>");
			PerfData p2 = new PerfData("AleRBTree<int, MapNode>");

			for (int j = 0; j < 50; j++)
			{
				Dictionary<int, MapNode> u = new Dictionary<int, MapNode>();

				int limit = 10000;

				for (int i = 0; i < limit; ++i)
				{
					MapNode n;
					switch (rnd.Next(2))
					{
						case 1:
							n = new ClosedNode();
							break;
						default:
							n = new OpenNode(0.5 + rnd.NextDouble() * 20);
							break;
					}

					int k = rnd.Next(limit * 100);

					while (u.ContainsKey(k))
						k = rnd.Next(limit * 100);

					u[k] = n;
				}

				KeyValuePair<int, MapNode>[] src = u.ToArray();

				//Console.WriteLine("Calculating times for: Insert + Scan + Delete");
				//Console.WriteLine("Calculating times for: Insert");

				for (int run = 0; run < 20; run++)
				{
					SortedSet<KeyValuePair<int, MapNode>> o1 = new SortedSet<KeyValuePair<int, MapNode>>(new KeyComp<int, MapNode>());

					p1.Time(() =>
						{
							foreach (var nxt in src)
								o1.Add(nxt);

							foreach (var nxt in src)
								if (!o1.Contains(nxt))
									throw new Exception();

							while (o1.Count > 0)
								o1.Remove(o1.First());
						});
				}

				for (int run = 0; run < 20; ++run)
				{
					AleRBTree<int, MapNode> o2 = new AleRBTree<int, MapNode>();

					p2.Time(() =>
						{
							foreach (var nxt in src)
								o2[nxt.Key] = nxt.Value;

							foreach (var nxt in src)
								if (!o2.ContainsKey(nxt.Key))
									throw new Exception();

							while (o2.Count > 0)
								o2.DeleteNode(o2.LeftMostKey);
						});
				}
			}

			p1.Dump();
			Console.WriteLine();
			p2.Dump();
			Console.WriteLine();

			Console.WriteLine("Relative performance: {0:0.00%} {1}",
					Math.Abs(p2.Avg / p1.Avg - 1),
					p1.Avg < p2.Avg ? "slower" : "faster");


			/*
			PathfinderSample();
			// */
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
			double maxCost = 50;
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
