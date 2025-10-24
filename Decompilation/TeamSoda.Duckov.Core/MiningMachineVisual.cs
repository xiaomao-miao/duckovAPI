using System;
using System.Collections.Generic;
using Duckov.Bitcoins;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class MiningMachineVisual : MonoBehaviour
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x00030DD4 File Offset: 0x0002EFD4
	private void Update()
	{
		if (!this.inited && BitcoinMiner.Instance && BitcoinMiner.Instance.Item != null)
		{
			this.inited = true;
			this.minnerItem = BitcoinMiner.Instance.Item;
			this.minnerItem.onSlotContentChanged += this.OnSlotContentChanged;
			this.slots = this.minnerItem.Slots;
			this.OnSlotContentChanged(this.minnerItem, null);
			return;
		}
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00030E54 File Offset: 0x0002F054
	private void OnDestroy()
	{
		if (this.minnerItem)
		{
			this.minnerItem.onSlotContentChanged -= this.OnSlotContentChanged;
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00030E7C File Offset: 0x0002F07C
	private void OnSlotContentChanged(Item minnerItem, Slot changedSlot)
	{
		for (int i = 0; i < this.slots.Count; i++)
		{
			if (!(this.cardsDisplay[i] == null))
			{
				Item content = this.slots[i].Content;
				MiningMachineCardDisplay.CardTypes cardType = MiningMachineCardDisplay.CardTypes.normal;
				if (content != null)
				{
					ItemSetting_GPU component = content.GetComponent<ItemSetting_GPU>();
					if (component)
					{
						cardType = component.cardType;
					}
				}
				this.cardsDisplay[i].SetVisualActive(content != null, cardType);
			}
		}
	}

	// Token: 0x040009DB RID: 2523
	public List<MiningMachineCardDisplay> cardsDisplay;

	// Token: 0x040009DC RID: 2524
	private bool inited;

	// Token: 0x040009DD RID: 2525
	private SlotCollection slots;

	// Token: 0x040009DE RID: 2526
	private Item minnerItem;
}
