using System;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000305 RID: 773
	public class DeathLotteryCard : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600191F RID: 6431 RVA: 0x0005B4D0 File Offset: 0x000596D0
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0005B4D8 File Offset: 0x000596D8
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.Target == null)
			{
				return;
			}
			DeathLottery.OptionalCosts cost = this.master.Target.GetCost();
			this.master.NotifyEntryClicked(this, cost.costA);
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0005B52C File Offset: 0x0005972C
		public void Setup(DeathLotteryVIew master, int index)
		{
			if (master == null)
			{
				return;
			}
			if (master.Target == null)
			{
				return;
			}
			this.master = master;
			this.targetItem = master.Target.ItemInstances[index];
			this.index = index;
			this.itemDisplay.Setup(this.targetItem);
			this.cardDisplay.SetFacing(master.Target.CurrentStatus.selectedItems.Contains(index), true);
			this.Refresh();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0005B5B0 File Offset: 0x000597B0
		public void NotifyFacing(bool uncovered)
		{
			this.cardDisplay.SetFacing(uncovered, false);
			this.Refresh();
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x0005B5C8 File Offset: 0x000597C8
		private bool Selected
		{
			get
			{
				return !(this.master == null) && !(this.master.Target == null) && this.master.Target.CurrentStatus.selectedItems != null && this.master.Target.CurrentStatus.selectedItems.Contains(this.Index);
			}
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0005B633 File Offset: 0x00059833
		private void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0005B646 File Offset: 0x00059846
		private void Awake()
		{
			this.costFade.Hide();
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0005B654 File Offset: 0x00059854
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.master.Target.CurrentStatus.SelectedCount >= this.master.Target.MaxChances)
			{
				return;
			}
			Cost costA = this.master.Target.GetCost().costA;
			this.costDisplay.Setup(costA, 1);
			this.freeIndicator.SetActive(costA.IsFree);
			this.costFade.Show();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0005B6CC File Offset: 0x000598CC
		public void OnPointerExit(PointerEventData eventData)
		{
			this.costFade.Hide();
		}

		// Token: 0x0400123A RID: 4666
		[SerializeField]
		private CardDisplay cardDisplay;

		// Token: 0x0400123B RID: 4667
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x0400123C RID: 4668
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x0400123D RID: 4669
		[SerializeField]
		private GameObject freeIndicator;

		// Token: 0x0400123E RID: 4670
		[SerializeField]
		private FadeGroup costFade;

		// Token: 0x0400123F RID: 4671
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001240 RID: 4672
		private DeathLotteryVIew master;

		// Token: 0x04001241 RID: 4673
		private int index;

		// Token: 0x04001242 RID: 4674
		private Item targetItem;
	}
}
