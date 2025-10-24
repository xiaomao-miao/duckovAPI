using System;
using Duckov.UI.Animations;
using Saves;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class RestoreFailureDetectedIndicator : MonoBehaviour
{
	// Token: 0x06000990 RID: 2448 RVA: 0x0002986E File Offset: 0x00027A6E
	private void OnEnable()
	{
		SavesSystem.OnRestoreFailureDetected += this.Refresh;
		SavesSystem.OnSetFile += this.Refresh;
		this.Refresh();
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00029898 File Offset: 0x00027A98
	private void OnDisable()
	{
		SavesSystem.OnRestoreFailureDetected -= this.Refresh;
		SavesSystem.OnSetFile -= this.Refresh;
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x000298BC File Offset: 0x00027ABC
	private void Refresh()
	{
		if (SavesSystem.RestoreFailureMarker)
		{
			this.fadeGroup.Show();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private FadeGroup fadeGroup;
}
