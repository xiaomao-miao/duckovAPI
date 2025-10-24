using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000346 RID: 838
	public class QuestGiverView : View, ISingleSelectionMenu<QuestEntry>
	{
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x00067ABF File Offset: 0x00065CBF
		public static QuestGiverView Instance
		{
			get
			{
				return View.GetViewInstance<QuestGiverView>();
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x00067AC6 File Offset: 0x00065CC6
		public string BtnText_CompleteQuest
		{
			get
			{
				return this.btnText_CompleteQuest.ToPlainText();
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x00067AD3 File Offset: 0x00065CD3
		public string BtnText_AcceptQuest
		{
			get
			{
				return this.btnText_AcceptQuest.ToPlainText();
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x00067AE0 File Offset: 0x00065CE0
		private PrefabPool<QuestEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<QuestEntry>(this.entryPrefab, this.questEntriesParent, delegate(QuestEntry e)
					{
						this.activeEntries.Add(e);
					}, delegate(QuestEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x00067B34 File Offset: 0x00065D34
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.RefreshList();
			this.RefreshDetails();
			QuestManager.onQuestListsChanged += this.OnQuestListChanged;
			Quest.onQuestStatusChanged += this.OnQuestStatusChanged;
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x00067B80 File Offset: 0x00065D80
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
			Quest.onQuestStatusChanged -= this.OnQuestStatusChanged;
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x00067BB5 File Offset: 0x00065DB5
		private void OnDisable()
		{
			if (this.details != null)
			{
				this.details.Setup(null);
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00067BD1 File Offset: 0x00065DD1
		private void OnQuestStatusChanged(Quest quest)
		{
			QuestEntry questEntry = this.selectedQuestEntry;
			if (quest == ((questEntry != null) ? questEntry.Target : null))
			{
				this.RefreshDetails();
			}
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x00067BF3 File Offset: 0x00065DF3
		protected override void Awake()
		{
			base.Awake();
			this.tabs.onSelectionChanged += this.OnTabChanged;
			this.btn_Interact.onClick.AddListener(new UnityAction(this.OnInteractButtonClicked));
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00067C30 File Offset: 0x00065E30
		private void OnInteractButtonClicked()
		{
			if (this.btnAcceptQuest)
			{
				Quest quest = this.details.Target;
				if (quest != null && QuestManager.IsQuestAvaliable(quest.ID))
				{
					QuestManager.Instance.ActivateQuest(quest.ID, new QuestGiverID?(this.target.ID));
					AudioManager.Post(this.sfx_AcceptQuest);
					return;
				}
			}
			else if (this.btnCompleteQuest)
			{
				Quest quest2 = this.details.Target;
				if (quest2 == null)
				{
					return;
				}
				if (quest2.TryComplete())
				{
					this.ShowCompleteUI(quest2);
					AudioManager.Post(this.sfx_CompleteQuest);
				}
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x00067CCD File Offset: 0x00065ECD
		private void ShowCompleteUI(Quest quest)
		{
			this.completeUITask = this.questCompletePanel.Show(quest);
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x00067CE1 File Offset: 0x00065EE1
		private void OnTabChanged(QuestGiverTabs tabs)
		{
			this.RefreshList();
			this.RefreshDetails();
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00067CEF File Offset: 0x00065EEF
		protected override void OnDestroy()
		{
			base.OnDestroy();
			QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x00067D08 File Offset: 0x00065F08
		private void OnQuestListChanged(QuestManager manager)
		{
			this.RefreshList();
			this.SetSelection(null);
			this.RefreshDetails();
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00067D1E File Offset: 0x00065F1E
		public void Setup(QuestGiver target)
		{
			this.target = target;
			this.RefreshList();
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00067D30 File Offset: 0x00065F30
		private void RefreshList()
		{
			QuestGiverView.<>c__DisplayClass42_0 CS$<>8__locals1 = new QuestGiverView.<>c__DisplayClass42_0();
			QuestGiverView.<>c__DisplayClass42_0 CS$<>8__locals2 = CS$<>8__locals1;
			QuestEntry questEntry = this.selectedQuestEntry;
			CS$<>8__locals2.keepQuest = ((questEntry != null) ? questEntry.Target : null);
			this.selectedQuestEntry = null;
			this.EntryPool.ReleaseAll();
			List<Quest> questsToShow = this.GetQuestsToShow();
			bool flag = questsToShow.Count > 0;
			this.entryPlaceHolder.SetActive(!flag);
			this.RefreshRedDots();
			if (!flag)
			{
				return;
			}
			foreach (Quest quest in questsToShow)
			{
				QuestEntry questEntry2 = this.EntryPool.Get(this.questEntriesParent);
				questEntry2.transform.SetAsLastSibling();
				questEntry2.SetMenu(this);
				questEntry2.Setup(quest);
			}
			QuestEntry questEntry3 = this.activeEntries.Find((QuestEntry e) => e.Target == CS$<>8__locals1.keepQuest);
			if (questEntry3 != null)
			{
				this.SetSelection(questEntry3);
				return;
			}
			this.SetSelection(null);
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00067E30 File Offset: 0x00066030
		private void RefreshRedDots()
		{
			this.uninspectedAvaliableRedDot.SetActive(this.AnyUninspectedAvaliableQuest());
			this.activeRedDot.SetActive(this.AnyUninspectedActiveQuest());
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x00067E54 File Offset: 0x00066054
		private bool AnyUninspectedActiveQuest()
		{
			return !(this.target == null) && QuestManager.AnyActiveQuestNeedsInspection(this.target.ID);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x00067E78 File Offset: 0x00066078
		private bool AnyUninspectedAvaliableQuest()
		{
			if (this.target == null)
			{
				return false;
			}
			return this.target.GetAvaliableQuests().Any((Quest e) => e != null && e.NeedInspection);
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00067EC4 File Offset: 0x000660C4
		private List<Quest> GetQuestsToShow()
		{
			List<Quest> list = new List<Quest>();
			if (this.target == null)
			{
				return list;
			}
			QuestStatus status = this.tabs.GetStatus();
			switch (status)
			{
			case QuestStatus.None:
				return list;
			case (QuestStatus)1:
			case (QuestStatus)3:
				break;
			case QuestStatus.Avaliable:
				list.AddRange(this.target.GetAvaliableQuests());
				break;
			case QuestStatus.Active:
				list.AddRange(QuestManager.GetActiveQuestsFromGiver(this.target.ID));
				break;
			default:
				if (status == QuestStatus.Finished)
				{
					list.AddRange(QuestManager.GetHistoryQuestsFromGiver(this.target.ID));
				}
				break;
			}
			return list;
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00067F58 File Offset: 0x00066158
		private void RefreshDetails()
		{
			QuestEntry questEntry = this.selectedQuestEntry;
			Quest quest = (questEntry != null) ? questEntry.Target : null;
			this.details.Setup(quest);
			this.RefreshInteractButton();
			bool interactable = quest && (QuestManager.IsQuestActive(quest) || quest.Complete);
			this.details.Interactable = interactable;
			this.details.Refresh();
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x00067FC0 File Offset: 0x000661C0
		private void RefreshInteractButton()
		{
			this.btnAcceptQuest = false;
			this.btnCompleteQuest = false;
			QuestEntry questEntry = this.selectedQuestEntry;
			Quest quest = (questEntry != null) ? questEntry.Target : null;
			if (quest == null)
			{
				this.btn_Interact.gameObject.SetActive(false);
				return;
			}
			QuestStatus status = this.tabs.GetStatus();
			bool active = false;
			switch (status)
			{
			case QuestStatus.None:
			case (QuestStatus)1:
			case (QuestStatus)3:
				break;
			case QuestStatus.Avaliable:
				active = true;
				this.btn_Interact.interactable = true;
				this.btnImage.color = this.interactableBtnImageColor;
				this.btnText.text = this.BtnText_AcceptQuest;
				this.btnAcceptQuest = true;
				break;
			case QuestStatus.Active:
			{
				active = true;
				bool flag = quest.AreTasksFinished();
				this.btn_Interact.interactable = flag;
				this.btnImage.color = (flag ? this.interactableBtnImageColor : this.uninteractableBtnImageColor);
				this.btnText.text = this.BtnText_CompleteQuest;
				this.btnCompleteQuest = true;
				break;
			}
			default:
				if (status != QuestStatus.Finished)
				{
				}
				break;
			}
			this.btn_Interact.gameObject.SetActive(active);
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000680D0 File Offset: 0x000662D0
		public QuestEntry GetSelection()
		{
			return this.selectedQuestEntry;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000680D8 File Offset: 0x000662D8
		public bool SetSelection(QuestEntry selection)
		{
			this.selectedQuestEntry = selection;
			if (selection != null)
			{
				QuestManager.SetEverInspected(selection.Target.ID);
			}
			this.RefreshDetails();
			this.RefreshEntries();
			this.RefreshRedDots();
			return true;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x00068110 File Offset: 0x00066310
		private void RefreshEntries()
		{
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x00068160 File Offset: 0x00066360
		internal override void TryQuit()
		{
			if (this.questCompletePanel.isActiveAndEnabled)
			{
				this.questCompletePanel.Skip();
				return;
			}
			base.Close();
		}

		// Token: 0x0400140A RID: 5130
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400140B RID: 5131
		[SerializeField]
		private RectTransform questEntriesParent;

		// Token: 0x0400140C RID: 5132
		[SerializeField]
		private QuestCompletePanel questCompletePanel;

		// Token: 0x0400140D RID: 5133
		[SerializeField]
		private QuestGiverTabs tabs;

		// Token: 0x0400140E RID: 5134
		[SerializeField]
		private QuestEntry entryPrefab;

		// Token: 0x0400140F RID: 5135
		[SerializeField]
		private GameObject entryPlaceHolder;

		// Token: 0x04001410 RID: 5136
		[SerializeField]
		private QuestViewDetails details;

		// Token: 0x04001411 RID: 5137
		[SerializeField]
		private Button btn_Interact;

		// Token: 0x04001412 RID: 5138
		[SerializeField]
		private TextMeshProUGUI btnText;

		// Token: 0x04001413 RID: 5139
		[SerializeField]
		private Image btnImage;

		// Token: 0x04001414 RID: 5140
		[SerializeField]
		private string btnText_AcceptQuest = "接受任务";

		// Token: 0x04001415 RID: 5141
		[SerializeField]
		private string btnText_CompleteQuest = "完成任务";

		// Token: 0x04001416 RID: 5142
		[SerializeField]
		private Color interactableBtnImageColor = Color.green;

		// Token: 0x04001417 RID: 5143
		[SerializeField]
		private Color uninteractableBtnImageColor = Color.gray;

		// Token: 0x04001418 RID: 5144
		[SerializeField]
		private GameObject uninspectedAvaliableRedDot;

		// Token: 0x04001419 RID: 5145
		[SerializeField]
		private GameObject activeRedDot;

		// Token: 0x0400141A RID: 5146
		private string sfx_AcceptQuest = "UI/mission_accept";

		// Token: 0x0400141B RID: 5147
		private string sfx_CompleteQuest = "UI/mission_large";

		// Token: 0x0400141C RID: 5148
		private PrefabPool<QuestEntry> _entryPool;

		// Token: 0x0400141D RID: 5149
		private List<QuestEntry> activeEntries = new List<QuestEntry>();

		// Token: 0x0400141E RID: 5150
		private QuestGiver target;

		// Token: 0x0400141F RID: 5151
		private QuestEntry selectedQuestEntry;

		// Token: 0x04001420 RID: 5152
		private UniTask completeUITask;

		// Token: 0x04001421 RID: 5153
		private bool btnAcceptQuest;

		// Token: 0x04001422 RID: 5154
		private bool btnCompleteQuest;
	}
}
