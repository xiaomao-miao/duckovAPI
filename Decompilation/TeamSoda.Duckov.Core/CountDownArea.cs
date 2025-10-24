using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200010E RID: 270
public class CountDownArea : MonoBehaviour
{
	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x00028C6E File Offset: 0x00026E6E
	public float RequiredExtrationTime
	{
		get
		{
			return this.requiredExtrationTime;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x00028C76 File Offset: 0x00026E76
	private float TimeSinceCountDownBegan
	{
		get
		{
			return Time.time - this.timeWhenCountDownBegan;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00028C84 File Offset: 0x00026E84
	public float RemainingTime
	{
		get
		{
			return Mathf.Clamp(this.RequiredExtrationTime - this.TimeSinceCountDownBegan, 0f, this.RequiredExtrationTime);
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000938 RID: 2360 RVA: 0x00028CA3 File Offset: 0x00026EA3
	public float Progress
	{
		get
		{
			if (this.requiredExtrationTime <= 0f)
			{
				return 1f;
			}
			return this.TimeSinceCountDownBegan / this.RequiredExtrationTime;
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00028CC8 File Offset: 0x00026EC8
	private void OnTriggerEnter(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (component == null)
		{
			return;
		}
		if (component.IsMainCharacter())
		{
			this.hoveringMainCharacters.Add(component);
			this.OnHoveringMainCharactersChanged();
		}
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00028D0C File Offset: 0x00026F0C
	private void OnTriggerExit(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (component == null)
		{
			return;
		}
		if (component.IsMainCharacter())
		{
			this.hoveringMainCharacters.Remove(component);
			this.OnHoveringMainCharactersChanged();
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00028D4E File Offset: 0x00026F4E
	private void OnHoveringMainCharactersChanged()
	{
		if (!this.countingDown && this.hoveringMainCharacters.Count > 0)
		{
			this.BeginCountDown();
			return;
		}
		if (this.countingDown && this.hoveringMainCharacters.Count < 1)
		{
			this.AbortCountDown();
		}
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00028D89 File Offset: 0x00026F89
	private void BeginCountDown()
	{
		this.countingDown = true;
		this.timeWhenCountDownBegan = Time.time;
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStarted;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(this);
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00028DAE File Offset: 0x00026FAE
	private void AbortCountDown()
	{
		this.countingDown = false;
		this.timeWhenCountDownBegan = float.MaxValue;
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStopped;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(this);
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00028DD4 File Offset: 0x00026FD4
	private void UpdateCountDown()
	{
		if (this.hoveringMainCharacters.All((CharacterMainControl e) => e.Health.IsDead))
		{
			this.AbortCountDown();
		}
		if (this.TimeSinceCountDownBegan >= this.RequiredExtrationTime)
		{
			this.OnCountdownSucceed();
		}
		int num = (int)(this.RemainingTime + Time.deltaTime);
		if ((int)this.RemainingTime != num)
		{
			UnityEvent unityEvent = this.onTickSecond;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00028E4F File Offset: 0x0002704F
	private void OnCountdownSucceed()
	{
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStopped;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		UnityEvent unityEvent2 = this.onCountDownSucceed;
		if (unityEvent2 != null)
		{
			unityEvent2.Invoke();
		}
		this.countingDown = false;
		if (this.disableWhenSucceed)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00028E8A File Offset: 0x0002708A
	private void Update()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.countingDown && View.ActiveView == null)
		{
			this.UpdateCountDown();
		}
	}

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private float requiredExtrationTime = 5f;

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private bool disableWhenSucceed = true;

	// Token: 0x04000838 RID: 2104
	public UnityEvent onCountDownSucceed;

	// Token: 0x04000839 RID: 2105
	public UnityEvent onTickSecond;

	// Token: 0x0400083A RID: 2106
	public UnityEvent<CountDownArea> onCountDownStarted;

	// Token: 0x0400083B RID: 2107
	public UnityEvent<CountDownArea> onCountDownStopped;

	// Token: 0x0400083C RID: 2108
	private bool countingDown;

	// Token: 0x0400083D RID: 2109
	private float timeWhenCountDownBegan = float.MaxValue;

	// Token: 0x0400083E RID: 2110
	private HashSet<CharacterMainControl> hoveringMainCharacters = new HashSet<CharacterMainControl>();
}
