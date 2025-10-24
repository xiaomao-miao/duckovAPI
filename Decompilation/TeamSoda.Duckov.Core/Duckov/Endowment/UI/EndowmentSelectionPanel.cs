using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Endowment.UI
{
	// Token: 0x020002F8 RID: 760
	public class EndowmentSelectionPanel : View
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00059DEC File Offset: 0x00057FEC
		private PrefabPool<EndowmentSelectionEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<EndowmentSelectionEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, delegate(EndowmentSelectionEntry e)
					{
						e.onClicked = (Action<EndowmentSelectionEntry, PointerEventData>)Delegate.Combine(e.onClicked, new Action<EndowmentSelectionEntry, PointerEventData>(this.OnEntryClicked));
					});
				}
				return this._pool;
			}
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x00059E30 File Offset: 0x00058030
		protected override void Awake()
		{
			base.Awake();
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00059E70 File Offset: 0x00058070
		protected override void OnCancel()
		{
			base.OnCancel();
			this.canceled = true;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00059E7F File Offset: 0x0005807F
		private void OnCancelButtonClicked()
		{
			this.canceled = true;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00059E88 File Offset: 0x00058088
		private void OnConfirmButtonClicked()
		{
			this.confirmed = true;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x00059E91 File Offset: 0x00058091
		private void OnEntryClicked(EndowmentSelectionEntry entry, PointerEventData data)
		{
			if (entry.Locked)
			{
				return;
			}
			this.Select(entry);
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x00059EA4 File Offset: 0x000580A4
		private void Select(EndowmentSelectionEntry entry)
		{
			this.Selection = entry;
			foreach (EndowmentSelectionEntry endowmentSelectionEntry in this.Pool.ActiveEntries)
			{
				endowmentSelectionEntry.SetSelection(endowmentSelectionEntry == entry);
			}
			this.RefreshDescription();
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00059F08 File Offset: 0x00058108
		// (set) Token: 0x060018C0 RID: 6336 RVA: 0x00059F10 File Offset: 0x00058110
		public EndowmentSelectionEntry Selection { get; private set; }

		// Token: 0x060018C1 RID: 6337 RVA: 0x00059F1C File Offset: 0x0005811C
		public void Setup()
		{
			if (EndowmentManager.Instance == null)
			{
				return;
			}
			this.Pool.ReleaseAll();
			foreach (EndowmentEntry endowmentEntry in EndowmentManager.Instance.Entries)
			{
				if (!(endowmentEntry == null))
				{
					this.Pool.Get(null).Setup(endowmentEntry);
				}
			}
			foreach (EndowmentSelectionEntry endowmentSelectionEntry in this.Pool.ActiveEntries)
			{
				if (endowmentSelectionEntry.Target.Index == EndowmentManager.SelectedIndex)
				{
					this.Select(endowmentSelectionEntry);
					break;
				}
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00059FF0 File Offset: 0x000581F0
		private void RefreshDescription()
		{
			if (this.Selection == null)
			{
				this.descriptionText.text = "-";
			}
			this.descriptionText.text = this.Selection.DescriptionAndEffects;
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0005A026 File Offset: 0x00058226
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Execute().Forget();
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0005A039 File Offset: 0x00058239
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0005A04C File Offset: 0x0005824C
		public UniTask Execute()
		{
			EndowmentSelectionPanel.<Execute>d__22 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<EndowmentSelectionPanel.<Execute>d__22>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0005A090 File Offset: 0x00058290
		private UniTask WaitForConfirm()
		{
			EndowmentSelectionPanel.<WaitForConfirm>d__25 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<EndowmentSelectionPanel.<WaitForConfirm>d__25>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0005A0D3 File Offset: 0x000582D3
		internal void SkipHide()
		{
			if (this.fadeGroup != null)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0005A0F0 File Offset: 0x000582F0
		public static void Show()
		{
			EndowmentSelectionPanel viewInstance = View.GetViewInstance<EndowmentSelectionPanel>();
			if (viewInstance == null)
			{
				return;
			}
			viewInstance.Open(null);
		}

		// Token: 0x040011F8 RID: 4600
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040011F9 RID: 4601
		[SerializeField]
		private EndowmentSelectionEntry entryTemplate;

		// Token: 0x040011FA RID: 4602
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x040011FB RID: 4603
		[SerializeField]
		private Button confirmButton;

		// Token: 0x040011FC RID: 4604
		[SerializeField]
		private Button cancelButton;

		// Token: 0x040011FD RID: 4605
		private PrefabPool<EndowmentSelectionEntry> _pool;

		// Token: 0x040011FF RID: 4607
		private bool confirmed;

		// Token: 0x04001200 RID: 4608
		private bool canceled;
	}
}
