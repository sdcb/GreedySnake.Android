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

namespace GreedySnake.Android
{
	class GLView1 : AndroidGameView
	{
		public GLView1(Context context) : base(context)
		{
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

			GL.MatrixMode(All.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, -1, 1);
			GL.Viewport(0, 0, Width, Height);

			GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
			GL.Clear((uint)All.ColorBufferBit);

			GL.EnableClientState(All.VertexArray);
			GL.EnableClientState(All.ColorArray);

			FillRect(50, 50, 50, 50, Color.Red);
			FillRect(50, 100, 50, 50, Color.NavajoWhite);
			FillRect(50, 150, 50, 50, Color.Yellow);

			SwapBuffers();
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
			var colors = new []
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
