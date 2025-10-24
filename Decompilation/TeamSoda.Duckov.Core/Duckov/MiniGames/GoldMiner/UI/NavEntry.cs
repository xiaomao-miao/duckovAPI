using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.MiniGames.GoldMiner.UI
{
	// Token: 0x020002AC RID: 684
	public class NavEntry : MonoBehaviour
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00052106 File Offset: 0x00050306
		// (set) Token: 0x06001641 RID: 5697 RVA: 0x0005210E File Offset: 0x0005030E
		public bool selectionState { get; private set; }

		// Token: 0x06001642 RID: 5698 RVA: 0x00052118 File Offset: 0x00050318
		private void Awake()
		{
			if (this.masterGroup == null)
			{
				this.masterGroup = base.GetComponentInParent<NavGroup>();
			}
			this.VCT = base.GetComponent<VirtualCursorTarget>();
			if (this.VCT)
			{
				this.VCT.onEnter.AddListener(new UnityAction(this.TrySelectThis));
				this.VCT.onClick.AddListener(new UnityAction(this.Interact));
			}
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00052190 File Offset: 0x00050390
		private void Interact()
		{
			this.NotifyInteract();
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00052198 File Offset: 0x00050398
		public void NotifySelectionState(bool value)
		{
			this.selectionState = value;
			this.selectedIndicator.SetActive(this.selectionState);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x000521B2 File Offset: 0x000503B2
		internal void NotifyInteract()
		{
			Action<NavEntry> action = this.onInteract;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000521C5 File Offset: 0x000503C5
		public void TrySelectThis()
		{
			if (this.masterGroup == null)
			{
				return;
			}
			this.masterGroup.TrySelect(this);
		}

		// Token: 0x0400107D RID: 4221
		public GameObject selectedIndicator;

		// Token: 0x0400107E RID: 4222
		public Action<NavEntry> onInteract;

		// Token: 0x0400107F RID: 4223
		public Action<NavEntry> onTrySelectThis;

		// Token: 0x04001080 RID: 4224
		public NavGroup masterGroup;

		// Token: 0x04001081 RID: 4225
		public VirtualCursorTarget VCT;
	}
}
