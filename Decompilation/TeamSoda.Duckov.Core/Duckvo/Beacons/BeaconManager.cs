using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using UnityEngine;

namespace Duckvo.Beacons
{
	// Token: 0x02000222 RID: 546
	public class BeaconManager : MonoBehaviour
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0003F591 File Offset: 0x0003D791
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x0003F598 File Offset: 0x0003D798
		public static BeaconManager Instance { get; private set; }

		// Token: 0x06001061 RID: 4193 RVA: 0x0003F5A0 File Offset: 0x0003D7A0
		private void Awake()
		{
			BeaconManager.Instance = this;
			this.Load();
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0003F5BF File Offset: 0x0003D7BF
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0003F5D2 File Offset: 0x0003D7D2
		public void Load()
		{
			if (SavesSystem.KeyExisits("BeaconManager"))
			{
				this.data = SavesSystem.Load<BeaconManager.Data>("BeaconManager");
			}
			if (this.data.entries == null)
			{
				this.data.entries = new List<BeaconManager.BeaconStatus>();
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0003F60D File Offset: 0x0003D80D
		public void Save()
		{
			SavesSystem.Save<BeaconManager.Data>("BeaconManager", this.data);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0003F620 File Offset: 0x0003D820
		public static void UnlockBeacon(string id, int index)
		{
			if (BeaconManager.Instance == null)
			{
				return;
			}
			if (BeaconManager.GetBeaconUnlocked(id, index))
			{
				return;
			}
			BeaconManager.Instance.data.entries.Add(new BeaconManager.BeaconStatus
			{
				beaconID = id,
				beaconIndex = index
			});
			Action<string, int> onBeaconUnlocked = BeaconManager.OnBeaconUnlocked;
			if (onBeaconUnlocked == null)
			{
				return;
			}
			onBeaconUnlocked(id, index);
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0003F684 File Offset: 0x0003D884
		public static bool GetBeaconUnlocked(string id, int index)
		{
			return !(BeaconManager.Instance == null) && BeaconManager.Instance.data.entries.Any((BeaconManager.BeaconStatus e) => e.beaconID == id && e.beaconIndex == index);
		}

		// Token: 0x04000D09 RID: 3337
		private BeaconManager.Data data;

		// Token: 0x04000D0A RID: 3338
		public static Action<string, int> OnBeaconUnlocked;

		// Token: 0x04000D0B RID: 3339
		private const string SaveKey = "BeaconManager";

		// Token: 0x02000503 RID: 1283
		[Serializable]
		public struct BeaconStatus
		{
			// Token: 0x04001DB7 RID: 7607
			public string beaconID;

			// Token: 0x04001DB8 RID: 7608
			public int beaconIndex;
		}

		// Token: 0x02000504 RID: 1284
		[Serializable]
		public struct Data
		{
			// Token: 0x04001DB9 RID: 7609
			public List<BeaconManager.BeaconStatus> entries;
		}
	}
}
