using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Economy;
using Duckov.Endowment;
using Duckov.Quests;
using Duckov.Rules.UI;
using Duckov.Scenes;
using Saves;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000322 RID: 802
	public class AchievementManager : MonoBehaviour
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x000605BB File Offset: 0x0005E7BB
		public static AchievementManager Instance
		{
			get
			{
				return GameManager.AchievementManager;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x000605C2 File Offset: 0x0005E7C2
		public static bool CanUnlockAchievement
		{
			get
			{
				return !DifficultySelection.CustomDifficultyMarker;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x000605CE File Offset: 0x0005E7CE
		public List<string> UnlockedAchievements
		{
			get
			{
				return this._unlockedAchievements;
			}
		}

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06001AAD RID: 6829 RVA: 0x000605D8 File Offset: 0x0005E7D8
		// (remove) Token: 0x06001AAE RID: 6830 RVA: 0x0006060C File Offset: 0x0005E80C
		public static event Action<AchievementManager> OnAchievementDataLoaded;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06001AAF RID: 6831 RVA: 0x00060640 File Offset: 0x0005E840
		// (remove) Token: 0x06001AB0 RID: 6832 RVA: 0x00060674 File Offset: 0x0005E874
		public static event Action<string> OnAchievementUnlocked;

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000606A7 File Offset: 0x0005E8A7
		private void Awake()
		{
			this.Load();
			this.RegisterEvents();
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000606B5 File Offset: 0x0005E8B5
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000606BD File Offset: 0x0005E8BD
		private void Start()
		{
			this.MakeSureMoneyAchievementsUnlocked();
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000606C8 File Offset: 0x0005E8C8
		private void RegisterEvents()
		{
			Quest.onQuestCompleted += this.OnQuestCompleted;
			SavesCounter.OnKillCountChanged = (Action<string, int>)Delegate.Combine(SavesCounter.OnKillCountChanged, new Action<string, int>(this.OnKillCountChanged));
			MultiSceneCore.OnSetSceneVisited += this.OnSetSceneVisited;
			LevelManager.OnEvacuated += this.OnEvacuated;
			EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
			EndowmentManager.OnEndowmentUnlock = (Action<EndowmentIndex>)Delegate.Combine(EndowmentManager.OnEndowmentUnlock, new Action<EndowmentIndex>(this.OnEndowmentUnlocked));
			EconomyManager.OnEconomyManagerLoaded += this.OnEconomyManagerLoaded;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0006076C File Offset: 0x0005E96C
		private void UnregisterEvents()
		{
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			SavesCounter.OnKillCountChanged = (Action<string, int>)Delegate.Remove(SavesCounter.OnKillCountChanged, new Action<string, int>(this.OnKillCountChanged));
			MultiSceneCore.OnSetSceneVisited -= this.OnSetSceneVisited;
			LevelManager.OnEvacuated -= this.OnEvacuated;
			EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
			EndowmentManager.OnEndowmentUnlock = (Action<EndowmentIndex>)Delegate.Remove(EndowmentManager.OnEndowmentUnlock, new Action<EndowmentIndex>(this.OnEndowmentUnlocked));
			EconomyManager.OnEconomyManagerLoaded -= this.OnEconomyManagerLoaded;
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x0006080E File Offset: 0x0005EA0E
		private void OnEconomyManagerLoaded()
		{
			this.MakeSureMoneyAchievementsUnlocked();
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00060816 File Offset: 0x0005EA16
		private void OnEndowmentUnlocked(EndowmentIndex index)
		{
			this.Unlock(string.Format("Endowmment_{0}", index));
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x0006082E File Offset: 0x0005EA2E
		public static void UnlockEndowmentAchievement(EndowmentIndex index)
		{
			if (AchievementManager.Instance == null)
			{
				return;
			}
			AchievementManager.Instance.Unlock(string.Format("Endowmment_{0}", index));
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00060858 File Offset: 0x0005EA58
		private void OnMoneyChanged(long oldValue, long newValue)
		{
			if (oldValue < 10000L && newValue >= 10000L)
			{
				this.Unlock("Money_10K");
			}
			if (oldValue < 100000L && newValue >= 100000L)
			{
				this.Unlock("Money_100K");
			}
			if (oldValue < 1000000L && newValue >= 1000000L)
			{
				this.Unlock("Money_1M");
			}
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000608BC File Offset: 0x0005EABC
		private void MakeSureMoneyAchievementsUnlocked()
		{
			long money = EconomyManager.Money;
			if (money >= 10000L)
			{
				this.Unlock("Money_10K");
			}
			if (money >= 100000L)
			{
				this.Unlock("Money_100K");
			}
			if (money >= 1000000L)
			{
				this.Unlock("Money_1M");
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0006090C File Offset: 0x0005EB0C
		private void OnEvacuated(EvacuationInfo info)
		{
			string mainSceneID = MultiSceneCore.MainSceneID;
			if (!this.evacuateSceneIDs.Contains(mainSceneID))
			{
				return;
			}
			this.Unlock("Evacuate_" + mainSceneID);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0006093F File Offset: 0x0005EB3F
		private void OnSetSceneVisited(string id)
		{
			if (!this.achievementSceneIDs.Contains(id))
			{
				return;
			}
			this.Unlock("Arrive_" + id);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x00060964 File Offset: 0x0005EB64
		private void OnKillCountChanged(string key, int value)
		{
			this.Unlock("FirstBlood");
			if (AchievementDatabase.Instance == null)
			{
				return;
			}
			Debug.Log("COUNTING " + key);
			foreach (AchievementManager.KillCountAchievement killCountAchievement in this.KillCountAchivements)
			{
				if (killCountAchievement.key == key && value >= killCountAchievement.value)
				{
					this.Unlock(string.Format("Kill_{0}_{1}", key, killCountAchievement.value));
				}
			}
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x000609EC File Offset: 0x0005EBEC
		private void OnQuestCompleted(Quest quest)
		{
			if (AchievementDatabase.Instance == null)
			{
				return;
			}
			string id = string.Format("Quest_{0}", quest.ID);
			AchievementDatabase.Achievement achievement;
			if (!AchievementDatabase.TryGetAchievementData(id, out achievement))
			{
				return;
			}
			this.Unlock(id);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x00060A2F File Offset: 0x0005EC2F
		private void Save()
		{
			SavesSystem.SaveGlobal<List<string>>("Achievements", this.UnlockedAchievements);
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x00060A44 File Offset: 0x0005EC44
		private void Load()
		{
			this.UnlockedAchievements.Clear();
			List<string> list = SavesSystem.LoadGlobal<List<string>>("Achievements", null);
			if (list != null)
			{
				this.UnlockedAchievements.AddRange(list);
			}
			Action<AchievementManager> onAchievementDataLoaded = AchievementManager.OnAchievementDataLoaded;
			if (onAchievementDataLoaded == null)
			{
				return;
			}
			onAchievementDataLoaded(this);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x00060A88 File Offset: 0x0005EC88
		public void Unlock(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				Debug.LogError("Trying to unlock a empty acheivement.", this);
				return;
			}
			id = id.Trim();
			AchievementDatabase.Achievement achievement;
			if (!AchievementDatabase.TryGetAchievementData(id, out achievement))
			{
				Debug.LogError("Invalid acheivement id: " + id);
			}
			if (this.UnlockedAchievements.Contains(id))
			{
				return;
			}
			if (!AchievementManager.CanUnlockAchievement)
			{
				return;
			}
			this.UnlockedAchievements.Add(id);
			this.Save();
			Action<string> onAchievementUnlocked = AchievementManager.OnAchievementUnlocked;
			if (onAchievementUnlocked == null)
			{
				return;
			}
			onAchievementUnlocked(id);
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x00060B04 File Offset: 0x0005ED04
		public static bool IsIDValid(string id)
		{
			return !(AchievementDatabase.Instance == null) && AchievementDatabase.Instance.IsIDValid(id);
		}

		// Token: 0x04001315 RID: 4885
		private List<string> _unlockedAchievements = new List<string>();

		// Token: 0x04001318 RID: 4888
		private readonly string[] evacuateSceneIDs = new string[]
		{
			"Level_GroundZero_Main"
		};

		// Token: 0x04001319 RID: 4889
		private readonly string[] achievementSceneIDs = new string[]
		{
			"Base",
			"Level_GroundZero_Main",
			"Level_HiddenWarehouse_Main",
			"Level_Farm_Main",
			"Level_JLab_Main",
			"Level_StormZone_Main"
		};

		// Token: 0x0400131A RID: 4890
		private readonly AchievementManager.KillCountAchievement[] KillCountAchivements = new AchievementManager.KillCountAchievement[]
		{
			new AchievementManager.KillCountAchievement("Cname_ShortEagle", 10),
			new AchievementManager.KillCountAchievement("Cname_ShortEagle", 1),
			new AchievementManager.KillCountAchievement("Cname_Speedy", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss1", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss2", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss3", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss4", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss5", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Sniper", 1),
			new AchievementManager.KillCountAchievement("Cname_Vida", 1),
			new AchievementManager.KillCountAchievement("Cname_Roadblock", 1),
			new AchievementManager.KillCountAchievement("Cname_SchoolBully", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Fly", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Arcade", 1),
			new AchievementManager.KillCountAchievement("Cname_UltraMan", 1),
			new AchievementManager.KillCountAchievement("Cname_LabTestObjective", 1)
		};

		// Token: 0x020005C1 RID: 1473
		private struct KillCountAchievement
		{
			// Token: 0x060028FE RID: 10494 RVA: 0x00097964 File Offset: 0x00095B64
			public KillCountAchievement(string key, int value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x0400207B RID: 8315
			public string key;

			// Token: 0x0400207C RID: 8316
			public int value;
		}
	}
}
