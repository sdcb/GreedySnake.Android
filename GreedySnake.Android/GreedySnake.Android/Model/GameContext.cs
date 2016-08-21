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

		public Vector2D FoodPosition { get; set; }

		public bool GameOver { get; set; } = false;

		public bool GameRunning => !GameOver;

		public GameTimer Timer = new GameTimer(300);

		public void Update()
		{
			if (GameOver)
			{
				return;
			}

			Timer.Update();
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

			Move(Snake.Body, requestedDirection);
			Timer.Restart();
		}

		private void Move(SnakeBody snakeBody, Direction direction)
		{
			var nextHeadPosition = snakeBody.NextHead(direction);
			if (nextHeadPosition == FoodPosition)
			{
				Snake = Details.GetEatedGrowedSnake(snakeBody, direction);
				PrepairNextFood();
			}
			else
			{
				var nextSnake = Details.GetByDirectionSnake(snakeBody, direction);
				if (!nextSnake.Body.IsAlive(Bound))
				{
					GameOver = true;
					return;
				}

				Snake = nextSnake;
			}
		}

		private void PrepairNextFood()
		{
			FoodPosition = Details.RandomAvailablePoints(Bound, Snake.Body);
		}

		public GameContext(int width = 10, int height = 15)
		{
			Bound.X = width;
			Bound.Y = height;
			PrepairNextFood();
			Timer.Tick += Timer_Tick;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			Move(Snake.Body, Snake.Direction);
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

			public static Snake GetEatedGrowedSnake(SnakeBody currentSnakeBody, Direction direction)
			{
				var nextBody = currentSnakeBody.GrowAtDirection(direction);
				return new Snake(direction, nextBody);
			}

			public static IEnumerable<Vector2D> AvailableBlocks(Vector2D bound, SnakeBody snakeBody)
			{
				for (var x = 0; x < bound.X; ++x)
				{
					for (var y = 0; y < bound.Y; ++y)
					{
						var point = new Vector2D(x, y);
						if (!snakeBody.Contains(point))
						{
							yield return point;
						}
					}
				}
			}

			public static Vector2D RandomAvailablePoints(Vector2D bound, SnakeBody snakeBody)
			{
				var allBlocks = AvailableBlocks(bound, snakeBody).ToList();
				var r = new Random();
				return allBlocks[r.Next(0, allBlocks.Count)];
			}
		}
	}
}