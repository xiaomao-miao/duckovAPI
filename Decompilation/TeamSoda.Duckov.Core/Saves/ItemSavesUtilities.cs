using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Data;

namespace Saves
{
	// Token: 0x02000225 RID: 549
	public static class ItemSavesUtilities
	{
		// Token: 0x06001076 RID: 4214 RVA: 0x0003FC9C File Offset: 0x0003DE9C
		public static void SaveAsLastDeadCharacter(Item item)
		{
			uint num = SavesSystem.Load<uint>("DeadCharacterToken");
			uint num2 = num;
			do
			{
				num2 += 1U;
			}
			while (num2 == num);
			SavesSystem.Save<uint>("DeadCharacterToken", num2);
			item.Save("LastDeadCharacter");
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0003FCD4 File Offset: 0x0003DED4
		public static UniTask<Item> LoadLastDeadCharacterItem()
		{
			ItemSavesUtilities.<LoadLastDeadCharacterItem>d__3 <LoadLastDeadCharacterItem>d__;
			<LoadLastDeadCharacterItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<LoadLastDeadCharacterItem>d__.<>1__state = -1;
			<LoadLastDeadCharacterItem>d__.<>t__builder.Start<ItemSavesUtilities.<LoadLastDeadCharacterItem>d__3>(ref <LoadLastDeadCharacterItem>d__);
			return <LoadLastDeadCharacterItem>d__.<>t__builder.Task;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0003FD10 File Offset: 0x0003DF10
		public static void Save(this Item item, string key)
		{
			ItemTreeData value = ItemTreeData.FromItem(item);
			SavesSystem.Save<ItemTreeData>("Item/", key, value);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0003FD30 File Offset: 0x0003DF30
		public static void Save(this Inventory inventory, string key)
		{
			InventoryData value = InventoryData.FromInventory(inventory);
			SavesSystem.Save<InventoryData>("Inventory/", key, value);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0003FD50 File Offset: 0x0003DF50
		public static UniTask<Item> LoadItem(string key)
		{
			ItemSavesUtilities.<LoadItem>d__6 <LoadItem>d__;
			<LoadItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<LoadItem>d__.key = key;
			<LoadItem>d__.<>1__state = -1;
			<LoadItem>d__.<>t__builder.Start<ItemSavesUtilities.<LoadItem>d__6>(ref <LoadItem>d__);
			return <LoadItem>d__.<>t__builder.Task;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0003FD94 File Offset: 0x0003DF94
		public static UniTask LoadInventory(string key, Inventory inventoryInstance)
		{
			ItemSavesUtilities.<LoadInventory>d__7 <LoadInventory>d__;
			<LoadInventory>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadInventory>d__.key = key;
			<LoadInventory>d__.inventoryInstance = inventoryInstance;
			<LoadInventory>d__.<>1__state = -1;
			<LoadInventory>d__.<>t__builder.Start<ItemSavesUtilities.<LoadInventory>d__7>(ref <LoadInventory>d__);
			return <LoadInventory>d__.<>t__builder.Task;
		}

		// Token: 0x04000D18 RID: 3352
		private const string InventoryPrefix = "Inventory/";

		// Token: 0x04000D19 RID: 3353
		private const string ItemPrefix = "Item/";
	}
}
