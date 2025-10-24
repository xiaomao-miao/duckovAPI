using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200015D RID: 349
public class DragHandler : MonoBehaviour, IDragHandler, IEventSystemHandler
{
	// Token: 0x06000AB0 RID: 2736 RVA: 0x0002E572 File Offset: 0x0002C772
	public void OnDrag(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onDrag;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x0400094C RID: 2380
	public UnityEvent<PointerEventData> onDrag;
}
