using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;
using Android.Views;
using Android.Content;
using Android.Util;
using System.Drawing;
using GreedySnake.Android.Model;
using System.Timers;
using Android.Runtime;
using System.Collections.Generic;

namespace GreedySnake.Android
{
	class GLView1 : AndroidGameView
	{
		GameContext game = new GameContext();
		SwipeDetector swipe = new SwipeDetector();

		public GLView1(Context context) : base(context)
		{
			swipe.Swipped += Swipe_Swipped;
		}

		private void Swipe_Swipped(SwipeDetector sender, SwipeTypes swipeType)
		{
			Log.Info(nameof(Swipe_Swipped), swipeType.ToString());
			var directionMap = new Dictionary<SwipeTypes, Direction>
			{
				{ SwipeTypes.TopToBottom, Direction.Down },
				{ SwipeTypes.RightToLeft, Direction.Left },
				{ SwipeTypes.BottomToTop, Direction.Top },
				{ SwipeTypes.LeftToRight, Direction.Right },
			};
			var direction = directionMap[swipeType];
			game.RequestDirection(direction);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			Log.Info(nameof(OnTouchEvent), e.ToString());
			return swipe.OnTouch(e);
		}

		// This gets called when the drawing surface is ready
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			// Run the render loop
			Run();
		}

		// This method is called everytime the context needs
		// to be recreated. Use it to set any egl-specific settings
		// prior to context creation
		//
		// In this particular case, we demonstrate how to set
		// the graphics mode and fallback in case the device doesn't
		// support the defaults
		protected override void CreateFrameBuffer()
		{
			// the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
			try
			{
				Log.Verbose("GLCube", "Loading with default settings");

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer();
				return;
			}
			catch (Exception ex)
			{
				Log.Verbose("GLCube", "{0}", ex);
			}

			// this is a graphics setting that sets everything to the lowest mode possible so
			// the device returns a reliable graphics setting.
			try
			{
				Log.Verbose("GLCube", "Loading with custom Android settings (low mode)");
				GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer();
				return;
			}
			catch (Exception ex)
			{
				Log.Verbose("GLCube", "{0}", ex);
			}
			throw new Exception("Can't load egl, aborting");
		}

		// This gets called on each frame render
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			game.Update();

			GL.MatrixMode(All.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, -1, 1);
			GL.Viewport(0, 0, Width, Height);

			GL.ClearColor(1, 1, 1, 1.0f);
			GL.Clear((uint)All.ColorBufferBit);

			GL.EnableClientState(All.VertexArray);
			GL.EnableClientState(All.ColorArray);

			RenderGame();

			SwapBuffers();
		}

		private void RenderGame()
		{
			var rectWidth = (float)Width / game.Bound.X;
			var rectHeight = ((float)Height - 20) / game.Bound.Y;
			var gap = rectWidth / 20;

			var snakeColor = Color.Red;
			var foodColor = Color.Blue;
			var backgroundColor = Color.Gray;

			FillRect(0, 0,
				game.Bound.X * rectWidth,
				game.Bound.Y * rectHeight, backgroundColor);
			
			game.Snake.Body.ForEach(v =>
			{
				FillRect(
					v.X * rectWidth + gap,
					v.Y * rectHeight + gap,
					rectWidth - gap, rectHeight - gap, snakeColor);
			});

			FillRect(
				game.FoodPosition.X * rectWidth,
				game.FoodPosition.Y * rectHeight,
				rectWidth, rectHeight, foodColor);
		}

		public static void FillRect(float x, float y, float width, float height, Color color)
		{
			var vertices = new[]
			{
				x, y,
				x + width, y,
				x, y + height,
				x + width, y + height
			};
			var colors = new[]
			{
				color.R, color.G, color.B, color.A,
				color.R, color.G, color.B, color.A,
				color.R, color.G, color.B, color.A,
				color.R, color.G, color.B, color.A,
			};
			GL.VertexPointer(2, All.Float, 0, vertices);
			GL.ColorPointer(4, All.UnsignedByte, 0, colors);


			GL.DrawArrays(All.TriangleStrip, 0, 4);
		}
	}
}
