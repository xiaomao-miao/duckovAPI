using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ItemStatsSystem.Items;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200001D RID: 29
	public static class ItemTreeExtensions
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00006D00 File Offset: 0x00004F00
		public static List<Item> GetAllChildren(this Item item, bool includingGrandChildren = true, bool excludeSelf = false)
		{
			ItemTreeExtensions.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.item = item;
			if (CS$<>8__locals1.item == null)
			{
				return new List<Item>();
			}
			CS$<>8__locals1.children = new List<Item>();
			CS$<>8__locals1.pendingItems = new Stack<Item>();
			ItemTreeExtensions.<GetAllChildren>g__PushAllInSlots|0_1(CS$<>8__locals1.item, ref CS$<>8__locals1);
			ItemTreeExtensions.<GetAllChildren>g__PushAllInInventory|0_2(CS$<>8__locals1.item, ref CS$<>8__locals1);
			if (includingGrandChildren)
			{
				while (CS$<>8__locals1.pendingItems.Count > 0)
				{
					Item item2 = CS$<>8__locals1.pendingItems.Pop();
					ItemTreeExtensions.<GetAllChildren>g__PushAllInSlots|0_1(item2, ref CS$<>8__locals1);
					ItemTreeExtensions.<GetAllChildren>g__PushAllInInventory|0_2(item2, ref CS$<>8__locals1);
				}
			}
			if (!excludeSelf)
			{
				CS$<>8__locals1.children.Add(CS$<>8__locals1.item);
			}
			return CS$<>8__locals1.children;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006DA4 File Offset: 0x00004FA4
		public static List<Item> GetAllParents(this Item item, bool excludeSelf = false)
		{
			List<Item> list = new List<Item>();
			if (item == null)
			{
				return list;
			}
			Item parentItem = item.ParentItem;
			while (parentItem != null)
			{
				if (list.Contains(parentItem))
				{
					Debug.LogError("Item parenting loop detected!");
					break;
				}
				list.Add(parentItem);
				parentItem = parentItem.ParentItem;
			}
			if (!excludeSelf)
			{
				list.Insert(0, item);
			}
			return list;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006E04 File Offset: 0x00005004
		public static Item GetRoot(this Item item)
		{
			if (item == null)
			{
				return null;
			}
			int num = 0;
			while (item.ParentItem != null)
			{
				item = item.ParentItem;
				num++;
				if (num >= 32)
				{
					Debug.LogError("Too much layers in Item. Check if item reference loop occurred!");
					break;
				}
			}
			return item;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006E4C File Offset: 0x0000504C
		public static void DestroyTree(this Item item)
		{
			if (item == null)
			{
				return;
			}
			item.MarkDestroyed();
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(item.gameObject);
				return;
			}
			UnityEngine.Object.DestroyImmediate(item.gameObject);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006E7C File Offset: 0x0000507C
		public static void DestroyTreeImmediate(this Item item)
		{
			if (item == null)
			{
				return;
			}
			UnityEngine.Object.DestroyImmediate(item.gameObject);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006E93 File Offset: 0x00005093
		public static List<Item> GetAllConnected(this Item item)
		{
			if (item == null)
			{
				return null;
			}
			return item.GetRoot().GetAllChildren(true, false);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006EAD File Offset: 0x000050AD
		[CompilerGenerated]
		internal static void <GetAllChildren>g__Push|0_0(Item pendingItem, ref ItemTreeExtensions.<>c__DisplayClass0_0 A_1)
		{
			if (pendingItem == null)
			{
				return;
			}
			if (pendingItem == A_1.item)
			{
				Debug.LogWarning("Item Loop Detected! Aborting!");
				return;
			}
			A_1.children.Add(pendingItem);
			A_1.pendingItems.Push(pendingItem);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006EEC File Offset: 0x000050EC
		[CompilerGenerated]
		internal static void <GetAllChildren>g__PushAllInSlots|0_1(Item item, ref ItemTreeExtensions.<>c__DisplayClass0_0 A_1)
		{
			if (item == null)
			{
				return;
			}
			if (item.Slots == null)
			{
				return;
			}
			foreach (Slot slot in item.Slots)
			{
				ItemTreeExtensions.<GetAllChildren>g__Push|0_0(slot.Content, ref A_1);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006F58 File Offset: 0x00005158
		[CompilerGenerated]
		internal static void <GetAllChildren>g__PushAllInInventory|0_2(Item item, ref ItemTreeExtensions.<>c__DisplayClass0_0 A_1)
		{
			if (item == null)
			{
				return;
			}
			if (item.Inventory == null)
			{
				return;
			}
			foreach (Item pendingItem in item.Inventory)
			{
				ItemTreeExtensions.<GetAllChildren>g__Push|0_0(pendingItem, ref A_1);
			}
		}
	}
}
