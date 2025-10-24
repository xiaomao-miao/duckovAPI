using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class LevelManagerProxy : MonoBehaviour
{
	// Token: 0x0600094D RID: 2381 RVA: 0x0002911B File Offset: 0x0002731B
	public void NotifyEvacuated()
	{
		LevelManager instance = LevelManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.NotifyEvacuated(new EvacuationInfo(MultiSceneCore.ActiveSubSceneID, base.transform.position));
	}
}
