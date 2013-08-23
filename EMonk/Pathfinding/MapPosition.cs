/*
 * MapPosition https://github.com/EMonk72/EMonk
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

namespace EMonk.Pathfinding
{
	/// <summary>Describes a position in 2-dimensional map space</summary>
	public class MapPosition : IEquatable<MapPosition>, IComparable<MapPosition>
	{
		/// <summary>Horizontal position</summary>
		public readonly int X;
		/// <summary>Vertical position</summary>
		public readonly int Y;

		private int _hash;

		/// <summary>Valued constructor</summary>
		/// <param name="x">Horizontal position</param>
		/// <param name="y">Vertical position</param>
		public MapPosition(int x, int y)
		{
			X = x;
			Y = y;
			_hash = ((((ulong)(uint)x) << 32) | (ulong)(uint)y).GetHashCode();
		}

		/// <summary>Copy constructor</summary>
		/// <param name="s">Source object to copy position from</param>
		public MapPosition(MapPosition s)
			: this(s.X, s.Y)
		{ }

		#region IEquatable<MapPosition> Members
		/// <summary>Compare this position to another</summary>
		/// <param name="other">Other position to compare to</param>
		/// <returns>True if positions are congruent, false otherwise</returns>
		public bool Equals(MapPosition other)
		{
			if (null == this)
				return null == other;
			if (null == other)
				return false;
			if (object.ReferenceEquals(this, other))
				return true;

			return this.X == other.X && this.Y == other.Y;
		}
		#endregion

		#region IComparable<MapPosition> Members
		/// <summary>Ordered comparison of this position to another position</summary>
		/// <param name="other">Other position to compare to</param>
		/// <returns>0 if congruent, -1 if this is 'before' other, 1 if this is 'after' other</returns>
		public int CompareTo(MapPosition other)
		{
			if (null == this)
				return (null == other ? 0 : -1);
			if (object.ReferenceEquals(this, other))
				return 0;

			if (this.Y != other.Y)
				return this.Y < other.Y ? -1 : 1;
			if (this.X != other.X)
				return this.X < other.X ? -1 : 1;
			return 0;
		}
		#endregion

		/// <summary>Compare this position to another object</summary>
		/// <param name="obj">Object to compare to</param>
		/// <returns>True if other object is a congruent position, false otherwise </returns>
		public override bool Equals(object obj)
		{
			if (null == this)
				return null == obj;
			if (null == obj)
				return false;
			if (object.ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(MapPosition))
				return false;

			return this.Equals((MapPosition)obj);
		}

		/// <summary>Return the position's hash code</summary>
		/// <returns>32-bit integer hash value for position</returns>
		public override int GetHashCode()
		{
			return _hash;
		}

		/// <summary>Convert to string representation</summary>
		/// <returns>String representation of this position</returns>
		public override string ToString()
		{
			return string.Format("({0},{1})", X, Y);
		}

		/// <summary>Add a single step in the supplied direction</summary>
		/// <param name="l">Position to start from</param>
		/// <param name="r">Direction to move in</param>
		/// <returns>New position</returns>
		public static MapPosition operator +(MapPosition l, MapDirection r)
		{
			int x = l.X;
			int y = l.Y;

			switch (r)
			{
				case MapDirection.Up: --y; break;
				case MapDirection.Right: ++x; break;
				case MapDirection.Down: ++y; break;
				case MapDirection.Left: --x; break;
				case MapDirection.UpRight: --y; ++x; break;
				case MapDirection.DownRight: ++y; ++x; break;
				case MapDirection.DownLeft: ++y; --x; break;
				case MapDirection.UpLeft: --y; --x; break;
			}

			return new MapPosition(x, y);
		}

		/// <summary>Get direction from this position to other position</summary>
		/// <param name="other">Other position to get direction to</param>
		/// <returns>Direction indicator</returns>
		public MapDirection DirectionTo(MapPosition other)
		{
			return GetDirection(this, other);
		}

		/// <summary>Get direction to this position from other position</summary>
		/// <param name="other">Other position to get direction from</param>
		/// <returns>Direction indicator</returns>
		public MapDirection DirectionFrom(MapPosition other)
		{
			return GetDirection(other, this);
		}

		/// <summary>Get direction between two positions</summary>
		/// <param name="from">Starting position</param>
		/// <param name="to">Ending position</param>
		/// <returns>Direction</returns>
		public static MapDirection GetDirection(MapPosition from, MapPosition to)
		{
			if (null == from || null == to || from == to)
				return MapDirection.None;

			// Down:
			if (to.Y > from.Y)
			{
				if (to.X < from.X)
					return MapDirection.DownLeft;
				if (to.X > from.X)
					return MapDirection.DownRight;
				return MapDirection.Down;
			}

			// Up:
			if (to.Y < from.Y)
			{
				if (to.X < from.X)
					return MapDirection.UpLeft;
				if (to.X > from.X)
					return MapDirection.UpRight;
				return MapDirection.Up;
			}

			// Left
			if (from.X > to.X)
				return MapDirection.Left;

			// Right
			return MapDirection.Right;
		}
	}
}
