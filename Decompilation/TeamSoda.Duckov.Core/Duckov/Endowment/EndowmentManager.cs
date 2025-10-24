using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.Achievements;
using Saves;
using UnityEngine;

namespace Duckov.Endowment
{
	// Token: 0x020002F4 RID: 756
	public class EndowmentManager : MonoBehaviour
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x000598E5 File Offset: 0x00057AE5
		// (set) Token: 0x06001893 RID: 6291 RVA: 0x000598EC File Offset: 0x00057AEC
		private static EndowmentManager _instance { get; set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x000598F4 File Offset: 0x00057AF4
		public static EndowmentManager Instance
		{
			get
			{
				if (EndowmentManager._instance == null)
				{
					GameManager instance = GameManager.Instance;
				}
				return EndowmentManager._instance;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x0005990E File Offset: 0x00057B0E
		// (set) Token: 0x06001896 RID: 6294 RVA: 0x0005991A File Offset: 0x00057B1A
		public static EndowmentIndex SelectedIndex
		{
			get
			{
				return SavesSystem.Load<EndowmentIndex>("Endowment_SelectedIndex");
			}
			private set
			{
				SavesSystem.Save<EndowmentIndex>("Endowment_SelectedIndex", value);
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x00059927 File Offset: 0x00057B27
		public ReadOnlyCollection<EndowmentEntry> Entries
		{
			get
			{
				if (this._entries_ReadOnly == null)
				{
					this._entries_ReadOnly = new ReadOnlyCollection<EndowmentEntry>(this.entries);
				}
				return this._entries_ReadOnly;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x00059948 File Offset: 0x00057B48
		public static EndowmentEntry Current
		{
			get
			{
				if (EndowmentManager._instance == null)
				{
					return null;
				}
				return EndowmentManager._instance.entries.Find((EndowmentEntry e) => e != null && e.Index == EndowmentManager.SelectedIndex);
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x00059987 File Offset: 0x00057B87
		public static EndowmentIndex CurrentIndex
		{
			get
			{
				if (EndowmentManager.Current == null)
				{
					return EndowmentIndex.None;
				}
				return EndowmentManager.Current.Index;
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000599A4 File Offset: 0x00057BA4
		private EndowmentEntry GetEntry(EndowmentIndex index)
		{
			return this.entries.Find((EndowmentEntry e) => e != null && e.Index == index);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000599D5 File Offset: 0x00057BD5
		private static string GetUnlockKey(EndowmentIndex index)
		{
			return string.Format("Endowment_Unlock_R_{0}", index);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000599E7 File Offset: 0x00057BE7
		public static bool GetEndowmentUnlocked(EndowmentIndex index)
		{
			if (EndowmentManager.Instance != null)
			{
				if (EndowmentManager.Instance.GetEntry(index).UnlockedByDefault)
				{
					return true;
				}
			}
			else
			{
				Debug.LogError("Endowment Manager 不存在。");
			}
			return SavesSystem.LoadGlobal<bool>(EndowmentManager.GetUnlockKey(index), false);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00059A20 File Offset: 0x00057C20
		private static void SetEndowmentUnlocked(EndowmentIndex index, bool value = true)
		{
			SavesSystem.SaveGlobal<bool>(EndowmentManager.GetUnlockKey(index), value);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00059A30 File Offset: 0x00057C30
		public static bool UnlockEndowment(EndowmentIndex index)
		{
			try
			{
				Action<EndowmentIndex> onEndowmentUnlock = EndowmentManager.OnEndowmentUnlock;
				if (onEndowmentUnlock != null)
				{
					onEndowmentUnlock(index);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			if (EndowmentManager.GetEndowmentUnlocked(index))
			{
				Debug.Log("尝试解锁天赋，但天赋已经解锁");
				return false;
			}
			EndowmentManager.SetEndowmentUnlocked(index, true);
			return true;
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00059A84 File Offset: 0x00057C84
		private void Awake()
		{
			if (EndowmentManager._instance != null)
			{
				Debug.LogError("检测到多个Endowment Manager");
				return;
			}
			EndowmentManager._instance = this;
			if (LevelManager.LevelInited)
			{
				this.ApplyCurrentEndowment();
			}
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00059AC2 File Offset: 0x00057CC2
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00059AD5 File Offset: 0x00057CD5
		private void OnLevelInitialized()
		{
			this.ApplyCurrentEndowment();
			this.MakeSureEndowmentAchievementsUnlocked();
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00059AE4 File Offset: 0x00057CE4
		private void MakeSureEndowmentAchievementsUnlocked()
		{
			for (int i = 0; i < 5; i++)
			{
				EndowmentIndex index = (EndowmentIndex)i;
				EndowmentEntry entry = EndowmentManager.Instance.GetEntry(index);
				if (!(entry == null) && !entry.UnlockedByDefault && EndowmentManager.GetEndowmentUnlocked(index))
				{
					AchievementManager.UnlockEndowmentAchievement(index);
				}
			}
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00059B2C File Offset: 0x00057D2C
		private void ApplyCurrentEndowment()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			foreach (EndowmentEntry endowmentEntry in this.entries)
			{
				if (!(endowmentEntry == null))
				{
					endowmentEntry.Deactivate();
				}
			}
			EndowmentEntry endowmentEntry2 = EndowmentManager.Current;
			if (endowmentEntry2 == null)
			{
				return;
			}
			endowmentEntry2.Activate();
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00059BA8 File Offset: 0x00057DA8
		internal void SelectIndex(EndowmentIndex index)
		{
			EndowmentManager.SelectedIndex = index;
			this.ApplyCurrentEndowment();
			Action<EndowmentIndex> onEndowmentChanged = EndowmentManager.OnEndowmentChanged;
			if (onEndowmentChanged == null)
			{
				return;
			}
			onEndowmentChanged(index);
		}

		// Token: 0x040011E2 RID: 4578
		private const string SaveKey = "Endowment_SelectedIndex";

		// Token: 0x040011E3 RID: 4579
		public static Action<EndowmentIndex> OnEndowmentChanged;

		// Token: 0x040011E4 RID: 4580
		public static Action<EndowmentIndex> OnEndowmentUnlock;

		// Token: 0x040011E5 RID: 4581
		[SerializeField]
		private List<EndowmentEntry> entries = new List<EndowmentEntry>();

		// Token: 0x040011E6 RID: 4582
		private ReadOnlyCollection<EndowmentEntry> _entries_ReadOnly;
	}
}
