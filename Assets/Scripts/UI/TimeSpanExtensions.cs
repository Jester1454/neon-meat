using System;

namespace UI
{
	public static class TimeSpanExtensions
	{
		public static string ToMyFormat(this TimeSpan t)
		{
			return t.ToString(@"mm\:ss\:fff");
			//return t.Minutes + ":" + t.Seconds + ":" + t.Milliseconds;
		}
	}
}