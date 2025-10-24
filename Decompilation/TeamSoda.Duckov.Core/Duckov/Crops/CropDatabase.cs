using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002E7 RID: 743
	[CreateAssetMenu]
	public class CropDatabase : ScriptableObject
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x00057721 File Offset: 0x00055921
		public static CropDatabase Instance
		{
			get
			{
				return GameplayDataSettings.CropDatabase;
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00057728 File Offset: 0x00055928
		public static CropInfo? GetCropInfo(string id)
		{
			CropDatabase instance = CropDatabase.Instance;
			for (int i = 0; i < instance.entries.Count; i++)
			{
				CropInfo cropInfo = instance.entries[i];
				if (cropInfo.id == id)
				{
					return new CropInfo?(cropInfo);
				}
			}
			return null;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0005777C File Offset: 0x0005597C
		internal static bool IsIdValid(string id)
		{
			return !(CropDatabase.Instance == null) && CropDatabase.Instance.entries.Any((CropInfo e) => e.id == id);
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x000577C0 File Offset: 0x000559C0
		internal static bool IsSeed(int itemTypeID)
		{
			return !(CropDatabase.Instance == null) && CropDatabase.Instance.seedInfos.Any((SeedInfo e) => e.itemTypeID == itemTypeID);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00057804 File Offset: 0x00055A04
		internal static SeedInfo GetSeedInfo(int seedItemTypeID)
		{
			if (CropDatabase.Instance == null)
			{
				return default(SeedInfo);
			}
			return CropDatabase.Instance.seedInfos.FirstOrDefault((SeedInfo e) => e.itemTypeID == seedItemTypeID);
		}

		// Token: 0x04001166 RID: 4454
		[SerializeField]
		public List<CropInfo> entries = new List<CropInfo>();

		// Token: 0x04001167 RID: 4455
		[SerializeField]
		public List<SeedInfo> seedInfos = new List<SeedInfo>();
	}
}
