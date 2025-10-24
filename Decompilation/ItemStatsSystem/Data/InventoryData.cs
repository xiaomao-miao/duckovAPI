using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

namespace ItemStatsSystem.Data
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class InventoryData
	{
		// Token: 0x0600023D RID: 573 RVA: 0x00008C40 File Offset: 0x00006E40
		public static InventoryData FromInventory(Inventory inventory)
		{
			InventoryData inventoryData = new InventoryData();
			inventoryData.capacity = inventory.Capacity;
			int lastItemPosition = inventory.GetLastItemPosition();
			for (int i = 0; i <= lastItemPosition; i++)
			{
				Item itemAt = inventory.GetItemAt(i);
				if (!(itemAt == null))
				{
					InventoryData.Entry entry = new InventoryData.Entry();
					entry.inventoryPosition = i;
					entry.itemTreeData = ItemTreeData.FromItem(itemAt);
					inventoryData.entries.Add(entry);
				}
			}
			inventoryData.lockedIndexes = new List<int>(inventory.lockedIndexes);
			return inventoryData;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008CC0 File Offset: 0x00006EC0
		public static UniTask LoadIntoInventory(InventoryData data, Inventory inventoryInstance)
		{
			InventoryData.<LoadIntoInventory>d__5 <LoadIntoInventory>d__;
			<LoadIntoInventory>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadIntoInventory>d__.data = data;
			<LoadIntoInventory>d__.inventoryInstance = inventoryInstance;
			<LoadIntoInventory>d__.<>1__state = -1;
			<LoadIntoInventory>d__.<>t__builder.Start<InventoryData.<LoadIntoInventory>d__5>(ref <LoadIntoInventory>d__);
			return <LoadIntoInventory>d__.<>t__builder.Task;
		}

		// Token: 0x040000CD RID: 205
		public int capacity = 16;

		// Token: 0x040000CE RID: 206
		public List<InventoryData.Entry> entries = new List<InventoryData.Entry>();

		// Token: 0x040000CF RID: 207
		public List<int> lockedIndexes = new List<int>();

		// Token: 0x02000053 RID: 83
		[Serializable]
		public class Entry
		{
			// Token: 0x04000141 RID: 321
			public int inventoryPosition;

			// Token: 0x04000142 RID: 322
			public ItemTreeData itemTreeData;
		}
	}
}
