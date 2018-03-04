using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Article.Services
{
	public static class ThreadSafeRandom
	{
		[ThreadStatic] private static Random Local;

		public static Random ThisThreadsRandom
		{
			get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
		}
	}
	public static class Extensions
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
		{
			var array = list.ToArray();

			int n = array.Length;
			while (n > 1)
			{
				n--;
				int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
				T value = array[k];
				array[k] = array[n];
				array[n] = value;
			}

			return array;
		}

	}
}