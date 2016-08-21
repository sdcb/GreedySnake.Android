using System.Collections.Generic;

namespace GreedySnake.Android.Model
{
	public struct Vector2D
	{
		public int X;

		public int Y;

		public Vector2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector2D By(Vector2D point)
		{
			return new Vector2D(X + point.X, Y + point.Y);
		}

		public bool IsInBound(Vector2D bound)
		{
			return
				X >= 0 && X < bound.X &&
				Y >= 0 && Y < bound.Y;
		}

		public Vector2D ByDirection(Direction direction)
		{
			var offsetMap = new Dictionary<Direction, Vector2D>
				{
					{ Direction.Top, new Vector2D(0, -1) },
					{ Direction.Down, new Vector2D(0, 1) },
					{ Direction.Left, new Vector2D(-1, 0) },
					{ Direction.Right, new Vector2D(1, 0) },
				};
			return By(offsetMap[direction]);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector2D))
			{
				return false;
			}

			return this == (Vector2D)obj;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public static bool operator ==(Vector2D left, Vector2D right)
		{
			return left.X == right.X &&
				left.Y == right.Y;
		}

		public static bool operator !=(Vector2D left, Vector2D right)
		{
			return !(left == right);
		}
	}
}