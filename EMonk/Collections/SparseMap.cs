/*
 * SparseMap https://github.com/EMonk72/EMonk
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EMonk.Collections
{

	public class SparseMap1<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
		where TValue : new()
	{
		#region Element comparisons
		static readonly IComparer<TKey> keycomp = Comparer<TKey>.Default;
		static readonly IComparer<TValue> valcomp = Comparer<TValue>.Default;
		internal class KeyComparer : IComparer<KeyValuePair<TKey, TValue>>
		{
			#region IComparer<KeyValuePair<TKey,TValue>> Members
			public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
			{
				/*
				if (null == (object)x)
					return null == (object)y ? 0 : 1;
				if (null == (object)y)
					return -1;
				//*/
				return keycomp.Compare(x.Key, y.Key);
			}
			#endregion
		}
		#endregion

		#region Storage & accessors
		protected readonly SortedSet<KeyValuePair<TKey, TValue>> data = new SortedSet<KeyValuePair<TKey, TValue>>(new KeyComparer());
		protected bool _createonread = false;

		protected virtual TValue CreateAt(TKey idx)
		{
			TValue res = new TValue();
			this[idx] = res;
			return res;
		}


		/// <summary>Value accessor</summary>
		/// <param name="index">Position in map to access</param>
		/// <returns>Value at position, or default(T) when position empty</returns>
		/// <remarks>Elements in the map are removed when set to the default value.</remarks>
		public TValue this[TKey key]
		{
			get
			{
				KeyValuePair<TKey, TValue> s = new KeyValuePair<TKey, TValue>(key, default(TValue));
				KeyValuePair<TKey, TValue> n = data.GetViewBetween(s, s).FirstOrDefault();
				if (keycomp.Compare(n.Key, key) == 0)
					return n.Value;

				if (_createonread)
					return CreateAt(key);

				return default(TValue);
			}
			set
			{
				KeyValuePair<TKey, TValue> s = new KeyValuePair<TKey, TValue>(key, value);
				data.Remove(s);
				if (valcomp.Compare(value, default(TValue)) != 0)
					data.Add(s);
			}
		}
		#endregion

		#region Constructors
		/// <summary>Default constructor</summary>
		public SparseMap1()
		{ }

		/// <summary>Initialized constructor</summary>
		/// <param name="elements">List of elements to initialize map with</param>
		public SparseMap1(IEnumerable<KeyValuePair<TKey, TValue>> elements)
		{
			AddRange(elements);
		}
		#endregion

		/// <summary>Get count of non-empty elements in map</summary>
		public int Count { get { return data.Count; } }

		/// <summary>Clear map contents</summary>
		public void Clear()
		{
			data.Clear();
		}

		/// <summary>Add a set of elements to the map</summary>
		/// <param name="elements">List of elements to add</param>
		public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> elements)
		{
			foreach (var nxt in elements)
				this[nxt.Key] = nxt.Value;
		}

		#region IEnumerable implementation
		/// <summary>Get an enumerator to iterate through non-empty elements in map</summary>
		/// <returns>IEnumerator&lt;&gt; instance</returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return data.GetEnumerator();
		}
		#endregion

		#region Utility methods
		public bool ContainsKey(TKey key)
		{
			return data.Contains(new KeyValuePair<TKey, TValue>(key, default(TValue)));
		}


		/// <summary>Extract a selection of elements to a new map using the supplied filter function</summary>
		/// <param name="fn">Filter function to select elements</param>
		/// <returns>New SparseMap instance with filtered elements</returns>
		public SparseMap1<TKey, TValue> Filter(Func<KeyValuePair<TKey, TValue>, bool> fn)
		{
			return new SparseMap1<TKey, TValue>(this.Where(f => fn(f)));
		}
		#endregion
	}

	//*
	/// <summary>Generic sparsely-populated map class</summary>
	/// <typeparam name="TIndex">Type of map</typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class SparseMap<TIndex, TValue> : IEnumerable<KeyValuePair<TIndex, TValue>>
		where TValue : new()
	{
		protected readonly Dictionary<TIndex, TValue> data = new Dictionary<TIndex, TValue>();
		protected bool _createonread = false;

		protected virtual TValue CreateAt(TIndex idx) 
		{ 
			TValue res = new TValue();
			data[idx] = res;
			return res; 
		}

		/// <summary>Value accessor</summary>
		/// <param name="index">Position in map to access</param>
		/// <returns>Value at position, or default(T) when position empty</returns>
		/// <remarks>Elements in the map are removed when set to the default value.</remarks>
		public TValue this[TIndex index]
		{
			get
			{
				if (data.ContainsKey(index))
					return data[index];

				if (_createonread)
					return CreateAt(index);
					
				return default(TValue);
			}
			set
			{
				if (object.Equals(value, default(TValue)))
				{
					if (data.ContainsKey(index))
						data.Remove(index);
				}
				else
					data[index] = value;
			}
		}

		#region Constructors
		/// <summary>Default constructor</summary>
		public SparseMap()
		{ }

		/// <summary>Initialized constructor</summary>
		/// <param name="elements">List of elements to initialize map with</param>
		public SparseMap(IEnumerable<KeyValuePair<TIndex, TValue>> elements)
		{
			AddRange(elements);
		}
		#endregion

		/// <summary>Get count of non-empty elements in map</summary>
		public int Count { get { return data.Count; } }

		/// <summary>Clear map contents</summary>
		public void Clear()
		{
			data.Clear();
		}

		/// <summary>Add a set of elements to the map</summary>
		/// <param name="elements">List of elements to add</param>
		public void AddRange(IEnumerable<KeyValuePair<TIndex, TValue>> elements)
		{
			foreach (var nxt in elements)
				this[nxt.Key] = nxt.Value;
		}

		#region IEnumerator implementation
		/// <summary>Get an enumerator to iterate through non-empty elements in map</summary>
		/// <returns>IEnumerator&lt;&gt; instance</returns>
		public IEnumerator<KeyValuePair<TIndex, TValue>> GetEnumerator()
		{
			return data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return data.GetEnumerator();
		}
		#endregion

		#region Utility methods
		/// <summary>Extract a selection of elements to a new map using the supplied filter function</summary>
		/// <param name="fn">Filter function to select elements</param>
		/// <returns>New SparseMap instance with filtered elements</returns>
		public SparseMap<TIndex, TValue> Filter(Func<KeyValuePair<TIndex, TValue>, bool> fn)
		{
			return new SparseMap<TIndex, TValue>(this.Where(f => fn(f)));
		}
		#endregion
	}

	// */
}
