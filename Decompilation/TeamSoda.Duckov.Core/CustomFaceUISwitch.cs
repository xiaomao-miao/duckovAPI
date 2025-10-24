using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200003C RID: 60
public class CustomFaceUISwitch : MonoBehaviour
{
	// Token: 0x06000164 RID: 356 RVA: 0x00006F8A File Offset: 0x0000518A
	private void Awake()
	{
		this.leftButton.onClick.AddListener(new UnityAction(this.OnLeftClick));
		this.rightButton.onClick.AddListener(new UnityAction(this.OnRightClick));
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00006FC4 File Offset: 0x000051C4
	public void Init(CustomFaceUI _master, CustomFacePartTypes partType, string title)
	{
		this.master = _master;
		this.type = partType;
		this.titleText.Key = title;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00006FE0 File Offset: 0x000051E0
	private void OnLeftClick()
	{
		this.Switch(-1);
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00006FE9 File Offset: 0x000051E9
	private void OnRightClick()
	{
		this.Switch(1);
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00006FF2 File Offset: 0x000051F2
	public void SetName(string name)
	{
		this.nameText.text = name;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00007000 File Offset: 0x00005200
	private void Switch(int direction)
	{
		string text = this.master.SwitchPart(this.type, direction);
		this.nameText.text = text;
	}

	// Token: 0x040000FA RID: 250
	public TextLocalizor titleText;

	// Token: 0x040000FB RID: 251
	public TextMeshProUGUI nameText;

	// Token: 0x040000FC RID: 252
	public Button leftButton;

	// Token: 0x040000FD RID: 253
	public Button rightButton;

	// Token: 0x040000FE RID: 254
	public CustomFaceUI master;

	// Token: 0x040000FF RID: 255
	public CustomFacePartTypes type;
}
