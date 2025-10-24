using System;
using Duckov.UI.Animations;
using Duckov.UI.SavesRestore;
using Saves;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000168 RID: 360
public class SavesBackupRestoreInvoker : MonoBehaviour
{
	// Token: 0x06000AE8 RID: 2792 RVA: 0x0002EC18 File Offset: 0x0002CE18
	private void Awake()
	{
		this.mainButton.onClick.AddListener(new UnityAction(this.OnMainButtonClicked));
		this.buttonSlot1.onClick.AddListener(delegate()
		{
			this.OnButtonClicked(1);
		});
		this.buttonSlot2.onClick.AddListener(delegate()
		{
			this.OnButtonClicked(2);
		});
		this.buttonSlot3.onClick.AddListener(delegate()
		{
			this.OnButtonClicked(3);
		});
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0002EC95 File Offset: 0x0002CE95
	private void OnMainButtonClicked()
	{
		this.menuFadeGroup.Toggle();
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0002ECA2 File Offset: 0x0002CEA2
	private void OnButtonClicked(int index)
	{
		this.menuFadeGroup.Hide();
		SavesSystem.SetFile(index);
		this.restorePanel.Open(index);
	}

	// Token: 0x04000963 RID: 2403
	[SerializeField]
	private Button mainButton;

	// Token: 0x04000964 RID: 2404
	[SerializeField]
	private FadeGroup menuFadeGroup;

	// Token: 0x04000965 RID: 2405
	[SerializeField]
	private Button buttonSlot1;

	// Token: 0x04000966 RID: 2406
	[SerializeField]
	private Button buttonSlot2;

	// Token: 0x04000967 RID: 2407
	[SerializeField]
	private Button buttonSlot3;

	// Token: 0x04000968 RID: 2408
	[SerializeField]
	private SavesBackupRestorePanel restorePanel;
}
