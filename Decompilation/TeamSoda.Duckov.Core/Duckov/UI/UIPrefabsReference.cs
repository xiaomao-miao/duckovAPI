using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000375 RID: 885
	[CreateAssetMenu]
	public class UIPrefabsReference : ScriptableObject
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x0006B981 File Offset: 0x00069B81
		public ItemDisplay ItemDisplay
		{
			get
			{
				return this.itemDisplay;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x0006B989 File Offset: 0x00069B89
		public SlotIndicator SlotIndicator
		{
			get
			{
				return this.slotIndicator;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x0006B991 File Offset: 0x00069B91
		public SlotDisplay SlotDisplay
		{
			get
			{
				return this.slotDisplay;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001E9C RID: 7836 RVA: 0x0006B999 File Offset: 0x00069B99
		public InventoryEntry InventoryEntry
		{
			get
			{
				return this.inventoryEntry;
			}
		}

		// Token: 0x040014E6 RID: 5350
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040014E7 RID: 5351
		[SerializeField]
		private SlotIndicator slotIndicator;

		// Token: 0x040014E8 RID: 5352
		[SerializeField]
		private SlotDisplay slotDisplay;

		// Token: 0x040014E9 RID: 5353
		[SerializeField]
		private InventoryEntry inventoryEntry;
	}
}
