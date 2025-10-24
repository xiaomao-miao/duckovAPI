using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000206 RID: 518
public class PointerDownUpEvents : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000F26 RID: 3878 RVA: 0x0003BCAB File Offset: 0x00039EAB
	public void OnPointerDown(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerDown;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0003BCBE File Offset: 0x00039EBE
	public void OnPointerUp(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerUp;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x04000C60 RID: 3168
	public UnityEvent<PointerEventData> onPointerDown;

	// Token: 0x04000C61 RID: 3169
	public UnityEvent<PointerEventData> onPointerUp;
}
