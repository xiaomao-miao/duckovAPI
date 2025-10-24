using System;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001E9 RID: 489
public class EventIfSteamChina : MonoBehaviour
{
	// Token: 0x06000E71 RID: 3697 RVA: 0x0003A16F File Offset: 0x0003836F
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (SteamUtils.IsSteamChinaLauncher())
		{
			this.onStart_IsSteamChina.Invoke();
			return;
		}
		this.onStart_IsNotSteamChina.Invoke();
	}

	// Token: 0x04000BF2 RID: 3058
	public UnityEvent onStart_IsSteamChina;

	// Token: 0x04000BF3 RID: 3059
	public UnityEvent onStart_IsNotSteamChina;
}
