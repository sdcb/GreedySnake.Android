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
using System.Diagnostics;

namespace GreedySnake.Android.Model
{
	public class GameTimer
	{
		private Stopwatch watch = new Stopwatch();

		public double Interval { get; set; }

		public void Update()
		{
			if (watch.ElapsedMilliseconds >= Interval)
			{
				Tick?.Invoke(this, EventArgs.Empty);
				watch.Restart();
			}
		}

		public event EventHandler Tick;

		public GameTimer(double interval)
		{
			Interval = interval;
			watch.Start();
		}
	}
}