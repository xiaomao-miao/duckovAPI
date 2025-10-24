using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200016A RID: 362
public class OnPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000AF5 RID: 2805 RVA: 0x0002EDEC File Offset: 0x0002CFEC
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerClick;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x0400096D RID: 2413
	public UnityEvent<PointerEventData> onPointerClick;
}
