using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200037C RID: 892
	public class ItemPicker : MonoBehaviour
	{
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x0006CB08 File Offset: 0x0006AD08
		// (set) Token: 0x06001EF0 RID: 7920 RVA: 0x0006CB0F File Offset: 0x0006AD0F
		public static ItemPicker Instance { get; private set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x0006CB18 File Offset: 0x0006AD18
		private PrefabPool<ItemPickerEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<ItemPickerEntry>(this.entryPrefab, this.contentParent ? this.contentParent : base.transform, new Action<ItemPickerEntry>(this.OnGetEntry), null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0006CB76 File Offset: 0x0006AD76
		private void OnGetEntry(ItemPickerEntry entry)
		{
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x0006CB78 File Offset: 0x0006AD78
		public bool Picking
		{
			get
			{
				return this.picking;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0006CB80 File Offset: 0x0006AD80
		private UniTask<Item> WaitForUserPick(ICollection<Item> candidates)
		{
			ItemPicker.<WaitForUserPick>d__19 <WaitForUserPick>d__;
			<WaitForUserPick>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<WaitForUserPick>d__.<>4__this = this;
			<WaitForUserPick>d__.candidates = candidates;
			<WaitForUserPick>d__.<>1__state = -1;
			<WaitForUserPick>d__.<>t__builder.Start<ItemPicker.<WaitForUserPick>d__19>(ref <WaitForUserPick>d__);
			return <WaitForUserPick>d__.<>t__builder.Task;
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0006CBCC File Offset: 0x0006ADCC
		private void Awake()
		{
			if (ItemPicker.Instance == null)
			{
				ItemPicker.Instance = this;
			}
			else
			{
				Debug.LogError("场景中存在两个ItemPicker，请检查。");
			}
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0006CC30 File Offset: 0x0006AE30
		private void OnCancelButtonClicked()
		{
			this.Cancel();
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0006CC38 File Offset: 0x0006AE38
		private void OnConfirmButtonClicked()
		{
			this.ConfirmPick(this.pickedItem);
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0006CC46 File Offset: 0x0006AE46
		private void OnDestroy()
		{
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0006CC48 File Offset: 0x0006AE48
		private void Update()
		{
			if (!this.picking && this.fadeGroup.IsShown)
			{
				this.fadeGroup.Hide();
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0006CC6C File Offset: 0x0006AE6C
		public static UniTask<Item> Pick(ICollection<Item> candidates)
		{
			ItemPicker.<Pick>d__25 <Pick>d__;
			<Pick>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Pick>d__.candidates = candidates;
			<Pick>d__.<>1__state = -1;
			<Pick>d__.<>t__builder.Start<ItemPicker.<Pick>d__25>(ref <Pick>d__);
			return <Pick>d__.<>t__builder.Task;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0006CCAF File Offset: 0x0006AEAF
		public void ConfirmPick(Item item)
		{
			this.confirmed = true;
			this.pickedItem = item;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0006CCBF File Offset: 0x0006AEBF
		public void Cancel()
		{
			this.canceled = true;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0006CCC8 File Offset: 0x0006AEC8
		private void SetupUI(ICollection<Item> candidates)
		{
			this.EntryPool.ReleaseAll();
			foreach (Item item in candidates)
			{
				if (!(item == null))
				{
					ItemPickerEntry itemPickerEntry = this.EntryPool.Get(null);
					itemPickerEntry.Setup(this, item);
					itemPickerEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0006CD3C File Offset: 0x0006AF3C
		internal void NotifyEntryClicked(ItemPickerEntry itemPickerEntry, Item target)
		{
			this.pickedItem = target;
		}

		// Token: 0x0400152E RID: 5422
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400152F RID: 5423
		[SerializeField]
		private ItemPickerEntry entryPrefab;

		// Token: 0x04001530 RID: 5424
		[SerializeField]
		private Transform contentParent;

		// Token: 0x04001531 RID: 5425
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04001532 RID: 5426
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04001533 RID: 5427
		private PrefabPool<ItemPickerEntry> _entryPool;

		// Token: 0x04001534 RID: 5428
		private bool picking;

		// Token: 0x04001535 RID: 5429
		private bool canceled;

		// Token: 0x04001536 RID: 5430
		private bool confirmed;

		// Token: 0x04001537 RID: 5431
		private Item pickedItem;
	}
}
