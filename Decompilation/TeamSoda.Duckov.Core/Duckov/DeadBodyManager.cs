using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules;
using Duckov.Scenes;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov
{
	// Token: 0x0200023E RID: 574
	public class DeadBodyManager : MonoBehaviour
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00044363 File Offset: 0x00042563
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x0004436A File Offset: 0x0004256A
		public static DeadBodyManager Instance { get; private set; }

		// Token: 0x060011C5 RID: 4549 RVA: 0x00044372 File Offset: 0x00042572
		private void AppendDeathInfo(DeadBodyManager.DeathInfo deathInfo)
		{
			while (this.deaths.Count >= GameRulesManager.Current.SaveDeadbodyCount)
			{
				this.deaths.RemoveAt(0);
			}
			this.deaths.Add(deathInfo);
			this.Save();
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000443AB File Offset: 0x000425AB
		private static List<DeadBodyManager.DeathInfo> LoadDeathInfos()
		{
			return SavesSystem.Load<List<DeadBodyManager.DeathInfo>>("DeathList");
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x000443B8 File Offset: 0x000425B8
		internal static void RecordDeath(CharacterMainControl mainCharacter)
		{
			if (DeadBodyManager.Instance == null)
			{
				Debug.LogError("DeadBodyManager Instance is null");
				return;
			}
			DeadBodyManager.DeathInfo deathInfo = new DeadBodyManager.DeathInfo();
			deathInfo.valid = true;
			deathInfo.raidID = RaidUtilities.CurrentRaid.ID;
			deathInfo.subSceneID = MultiSceneCore.ActiveSubSceneID;
			deathInfo.worldPosition = mainCharacter.transform.position;
			deathInfo.itemTreeData = ItemTreeData.FromItem(mainCharacter.CharacterItem);
			DeadBodyManager.Instance.AppendDeathInfo(deathInfo);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00044434 File Offset: 0x00042634
		private void Awake()
		{
			DeadBodyManager.Instance = this;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
			this.deaths.Clear();
			List<DeadBodyManager.DeathInfo> list = DeadBodyManager.LoadDeathInfos();
			if (list != null)
			{
				this.deaths.AddRange(list);
			}
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00044489 File Offset: 0x00042689
		private void OnDestroy()
		{
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000444AD File Offset: 0x000426AD
		private void Save()
		{
			SavesSystem.Save<List<DeadBodyManager.DeathInfo>>("DeathList", this.deaths);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000444C0 File Offset: 0x000426C0
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning bodies";
			if (!LevelConfig.SpawnTomb)
			{
				return;
			}
			foreach (DeadBodyManager.DeathInfo info in this.deaths)
			{
				if (this.ShouldSpawnDeadBody(info))
				{
					this.SpawnDeadBody(info).Forget();
				}
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00044534 File Offset: 0x00042734
		private UniTask SpawnDeadBody(DeadBodyManager.DeathInfo info)
		{
			DeadBodyManager.<SpawnDeadBody>d__13 <SpawnDeadBody>d__;
			<SpawnDeadBody>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SpawnDeadBody>d__.info = info;
			<SpawnDeadBody>d__.<>1__state = -1;
			<SpawnDeadBody>d__.<>t__builder.Start<DeadBodyManager.<SpawnDeadBody>d__13>(ref <SpawnDeadBody>d__);
			return <SpawnDeadBody>d__.<>t__builder.Task;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00044577 File Offset: 0x00042777
		private static void NotifyDeadbodyTouched(DeadBodyManager.DeathInfo info)
		{
			if (DeadBodyManager.Instance == null)
			{
				return;
			}
			DeadBodyManager.Instance.OnDeadbodyTouched(info);
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00044594 File Offset: 0x00042794
		private void OnDeadbodyTouched(DeadBodyManager.DeathInfo info)
		{
			DeadBodyManager.DeathInfo deathInfo = this.deaths.Find((DeadBodyManager.DeathInfo e) => e.raidID == info.raidID);
			if (deathInfo == null)
			{
				return;
			}
			deathInfo.touched = true;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000445D4 File Offset: 0x000427D4
		private bool ShouldSpawnDeadBody(DeadBodyManager.DeathInfo info)
		{
			return info != null && GameRulesManager.Current.SpawnDeadBody && LevelManager.Instance && LevelManager.Instance.IsRaidMap && info != null && info.valid && !info.touched && !(MultiSceneCore.ActiveSubSceneID != info.subSceneID);
		}

		// Token: 0x04000DB3 RID: 3507
		private List<DeadBodyManager.DeathInfo> deaths = new List<DeadBodyManager.DeathInfo>();

		// Token: 0x02000529 RID: 1321
		[Serializable]
		public class DeathInfo
		{
			// Token: 0x04001E57 RID: 7767
			public bool valid;

			// Token: 0x04001E58 RID: 7768
			public uint raidID;

			// Token: 0x04001E59 RID: 7769
			public string subSceneID;

			// Token: 0x04001E5A RID: 7770
			public Vector3 worldPosition;

			// Token: 0x04001E5B RID: 7771
			public ItemTreeData itemTreeData;

			// Token: 0x04001E5C RID: 7772
			public bool spawned;

			// Token: 0x04001E5D RID: 7773
			public bool touched;
		}
	}
}
