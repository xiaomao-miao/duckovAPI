using System;
using System.Collections.Generic;
using Duckov.Quests.Tasks;
using Duckov.Scenes;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests
{
	// Token: 0x02000340 RID: 832
	public class SpawnPrefabForTask : MonoBehaviour
	{
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x000672BF File Offset: 0x000654BF
		private Task task
		{
			get
			{
				if (this._taskCache == null)
				{
					this._taskCache = base.GetComponent<Task>();
				}
				return this._taskCache;
			}
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000672E1 File Offset: 0x000654E1
		private void Awake()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x00067305 File Offset: 0x00065505
		private void Start()
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0006730D File Offset: 0x0006550D
		private void OnDestroy()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x00067331 File Offset: 0x00065531
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning prefabs for task";
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x00067343 File Offset: 0x00065543
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0006734C File Offset: 0x0006554C
		private void SpawnIfNeeded()
		{
			if (this.prefab == null)
			{
				return;
			}
			if (this.task == null)
			{
				Debug.LogWarning("未配置Task");
				return;
			}
			if (this.task.IsFinished())
			{
				return;
			}
			if (this.IsSpawned())
			{
				return;
			}
			this.Spawn();
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x000673A0 File Offset: 0x000655A0
		private int SpawnKey
		{
			get
			{
				return string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					"SpawnPrefabForTask",
					this.task.Master.ID,
					this.task.ID,
					this.componentID
				}).GetHashCode();
			}
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x00067400 File Offset: 0x00065600
		private bool IsSpawned()
		{
			object obj;
			return this.spawned || (!(MultiSceneCore.Instance == null) && MultiSceneCore.Instance.inLevelData.TryGetValue(this.SpawnKey, out obj) && obj is bool && (bool)obj);
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00067454 File Offset: 0x00065654
		private void Spawn()
		{
			Vector3 position;
			if (!this.locations.GetRandom<MultiSceneLocation>().TryGetLocationPosition(out position))
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, position, Quaternion.identity);
			QuestTask_TaskEvent questTask_TaskEvent = this.task as QuestTask_TaskEvent;
			if (questTask_TaskEvent)
			{
				TaskEventEmitter component = gameObject.GetComponent<TaskEventEmitter>();
				if (component)
				{
					component.SetKey(questTask_TaskEvent.EventKey);
				}
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.MoveToActiveWithScene(gameObject, base.transform.gameObject.scene.buildIndex);
				MultiSceneCore.Instance.inLevelData[this.SpawnKey] = true;
			}
			this.spawned = true;
		}

		// Token: 0x040013ED RID: 5101
		[SerializeField]
		private string componentID = "SpawnPrefabForTask";

		// Token: 0x040013EE RID: 5102
		private Task _taskCache;

		// Token: 0x040013EF RID: 5103
		[SerializeField]
		private List<MultiSceneLocation> locations;

		// Token: 0x040013F0 RID: 5104
		[SerializeField]
		private GameObject prefab;

		// Token: 0x040013F1 RID: 5105
		private bool spawned;
	}
}
