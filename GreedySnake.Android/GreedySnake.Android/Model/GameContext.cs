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
	public class GameContext
	{
		public readonly Vector2D Bound;

		public Snake Snake { get; set; } = new Snake();

		public Vector2D Food { get; set; }

		public bool GameOver { get; set; } = false;

		public bool GameRunning => !GameOver;

		public void Tick()
		{
			if (GameOver)
			{
				return;
			}

			var nextSnake = Details.GetByDirectionSnake(Snake.Body, Snake.Direction);
			if (!nextSnake.Body.IsAlive(Bound))
			{
				GameOver = true;
				return;
			}

			Snake = nextSnake;
		}

		public void RequestDirection(Direction requestedDirection)
		{
			if (GameOver)
			{
				return;
			}

			if (!Details.IsTargetDirectionOk(Snake.Direction, requestedDirection))
			{
				return;
			}

			var nextSnake = Details.GetByDirectionSnake(Snake.Body, requestedDirection);
			if (!nextSnake.Body.IsAlive(Bound))
			{
				GameOver = true;
				return;
			}

			Snake = nextSnake;
		}


		public GameContext(int width = 10, int height = 20)
		{
			Bound.X = width;
			Bound.Y = height;
		}

		public static class Details
		{
			public static bool IsTargetDirectionOk(
				Direction currentDirection, Direction targetDirection)
			{
				var directionMap = new Dictionary<Direction, Direction[]>
				{
					{Direction.Left, new [] {Direction.Top, Direction.Down } },
					{Direction.Right, new [] {Direction.Top, Direction.Down } },
					{Direction.Top, new [] {Direction.Left, Direction.Right } },
					{Direction.Down, new [] {Direction.Left, Direction.Right } },
				};
				var availableDirections = directionMap[currentDirection];
				return availableDirections.Contains(targetDirection);
			}

			public static Snake GetByDirectionSnake(SnakeBody currentSnakeBody, Direction direction)
			{
				var nextBody = currentSnakeBody.By(direction);
				return new Snake(direction, nextBody);
			}
		}
	}
}