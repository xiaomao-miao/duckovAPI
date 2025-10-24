using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class ToggleHUD : MonoBehaviour
{
	// Token: 0x06000A66 RID: 2662 RVA: 0x0002DAE0 File Offset: 0x0002BCE0
	private void Awake()
	{
		foreach (GameObject gameObject in this.toggleTargets)
		{
			if (gameObject != null && !gameObject.activeInHierarchy)
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000919 RID: 2329
	public List<GameObject> toggleTargets;
}
