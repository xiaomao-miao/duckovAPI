using System;
using ItemStatsSystem;

// Token: 0x020000E9 RID: 233
public static class InventoryExtensions
{
	// Token: 0x060007BE RID: 1982 RVA: 0x000229B8 File Offset: 0x00020BB8
	private static void Sort(this Inventory inventory, Comparison<Item> comparison)
	{
		inventory.Content.Sort(comparison);
	}
}
