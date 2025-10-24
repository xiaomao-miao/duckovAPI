using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests
{
	// Token: 0x0200033F RID: 831
	public class SpawnItemForTask : MonoBehaviour
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00067014 File Offset: 0x00065214
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

		// Token: 0x06001C9D RID: 7325 RVA: 0x00067036 File Offset: 0x00065236
		private void Awake()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0006705A File Offset: 0x0006525A
		private void Start()
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00067062 File Offset: 0x00065262
		private void OnDestroy()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x00067086 File Offset: 0x00065286
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning item for task";
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00067098 File Offset: 0x00065298
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000670A0 File Offset: 0x000652A0
		private void SpawnIfNeeded()
		{
			if (this.itemID < 0)
			{
				return;
			}
			if (this.task == null)
			{
				Debug.Log("spawn item task is null");
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

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000670F0 File Offset: 0x000652F0
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

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00067150 File Offset: 0x00065350
		private bool IsSpawned()
		{
			object obj;
			return this.spawned || (!(MultiSceneCore.Instance == null) && MultiSceneCore.Instance.inLevelData.TryGetValue(this.SpawnKey, out obj) && obj is bool && (bool)obj);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000671A4 File Offset: 0x000653A4
		private void Spawn()
		{
			MultiSceneLocation random = this.locations.GetRandom<MultiSceneLocation>();
			Vector3 pos;
			if (!random.TryGetLocationPosition(out pos))
			{
				return;
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.Instance.inLevelData[this.SpawnKey] = true;
			}
			this.spawned = true;
			this.SpawnItem(pos, base.transform.gameObject.scene, random).Forget();
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00067218 File Offset: 0x00065418
		private UniTaskVoid SpawnItem(Vector3 pos, Scene scene, MultiSceneLocation location)
		{
			SpawnItemForTask.<SpawnItem>d__18 <SpawnItem>d__;
			<SpawnItem>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SpawnItem>d__.<>4__this = this;
			<SpawnItem>d__.pos = pos;
			<SpawnItem>d__.location = location;
			<SpawnItem>d__.<>1__state = -1;
			<SpawnItem>d__.<>t__builder.Start<SpawnItemForTask.<SpawnItem>d__18>(ref <SpawnItem>d__);
			return <SpawnItem>d__.<>t__builder.Task;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0006726B File Offset: 0x0006546B
		private void OnItemTreeChanged(Item selfItem)
		{
			if (this.mapElement && selfItem.ParentItem)
			{
				this.mapElement.SetVisibility(false);
				selfItem.onItemTreeChanged -= this.OnItemTreeChanged;
			}
		}

		// Token: 0x040013E7 RID: 5095
		[SerializeField]
		private string componentID = "SpawnItemForTask";

		// Token: 0x040013E8 RID: 5096
		private Task _taskCache;

		// Token: 0x040013E9 RID: 5097
		[SerializeField]
		private List<MultiSceneLocation> locations;

		// Token: 0x040013EA RID: 5098
		[ItemTypeID]
		[SerializeField]
		private int itemID = -1;

		// Token: 0x040013EB RID: 5099
		[SerializeField]
		private MapElementForTask mapElement;

		// Token: 0x040013EC RID: 5100
		private bool spawned;
	}
}
