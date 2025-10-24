using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x0200000C RID: 12
	public static class RandomUtilities
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000328C File Offset: 0x0000148C
		public static void RandomizeOrder<T>(this List<T> list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				int index = UnityEngine.Random.Range(i, count - 1);
				T item = list[index];
				list.RemoveAt(index);
				list.Insert(i, item);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000032D0 File Offset: 0x000014D0
		public static T GetRandom<T>(this IList<T> list)
		{
			if (list.Count < 1)
			{
				return default(T);
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			return list[index];
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003304 File Offset: 0x00001504
		public static T GetRandom<T>(this IList<T> list, System.Random rng)
		{
			if (list.Count < 1)
			{
				return default(T);
			}
			int index = rng.Next(0, list.Count);
			return list[index];
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000333C File Offset: 0x0000153C
		public static T[] GetRandomSubSet<T>(this IList<T> list, int amount)
		{
			if (list.Count < 1)
			{
				return null;
			}
			if (list.Count <= amount)
			{
				return list.ToArray<T>();
			}
			T[] array = new T[amount];
			HashSet<int> hashSet = new HashSet<int>();
			int num = amount * 100;
			int num2 = 0;
			for (int i = 0; i < amount; i++)
			{
				num2++;
				if (num2 >= num)
				{
					Debug.LogError("在选取子集的时候尝试了过多次数，选取失败");
					return null;
				}
				int num3 = UnityEngine.Random.Range(0, list.Count);
				if (hashSet.Contains(num3))
				{
					i--;
				}
				else
				{
					array[i] = list[num3];
				}
			}
			return array;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000033D0 File Offset: 0x000015D0
		public static T GetRandom<T>(this T[] array)
		{
			if (array.Length < 1)
			{
				return default(T);
			}
			int num = UnityEngine.Random.Range(0, array.Length);
			return array[num];
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003400 File Offset: 0x00001600
		public static T GetRandomWeighted<T>(this IList<T> list, Func<T, float> weightFunction, float lowPercent = 0f)
		{
			if (list.Count < 1)
			{
				return default(T);
			}
			if (weightFunction == null)
			{
				return list.GetRandom<T>();
			}
			float num = 0f;
			float[] array = new float[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = weightFunction(list[i]);
				num += array[i];
			}
			if (num <= 0f)
			{
				return list.GetRandom<T>();
			}
			float num2 = 0f;
			float num3 = UnityEngine.Random.Range(num * lowPercent, num);
			for (int j = 0; j < list.Count; j++)
			{
				num2 += array[j];
				if (num2 >= num3)
				{
					return list[j];
				}
			}
			return list[list.Count - 1];
		}
	}
}
