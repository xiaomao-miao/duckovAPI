using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000230 RID: 560
	public class ItemShortcut : MonoBehaviour
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x000434E0 File Offset: 0x000416E0
		private static CharacterMainControl Master
		{
			get
			{
				return CharacterMainControl.Main;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x000434E7 File Offset: 0x000416E7
		private static Inventory MainInventory
		{
			get
			{
				if (ItemShortcut.Master == null)
				{
					return null;
				}
				if (!ItemShortcut.Master.CharacterItem)
				{
					return null;
				}
				return ItemShortcut.Master.CharacterItem.Inventory;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x0004351A File Offset: 0x0004171A
		public static int MaxIndex
		{
			get
			{
				if (ItemShortcut.Instance == null)
				{
					return 0;
				}
				return ItemShortcut.Instance.maxIndex;
			}
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00043538 File Offset: 0x00041738
		private void Awake()
		{
			if (ItemShortcut.Instance == null)
			{
				ItemShortcut.Instance = this;
			}
			else
			{
				Debug.LogError("检测到多个ItemShortcut");
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00043597 File Offset: 0x00041797
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x000435CC File Offset: 0x000417CC
		private void Start()
		{
			this.Load();
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000435D4 File Offset: 0x000417D4
		private void OnLevelInitialized()
		{
			this.Load();
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x000435DC File Offset: 0x000417DC
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x000435E4 File Offset: 0x000417E4
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x000435EC File Offset: 0x000417EC
		private void Load()
		{
			ItemShortcut.SaveData saveData = SavesSystem.Load<ItemShortcut.SaveData>("ItemShortcut_Data");
			if (saveData == null)
			{
				return;
			}
			saveData.ApplyTo(this);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00043604 File Offset: 0x00041804
		private void Save()
		{
			ItemShortcut.SaveData saveData = new ItemShortcut.SaveData();
			saveData.Generate(this);
			SavesSystem.Save<ItemShortcut.SaveData>("ItemShortcut_Data", saveData);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004362C File Offset: 0x0004182C
		public static bool IsItemValid(Item item)
		{
			return !(item == null) && !(ItemShortcut.MainInventory == null) && !(ItemShortcut.MainInventory != item.InInventory) && !item.Tags.Contains("Weapon");
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0004367C File Offset: 0x0004187C
		private bool Set_Local(int index, Item item)
		{
			if (ItemShortcut.Master == null)
			{
				return false;
			}
			if (index < 0 || index > this.maxIndex)
			{
				return false;
			}
			if (!ItemShortcut.IsItemValid(item))
			{
				return false;
			}
			while (this.items.Count <= index)
			{
				this.items.Add(null);
			}
			while (this.itemTypes.Count <= index)
			{
				this.itemTypes.Add(-1);
			}
			this.items[index] = item;
			this.itemTypes[index] = item.TypeID;
			Action<int> onSetItem = ItemShortcut.OnSetItem;
			if (onSetItem != null)
			{
				onSetItem(index);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (i != index)
				{
					bool flag = false;
					if (this.items[i] == item)
					{
						this.items[i] = null;
						flag = true;
					}
					if (this.itemTypes[i] == item.TypeID)
					{
						this.itemTypes[i] = -1;
						this.items[i] = null;
						flag = true;
					}
					if (flag)
					{
						ItemShortcut.OnSetItem(i);
					}
				}
			}
			return true;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00043798 File Offset: 0x00041998
		private Item Get_Local(int index)
		{
			if (index >= this.items.Count)
			{
				return null;
			}
			Item item = this.items[index];
			if (item == null)
			{
				item = ItemShortcut.MainInventory.Find(this.itemTypes[index]);
				if (item != null)
				{
					this.items[index] = item;
				}
			}
			if (!ItemShortcut.IsItemValid(item))
			{
				this.SetDirty(index);
				return null;
			}
			return item;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004380A File Offset: 0x00041A0A
		private void SetDirty(int index)
		{
			this.dirtyIndexes.Add(index);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004381C File Offset: 0x00041A1C
		private void Update()
		{
			if (this.dirtyIndexes.Count > 0)
			{
				foreach (int num in this.dirtyIndexes.ToArray<int>())
				{
					if (num < this.items.Count && !ItemShortcut.IsItemValid(this.items[num]))
					{
						this.items[num] = null;
						Action<int> onSetItem = ItemShortcut.OnSetItem;
						if (onSetItem != null)
						{
							onSetItem(num);
						}
					}
				}
				this.dirtyIndexes.Clear();
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x0600116A RID: 4458 RVA: 0x000438A0 File Offset: 0x00041AA0
		// (remove) Token: 0x0600116B RID: 4459 RVA: 0x000438D4 File Offset: 0x00041AD4
		public static event Action<int> OnSetItem;

		// Token: 0x0600116C RID: 4460 RVA: 0x00043907 File Offset: 0x00041B07
		public static Item Get(int index)
		{
			if (ItemShortcut.Instance == null)
			{
				return null;
			}
			return ItemShortcut.Instance.Get_Local(index);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00043923 File Offset: 0x00041B23
		public static bool Set(int index, Item item)
		{
			return !(ItemShortcut.Instance == null) && ItemShortcut.Instance.Set_Local(index, item);
		}

		// Token: 0x04000D7B RID: 3451
		public static ItemShortcut Instance;

		// Token: 0x04000D7C RID: 3452
		[SerializeField]
		private int maxIndex = 3;

		// Token: 0x04000D7D RID: 3453
		[SerializeField]
		private List<Item> items = new List<Item>();

		// Token: 0x04000D7E RID: 3454
		[SerializeField]
		private List<int> itemTypes = new List<int>();

		// Token: 0x04000D7F RID: 3455
		private const string SaveKey = "ItemShortcut_Data";

		// Token: 0x04000D80 RID: 3456
		private HashSet<int> dirtyIndexes = new HashSet<int>();

		// Token: 0x02000528 RID: 1320
		[Serializable]
		private class SaveData
		{
			// Token: 0x17000754 RID: 1876
			// (get) Token: 0x060027A7 RID: 10151 RVA: 0x00090CB3 File Offset: 0x0008EEB3
			public int Count
			{
				get
				{
					return this.inventoryIndexes.Count;
				}
			}

			// Token: 0x060027A8 RID: 10152 RVA: 0x00090CC0 File Offset: 0x0008EEC0
			public void Generate(ItemShortcut shortcut)
			{
				this.inventoryIndexes.Clear();
				Inventory mainInventory = ItemShortcut.MainInventory;
				if (mainInventory == null)
				{
					return;
				}
				for (int i = 0; i < shortcut.items.Count; i++)
				{
					Item item = shortcut.items[i];
					int index = mainInventory.GetIndex(item);
					this.inventoryIndexes.Add(index);
				}
			}

			// Token: 0x060027A9 RID: 10153 RVA: 0x00090D20 File Offset: 0x0008EF20
			public void ApplyTo(ItemShortcut shortcut)
			{
				Inventory mainInventory = ItemShortcut.MainInventory;
				if (mainInventory == null)
				{
					return;
				}
				for (int i = 0; i < this.inventoryIndexes.Count; i++)
				{
					int num = this.inventoryIndexes[i];
					if (num >= 0)
					{
						Item itemAt = mainInventory.GetItemAt(num);
						shortcut.Set_Local(i, itemAt);
					}
				}
			}

			// Token: 0x04001E56 RID: 7766
			[SerializeField]
			internal List<int> inventoryIndexes = new List<int>();
		}
	}
}
