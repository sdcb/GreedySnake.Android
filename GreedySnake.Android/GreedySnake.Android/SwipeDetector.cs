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
using static Android.Views.View;
using Android.Util;
using GreedySnake.Android;

namespace GreedySnake.Android
{
	class SwipeDetector
	{
		private int min_distance = 100;
		private float downX, downY, upX, upY;

		public event SwipeEventDetectedEventHandler Swipped;
		
		public void onRightToLeftSwipe()
		{
			if (Swipped != null)
				Swipped(this, SwipeTypes.RightToLeft);
			else
				Log.Error("SwipeDetector error", "please pass SwipeDetector.onSwipeEvent Interface instance");
		}

		public void onLeftToRightSwipe()
		{
			if (Swipped != null)
				Swipped(this, SwipeTypes.LeftToRight);
			else
				Log.Error("SwipeDetector error", "please pass SwipeDetector.onSwipeEvent Interface instance");
		}

		public void onTopToBottomSwipe()
		{
			if (Swipped != null)
				Swipped(this, SwipeTypes.TopToBottom);
			else
				Log.Error("SwipeDetector error", "please pass SwipeDetector.onSwipeEvent Interface instance");
		}

		public void onBottomToTopSwipe()
		{
			if (Swipped != null)
				Swipped(this, SwipeTypes.BottomToTop);
			else
				Log.Error("SwipeDetector error", "please pass SwipeDetector.onSwipeEvent Interface instance");
		}

		public bool OnTouch(MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Down:
					downX = e.GetX();
					downY = e.GetY();
					return true;
				case MotionEventActions.Up:
					upX = e.GetX();
					upY = e.GetY();

					float deltaX = downX - upX;
					float deltaY = downY - upY;

					//HORIZONTAL SCROLL
					if (Math.Abs(deltaX) > Math.Abs(deltaY))
					{
						if (Math.Abs(deltaX) > min_distance)
						{
							// left or right
							if (deltaX < 0)
							{
								onLeftToRightSwipe();
								return true;
							}
							if (deltaX > 0)
							{

								onRightToLeftSwipe();
								return true;
							}
						}
						else
						{
							//not long enough swipe...
							return false;
						}
					}
					//VERTICAL SCROLL
					else
					{
						if (Math.Abs(deltaY) > min_distance)
						{
							// top or down
							if (deltaY < 0)
							{
								onTopToBottomSwipe();
								return true;
							}
							if (deltaY > 0)
							{
								onBottomToTopSwipe();
								return true;
							}
						}
						else
						{
							//not long enough swipe...
							return false;
						}
					}
					return true;
				default:
					return false;
			}
		}

		public delegate void SwipeEventDetectedEventHandler(SwipeDetector sender, SwipeTypes swipeType);
		
		public SwipeDetector setMinDistanceInPixels(int min_distance)
		{
			this.min_distance = min_distance;
			return this;
		}
	}

	public enum SwipeTypes
	{
		RightToLeft,
		LeftToRight,
		TopToBottom,
		BottomToTop
	}
}