using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class CustomFaceLoadSaveButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x0600011D RID: 285 RVA: 0x000059C8 File Offset: 0x00003BC8
	private void Awake()
	{
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000059CA File Offset: 0x00003BCA
	public void Init(CustomFaceSaveLoad _master, int _index, string name)
	{
		this.text.text = name;
		this.master = _master;
		this.index = _index;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000059E6 File Offset: 0x00003BE6
	public void SetSelection(bool selected)
	{
		this.image.color = (selected ? this.selectedColor : this.unselectedColor);
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00005A04 File Offset: 0x00003C04
	public void OnPointerClick(PointerEventData eventData)
	{
		this.master.SetSlotAndLoad(this.index);
	}

	// Token: 0x040000AC RID: 172
	public Color selectedColor;

	// Token: 0x040000AD RID: 173
	public Color unselectedColor;

	// Token: 0x040000AE RID: 174
	public Image image;

	// Token: 0x040000AF RID: 175
	public CustomFaceSaveLoad master;

	// Token: 0x040000B0 RID: 176
	public int index;

	// Token: 0x040000B1 RID: 177
	public TextMeshProUGUI text;
}
