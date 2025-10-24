using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem.Items;
using UnityEngine;

namespace ItemStatsSystem.Data
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	public class ItemTreeData
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000232 RID: 562 RVA: 0x000087FC File Offset: 0x000069FC
		// (remove) Token: 0x06000233 RID: 563 RVA: 0x00008830 File Offset: 0x00006A30
		public static event Action<Item> OnItemLoaded;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00008864 File Offset: 0x00006A64
		public ItemTreeData.DataEntry RootData
		{
			get
			{
				ItemTreeData.DataEntry dataEntry = this.entries.Find((ItemTreeData.DataEntry e) => e.instanceID == this.rootInstanceID);
				if (dataEntry == null)
				{
					return null;
				}
				return dataEntry;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00008890 File Offset: 0x00006A90
		public int RootTypeID
		{
			get
			{
				ItemTreeData.DataEntry dataEntry = this.entries.Find((ItemTreeData.DataEntry e) => e.instanceID == this.rootInstanceID);
				if (dataEntry == null)
				{
					return 0;
				}
				return dataEntry.typeID;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000088C0 File Offset: 0x00006AC0
		public static ItemTreeData FromItem(Item item)
		{
			ItemTreeData itemTreeData = new ItemTreeData();
			Dictionary<int, ItemTreeData.DataEntry> dictionary = new Dictionary<int, ItemTreeData.DataEntry>();
			itemTreeData.rootInstanceID = item.GetInstanceID();
			List<Item> allChildren = item.GetAllChildren(true, false);
			foreach (Item item2 in allChildren)
			{
				ItemTreeData.DataEntry dataEntry = new ItemTreeData.DataEntry
				{
					instanceID = item2.GetInstanceID(),
					typeID = item2.TypeID
				};
				foreach (CustomData copyFrom in item2.Variables)
				{
					dataEntry.variables.Add(new CustomData(copyFrom));
				}
				if (item2.Inventory != null)
				{
					int lastItemPosition = item2.Inventory.GetLastItemPosition();
					for (int i = 0; i <= lastItemPosition; i++)
					{
						Item item3 = item2.Inventory[i];
						if (item3 != null)
						{
							dataEntry.inventory.Add(new ItemTreeData.InventoryDataEntry(i, item3.GetInstanceID()));
						}
					}
					dataEntry.inventorySortLocks = new List<int>(item2.Inventory.lockedIndexes);
				}
				dictionary.Add(dataEntry.instanceID, dataEntry);
				itemTreeData.entries.Add(dataEntry);
			}
			foreach (Item item4 in allChildren)
			{
				ItemTreeData.DataEntry dataEntry2 = dictionary[item4.GetInstanceID()];
				if (!(item4.Slots == null))
				{
					foreach (Slot slot in item4.Slots)
					{
						if (slot.Content != null)
						{
							dataEntry2.slotContents.Add(new ItemTreeData.SlotInstanceIDPair(slot.Key, slot.Content.GetInstanceID()));
						}
					}
				}
			}
			return itemTreeData;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00008B30 File Offset: 0x00006D30
		public static UniTask<Item> InstantiateAsync(ItemTreeData data)
		{
			ItemTreeData.<InstantiateAsync>d__13 <InstantiateAsync>d__;
			<InstantiateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<InstantiateAsync>d__.data = data;
			<InstantiateAsync>d__.<>1__state = -1;
			<InstantiateAsync>d__.<>t__builder.Start<ItemTreeData.<InstantiateAsync>d__13>(ref <InstantiateAsync>d__);
			return <InstantiateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008B74 File Offset: 0x00006D74
		public ItemTreeData.DataEntry GetEntry(int instanceID)
		{
			return this.entries.Find((ItemTreeData.DataEntry e) => e.instanceID == instanceID);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008BA8 File Offset: 0x00006DA8
		public override string ToString()
		{
			ItemTreeData.<>c__DisplayClass15_0 CS$<>8__locals1 = new ItemTreeData.<>c__DisplayClass15_0();
			CS$<>8__locals1.<>4__this = this;
			ItemTreeData.DataEntry dataEntry = this.entries.Find((ItemTreeData.DataEntry e) => e.instanceID == CS$<>8__locals1.<>4__this.rootInstanceID);
			if (dataEntry == null)
			{
				Debug.LogError("No Root Entry in Tree");
				return "Invalid Item Tree";
			}
			CS$<>8__locals1.indent = 0;
			CS$<>8__locals1.result = "";
			CS$<>8__locals1.<ToString>g__PrintEntry|1(dataEntry);
			return CS$<>8__locals1.result;
		}

		// Token: 0x040000CB RID: 203
		public int rootInstanceID;

		// Token: 0x040000CC RID: 204
		public List<ItemTreeData.DataEntry> entries = new List<ItemTreeData.DataEntry>();

		// Token: 0x0200004D RID: 77
		[Serializable]
		public class DataEntry
		{
			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000290 RID: 656 RVA: 0x00009729 File Offset: 0x00007929
			public string TypeName
			{
				get
				{
					return string.Format("TYPE_{0}", this.typeID);
				}
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x06000291 RID: 657 RVA: 0x00009740 File Offset: 0x00007940
			public int StackCount
			{
				get
				{
					CustomData customData = this.variables.Find((CustomData e) => e.Key == "Count");
					if (customData == null)
					{
						return 1;
					}
					if (customData.DataType != CustomDataType.Int)
					{
						return 1;
					}
					return customData.GetInt();
				}
			}

			// Token: 0x04000128 RID: 296
			public int instanceID;

			// Token: 0x04000129 RID: 297
			public int typeID;

			// Token: 0x0400012A RID: 298
			public List<CustomData> variables = new List<CustomData>();

			// Token: 0x0400012B RID: 299
			public List<ItemTreeData.SlotInstanceIDPair> slotContents = new List<ItemTreeData.SlotInstanceIDPair>();

			// Token: 0x0400012C RID: 300
			public List<ItemTreeData.InventoryDataEntry> inventory = new List<ItemTreeData.InventoryDataEntry>();

			// Token: 0x0400012D RID: 301
			public List<int> inventorySortLocks = new List<int>();
		}

		// Token: 0x0200004E RID: 78
		public class SlotInstanceIDPair
		{
			// Token: 0x06000293 RID: 659 RVA: 0x000097C2 File Offset: 0x000079C2
			public SlotInstanceIDPair(string slot, int instanceID)
			{
				this.slot = slot;
				this.instanceID = instanceID;
			}

			// Token: 0x0400012E RID: 302
			public string slot;

			// Token: 0x0400012F RID: 303
			public int instanceID;
		}

		// Token: 0x0200004F RID: 79
		public class InventoryDataEntry
		{
			// Token: 0x06000294 RID: 660 RVA: 0x000097D8 File Offset: 0x000079D8
			public InventoryDataEntry(int position, int instanceID)
			{
				this.position = position;
				this.instanceID = instanceID;
			}

			// Token: 0x04000130 RID: 304
			public int position;

			// Token: 0x04000131 RID: 305
			public int instanceID;
		}
	}
}
