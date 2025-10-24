using System;
using Duckov;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000049 RID: 73
public class SfxOnClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060001C7 RID: 455 RVA: 0x00008960 File Offset: 0x00006B60
	public void OnPointerClick(PointerEventData eventData)
	{
		AudioManager.Post(this.sfx);
	}

	// Token: 0x04000172 RID: 370
	[SerializeField]
	private string sfx;
}
