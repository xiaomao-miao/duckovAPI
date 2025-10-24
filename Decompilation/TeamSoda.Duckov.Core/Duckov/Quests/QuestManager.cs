using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Quests.Relations;
using Duckov.Quests.Tasks;
using Duckov.UI;
using Duckov.Utilities;
using Saves;
using Sirenix.Utilities;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000337 RID: 823
	public class QuestManager : MonoBehaviour, ISaveDataProvider, INeedInspection
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x00065BB6 File Offset: 0x00063DB6
		public string TaskFinishNotificationFormat
		{
			get
			{
				return this.taskFinishNotificationFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x00065BC3 File Offset: 0x00063DC3
		public static QuestManager Instance
		{
			get
			{
				return QuestManager.instance;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x00065BCA File Offset: 0x00063DCA
		public static bool AnyQuestNeedsInspection
		{
			get
			{
				return !(QuestManager.Instance == null) && QuestManager.Instance.NeedInspection;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x00065BE5 File Offset: 0x00063DE5
		public bool NeedInspection
		{
			get
			{
				if (this.activeQuests == null)
				{
					return false;
				}
				return this.activeQuests.Any((Quest e) => e != null && e.NeedInspection);
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00065C1B File Offset: 0x00063E1B
		public static IEnumerable<int> GetAllRequiredItems()
		{
			if (QuestManager.Instance == null)
			{
				yield break;
			}
			List<Quest> list = QuestManager.Instance.ActiveQuests;
			foreach (Quest quest in list)
			{
				if (quest.tasks != null)
				{
					foreach (Task task in quest.tasks)
					{
						SubmitItems submitItems = task as SubmitItems;
						if (submitItems != null && !submitItems.IsFinished())
						{
							yield return submitItems.ItemTypeID;
						}
					}
					List<Task>.Enumerator enumerator2 = default(List<Task>.Enumerator);
				}
			}
			List<Quest>.Enumerator enumerator = default(List<Quest>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00065C24 File Offset: 0x00063E24
		public static bool AnyActiveQuestNeedsInspection(QuestGiverID giverID)
		{
			return !(QuestManager.Instance == null) && QuestManager.Instance.activeQuests != null && QuestManager.Instance.activeQuests.Any((Quest e) => e != null && e.QuestGiverID == giverID && e.NeedInspection);
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x00065C76 File Offset: 0x00063E76
		private ICollection<Quest> QuestPrefabCollection
		{
			get
			{
				return GameplayDataSettings.QuestCollection;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x00065C7D File Offset: 0x00063E7D
		private QuestRelationGraph QuestRelation
		{
			get
			{
				return GameplayDataSettings.QuestRelation;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x00065C84 File Offset: 0x00063E84
		public List<Quest> ActiveQuests
		{
			get
			{
				this.activeQuests.Sort(delegate(Quest a, Quest b)
				{
					int num = a.AreTasksFinished() ? 1 : 0;
					return (b.AreTasksFinished() ? 1 : 0) - num;
				});
				return this.activeQuests;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00065CB6 File Offset: 0x00063EB6
		public List<Quest> HistoryQuests
		{
			get
			{
				return this.historyQuests;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x00065CBE File Offset: 0x00063EBE
		public List<int> EverInspectedQuest
		{
			get
			{
				return this.everInspectedQuest;
			}
		}

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06001C26 RID: 7206 RVA: 0x00065CC8 File Offset: 0x00063EC8
		// (remove) Token: 0x06001C27 RID: 7207 RVA: 0x00065CFC File Offset: 0x00063EFC
		public static event Action<QuestManager> onQuestListsChanged;

		// Token: 0x06001C28 RID: 7208 RVA: 0x00065D30 File Offset: 0x00063F30
		public object GenerateSaveData()
		{
			QuestManager.SaveData saveData = default(QuestManager.SaveData);
			saveData.activeQuestsData = new List<object>();
			saveData.historyQuestsData = new List<object>();
			saveData.everInspectedQuest = new List<int>();
			foreach (Quest quest in this.ActiveQuests)
			{
				saveData.activeQuestsData.Add(quest.GenerateSaveData());
			}
			foreach (Quest quest2 in this.HistoryQuests)
			{
				saveData.historyQuestsData.Add(quest2.GenerateSaveData());
			}
			saveData.everInspectedQuest.AddRange(this.EverInspectedQuest);
			return saveData;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x00065E1C File Offset: 0x0006401C
		public void SetupSaveData(object dataObj)
		{
			if (dataObj is QuestManager.SaveData)
			{
				QuestManager.SaveData saveData = (QuestManager.SaveData)dataObj;
				if (saveData.activeQuestsData != null)
				{
					foreach (object obj in saveData.activeQuestsData)
					{
						int id = ((Quest.SaveData)obj).id;
						Quest questPrefab = this.GetQuestPrefab(id);
						if (questPrefab == null)
						{
							Debug.LogError(string.Format("未找到Quest {0}", id));
						}
						else
						{
							Quest quest = UnityEngine.Object.Instantiate<Quest>(questPrefab, base.transform);
							quest.SetupSaveData(obj);
							this.activeQuests.Add(quest);
						}
					}
				}
				if (saveData.historyQuestsData != null)
				{
					foreach (object obj2 in saveData.historyQuestsData)
					{
						int id2 = ((Quest.SaveData)obj2).id;
						Quest questPrefab2 = this.GetQuestPrefab(id2);
						if (questPrefab2 == null)
						{
							Debug.LogError(string.Format("未找到Quest {0}", id2));
						}
						else
						{
							Quest quest2 = UnityEngine.Object.Instantiate<Quest>(questPrefab2, base.transform);
							quest2.SetupSaveData(obj2);
							this.historyQuests.Add(quest2);
						}
					}
				}
				if (saveData.everInspectedQuest != null)
				{
					this.EverInspectedQuest.Clear();
					this.EverInspectedQuest.AddRange(saveData.everInspectedQuest);
				}
				return;
			}
			Debug.LogError("错误的数据类型");
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00065FB4 File Offset: 0x000641B4
		private void Save()
		{
			SavesSystem.Save<object>("Quest", "Data", this.GenerateSaveData());
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x00065FCC File Offset: 0x000641CC
		private void Load()
		{
			try
			{
				QuestManager.SaveData saveData = SavesSystem.Load<QuestManager.SaveData>("Quest", "Data");
				this.SetupSaveData(saveData);
			}
			catch
			{
				Debug.LogError("在加载Quest存档时出现了错误");
			}
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00066014 File Offset: 0x00064214
		public IEnumerable<Quest> GetAllQuestsByQuestGiverID(QuestGiverID questGiverID)
		{
			return from e in this.QuestPrefabCollection
			where e != null && e.QuestGiverID == questGiverID
			select e;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00066048 File Offset: 0x00064248
		private Quest GetQuestPrefab(int id)
		{
			return this.QuestPrefabCollection.FirstOrDefault((Quest q) => q != null && q.ID == id);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00066079 File Offset: 0x00064279
		private void Awake()
		{
			if (QuestManager.instance == null)
			{
				QuestManager.instance = this;
			}
			if (QuestManager.instance != this)
			{
				Debug.LogError("侦测到多个QuestManager！");
				return;
			}
			this.RegisterEvents();
			this.Load();
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000660B2 File Offset: 0x000642B2
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000660BC File Offset: 0x000642BC
		private void RegisterEvents()
		{
			Quest.onQuestStatusChanged += this.OnQuestStatusChanged;
			Quest.onQuestCompleted += this.OnQuestCompleted;
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00066110 File Offset: 0x00064310
		private void UnregisterEvents()
		{
			Quest.onQuestStatusChanged -= this.OnQuestStatusChanged;
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00066164 File Offset: 0x00064364
		private void OnQuestCompleted(Quest quest)
		{
			if (!this.activeQuests.Remove(quest))
			{
				Debug.LogWarning(quest.DisplayName + " 并不存在于活跃任务表中。已终止操作。");
				return;
			}
			this.historyQuests.Add(quest);
			Action<QuestManager> action = QuestManager.onQuestListsChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000661B1 File Offset: 0x000643B1
		private void OnQuestStatusChanged(Quest quest)
		{
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x000661B4 File Offset: 0x000643B4
		public void ActivateQuest(int id, QuestGiverID? overrideQuestGiverID = null)
		{
			Quest quest = UnityEngine.Object.Instantiate<Quest>(this.GetQuestPrefab(id), base.transform);
			if (overrideQuestGiverID != null)
			{
				quest.QuestGiverID = overrideQuestGiverID.Value;
			}
			this.activeQuests.Add(quest);
			quest.NotifyActivated();
			Action<QuestManager> action = QuestManager.onQuestListsChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0006620C File Offset: 0x0006440C
		internal static bool IsQuestAvaliable(int id)
		{
			return !(QuestManager.Instance == null) && !QuestManager.IsQuestFinished(id) && !QuestManager.instance.activeQuests.Any((Quest e) => e.ID == id) && QuestManager.Instance.GetQuestPrefab(id).MeetsPrerequisit();
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x00066278 File Offset: 0x00064478
		internal static bool IsQuestFinished(int id)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.historyQuests.Any((Quest e) => e.ID == id);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000662BC File Offset: 0x000644BC
		internal static bool AreQuestFinished(IEnumerable<int> requiredQuestIDs)
		{
			if (QuestManager.instance == null)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.AddRange(requiredQuestIDs);
			foreach (Quest quest in QuestManager.instance.historyQuests)
			{
				hashSet.Remove(quest.ID);
			}
			return hashSet.Count <= 0;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00066344 File Offset: 0x00064544
		internal static List<Quest> GetActiveQuestsFromGiver(QuestGiverID giverID)
		{
			List<Quest> result = new List<Quest>();
			if (QuestManager.instance == null)
			{
				return result;
			}
			return (from e in QuestManager.instance.ActiveQuests
			where e.QuestGiverID == giverID
			select e).ToList<Quest>();
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00066398 File Offset: 0x00064598
		internal static List<Quest> GetHistoryQuestsFromGiver(QuestGiverID giverID)
		{
			List<Quest> result = new List<Quest>();
			if (QuestManager.instance == null)
			{
				return result;
			}
			return (from e in QuestManager.instance.historyQuests
			where e != null && e.QuestGiverID == giverID
			select e).ToList<Quest>();
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000663E7 File Offset: 0x000645E7
		internal static bool IsQuestActive(Quest quest)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.activeQuests.Contains(quest);
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00066408 File Offset: 0x00064608
		internal static bool IsQuestActive(int questID)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.activeQuests.Any((Quest e) => e.ID == questID);
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00066454 File Offset: 0x00064654
		internal static bool AreQuestsActive(IEnumerable<int> requiredQuestIDs)
		{
			if (QuestManager.instance == null)
			{
				return false;
			}
			using (IEnumerator<int> enumerator = requiredQuestIDs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int id = enumerator.Current;
					if (!QuestManager.instance.activeQuests.Any((Quest e) => e.ID == id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000664D4 File Offset: 0x000646D4
		private void OnTaskFinished(Quest quest, Task task)
		{
			NotificationText.Push(this.TaskFinishNotificationFormat.Format(new
			{
				questDisplayName = quest.DisplayName,
				finishedTasks = quest.FinishedTaskCount,
				totalTasks = quest.tasks.Count
			}));
			Action<Quest, Task> onTaskFinishedEvent = QuestManager.OnTaskFinishedEvent;
			if (onTaskFinishedEvent != null)
			{
				onTaskFinishedEvent(quest, task);
			}
			AudioManager.Post("UI/mission_small");
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0006652A File Offset: 0x0006472A
		internal static void NotifyTaskFinished(Quest quest, Task task)
		{
			QuestManager questManager = QuestManager.instance;
			if (questManager == null)
			{
				return;
			}
			questManager.OnTaskFinished(quest, task);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x0006653D File Offset: 0x0006473D
		internal static bool EverInspected(int id)
		{
			return !(QuestManager.Instance == null) && QuestManager.Instance.EverInspectedQuest.Contains(id);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x0006655E File Offset: 0x0006475E
		internal static void SetEverInspected(int id)
		{
			if (QuestManager.EverInspected(id))
			{
				return;
			}
			if (QuestManager.Instance == null)
			{
				return;
			}
			QuestManager.Instance.EverInspectedQuest.Add(id);
		}

		// Token: 0x040013C0 RID: 5056
		[SerializeField]
		private string taskFinishNotificationFormatKey = "UI_Quest_TaskFinishedNotification";

		// Token: 0x040013C1 RID: 5057
		private static QuestManager instance;

		// Token: 0x040013C2 RID: 5058
		public static Action<Quest, Task> OnTaskFinishedEvent;

		// Token: 0x040013C3 RID: 5059
		private List<Quest> activeQuests = new List<Quest>();

		// Token: 0x040013C4 RID: 5060
		private List<Quest> historyQuests = new List<Quest>();

		// Token: 0x040013C5 RID: 5061
		private List<int> everInspectedQuest = new List<int>();

		// Token: 0x040013C7 RID: 5063
		private const string savePrefix = "Quest";

		// Token: 0x040013C8 RID: 5064
		private const string saveKey = "Data";

		// Token: 0x020005EC RID: 1516
		[Serializable]
		public struct SaveData
		{
			// Token: 0x04002104 RID: 8452
			public List<object> activeQuestsData;

			// Token: 0x04002105 RID: 8453
			public List<object> historyQuestsData;

			// Token: 0x04002106 RID: 8454
			public List<int> everInspectedQuest;
		}
	}
}
