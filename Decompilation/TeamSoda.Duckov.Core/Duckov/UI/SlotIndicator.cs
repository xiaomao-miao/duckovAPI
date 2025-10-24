using System;
using Duckov.Utilities;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039E RID: 926
	public class SlotIndicator : MonoBehaviour, IPoolable
	{
		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002107 RID: 8455 RVA: 0x000735D7 File Offset: 0x000717D7
		// (set) Token: 0x06002108 RID: 8456 RVA: 0x000735DF File Offset: 0x000717DF
		public Slot Target { get; private set; }

		// Token: 0x06002109 RID: 8457 RVA: 0x000735E8 File Offset: 0x000717E8
		public void Setup(Slot target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00073603 File Offset: 0x00071803
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregisterEvents();
			this.Target.onSlotContentChanged += this.OnSlotContentChanged;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0007362B File Offset: 0x0007182B
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onSlotContentChanged -= this.OnSlotContentChanged;
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0007364D File Offset: 0x0007184D
		private void OnSlotContentChanged(Slot slot)
		{
			if (slot != this.Target)
			{
				Debug.LogError("Slot内容改变事件触发了，但它来自别的Slot。这说明Slot Indicator注册的事件发生了泄露，请检查代码。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00073669 File Offset: 0x00071869
		private void Refresh()
		{
			if (this.contentIndicator == null)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			this.contentIndicator.SetActive(this.Target.Content);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0007369E File Offset: 0x0007189E
		public void NotifyPooled()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000736AC File Offset: 0x000718AC
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
			this.contentIndicator.SetActive(false);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000736C7 File Offset: 0x000718C7
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000736D5 File Offset: 0x000718D5
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x04001670 RID: 5744
		[SerializeField]
		private GameObject contentIndicator;
	}
}
