using System;
using System.Collections.Generic;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000039 RID: 57
public class CustomFaceUIColorPicker : MonoBehaviour
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000151 RID: 337 RVA: 0x00006C31 File Offset: 0x00004E31
	public IEnumerable<Color> colors
	{
		get
		{
			foreach (GimpPalette.Entry entry in this.gimpPalette.entries)
			{
				yield return entry.color;
			}
			GimpPalette.Entry[] array = null;
			yield break;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000152 RID: 338 RVA: 0x00006C41 File Offset: 0x00004E41
	public Color CurrentColor
	{
		get
		{
			return this.currentColor;
		}
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000153 RID: 339 RVA: 0x00006C4C File Offset: 0x00004E4C
	// (remove) Token: 0x06000154 RID: 340 RVA: 0x00006C84 File Offset: 0x00004E84
	public event Action<Color> OnSetColor;

	// Token: 0x06000155 RID: 341 RVA: 0x00006CB9 File Offset: 0x00004EB9
	private void Awake()
	{
		this.pickerToggleBtn.onClick.AddListener(new UnityAction(this.OnPickToggleBtnClicked));
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00006CD7 File Offset: 0x00004ED7
	private void OnPickToggleBtnClicked()
	{
		this.buttonParent.gameObject.SetActive(!this.buttonParent.gameObject.activeSelf);
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00006CFC File Offset: 0x00004EFC
	public void Init(CustomFaceUI _master, string titleKey)
	{
		this.master = _master;
		this.titleText.Key = titleKey;
		if (this.buttons == null)
		{
			this.buttons = new List<CustomFaceUIColorPickerButton>();
		}
		if (!this.created)
		{
			foreach (Color color in this.colors)
			{
				Color color2 = new Color(color.r, color.g, color.b, 1f);
				CustomFaceUIColorPickerButton customFaceUIColorPickerButton = UnityEngine.Object.Instantiate<CustomFaceUIColorPickerButton>(this.singleButton, this.buttonParent);
				customFaceUIColorPickerButton.Init(this, color2);
				customFaceUIColorPickerButton.transform.SetParent(this.buttonParent);
				this.buttons.Add(customFaceUIColorPickerButton);
			}
			this.singleButton.gameObject.SetActive(false);
			this.created = true;
		}
		this.UpdateSelection();
		this.buttonParent.gameObject.SetActive(false);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00006DF8 File Offset: 0x00004FF8
	private void UpdateSelection()
	{
		if (this.buttons == null || this.buttons.Count < 1)
		{
			return;
		}
		foreach (CustomFaceUIColorPickerButton customFaceUIColorPickerButton in this.buttons)
		{
			customFaceUIColorPickerButton.SetSelection(customFaceUIColorPickerButton.Color.CompareRGB(this.currentColor));
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00006E70 File Offset: 0x00005070
	public void SetColor(Color _color)
	{
		this.currentColor = _color;
		this.master.SetDirty();
		this.UpdateSelection();
		this.currentColor.a = 1f;
		this.colorDisplay.color = this.currentColor;
		this.buttonParent.gameObject.SetActive(false);
	}

	// Token: 0x040000EB RID: 235
	public CustomFaceUI master;

	// Token: 0x040000EC RID: 236
	public CustomFaceUIColorPickerButton singleButton;

	// Token: 0x040000ED RID: 237
	public Transform buttonParent;

	// Token: 0x040000EE RID: 238
	public TextLocalizor titleText;

	// Token: 0x040000EF RID: 239
	public GimpPalette gimpPalette;

	// Token: 0x040000F0 RID: 240
	public Image colorDisplay;

	// Token: 0x040000F1 RID: 241
	public Button pickerToggleBtn;

	// Token: 0x040000F2 RID: 242
	private List<CustomFaceUIColorPickerButton> buttons;

	// Token: 0x040000F3 RID: 243
	private bool created;

	// Token: 0x040000F4 RID: 244
	private Color currentColor;
}
