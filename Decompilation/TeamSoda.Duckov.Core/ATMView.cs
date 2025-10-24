using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class ATMView : View
{
	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000C2A RID: 3114 RVA: 0x000336DE File Offset: 0x000318DE
	public static ATMView Instance
	{
		get
		{
			return View.GetViewInstance<ATMView>();
		}
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x000336E5 File Offset: 0x000318E5
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x000336F0 File Offset: 0x000318F0
	public static void Show()
	{
		ATMView instance = ATMView.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Open(null);
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00033714 File Offset: 0x00031914
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.atmPanel.ShowSelectPanel(true);
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00033733 File Offset: 0x00031933
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x04000A91 RID: 2705
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	private ATMPanel atmPanel;
}
