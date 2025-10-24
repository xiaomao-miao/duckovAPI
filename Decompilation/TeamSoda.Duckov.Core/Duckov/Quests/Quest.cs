using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.Quests.Relations;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Duckov.Quests
{
	// Token: 0x02000333 RID: 819
	public class Quest : MonoBehaviour, ISaveDataProvider, INeedInspection
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001BBD RID: 7101 RVA: 0x000648D4 File Offset: 0x00062AD4
		public SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000648E4 File Offset: 0x00062AE4
		public SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x00064903 File Offset: 0x00062B03
		public List<Task> Tasks
		{
			get
			{
				return this.tasks;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x0006490B File Offset: 0x00062B0B
		public ReadOnlyCollection<Reward> Rewards
		{
			get
			{
				if (this._readonly_rewards == null)
				{
					this._readonly_rewards = new ReadOnlyCollection<Reward>(this.rewards);
				}
				return this._readonly_rewards;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x0006492C File Offset: 0x00062B2C
		public ReadOnlyCollection<Condition> Prerequisits
		{
			get
			{
				if (this.prerequisits_ReadOnly == null)
				{
					this.prerequisits_ReadOnly = new ReadOnlyCollection<Condition>(this.prerequisit);
				}
				return this.prerequisits_ReadOnly;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x00064950 File Offset: 0x00062B50
		public bool SceneRequirementSatisfied
		{
			get
			{
				SceneReference requireScene = this.RequireScene;
				return requireScene == null || requireScene.UnsafeReason == SceneReferenceUnsafeReason.Empty || requireScene.UnsafeReason != SceneReferenceUnsafeReason.None || requireScene.LoadedScene.isLoaded;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0006498C File Offset: 0x00062B8C
		public int RequireLevel
		{
			get
			{
				return this.requireLevel;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x00064994 File Offset: 0x00062B94
		public bool LockInDemo
		{
			get
			{
				return this.lockInDemo;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0006499C File Offset: 0x00062B9C
		// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x000649A4 File Offset: 0x00062BA4
		public bool Complete
		{
			get
			{
				return this.complete;
			}
			internal set
			{
				this.complete = value;
				Action<Quest> action = this.onStatusChanged;
				if (action != null)
				{
					action(this);
				}
				Action<Quest> action2 = Quest.onQuestStatusChanged;
				if (action2 != null)
				{
					action2(this);
				}
				if (this.complete)
				{
					Action<Quest> action3 = this.onCompleted;
					if (action3 != null)
					{
						action3(this);
					}
					UnityEvent onCompletedUnityEvent = this.OnCompletedUnityEvent;
					if (onCompletedUnityEvent != null)
					{
						onCompletedUnityEvent.Invoke();
					}
					Action<Quest> action4 = Quest.onQuestCompleted;
					if (action4 == null)
					{
						return;
					}
					action4(this);
				}
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x00064A18 File Offset: 0x00062C18
		// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x00064A68 File Offset: 0x00062C68
		public bool NeedInspection
		{
			get
			{
				return (!this.Active && !QuestManager.EverInspected(this.ID)) || (this.Active && ((this.Active && this.AreTasksFinished()) || this.AnyTaskNeedInspection() || this.needInspection));
			}
			set
			{
				this.needInspection = value;
				Action<Quest> action = this.onNeedInspectionChanged;
				if (action != null)
				{
					action(this);
				}
				Action<Quest> action2 = Quest.onQuestNeedInspectionChanged;
				if (action2 == null)
				{
					return;
				}
				action2(this);
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00064A94 File Offset: 0x00062C94
		private bool AnyTaskNeedInspection()
		{
			if (this.tasks != null)
			{
				foreach (Task task in this.tasks)
				{
					if (!(task == null) && task.NeedInspection)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06001BCA RID: 7114 RVA: 0x00064B00 File Offset: 0x00062D00
		// (remove) Token: 0x06001BCB RID: 7115 RVA: 0x00064B38 File Offset: 0x00062D38
		public event Action<Quest> onNeedInspectionChanged;

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06001BCC RID: 7116 RVA: 0x00064B70 File Offset: 0x00062D70
		// (remove) Token: 0x06001BCD RID: 7117 RVA: 0x00064BA8 File Offset: 0x00062DA8
		internal event Action<Quest> onStatusChanged;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06001BCE RID: 7118 RVA: 0x00064BE0 File Offset: 0x00062DE0
		// (remove) Token: 0x06001BCF RID: 7119 RVA: 0x00064C18 File Offset: 0x00062E18
		internal event Action<Quest> onActivated;

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06001BD0 RID: 7120 RVA: 0x00064C50 File Offset: 0x00062E50
		// (remove) Token: 0x06001BD1 RID: 7121 RVA: 0x00064C88 File Offset: 0x00062E88
		internal event Action<Quest> onCompleted;

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06001BD2 RID: 7122 RVA: 0x00064CC0 File Offset: 0x00062EC0
		// (remove) Token: 0x06001BD3 RID: 7123 RVA: 0x00064CF4 File Offset: 0x00062EF4
		public static event Action<Quest> onQuestStatusChanged;

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06001BD4 RID: 7124 RVA: 0x00064D28 File Offset: 0x00062F28
		// (remove) Token: 0x06001BD5 RID: 7125 RVA: 0x00064D5C File Offset: 0x00062F5C
		public static event Action<Quest> onQuestNeedInspectionChanged;

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06001BD6 RID: 7126 RVA: 0x00064D90 File Offset: 0x00062F90
		// (remove) Token: 0x06001BD7 RID: 7127 RVA: 0x00064DC4 File Offset: 0x00062FC4
		public static event Action<Quest> onQuestActivated;

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06001BD8 RID: 7128 RVA: 0x00064DF8 File Offset: 0x00062FF8
		// (remove) Token: 0x06001BD9 RID: 7129 RVA: 0x00064E2C File Offset: 0x0006302C
		public static event Action<Quest> onQuestCompleted;

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x00064E5F File Offset: 0x0006305F
		// (set) Token: 0x06001BDB RID: 7131 RVA: 0x00064E67 File Offset: 0x00063067
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x00064E70 File Offset: 0x00063070
		public bool Active
		{
			get
			{
				return QuestManager.IsQuestActive(this);
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x00064E78 File Offset: 0x00063078
		public int RequiredItemID
		{
			get
			{
				return this.requiredItemID;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x00064E80 File Offset: 0x00063080
		public int RequiredItemCount
		{
			get
			{
				return this.requiredItemCount;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x00064E88 File Offset: 0x00063088
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x00064E95 File Offset: 0x00063095
		public string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x00064EA2 File Offset: 0x000630A2
		// (set) Token: 0x06001BE2 RID: 7138 RVA: 0x00064EAA File Offset: 0x000630AA
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
			set
			{
				this.displayName = value;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x00064EB3 File Offset: 0x000630B3
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x00064EBB File Offset: 0x000630BB
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x00064EC4 File Offset: 0x000630C4
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x00064ECC File Offset: 0x000630CC
		public QuestGiverID QuestGiverID
		{
			get
			{
				return this.questGiverID;
			}
			internal set
			{
				this.questGiverID = value;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00064ED5 File Offset: 0x000630D5
		public object FinishedTaskCount
		{
			get
			{
				return this.tasks.Count((Task e) => e.IsFinished());
			}
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x00064F08 File Offset: 0x00063108
		public bool MeetsPrerequisit()
		{
			if (this.RequireLevel > EXPManager.Level)
			{
				return false;
			}
			if (this.LockInDemo && GameMetaData.Instance.IsDemo)
			{
				return false;
			}
			QuestRelationGraph questRelation = GameplayDataSettings.QuestRelation;
			if (questRelation.GetNode(this.id) == null)
			{
				return false;
			}
			if (!QuestManager.AreQuestFinished(questRelation.GetRequiredIDs(this.id)))
			{
				return false;
			}
			using (List<Condition>.Enumerator enumerator = this.prerequisit.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Evaluate())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00064FB4 File Offset: 0x000631B4
		public bool AreTasksFinished()
		{
			foreach (Task task in this.tasks)
			{
				if (task == null)
				{
					Debug.LogError(string.Format("存在空的Task，QuestID：{0}", this.id));
				}
				else if (!task.IsFinished())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x00065034 File Offset: 0x00063234
		public void Initialize()
		{
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x00065036 File Offset: 0x00063236
		public void OnValidate()
		{
			this.displayName = string.Format("Quest_{0}", this.id);
			this.description = string.Format("Quest_{0}_Desc", this.id);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x00065070 File Offset: 0x00063270
		public object GenerateSaveData()
		{
			Quest.SaveData saveData = default(Quest.SaveData);
			saveData.id = this.id;
			saveData.complete = this.complete;
			saveData.needInspection = this.needInspection;
			saveData.taskStatus = new List<ValueTuple<int, object>>();
			saveData.rewardStatus = new List<ValueTuple<int, object>>();
			foreach (Task task in this.tasks)
			{
				int item = task.ID;
				object item2 = task.GenerateSaveData();
				if (!(task == null))
				{
					saveData.taskStatus.Add(new ValueTuple<int, object>(item, item2));
				}
			}
			foreach (Reward reward in this.rewards)
			{
				if (reward == null)
				{
					Debug.LogError(string.Format("Null Reward detected in quest {0}", this.id));
				}
				else
				{
					int item3 = reward.ID;
					object item4 = reward.GenerateSaveData();
					saveData.rewardStatus.Add(new ValueTuple<int, object>(item3, item4));
				}
			}
			return saveData;
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000651BC File Offset: 0x000633BC
		public void SetupSaveData(object obj)
		{
			Quest.SaveData saveData = (Quest.SaveData)obj;
			if (saveData.id != this.id)
			{
				Debug.LogError("任务ID不匹配，加载失败");
				return;
			}
			this.complete = saveData.complete;
			this.needInspection = saveData.needInspection;
			using (List<ValueTuple<int, object>>.Enumerator enumerator = saveData.taskStatus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<int, object> cur = enumerator.Current;
					Task task = this.tasks.Find((Task e) => e.ID == cur.Item1);
					if (task == null)
					{
						Debug.LogWarning(string.Format("未找到Task {0}", cur.Item1));
					}
					else
					{
						task.SetupSaveData(cur.Item2);
					}
				}
			}
			using (List<ValueTuple<int, object>>.Enumerator enumerator = saveData.rewardStatus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<int, object> cur = enumerator.Current;
					Reward reward = this.rewards.Find((Reward e) => e.ID == cur.Item1);
					if (reward == null)
					{
						Debug.LogWarning(string.Format("未找到Reward {0}", cur.Item1));
					}
					else
					{
						reward.SetupSaveData(cur.Item2);
						reward.NotifyReload(this);
					}
				}
			}
			this.InitTasks();
			if (this.complete)
			{
				foreach (Reward reward2 in this.rewards)
				{
					if (!(reward2 == null) && !reward2.Claimed && reward2.AutoClaim)
					{
						reward2.Claim();
					}
				}
			}
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000653B8 File Offset: 0x000635B8
		internal void NotifyTaskFinished(Task task)
		{
			if (task.Master != this)
			{
				Debug.LogError("Task.Master 与 Quest不匹配");
				return;
			}
			Action<Quest> action = Quest.onQuestStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onStatusChanged;
			if (action2 != null)
			{
				action2(this);
			}
			QuestManager.NotifyTaskFinished(this, task);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x00065408 File Offset: 0x00063608
		internal void NotifyRewardClaimed(Reward reward)
		{
			if (reward.Master != this)
			{
				Debug.LogError("Reward.Master 与Quest 不匹配");
			}
			if (this.AreRewardsClaimed())
			{
				this.needInspection = false;
			}
			Action<Quest> action = Quest.onQuestStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onStatusChanged;
			if (action2 != null)
			{
				action2(this);
			}
			Action<Quest> action3 = Quest.onQuestNeedInspectionChanged;
			if (action3 == null)
			{
				return;
			}
			action3(this);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00065470 File Offset: 0x00063670
		internal bool AreRewardsClaimed()
		{
			using (List<Reward>.Enumerator enumerator = this.rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Claimed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000654CC File Offset: 0x000636CC
		internal void NotifyActivated()
		{
			this.InitTasks();
			Action<Quest> action = this.onStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onActivated;
			if (action2 != null)
			{
				action2(this);
			}
			Action<Quest> action3 = Quest.onQuestActivated;
			if (action3 == null)
			{
				return;
			}
			action3(this);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00065508 File Offset: 0x00063708
		private void InitTasks()
		{
			foreach (Task task in this.tasks)
			{
				task.Init();
			}
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00065558 File Offset: 0x00063758
		public bool TryComplete()
		{
			if (this.Complete)
			{
				return false;
			}
			if (!this.AreTasksFinished())
			{
				return false;
			}
			this.Complete = true;
			return true;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x00065576 File Offset: 0x00063776
		internal Quest.QuestInfo GetInfo()
		{
			return new Quest.QuestInfo(this);
		}

		// Token: 0x04001396 RID: 5014
		[SerializeField]
		private int id;

		// Token: 0x04001397 RID: 5015
		[LocalizationKey("Quests")]
		[SerializeField]
		private string displayName;

		// Token: 0x04001398 RID: 5016
		[LocalizationKey("Quests")]
		[SerializeField]
		private string description;

		// Token: 0x04001399 RID: 5017
		[SerializeField]
		private int requireLevel;

		// Token: 0x0400139A RID: 5018
		[SerializeField]
		private bool lockInDemo;

		// Token: 0x0400139B RID: 5019
		[FormerlySerializedAs("requiredItem")]
		[SerializeField]
		[ItemTypeID]
		private int requiredItemID;

		// Token: 0x0400139C RID: 5020
		[SerializeField]
		private int requiredItemCount = 1;

		// Token: 0x0400139D RID: 5021
		[SceneID]
		[SerializeField]
		private string requireSceneID;

		// Token: 0x0400139E RID: 5022
		[SerializeField]
		private QuestGiverID questGiverID;

		// Token: 0x0400139F RID: 5023
		[SerializeField]
		internal List<Condition> prerequisit = new List<Condition>();

		// Token: 0x040013A0 RID: 5024
		[SerializeField]
		internal List<Task> tasks = new List<Task>();

		// Token: 0x040013A1 RID: 5025
		[SerializeField]
		internal List<Reward> rewards = new List<Reward>();

		// Token: 0x040013A2 RID: 5026
		private ReadOnlyCollection<Reward> _readonly_rewards;

		// Token: 0x040013A3 RID: 5027
		[SerializeField]
		[HideInInspector]
		private int nextTaskID;

		// Token: 0x040013A4 RID: 5028
		[SerializeField]
		[HideInInspector]
		private int nextRewardID;

		// Token: 0x040013A5 RID: 5029
		private ReadOnlyCollection<Condition> prerequisits_ReadOnly;

		// Token: 0x040013A6 RID: 5030
		[SerializeField]
		private bool complete;

		// Token: 0x040013A7 RID: 5031
		[SerializeField]
		private bool needInspection;

		// Token: 0x040013AC RID: 5036
		public UnityEvent OnCompletedUnityEvent;

		// Token: 0x020005E4 RID: 1508
		[Serializable]
		public struct SaveData
		{
			// Token: 0x040020EF RID: 8431
			public int id;

			// Token: 0x040020F0 RID: 8432
			public bool complete;

			// Token: 0x040020F1 RID: 8433
			public bool needInspection;

			// Token: 0x040020F2 RID: 8434
			public QuestGiverID questGiverID;

			// Token: 0x040020F3 RID: 8435
			[TupleElementNames(new string[]
			{
				"id",
				"data"
			})]
			public List<ValueTuple<int, object>> taskStatus;

			// Token: 0x040020F4 RID: 8436
			[TupleElementNames(new string[]
			{
				"id",
				"data"
			})]
			public List<ValueTuple<int, object>> rewardStatus;
		}

		// Token: 0x020005E5 RID: 1509
		public struct QuestInfo
		{
			// Token: 0x0600294D RID: 10573 RVA: 0x000991F3 File Offset: 0x000973F3
			public QuestInfo(Quest quest)
			{
				this.questId = quest.id;
			}

			// Token: 0x040020F5 RID: 8437
			public int questId;
		}
	}
}
