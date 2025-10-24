using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class OverrideDeathSceneRouting : MonoBehaviour
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002A818 File Offset: 0x00028A18
	// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0002A81F File Offset: 0x00028A1F
	public static OverrideDeathSceneRouting Instance { get; private set; }

	// Token: 0x060009E8 RID: 2536 RVA: 0x0002A827 File Offset: 0x00028A27
	private void OnEnable()
	{
		if (OverrideDeathSceneRouting.Instance != null)
		{
			Debug.LogError("存在多个OverrideDeathSceneRouting实例");
		}
		OverrideDeathSceneRouting.Instance = this;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0002A846 File Offset: 0x00028A46
	private void OnDisable()
	{
		if (OverrideDeathSceneRouting.Instance == this)
		{
			OverrideDeathSceneRouting.Instance = null;
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0002A85B File Offset: 0x00028A5B
	public string GetSceneID()
	{
		return this.sceneID;
	}

	// Token: 0x040008B5 RID: 2229
	[SceneID]
	[SerializeField]
	private string sceneID;
}
