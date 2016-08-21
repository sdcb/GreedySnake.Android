using System.Collections.Generic;

namespace GreedySnake.Android.Model
{
	public class Snake
	{
		public Direction Direction { get; set; }

		public SnakeBody Body { get; set; }

		public Snake(Direction direction, SnakeBody body)
		{
			Direction = direction;
			Body = body;
		}

		public Snake()
		{
			Direction = Direction.Right;
			Body = new SnakeBody(new Vector2D(0, 0));
		}
	}
}