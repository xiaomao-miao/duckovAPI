using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class TimeScaleManager : MonoBehaviour
{
	// Token: 0x06000542 RID: 1346 RVA: 0x00017831 File Offset: 0x00015A31
	private void Awake()
	{
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00017834 File Offset: 0x00015A34
	private void Update()
	{
		float timeScale = 1f;
		if (GameManager.Paused)
		{
			timeScale = 0f;
		}
		if (CameraMode.Active)
		{
			timeScale = 0f;
		}
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = Mathf.Max(0.0005f, Time.timeScale * 0.02f);
	}
}
