using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000208 RID: 520
public class ScrollViewEventReceiver : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	// Token: 0x06000F2F RID: 3887 RVA: 0x0003BDB0 File Offset: 0x00039FB0
	private void Awake()
	{
		if (this.scrollRect == null)
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0003BDCC File Offset: 0x00039FCC
	public void OnScroll(PointerEventData eventData)
	{
	}

	// Token: 0x04000C64 RID: 3172
	[SerializeField]
	private ScrollRect scrollRect;
}
