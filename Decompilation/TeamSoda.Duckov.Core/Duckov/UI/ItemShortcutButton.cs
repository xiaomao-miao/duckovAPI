using System;
using DG.Tweening;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A9 RID: 937
	public class ItemShortcutButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x00075318 File Offset: 0x00073518
		// (set) Token: 0x0600218F RID: 8591 RVA: 0x00075320 File Offset: 0x00073520
		public int Index { get; private set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x00075329 File Offset: 0x00073529
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x00075331 File Offset: 0x00073531
		public ItemShortcutPanel Master { get; private set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0007533A File Offset: 0x0007353A
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x00075342 File Offset: 0x00073542
		public Inventory Inventory { get; private set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x0007534B File Offset: 0x0007354B
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x00075353 File Offset: 0x00073553
		public CharacterMainControl Character { get; private set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x0007535C File Offset: 0x0007355C
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x00075364 File Offset: 0x00073564
		public Item TargetItem { get; private set; }

		// Token: 0x06002198 RID: 8600 RVA: 0x0007536D File Offset: 0x0007356D
		private Item GetTargetItem()
		{
			return ItemShortcut.Get(this.Index);
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x0007537C File Offset: 0x0007357C
		private bool Interactable
		{
			get
			{
				Item targetItem = this.TargetItem;
				return ((targetItem != null) ? targetItem.UsageUtilities : null) || (this.TargetItem && this.TargetItem.HasHandHeldAgent) || (this.TargetItem && this.TargetItem.GetBool("IsSkill", false));
			}
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000753E4 File Offset: 0x000735E4
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.Interactable)
			{
				this.denialIndicator.color = this.denialColor;
				this.denialIndicator.DOColor(Color.clear, 0.1f);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.UsageUtilities && this.TargetItem.UsageUtilities.IsUsable(this.TargetItem, this.Character))
			{
				this.Character.UseItem(this.TargetItem);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.GetBool("IsSkill", false))
			{
				this.Character.ChangeHoldItem(this.TargetItem);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.HasHandHeldAgent)
			{
				this.Character.ChangeHoldItem(this.TargetItem);
				return;
			}
			this.AnimateDenial();
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000754FD File Offset: 0x000736FD
		public void AnimateDenial()
		{
			this.denialIndicator.DOKill(false);
			this.denialIndicator.color = this.denialColor;
			this.denialIndicator.DOColor(Color.clear, 0.1f);
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00075533 File Offset: 0x00073733
		private void Awake()
		{
			ItemShortcutButton.OnRequireAnimateDenial += this.OnStaticAnimateDenial;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00075546 File Offset: 0x00073746
		private void OnDestroy()
		{
			ItemShortcutButton.OnRequireAnimateDenial -= this.OnStaticAnimateDenial;
			this.isBeingDestroyed = true;
			this.UnregisterEvents();
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x00075566 File Offset: 0x00073766
		private void OnStaticAnimateDenial(int index)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (index == this.Index)
			{
				this.AnimateDenial();
			}
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x0600219F RID: 8607 RVA: 0x00075580 File Offset: 0x00073780
		// (remove) Token: 0x060021A0 RID: 8608 RVA: 0x000755B4 File Offset: 0x000737B4
		private static event Action<int> OnRequireAnimateDenial;

		// Token: 0x060021A1 RID: 8609 RVA: 0x000755E7 File Offset: 0x000737E7
		public static void AnimateDenial(int index)
		{
			Action<int> onRequireAnimateDenial = ItemShortcutButton.OnRequireAnimateDenial;
			if (onRequireAnimateDenial == null)
			{
				return;
			}
			onRequireAnimateDenial(index);
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000755FC File Offset: 0x000737FC
		internal void Initialize(ItemShortcutPanel itemShortcutPanel, int index)
		{
			this.UnregisterEvents();
			this.Master = itemShortcutPanel;
			this.Inventory = this.Master.Target;
			this.Index = index;
			this.Character = this.Master.Character;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x0007564C File Offset: 0x0007384C
		private void Refresh()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this.UnregisterEvents();
			this.TargetItem = this.GetTargetItem();
			if (this.TargetItem == null)
			{
				this.SetupEmpty();
			}
			else
			{
				this.SetupItem(this.TargetItem);
			}
			this.RegisterEvents();
			this.requireRefresh = false;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000756A4 File Offset: 0x000738A4
		private void SetupItem(Item targetItem)
		{
			if (this.notInteractableIndicator)
			{
				this.notInteractableIndicator.gameObject.SetActive(false);
			}
			this.itemDisplay.Setup(targetItem);
			this.itemDisplay.gameObject.SetActive(true);
			this.notInteractableIndicator.gameObject.SetActive(!this.Interactable);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00075705 File Offset: 0x00073905
		private void SetupEmpty()
		{
			this.itemDisplay.gameObject.SetActive(false);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00075718 File Offset: 0x00073918
		private void RegisterEvents()
		{
			ItemShortcut.OnSetItem += this.OnItemShortcutSetItem;
			if (this.Inventory != null)
			{
				this.Inventory.onContentChanged += this.OnContentChanged;
			}
			if (this.TargetItem != null)
			{
				this.TargetItem.onSetStackCount += this.OnItemStackCountChanged;
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00075780 File Offset: 0x00073980
		private void UnregisterEvents()
		{
			ItemShortcut.OnSetItem -= this.OnItemShortcutSetItem;
			if (this.Inventory != null)
			{
				this.Inventory.onContentChanged -= this.OnContentChanged;
			}
			if (this.TargetItem != null)
			{
				this.TargetItem.onSetStackCount -= this.OnItemStackCountChanged;
			}
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000757E8 File Offset: 0x000739E8
		private void OnItemShortcutSetItem(int obj)
		{
			this.Refresh();
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000757F0 File Offset: 0x000739F0
		private void OnItemStackCountChanged(Item item)
		{
			if (item != this.TargetItem)
			{
				return;
			}
			this.requireRefresh = true;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00075808 File Offset: 0x00073A08
		private void OnContentChanged(Inventory inventory, int index)
		{
			this.requireRefresh = true;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00075814 File Offset: 0x00073A14
		private void Update()
		{
			if (this.requireRefresh)
			{
				this.Refresh();
			}
			bool flag = this.TargetItem != null && this.Character.CurrentHoldItemAgent != null && this.TargetItem == this.Character.CurrentHoldItemAgent.Item;
			if (flag && !this.lastFrameUsing)
			{
				this.OnStartedUsing();
			}
			else if (!flag && this.lastFrameUsing)
			{
				this.OnStoppedUsing();
			}
			this.usingIndicator.gameObject.SetActive(flag);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000758A4 File Offset: 0x00073AA4
		private void OnStartedUsing()
		{
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x000758A6 File Offset: 0x00073AA6
		private void OnStoppedUsing()
		{
		}

		// Token: 0x040016BA RID: 5818
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040016BB RID: 5819
		[SerializeField]
		private GameObject usingIndicator;

		// Token: 0x040016BC RID: 5820
		[SerializeField]
		private GameObject notInteractableIndicator;

		// Token: 0x040016BD RID: 5821
		[SerializeField]
		private Image denialIndicator;

		// Token: 0x040016BE RID: 5822
		[SerializeField]
		private Color denialColor;

		// Token: 0x040016C5 RID: 5829
		private bool isBeingDestroyed;

		// Token: 0x040016C6 RID: 5830
		private bool requireRefresh;

		// Token: 0x040016C7 RID: 5831
		private bool lastFrameUsing;
	}
}
