using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Weathers;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x020003F8 RID: 1016
	public class FishSpawner : MonoBehaviour
	{
		// Token: 0x060024CE RID: 9422 RVA: 0x0007F3E2 File Offset: 0x0007D5E2
		public void CalculateChances()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0007F3FA File Offset: 0x0007D5FA
		private void Awake()
		{
			this.excludeTagsReal = new List<Tag>();
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x0007F407 File Offset: 0x0007D607
		private void Start()
		{
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x0007F40C File Offset: 0x0007D60C
		public UniTask<Item> Spawn(int baitID, float luck)
		{
			FishSpawner.<Spawn>d__14 <Spawn>d__;
			<Spawn>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Spawn>d__.<>4__this = this;
			<Spawn>d__.baitID = baitID;
			<Spawn>d__.luck = luck;
			<Spawn>d__.<>1__state = -1;
			<Spawn>d__.<>t__builder.Start<FishSpawner.<Spawn>d__14>(ref <Spawn>d__);
			return <Spawn>d__.<>t__builder.Task;
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x0007F45F File Offset: 0x0007D65F
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x0007F468 File Offset: 0x0007D668
		private void CalculateTags(bool atNight, Weather weather)
		{
			this.excludeTagsReal.Clear();
			this.excludeTagsReal.AddRange(this.excludeTags);
			if (atNight)
			{
				this.excludeTagsReal.Add(this.Fish_OnlyDay);
			}
			else
			{
				this.excludeTagsReal.Add(this.Fish_OnlyNight);
			}
			this.excludeTagsReal.Add(this.Fish_OnlySunDay);
			this.excludeTagsReal.Add(this.Fish_OnlyRainDay);
			this.excludeTagsReal.Add(this.Fish_OnlyStorm);
			switch (weather)
			{
			case Weather.Sunny:
				this.excludeTagsReal.Remove(this.Fish_OnlySunDay);
				return;
			case Weather.Cloudy:
				break;
			case Weather.Rainy:
				this.excludeTagsReal.Remove(this.Fish_OnlyRainDay);
				return;
			case Weather.Stormy_I:
				this.excludeTagsReal.Remove(this.Fish_OnlyStorm);
				return;
			case Weather.Stormy_II:
				this.excludeTagsReal.Remove(this.Fish_OnlyStorm);
				break;
			default:
				return;
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x0007F554 File Offset: 0x0007D754
		private bool CheckFishDayNightAndWeather(int fishID, bool atNight, Weather currentWeather)
		{
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(fishID);
			return (!metaData.tags.Contains(this.Fish_OnlyNight) || atNight) && (!metaData.tags.Contains(this.Fish_OnlyDay) || !atNight) && (!metaData.tags.Contains(this.Fish_OnlyRainDay) || currentWeather == Weather.Rainy) && (!metaData.tags.Contains(this.Fish_OnlySunDay) || currentWeather == Weather.Sunny) && (!metaData.tags.Contains(this.Fish_OnlyStorm) || currentWeather == Weather.Stormy_I || currentWeather == Weather.Stormy_II);
		}

		// Token: 0x0400190B RID: 6411
		[SerializeField]
		private List<FishSpawner.SpecialPair> specialPairs;

		// Token: 0x0400190C RID: 6412
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x0400190D RID: 6413
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x0400190E RID: 6414
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x0400190F RID: 6415
		private List<Tag> excludeTagsReal;

		// Token: 0x04001910 RID: 6416
		[SerializeField]
		private Tag Fish_OnlyDay;

		// Token: 0x04001911 RID: 6417
		[SerializeField]
		private Tag Fish_OnlyNight;

		// Token: 0x04001912 RID: 6418
		[SerializeField]
		private Tag Fish_OnlySunDay;

		// Token: 0x04001913 RID: 6419
		[SerializeField]
		private Tag Fish_OnlyRainDay;

		// Token: 0x04001914 RID: 6420
		[SerializeField]
		private Tag Fish_OnlyStorm;

		// Token: 0x0200065F RID: 1631
		[Serializable]
		private struct SpecialPair
		{
			// Token: 0x040022F2 RID: 8946
			[ItemTypeID]
			public int baitID;

			// Token: 0x040022F3 RID: 8947
			[ItemTypeID]
			public int fishID;

			// Token: 0x040022F4 RID: 8948
			[Range(0f, 1f)]
			public float chance;
		}
	}
}
