using System;
using UnityEngine;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002DC RID: 732
	public class BubblePopperLevelDataProvider : MonoBehaviour
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00055F38 File Offset: 0x00054138
		public int TotalLevels
		{
			get
			{
				return this.totalLevels;
			}
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00055F40 File Offset: 0x00054140
		internal int[] GetData(int levelIndex)
		{
			int num = this.seed + levelIndex;
			int[] array = new int[60 + 10 * (levelIndex / 2)];
			System.Random random = new System.Random(num);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = random.Next(0, this.master.AvaliableColorCount);
			}
			return array;
		}

		// Token: 0x0400111C RID: 4380
		[SerializeField]
		private BubblePopper master;

		// Token: 0x0400111D RID: 4381
		[SerializeField]
		private int totalLevels = 10;

		// Token: 0x0400111E RID: 4382
		[SerializeField]
		public int seed;
	}
}
