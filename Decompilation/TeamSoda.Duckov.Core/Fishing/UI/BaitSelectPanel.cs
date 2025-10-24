using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Fishing.UI
{
	// Token: 0x02000217 RID: 535
	public class BaitSelectPanel : MonoBehaviour, ISingleSelectionMenu<BaitSelectPanelEntry>
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0003E470 File Offset: 0x0003C670
		private PrefabPool<BaitSelectPanelEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<BaitSelectPanelEntry>(this.entry, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06000FE8 RID: 4072 RVA: 0x0003E4AC File Offset: 0x0003C6AC
		// (remove) Token: 0x06000FE9 RID: 4073 RVA: 0x0003E4E4 File Offset: 0x0003C6E4
		internal event Action onSetSelection;

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003E51C File Offset: 0x0003C71C
		internal UniTask DoBaitSelection(ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			BaitSelectPanel.<DoBaitSelection>d__12 <DoBaitSelection>d__;
			<DoBaitSelection>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoBaitSelection>d__.<>4__this = this;
			<DoBaitSelection>d__.availableBaits = availableBaits;
			<DoBaitSelection>d__.baitSelectionResultCallback = baitSelectionResultCallback;
			<DoBaitSelection>d__.<>1__state = -1;
			<DoBaitSelection>d__.<>t__builder.Start<BaitSelectPanel.<DoBaitSelection>d__12>(ref <DoBaitSelection>d__);
			return <DoBaitSelection>d__.<>t__builder.Task;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0003E56F File Offset: 0x0003C76F
		private void Open()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003E57C File Offset: 0x0003C77C
		private void Close()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0003E589 File Offset: 0x0003C789
		private Item SelectedItem
		{
			get
			{
				BaitSelectPanelEntry baitSelectPanelEntry = this.selectedEntry;
				if (baitSelectPanelEntry == null)
				{
					return null;
				}
				return baitSelectPanelEntry.Target;
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003E59C File Offset: 0x0003C79C
		private UniTask<Item> WaitForSelection()
		{
			BaitSelectPanel.<WaitForSelection>d__20 <WaitForSelection>d__;
			<WaitForSelection>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<WaitForSelection>d__.<>4__this = this;
			<WaitForSelection>d__.<>1__state = -1;
			<WaitForSelection>d__.<>t__builder.Start<BaitSelectPanel.<WaitForSelection>d__20>(ref <WaitForSelection>d__);
			return <WaitForSelection>d__.<>t__builder.Task;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003E5E0 File Offset: 0x0003C7E0
		private void Setup(ICollection<Item> availableBaits)
		{
			this.selectedEntry = null;
			this.EntryPool.ReleaseAll();
			foreach (Item cur in availableBaits)
			{
				this.EntryPool.Get(null).Setup(this, cur);
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0003E648 File Offset: 0x0003C848
		internal void NotifyStop()
		{
			this.Close();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0003E650 File Offset: 0x0003C850
		private void Awake()
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003E68A File Offset: 0x0003C88A
		private void OnConfirmButtonClicked()
		{
			if (this.SelectedItem == null)
			{
				NotificationText.Push("Fishing_PleaseSelectBait".ToPlainText());
				return;
			}
			this.confirmed = true;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0003E6B1 File Offset: 0x0003C8B1
		private void OnCancelButtonClicked()
		{
			this.canceled = true;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003E6BA File Offset: 0x0003C8BA
		internal void NotifySelect(BaitSelectPanelEntry baitSelectPanelEntry)
		{
			this.SetSelection(baitSelectPanelEntry);
			if (this.SelectedItem != null)
			{
				this.details.Setup(this.SelectedItem);
				this.detailsFadeGroup.Show();
				return;
			}
			this.detailsFadeGroup.SkipHide();
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003E6FA File Offset: 0x0003C8FA
		public BaitSelectPanelEntry GetSelection()
		{
			return this.selectedEntry;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003E702 File Offset: 0x0003C902
		public bool SetSelection(BaitSelectPanelEntry selection)
		{
			this.selectedEntry = selection;
			Action action = this.onSetSelection;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x04000CC2 RID: 3266
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CC3 RID: 3267
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04000CC4 RID: 3268
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04000CC5 RID: 3269
		[SerializeField]
		private ItemDetailsDisplay details;

		// Token: 0x04000CC6 RID: 3270
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04000CC7 RID: 3271
		[SerializeField]
		private BaitSelectPanelEntry entry;

		// Token: 0x04000CC8 RID: 3272
		private PrefabPool<BaitSelectPanelEntry> _entryPool;

		// Token: 0x04000CCA RID: 3274
		private BaitSelectPanelEntry selectedEntry;

		// Token: 0x04000CCB RID: 3275
		private bool canceled;

		// Token: 0x04000CCC RID: 3276
		private bool confirmed;
	}
}
