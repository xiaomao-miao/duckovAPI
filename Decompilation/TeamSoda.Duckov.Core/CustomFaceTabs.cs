using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000037 RID: 55
public class CustomFaceTabs : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000138 RID: 312 RVA: 0x00005D5C File Offset: 0x00003F5C
	public void OnPointerClick(PointerEventData eventData)
	{
		this.master.SelectTab(this);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00005D6A File Offset: 0x00003F6A
	public void SetSelectVisual(bool selected)
	{
		this.background.color = (selected ? this.selectedColor : this.normalColor);
	}

	// Token: 0x040000BC RID: 188
	public CustomFaceUI master;

	// Token: 0x040000BD RID: 189
	public List<GameObject> panels;

	// Token: 0x040000BE RID: 190
	public ProceduralImage background;

	// Token: 0x040000BF RID: 191
	public Color normalColor;

	// Token: 0x040000C0 RID: 192
	public Color selectedColor;
}
