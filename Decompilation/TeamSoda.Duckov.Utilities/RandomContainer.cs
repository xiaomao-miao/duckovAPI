using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class RandomContainer<T> : IPercentRefreshable
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000034C1 File Offset: 0x000016C1
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000034D0 File Offset: 0x000016D0
		public void AddEntry(T _value, float _weight)
		{
			this.entries.Add(new RandomContainer<T>.Entry
			{
				value = _value,
				weight = _weight
			});
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003504 File Offset: 0x00001704
		public T GetRandom(float lowPercent = 0f)
		{
			if (this.entries.Count < 1)
			{
				return default(T);
			}
			float num = 0f;
			for (int i = 0; i < this.entries.Count; i++)
			{
				num += this.entries[i].weight;
			}
			float num2 = UnityEngine.Random.Range(num * lowPercent, num);
			float num3 = 0f;
			for (int j = 0; j < this.entries.Count; j++)
			{
				RandomContainer<T>.Entry entry = this.entries[j];
				num3 += entry.weight;
				if (num3 >= num2)
				{
					return entry.value;
				}
			}
			List<RandomContainer<T>.Entry> list = this.entries;
			return list[list.Count - 1].value;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000035C8 File Offset: 0x000017C8
		public T GetRandom(System.Random overrideRandom, float lowPercent = 0f)
		{
			if (this.entries.Count < 1)
			{
				return default(T);
			}
			float num = 0f;
			for (int i = 0; i < this.entries.Count; i++)
			{
				num += this.entries[i].weight;
			}
			float a = num * lowPercent;
			float b = num;
			float num2 = Mathf.Lerp(a, b, (float)overrideRandom.NextDouble());
			float num3 = 0f;
			for (int j = 0; j < this.entries.Count; j++)
			{
				RandomContainer<T>.Entry entry = this.entries[j];
				num3 += entry.weight;
				if (num3 >= num2)
				{
					return entry.value;
				}
			}
			List<RandomContainer<T>.Entry> list = this.entries;
			return list[list.Count - 1].value;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000369C File Offset: 0x0000189C
		public T GetRandom(System.Random overrideRandom, Func<T, bool> predicator, float lowPercent = 0f)
		{
			if (this.entries.Count < 1)
			{
				return default(T);
			}
			List<RandomContainer<T>.Entry> list = (from e in this.entries
			where predicator(e.value)
			select e).ToList<RandomContainer<T>.Entry>();
			if (list.Count < 1)
			{
				return default(T);
			}
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].weight;
			}
			float a = num * lowPercent;
			float b = num;
			float num2 = Mathf.Lerp(a, b, (float)overrideRandom.NextDouble());
			float num3 = 0f;
			for (int j = 0; j < list.Count; j++)
			{
				RandomContainer<T>.Entry entry = list[j];
				num3 += entry.weight;
				if (num3 >= num2)
				{
					return entry.value;
				}
			}
			List<RandomContainer<T>.Entry> list2 = list;
			return list2[list2.Count - 1].value;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003798 File Offset: 0x00001998
		public List<T> GetRandomMultiple(int count, bool repeatable = true)
		{
			List<T> list = new List<T>();
			if (count < 1)
			{
				return list;
			}
			if (this.entries.Count < 1)
			{
				return list;
			}
			RandomContainer<T>.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.candidates = new List<RandomContainer<T>.Entry>(this.entries);
			RandomContainer<T>.<GetRandomMultiple>g__RecalculateTotalWeight|8_0(ref CS$<>8__locals1);
			for (int i = 0; i < count; i++)
			{
				if (CS$<>8__locals1.candidates.Count < 1)
				{
					return list;
				}
				int index;
				T item = RandomContainer<T>.<GetRandomMultiple>g__GetOnceInCandidates|8_1(out index, ref CS$<>8__locals1);
				list.Add(item);
				if (!repeatable)
				{
					CS$<>8__locals1.candidates.RemoveAt(index);
					RandomContainer<T>.<GetRandomMultiple>g__RecalculateTotalWeight|8_0(ref CS$<>8__locals1);
				}
			}
			return list;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003820 File Offset: 0x00001A20
		public void RefreshPercent()
		{
			float num = 0f;
			foreach (RandomContainer<T>.Entry entry in new List<RandomContainer<T>.Entry>(this.entries))
			{
				num += entry.weight;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				RandomContainer<T>.Entry entry2 = this.entries[i];
				float num2 = entry2.weight * 100f / num;
				if (num2 >= 0.01f)
				{
					entry2.percent = num2.ToString("0.00") + "%";
				}
				else
				{
					entry2.percent = num2.ToString("0.000") + "%";
				}
				this.entries[i] = entry2;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000390C File Offset: 0x00001B0C
		public static RandomContainer<string> FromString(string str)
		{
			string[] array = str.Split(",", StringSplitOptions.None);
			RandomContainer<string> randomContainer = new RandomContainer<string>();
			foreach (string text in array)
			{
				string[] array3 = text.Split(":", StringSplitOptions.None);
				if (array3.Length > 2 || array3.Length < 1)
				{
					Debug.LogError("Invalid entry format:\n " + text);
				}
				else
				{
					string value = array3[0].Trim();
					if (string.IsNullOrEmpty(value))
					{
						Debug.LogError("Empty value, skip");
					}
					else
					{
						float weight = 1f;
						if (array3.Length == 2 && !float.TryParse(array3[1], out weight))
						{
							Debug.LogError("Cannot resolve random container entry:\n " + text + " \n cannot resolve weight");
						}
						else
						{
							randomContainer.AddEntry(value, weight);
						}
					}
				}
			}
			return randomContainer;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000039E4 File Offset: 0x00001BE4
		[CompilerGenerated]
		internal static void <GetRandomMultiple>g__RecalculateTotalWeight|8_0(ref RandomContainer<T>.<>c__DisplayClass8_0 A_0)
		{
			A_0.totalWeight = 0f;
			foreach (RandomContainer<T>.Entry entry in A_0.candidates)
			{
				A_0.totalWeight += entry.weight;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003A50 File Offset: 0x00001C50
		[CompilerGenerated]
		internal static T <GetRandomMultiple>g__GetOnceInCandidates|8_1(out int candidateIndex, ref RandomContainer<T>.<>c__DisplayClass8_0 A_1)
		{
			float num = UnityEngine.Random.Range(0f, A_1.totalWeight);
			float num2 = 0f;
			for (int i = 0; i < A_1.candidates.Count; i++)
			{
				RandomContainer<T>.Entry entry = A_1.candidates[i];
				num2 += entry.weight;
				if (num2 >= num)
				{
					candidateIndex = i;
					return entry.value;
				}
			}
			candidateIndex = A_1.candidates.Count - 1;
			return A_1.candidates[candidateIndex].value;
		}

		// Token: 0x04000027 RID: 39
		public List<RandomContainer<T>.Entry> entries = new List<RandomContainer<T>.Entry>();

		// Token: 0x0200001A RID: 26
		[Serializable]
		public struct Entry
		{
			// Token: 0x04000042 RID: 66
			public T value;

			// Token: 0x04000043 RID: 67
			public float weight;

			// Token: 0x04000044 RID: 68
			public string percent;
		}
	}
}
