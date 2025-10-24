using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x0200037D RID: 893
	public class ItemPickerEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06001F00 RID: 7936 RVA: 0x0006CD4D File Offset: 0x0006AF4D
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0006CD66 File Offset: 0x0006AF66
		private void OnDestroy()
		{
			this.itemDisplay.onPointerClick -= this.OnItemDisplayClicked;
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0006CD7F File Offset: 0x0006AF7F
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData eventData)
		{
			this.master.NotifyEntryClicked(this, this.target);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0006CD94 File Offset: 0x0006AF94
		public void Setup(ItemPicker master, Item item)
		{
			this.master = master;
			this.target = item;
			if (this.target != null)
			{
				this.itemDisplay.Setup(this.target);
			}
			else
			{
				Debug.LogError("Item Picker不应当展示空的Item。");
			}
			this.itemDisplay.ShowOperationButtons = false;
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0006CDE6 File Offset: 0x0006AFE6
		public void NotifyPooled()
		{
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0006CDE8 File Offset: 0x0006AFE8
		public void NotifyReleased()
		{
		}

		// Token: 0x04001538 RID: 5432
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001539 RID: 5433
		private ItemPicker master;

		// Token: 0x0400153A RID: 5434
		private Item target;
	}
}
