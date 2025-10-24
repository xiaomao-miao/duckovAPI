using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishing.UI
{
	// Token: 0x02000218 RID: 536
	public class BaitSelectPanelEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0003E725 File Offset: 0x0003C925
		public Item Target
		{
			get
			{
				return this.targetItem;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0003E72D File Offset: 0x0003C92D
		private bool Selected
		{
			get
			{
				return !(this.master == null) && this.master.GetSelection() == this;
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0003E750 File Offset: 0x0003C950
		internal void Setup(BaitSelectPanel master, Item cur)
		{
			this.UnregisterEvents();
			this.master = master;
			this.targetItem = cur;
			this.itemDisplay.Setup(this.targetItem);
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0003E783 File Offset: 0x0003C983
		private void RegisterEvents()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetSelection += this.Refresh;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0003E7AB File Offset: 0x0003C9AB
		private void UnregisterEvents()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetSelection -= this.Refresh;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0003E7D3 File Offset: 0x0003C9D3
		private void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0003E7E6 File Offset: 0x0003C9E6
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnPointerClick;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0003E7FF File Offset: 0x0003C9FF
		public void OnPointerClick(PointerEventData eventData)
		{
			eventData.Use();
			this.master.NotifySelect(this);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0003E813 File Offset: 0x0003CA13
		private void OnPointerClick(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
		}

		// Token: 0x04000CCD RID: 3277
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04000CCE RID: 3278
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04000CCF RID: 3279
		private BaitSelectPanel master;

		// Token: 0x04000CD0 RID: 3280
		private Item targetItem;
	}
}
