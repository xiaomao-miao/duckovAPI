using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000313 RID: 787
	[CreateAssetMenu]
	public class BuildingDataCollection : ScriptableObject
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x0005DDC0 File Offset: 0x0005BFC0
		public static BuildingDataCollection Instance
		{
			get
			{
				return GameplayDataSettings.BuildingDataCollection;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0005DDC7 File Offset: 0x0005BFC7
		public ReadOnlyCollection<BuildingInfo> Infos
		{
			get
			{
				if (this.readonlyInfos == null)
				{
					this.readonlyInfos = new ReadOnlyCollection<BuildingInfo>(this.infos);
				}
				return this.readonlyInfos;
			}
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0005DDE8 File Offset: 0x0005BFE8
		internal static BuildingInfo GetInfo(string id)
		{
			if (BuildingDataCollection.Instance == null)
			{
				return default(BuildingInfo);
			}
			return BuildingDataCollection.Instance.infos.FirstOrDefault((BuildingInfo e) => e.id == id);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0005DE34 File Offset: 0x0005C034
		internal static Building GetPrefab(string prefabName)
		{
			if (BuildingDataCollection.Instance == null)
			{
				return null;
			}
			return BuildingDataCollection.Instance.prefabs.FirstOrDefault((Building e) => e != null && e.name == prefabName);
		}

		// Token: 0x040012B0 RID: 4784
		[SerializeField]
		private List<BuildingInfo> infos = new List<BuildingInfo>();

		// Token: 0x040012B1 RID: 4785
		[SerializeField]
		private List<Building> prefabs;

		// Token: 0x040012B2 RID: 4786
		public ReadOnlyCollection<BuildingInfo> readonlyInfos;
	}
}
