using System;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class FishingRod : MonoBehaviour
{
	// Token: 0x1700014D RID: 333
	// (get) Token: 0x0600072F RID: 1839 RVA: 0x0002022B File Offset: 0x0001E42B
	private ItemAgent selfAgent
	{
		get
		{
			if (this._selfAgent == null)
			{
				this._selfAgent = base.GetComponent<ItemAgent>();
			}
			return this._selfAgent;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000730 RID: 1840 RVA: 0x0002024D File Offset: 0x0001E44D
	public Item Bait
	{
		get
		{
			if (this.baitSlot == null)
			{
				this.baitSlot = this.selfAgent.Item.Slots.GetSlot("Bait");
			}
			if (this.baitSlot != null)
			{
				return this.baitSlot.Content;
			}
			return null;
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002028C File Offset: 0x0001E48C
	public bool UseBait()
	{
		Item bait = this.Bait;
		if (bait == null)
		{
			return false;
		}
		if (bait.Stackable)
		{
			bait.StackCount--;
		}
		else
		{
			bait.DestroyTree();
		}
		return true;
	}

	// Token: 0x040006D8 RID: 1752
	[SerializeField]
	private ItemAgent _selfAgent;

	// Token: 0x040006D9 RID: 1753
	private Slot baitSlot;

	// Token: 0x040006DA RID: 1754
	public Transform lineStart;
}
