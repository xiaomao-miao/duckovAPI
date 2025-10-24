using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034A RID: 842
	public class QuestView : View, ISingleSelectionMenu<QuestEntry>
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00068437 File Offset: 0x00066637
		public static QuestView Instance
		{
			get
			{
				return View.GetViewInstance<QuestView>();
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x0006843E File Offset: 0x0006663E
		public QuestView.ShowContent ShowingContentType
		{
			get
			{
				return this.showingContentType;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x00068448 File Offset: 0x00066648
		public IList<Quest> ShowingContent
		{
			get
			{
				if (this.target == null)
				{
					return null;
				}
				QuestView.ShowContent showContent = this.showingContentType;
				if (showContent == QuestView.ShowContent.Active)
				{
					return this.target.ActiveQuests;
				}
				if (showContent != QuestView.ShowContent.History)
				{
					return null;
				}
				return this.target.HistoryQuests;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x00068490 File Offset: 0x00066690
		private PrefabPool<QuestEntry> QuestEntryPool
		{
			get
			{
				if (this._questEntryPool == null)
				{
					this._questEntryPool = new PrefabPool<QuestEntry>(this.questEntry, this.questEntryParent, delegate(QuestEntry e)
					{
						this.activeEntries.Add(e);
						e.SetMenu(this);
					}, delegate(QuestEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._questEntryPool;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x000684E4 File Offset: 0x000666E4
		private QuestEntry SelectedQuestEntry
		{
			get
			{
				return this.selectedQuestEntry;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x000684EC File Offset: 0x000666EC
		public Quest SelectedQuest
		{
			get
			{
				QuestEntry questEntry = this.selectedQuestEntry;
				if (questEntry == null)
				{
					return null;
				}
				return questEntry.Target;
			}
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06001D17 RID: 7447 RVA: 0x00068500 File Offset: 0x00066700
		// (remove) Token: 0x06001D18 RID: 7448 RVA: 0x00068538 File Offset: 0x00066738
		internal event Action<QuestView, QuestView.ShowContent> onShowingContentChanged;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06001D19 RID: 7449 RVA: 0x00068570 File Offset: 0x00066770
		// (remove) Token: 0x06001D1A RID: 7450 RVA: 0x000685A8 File Offset: 0x000667A8
		internal event Action<QuestView, QuestEntry> onSelectedEntryChanged;

		// Token: 0x06001D1B RID: 7451 RVA: 0x000685DD File Offset: 0x000667DD
		public void Setup()
		{
			this.Setup(QuestManager.Instance);
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000685EC File Offset: 0x000667EC
		private void Setup(QuestManager target)
		{
			this.target = target;
			Quest oldSelection = this.SelectedQuest;
			this.RefreshEntryList();
			QuestEntry questEntry = this.activeEntries.Find((QuestEntry e) => e.Target == oldSelection);
			if (questEntry != null)
			{
				this.SetSelection(questEntry);
			}
			else
			{
				this.SetSelection(null);
			}
			this.RefreshDetails();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00068651 File Offset: 0x00066851
		public static void Show()
		{
			QuestView instance = QuestView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Open(null);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00068663 File Offset: 0x00066863
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Setup();
			this.fadeGroup.Show();
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0006867C File Offset: 0x0006687C
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0006868F File Offset: 0x0006688F
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00068697 File Offset: 0x00066897
		private void OnEnable()
		{
			this.RegisterStaticEvents();
			this.Setup(QuestManager.Instance);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000686AA File Offset: 0x000668AA
		private void OnDisable()
		{
			if (this.details != null)
			{
				this.details.Setup(null);
			}
			this.UnregisterStaticEvents();
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000686CC File Offset: 0x000668CC
		private void RegisterStaticEvents()
		{
			QuestManager.onQuestListsChanged += this.Setup;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000686DF File Offset: 0x000668DF
		private void UnregisterStaticEvents()
		{
			QuestManager.onQuestListsChanged -= this.Setup;
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000686F4 File Offset: 0x000668F4
		private void RefreshEntryList()
		{
			this.QuestEntryPool.ReleaseAll();
			bool flag = this.target != null && this.ShowingContent != null && this.ShowingContent.Count > 0;
			this.entryListPlaceHolder.SetActive(!flag);
			if (!flag)
			{
				return;
			}
			foreach (Quest quest in this.ShowingContent)
			{
				QuestEntry questEntry = this.QuestEntryPool.Get(this.questEntryParent);
				questEntry.Setup(quest);
				questEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000687A4 File Offset: 0x000669A4
		private void RefreshDetails()
		{
			this.details.Setup(this.SelectedQuest);
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000687B8 File Offset: 0x000669B8
		public void SetShowingContent(QuestView.ShowContent flags)
		{
			this.showingContentType = flags;
			this.RefreshEntryList();
			List<QuestEntry> list = this.activeEntries;
			if (list != null && list.Count > 0)
			{
				this.SetSelection(this.activeEntries[0]);
			}
			else
			{
				this.SetSelection(null);
			}
			this.RefreshDetails();
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
			Action<QuestView, QuestView.ShowContent> action = this.onShowingContentChanged;
			if (action == null)
			{
				return;
			}
			action(this, flags);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00068864 File Offset: 0x00066A64
		public void ShowActiveQuests()
		{
			this.SetShowingContent(QuestView.ShowContent.Active);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0006886D File Offset: 0x00066A6D
		public void ShowHistoryQuests()
		{
			this.SetShowingContent(QuestView.ShowContent.History);
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00068876 File Offset: 0x00066A76
		public QuestEntry GetSelection()
		{
			return this.selectedQuestEntry;
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00068880 File Offset: 0x00066A80
		public bool SetSelection(QuestEntry selection)
		{
			this.selectedQuestEntry = selection;
			Action<QuestView, QuestEntry> action = this.onSelectedEntryChanged;
			if (action != null)
			{
				action(this, this.selectedQuestEntry);
			}
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
			this.RefreshDetails();
			return true;
		}

		// Token: 0x04001430 RID: 5168
		[SerializeField]
		private QuestEntry questEntry;

		// Token: 0x04001431 RID: 5169
		[SerializeField]
		private Transform questEntryParent;

		// Token: 0x04001432 RID: 5170
		[SerializeField]
		private GameObject entryListPlaceHolder;

		// Token: 0x04001433 RID: 5171
		[SerializeField]
		private QuestViewDetails details;

		// Token: 0x04001434 RID: 5172
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001435 RID: 5173
		private QuestManager target;

		// Token: 0x04001436 RID: 5174
		[SerializeField]
		private QuestView.ShowContent showingContentType;

		// Token: 0x04001437 RID: 5175
		private PrefabPool<QuestEntry> _questEntryPool;

		// Token: 0x04001438 RID: 5176
		private List<QuestEntry> activeEntries = new List<QuestEntry>();

		// Token: 0x04001439 RID: 5177
		private QuestEntry selectedQuestEntry;

		// Token: 0x02000602 RID: 1538
		[Flags]
		public enum ShowContent
		{
			// Token: 0x0400213F RID: 8511
			Active = 1,
			// Token: 0x04002140 RID: 8512
			History = 2
		}
	}
}
