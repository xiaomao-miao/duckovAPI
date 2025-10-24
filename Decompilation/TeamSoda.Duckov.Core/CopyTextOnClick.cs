using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000164 RID: 356
public class CopyTextOnClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0002EA00 File Offset: 0x0002CC00
	[SerializeField]
	private string content
	{
		get
		{
			return Path.Combine(Application.persistentDataPath, "Saves");
		}
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0002EA11 File Offset: 0x0002CC11
	public void OnPointerClick(PointerEventData eventData)
	{
		GUIUtility.systemCopyBuffer = this.content;
	}
}
