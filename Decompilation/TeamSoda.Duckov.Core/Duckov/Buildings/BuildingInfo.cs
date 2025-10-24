using System;
using Duckov.Economy;
using Duckov.Quests;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000314 RID: 788
	[Serializable]
	public struct BuildingInfo
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0005DE8B File Offset: 0x0005C08B
		public bool Valid
		{
			get
			{
				return !string.IsNullOrEmpty(this.id);
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x0005DE9B File Offset: 0x0005C09B
		public Building Prefab
		{
			get
			{
				return BuildingDataCollection.GetPrefab(this.prefabName);
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0005DEA8 File Offset: 0x0005C0A8
		public Vector2Int Dimensions
		{
			get
			{
				if (!this.Prefab)
				{
					return default(Vector2Int);
				}
				return this.Prefab.Dimensions;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x0005DED7 File Offset: 0x0005C0D7
		[LocalizationKey("Default")]
		public string DisplayNameKey
		{
			get
			{
				return "Building_" + this.id;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0005DEE9 File Offset: 0x0005C0E9
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0005DEF6 File Offset: 0x0005C0F6
		public static string GetDisplayName(string id)
		{
			return ("Building_" + id).ToPlainText();
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0005DF08 File Offset: 0x0005C108
		internal bool RequirementsSatisfied()
		{
			string[] array = this.requireBuildings;
			for (int i = 0; i < array.Length; i++)
			{
				if (!BuildingManager.Any(array[i], false))
				{
					return false;
				}
			}
			return QuestManager.AreQuestFinished(this.requireQuests);
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x0005DF47 File Offset: 0x0005C147
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Building_" + this.id + "_Desc";
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x0005DF5E File Offset: 0x0005C15E
		public string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x0005DF6B File Offset: 0x0005C16B
		public int CurrentAmount
		{
			get
			{
				if (BuildingManager.Instance == null)
				{
					return 0;
				}
				return BuildingManager.GetBuildingAmount(this.id);
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x0005DF87 File Offset: 0x0005C187
		public bool ReachedAmountLimit
		{
			get
			{
				return this.maxAmount > 0 && this.CurrentAmount >= this.maxAmount;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0005DFA5 File Offset: 0x0005C1A5
		public int TokenAmount
		{
			get
			{
				if (BuildingManager.Instance == null)
				{
					return 0;
				}
				return BuildingManager.Instance.GetTokenAmount(this.id);
			}
		}

		// Token: 0x040012B3 RID: 4787
		public string id;

		// Token: 0x040012B4 RID: 4788
		public string prefabName;

		// Token: 0x040012B5 RID: 4789
		public int maxAmount;

		// Token: 0x040012B6 RID: 4790
		public Cost cost;

		// Token: 0x040012B7 RID: 4791
		public string[] requireBuildings;

		// Token: 0x040012B8 RID: 4792
		public string[] alternativeFor;

		// Token: 0x040012B9 RID: 4793
		public int[] requireQuests;

		// Token: 0x040012BA RID: 4794
		public Sprite iconReference;
	}
}
