using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000391 RID: 913
	public class ItemDetailsPanel : ManagedUIElement
	{
		// Token: 0x06002023 RID: 8227 RVA: 0x0007086A File Offset: 0x0006EA6A
		protected override void Awake()
		{
			base.Awake();
			if (ItemDetailsPanel.instance == null)
			{
				ItemDetailsPanel.instance = this;
			}
			this.closeButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000708A1 File Offset: 0x0006EAA1
		private void OnCloseButtonClicked()
		{
			base.Close();
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x000708A9 File Offset: 0x0006EAA9
		public static void Show(Item target, ManagedUIElement source = null)
		{
			if (ItemDetailsPanel.instance == null)
			{
				return;
			}
			ItemDetailsPanel.instance.Open(target, source);
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x000708C5 File Offset: 0x0006EAC5
		public void Open(Item target, ManagedUIElement source)
		{
			this.target = target;
			this.source = source;
			base.Open(source);
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000708DC File Offset: 0x0006EADC
		protected override void OnOpen()
		{
			if (this.target == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.Setup(this.target);
			this.fadeGroup.Show();
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00070910 File Offset: 0x0006EB10
		protected override void OnClose()
		{
			this.UnregisterEvents();
			this.target = null;
			this.fadeGroup.Hide();
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0007092A File Offset: 0x0006EB2A
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00070932 File Offset: 0x0006EB32
		internal void Setup(Item target)
		{
			this.display.Setup(target);
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00070940 File Offset: 0x0006EB40
		private void UnregisterEvents()
		{
			this.display.UnregisterEvents();
		}

		// Token: 0x040015F0 RID: 5616
		private static ItemDetailsPanel instance;

		// Token: 0x040015F1 RID: 5617
		private Item target;

		// Token: 0x040015F2 RID: 5618
		[SerializeField]
		private ItemDetailsDisplay display;

		// Token: 0x040015F3 RID: 5619
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015F4 RID: 5620
		[SerializeField]
		private Button closeButton;

		// Token: 0x040015F5 RID: 5621
		private ManagedUIElement source;
	}
}
