using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// Token: 0x0200013A RID: 314
public class PlayableDirectorEvents : MonoBehaviour
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0002B12C File Offset: 0x0002932C
	private void OnEnable()
	{
		this.playableDirector.played += this.OnPlayed;
		this.playableDirector.paused += this.OnPaused;
		this.playableDirector.stopped += this.OnStopped;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0002B180 File Offset: 0x00029380
	private void OnDisable()
	{
		this.playableDirector.played -= this.OnPlayed;
		this.playableDirector.paused -= this.OnPaused;
		this.playableDirector.stopped -= this.OnStopped;
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0002B1D2 File Offset: 0x000293D2
	private void OnStopped(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onStopped;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0002B1E4 File Offset: 0x000293E4
	private void OnPaused(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onPaused;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0002B1F6 File Offset: 0x000293F6
	private void OnPlayed(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onPlayed;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040008CA RID: 2250
	[SerializeField]
	private PlayableDirector playableDirector;

	// Token: 0x040008CB RID: 2251
	[SerializeField]
	private UnityEvent onPlayed;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private UnityEvent onPaused;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private UnityEvent onStopped;
}
