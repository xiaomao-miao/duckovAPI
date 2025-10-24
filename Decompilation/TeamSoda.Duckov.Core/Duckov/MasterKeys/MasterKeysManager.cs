using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.MasterKeys
{
	// Token: 0x020002DE RID: 734
	public class MasterKeysManager : MonoBehaviour
	{
		// Token: 0x14000097 RID: 151
		// (add) Token: 0x0600176A RID: 5994 RVA: 0x00055FBC File Offset: 0x000541BC
		// (remove) Token: 0x0600176B RID: 5995 RVA: 0x00055FF0 File Offset: 0x000541F0
		public static event Action<int> OnMasterKeyUnlocked;

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x00056023 File Offset: 0x00054223
		// (set) Token: 0x0600176D RID: 5997 RVA: 0x0005602A File Offset: 0x0005422A
		public static MasterKeysManager Instance { get; private set; }

		// Token: 0x0600176E RID: 5998 RVA: 0x00056034 File Offset: 0x00054234
		public static bool SubmitAndActivate(Item item)
		{
			if (MasterKeysManager.Instance == null)
			{
				return false;
			}
			if (item == null)
			{
				return false;
			}
			int typeID = item.TypeID;
			if (MasterKeysManager.IsActive(typeID))
			{
				return false;
			}
			if (item.StackCount > 1)
			{
				int stackCount = item.StackCount;
				item.StackCount = stackCount - 1;
			}
			else
			{
				item.Detach();
				item.DestroyTree();
			}
			MasterKeysManager.Activate(typeID);
			return true;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0005609A File Offset: 0x0005429A
		public static bool IsActive(int id)
		{
			return !(MasterKeysManager.Instance == null) && MasterKeysManager.Instance.IsActive_Local(id);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x000560B6 File Offset: 0x000542B6
		internal static void Activate(int id)
		{
			if (MasterKeysManager.Instance == null)
			{
				return;
			}
			MasterKeysManager.Instance.Activate_Local(id);
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x000560D1 File Offset: 0x000542D1
		internal static MasterKeysManager.Status GetStatus(int id)
		{
			if (MasterKeysManager.Instance == null)
			{
				return null;
			}
			return MasterKeysManager.Instance.GetStatus_Local(id);
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x000560ED File Offset: 0x000542ED
		public int Count
		{
			get
			{
				return this.keysStatus.Count;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x000560FC File Offset: 0x000542FC
		public static List<int> AllPossibleKeys
		{
			get
			{
				if (MasterKeysManager._cachedKeyItemIds == null)
				{
					MasterKeysManager._cachedKeyItemIds = new List<int>();
					foreach (ItemAssetsCollection.Entry entry in ItemAssetsCollection.Instance.entries)
					{
						Tag[] tags = entry.metaData.tags;
						if (tags.Any((Tag e) => Tag.Match(e, "Key")))
						{
							if (GameMetaData.Instance.IsDemo)
							{
								if (tags.Any((Tag e) => e.name == GameplayDataSettings.Tags.LockInDemoTag.name))
								{
									continue;
								}
							}
							if (!tags.Any((Tag e) => MasterKeysManager.excludeTags.Contains(e.name)))
							{
								MasterKeysManager._cachedKeyItemIds.Add(entry.typeID);
							}
						}
					}
				}
				return MasterKeysManager._cachedKeyItemIds;
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00056208 File Offset: 0x00054408
		private void Awake()
		{
			if (MasterKeysManager.Instance == null)
			{
				MasterKeysManager.Instance = this;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			this.Load();
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00056234 File Offset: 0x00054434
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00056247 File Offset: 0x00054447
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00056250 File Offset: 0x00054450
		public bool IsActive_Local(int id)
		{
			MasterKeysManager.Status status = MasterKeysManager.GetStatus(id);
			return status != null && status.active;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0005626F File Offset: 0x0005446F
		private void Activate_Local(int id)
		{
			if (id < 0)
			{
				return;
			}
			if (!MasterKeysManager.AllPossibleKeys.Contains(id))
			{
				return;
			}
			this.GetOrCreateStatus(id).active = true;
			Action<int> onMasterKeyUnlocked = MasterKeysManager.OnMasterKeyUnlocked;
			if (onMasterKeyUnlocked == null)
			{
				return;
			}
			onMasterKeyUnlocked(id);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000562A4 File Offset: 0x000544A4
		public MasterKeysManager.Status GetStatus_Local(int id)
		{
			return this.keysStatus.Find((MasterKeysManager.Status e) => e.id == id);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000562D8 File Offset: 0x000544D8
		public MasterKeysManager.Status GetOrCreateStatus(int id)
		{
			MasterKeysManager.Status status_Local = this.GetStatus_Local(id);
			if (status_Local != null)
			{
				return status_Local;
			}
			MasterKeysManager.Status status = new MasterKeysManager.Status();
			status.id = id;
			this.keysStatus.Add(status);
			return status;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0005630C File Offset: 0x0005450C
		private void Save()
		{
			SavesSystem.Save<List<MasterKeysManager.Status>>("MasterKeys", this.keysStatus);
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0005631E File Offset: 0x0005451E
		private void Load()
		{
			if (SavesSystem.KeyExisits("MasterKeys"))
			{
				this.keysStatus = SavesSystem.Load<List<MasterKeysManager.Status>>("MasterKeys");
				return;
			}
			this.keysStatus = new List<MasterKeysManager.Status>();
		}

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private List<MasterKeysManager.Status> keysStatus = new List<MasterKeysManager.Status>();

		// Token: 0x04001122 RID: 4386
		private static List<int> _cachedKeyItemIds;

		// Token: 0x04001123 RID: 4387
		private static string[] excludeTags = new string[]
		{
			"SpecialKey"
		};

		// Token: 0x04001124 RID: 4388
		private const string SaveKey = "MasterKeys";

		// Token: 0x02000579 RID: 1401
		[Serializable]
		public class Status
		{
			// Token: 0x04001F9B RID: 8091
			[ItemTypeID]
			public int id;

			// Token: 0x04001F9C RID: 8092
			public bool active;
		}
	}
}
