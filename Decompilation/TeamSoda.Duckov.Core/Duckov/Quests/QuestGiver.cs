using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buildings;
using Duckov.Quests.UI;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000335 RID: 821
	public class QuestGiver : InteractableBase
	{
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00065861 File Offset: 0x00063A61
		private IEnumerable<Quest> PossibleQuests
		{
			get
			{
				if (this._possibleQuests == null && QuestManager.Instance != null)
				{
					this._possibleQuests = QuestManager.Instance.GetAllQuestsByQuestGiverID(this.questGiverID);
				}
				return this._possibleQuests;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00065894 File Offset: 0x00063A94
		public QuestGiverID ID
		{
			get
			{
				return this.questGiverID;
			}
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0006589C File Offset: 0x00063A9C
		protected override void Awake()
		{
			base.Awake();
			QuestManager.onQuestListsChanged += this.OnQuestListsChanged;
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
			QuestManager.OnTaskFinishedEvent = (Action<Quest, Task>)Delegate.Combine(QuestManager.OnTaskFinishedEvent, new Action<Quest, Task>(this.OnTaskFinished));
			this.inspectionIndicator = UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.QuestMarker);
			this.inspectionIndicator.transform.SetParent(base.transform);
			this.inspectionIndicator.transform.position = base.transform.TransformPoint(this.interactMarkerOffset + Vector3.up * 0.5f);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x00065951 File Offset: 0x00063B51
		protected override void Start()
		{
			base.Start();
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x00065960 File Offset: 0x00063B60
		protected override void OnDestroy()
		{
			base.OnDestroy();
			QuestManager.onQuestListsChanged -= this.OnQuestListsChanged;
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
			QuestManager.OnTaskFinishedEvent = (Action<Quest, Task>)Delegate.Remove(QuestManager.OnTaskFinishedEvent, new Action<Quest, Task>(this.OnTaskFinished));
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000659B5 File Offset: 0x00063BB5
		private void OnTaskFinished(Quest quest, Task task)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000659BD File Offset: 0x00063BBD
		private void OnBuildingBuilt(int buildingID)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000659C5 File Offset: 0x00063BC5
		private bool AnyQuestNeedsInspection()
		{
			return QuestManager.GetActiveQuestsFromGiver(this.questGiverID).Any((Quest e) => e != null && e.NeedInspection);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000659F8 File Offset: 0x00063BF8
		private bool AnyQuestAvaliable()
		{
			using (IEnumerator<Quest> enumerator = this.PossibleQuests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (QuestManager.IsQuestAvaliable(enumerator.Current.ID))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00065A50 File Offset: 0x00063C50
		private void OnQuestListsChanged(QuestManager manager)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x00065A58 File Offset: 0x00063C58
		private void RefreshInspectionIndicator()
		{
			if (this.inspectionIndicator)
			{
				bool flag = this.AnyQuestNeedsInspection();
				bool flag2 = this.AnyQuestAvaliable();
				bool active = flag || flag2;
				this.inspectionIndicator.gameObject.SetActive(active);
			}
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00065A93 File Offset: 0x00063C93
		public void ActivateQuest(Quest quest)
		{
			QuestManager.Instance.ActivateQuest(quest.ID, new QuestGiverID?(this.questGiverID));
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00065AB0 File Offset: 0x00063CB0
		internal List<Quest> GetAvaliableQuests()
		{
			List<Quest> list = new List<Quest>();
			foreach (Quest quest in this.PossibleQuests)
			{
				if (QuestManager.IsQuestAvaliable(quest.ID))
				{
					list.Add(quest);
				}
			}
			return list;
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00065B14 File Offset: 0x00063D14
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			base.OnInteractStart(interactCharacter);
			QuestGiverView instance = QuestGiverView.Instance;
			if (instance == null)
			{
				base.StopInteract();
				return;
			}
			instance.Setup(this);
			instance.Open(null);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00065B4C File Offset: 0x00063D4C
		protected override void OnInteractStop()
		{
			base.OnInteractStop();
			if (QuestGiverView.Instance && QuestGiverView.Instance.open)
			{
				QuestGiverView instance = QuestGiverView.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Close();
			}
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00065B7B File Offset: 0x00063D7B
		protected override void OnUpdate(CharacterMainControl _interactCharacter, float deltaTime)
		{
			base.OnUpdate(_interactCharacter, deltaTime);
			if (!QuestGiverView.Instance || !QuestGiverView.Instance.open)
			{
				base.StopInteract();
			}
		}

		// Token: 0x040013B2 RID: 5042
		[SerializeField]
		private QuestGiverID questGiverID;

		// Token: 0x040013B3 RID: 5043
		private GameObject inspectionIndicator;

		// Token: 0x040013B4 RID: 5044
		private IEnumerable<Quest> _possibleQuests;

		// Token: 0x040013B5 RID: 5045
		private List<Quest> avaliableQuests = new List<Quest>();
	}
}
