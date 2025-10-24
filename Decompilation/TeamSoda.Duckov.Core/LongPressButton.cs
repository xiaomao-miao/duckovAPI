using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000158 RID: 344
public class LongPressButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0002DF37 File Offset: 0x0002C137
	private float TimeSincePressStarted
	{
		get
		{
			return Time.unscaledTime - this.timeWhenPressStarted;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002DF45 File Offset: 0x0002C145
	private float Progress
	{
		get
		{
			if (!this.pressed)
			{
				return 0f;
			}
			return this.TimeSincePressStarted / this.pressTime;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0002DF62 File Offset: 0x0002C162
	private void Update()
	{
		this.fill.fillAmount = this.Progress;
		if (this.pressed && this.Progress >= 1f)
		{
			UnityEvent unityEvent = this.onPressFullfilled;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.pressed = false;
		}
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0002DFA2 File Offset: 0x0002C1A2
	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressed = true;
		this.timeWhenPressStarted = Time.unscaledTime;
		UnityEvent unityEvent = this.onPressStarted;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0002DFC6 File Offset: 0x0002C1C6
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.pressed)
		{
			return;
		}
		this.pressed = false;
		UnityEvent unityEvent = this.onPressCanceled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0002DFE8 File Offset: 0x0002C1E8
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!this.pressed)
		{
			return;
		}
		this.pressed = false;
		UnityEvent unityEvent = this.onPressCanceled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0400092A RID: 2346
	[SerializeField]
	private Image fill;

	// Token: 0x0400092B RID: 2347
	[SerializeField]
	private float pressTime = 1f;

	// Token: 0x0400092C RID: 2348
	public UnityEvent onPressStarted;

	// Token: 0x0400092D RID: 2349
	public UnityEvent onPressCanceled;

	// Token: 0x0400092E RID: 2350
	public UnityEvent onPressFullfilled;

	// Token: 0x0400092F RID: 2351
	private float timeWhenPressStarted;

	// Token: 0x04000930 RID: 2352
	private bool pressed;
}
