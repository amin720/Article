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
		private static readonly Random _random = new Random();

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

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[_random.Next(s.Length)]).ToArray());
		}

	}
}