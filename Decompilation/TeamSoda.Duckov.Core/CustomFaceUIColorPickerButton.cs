using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200003A RID: 58
public class CustomFaceUIColorPickerButton : MonoBehaviour
{
	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600015B RID: 347 RVA: 0x00006ECF File Offset: 0x000050CF
	public Color Color
	{
		get
		{
			return this.color;
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00006ED8 File Offset: 0x000050D8
	public void Init(CustomFaceUIColorPicker _master, Color _color)
	{
		this.master = _master;
		this.color = _color;
		ColorBlock colors = this.button.colors;
		colors.normalColor = this.color;
		colors.highlightedColor = this.color;
		colors.selectedColor = this.color;
		this.button.colors = colors;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00006F32 File Offset: 0x00005132
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00006F50 File Offset: 0x00005150
	private void OnClick()
	{
		this.master.SetColor(this.color);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00006F63 File Offset: 0x00005163
	public void SetSelection(bool selected)
	{
		this.selectedFrameImage.gameObject.SetActive(selected);
	}

	// Token: 0x040000F6 RID: 246
	private CustomFaceUIColorPicker master;

	// Token: 0x040000F7 RID: 247
	private Color color;

	// Token: 0x040000F8 RID: 248
	public Button button;

	// Token: 0x040000F9 RID: 249
	public Image selectedFrameImage;
}
