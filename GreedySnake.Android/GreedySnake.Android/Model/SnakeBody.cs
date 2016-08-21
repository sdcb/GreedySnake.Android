using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GreedySnake.Android.Model
{
	public class SnakeBody : List<Vector2D>
	{
		public Vector2D Head() => this.First();

		public Vector2D Tail() => this.Last();

		public IEnumerable<Vector2D> HeadToTail() => this;

		public IEnumerable<Vector2D> TailToHead() => this.Reverse<Vector2D>();

		public SnakeBody Clone()
		{
			return new SnakeBody(this);
		}

		public SnakeBody By(Direction direction)
		{
			var resultSnakeBody = HeadToTail().Skip(1).Concat(new[]
				{
					NextHead(direction)
				});
			return new SnakeBody(resultSnakeBody);
		}

		public Vector2D NextHead(Direction direction)
		{
			var directionOffset = Details.GetDirectionOffset(direction);
			return Head().By(directionOffset);
		}

		public SnakeBody GrowAtDirection(Direction direction)
		{
			var nextHead = NextHead(direction);
			var resultSnakeBody = new[] { nextHead }.Concat(HeadToTail());
			return new SnakeBody(resultSnakeBody);
		}

		public bool IsAlive(Vector2D bound)
		{
			if (Details.IsSnakeHitSelf(this))
			{
				return false;
			}

			if (!Head().IsInBound(bound))
			{
				return false;
			}

			return true;
		}

		public SnakeBody(Vector2D p) :
			base(new[] { p })
		{
		}

		public SnakeBody(IEnumerable<Vector2D> points) :
			base(points)
		{
		}

		public static class Details
		{
			public static Vector2D GetDirectionOffset(Direction direction)
			{
				var offsetMap = new Dictionary<Direction, Vector2D>
				{
					{ Direction.Top, new Vector2D(-1, 0) },
					{ Direction.Down, new Vector2D(1, 0) },
					{ Direction.Left, new Vector2D(0, -1) },
					{ Direction.Right, new Vector2D(0, 1) },
				};
				return offsetMap[direction];
			}

			public static bool IsSnakeHitSelf(SnakeBody snakeBody)
			{
				var head = snakeBody.Head();
				var bodyExcepHead = snakeBody.HeadToTail().Skip(0);
				return bodyExcepHead.Any(bodyPart => bodyPart == head);
			}
		}
	}
}