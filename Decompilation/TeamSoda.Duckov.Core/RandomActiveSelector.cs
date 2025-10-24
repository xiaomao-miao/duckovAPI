using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class RandomActiveSelector : MonoBehaviour
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x0001981C File Offset: 0x00017A1C
	private void Awake()
	{
		foreach (GameObject gameObject in this.selections)
		{
			if (!(gameObject == null))
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00019878 File Offset: 0x00017A78
	private void Update()
	{
		if (!this.setted && LevelManager.LevelInited)
		{
			this.Set();
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00019890 File Offset: 0x00017A90
	private void Set()
	{
		if (MultiSceneCore.Instance == null)
		{
			return;
		}
		object obj;
		if (MultiSceneCore.Instance.inLevelData.TryGetValue(this.guid, out obj))
		{
			this.activeIndex = (int)obj;
		}
		else
		{
			if (UnityEngine.Random.Range(0f, 1f) > this.activeChance)
			{
				this.activeIndex = -1;
			}
			else
			{
				this.activeIndex = UnityEngine.Random.Range(0, this.selections.Count);
			}
			MultiSceneCore.Instance.inLevelData.Add(this.guid, this.activeIndex);
		}
		if (this.activeIndex >= 0)
		{
			GameObject gameObject = this.selections[this.activeIndex];
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		this.setted = true;
		base.enabled = false;
	}

	// Token: 0x04000533 RID: 1331
	[Range(0f, 1f)]
	public float activeChance = 1f;

	// Token: 0x04000534 RID: 1332
	private int activeIndex;

	// Token: 0x04000535 RID: 1333
	private int guid;

	// Token: 0x04000536 RID: 1334
	private bool setted;

	// Token: 0x04000537 RID: 1335
	public List<GameObject> selections;
}
