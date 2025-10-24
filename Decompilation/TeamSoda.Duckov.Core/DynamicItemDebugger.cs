using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class DynamicItemDebugger : MonoBehaviour
{
	// Token: 0x060004B5 RID: 1205 RVA: 0x0001588C File Offset: 0x00013A8C
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.Add();
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000158A0 File Offset: 0x00013AA0
	private void Add()
	{
		foreach (Item prefab in this.prefabs)
		{
			ItemAssetsCollection.AddDynamicEntry(prefab);
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x000158F4 File Offset: 0x00013AF4
	private void CreateCorresponding()
	{
		this.CreateTask().Forget();
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00015904 File Offset: 0x00013B04
	private UniTask CreateTask()
	{
		DynamicItemDebugger.<CreateTask>d__4 <CreateTask>d__;
		<CreateTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateTask>d__.<>4__this = this;
		<CreateTask>d__.<>1__state = -1;
		<CreateTask>d__.<>t__builder.Start<DynamicItemDebugger.<CreateTask>d__4>(ref <CreateTask>d__);
		return <CreateTask>d__.<>t__builder.Task;
	}

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private List<Item> prefabs;
}
