using System;

// Token: 0x0200016B RID: 363
public class PauseMenu : UIPanel
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0002EE07 File Offset: 0x0002D007
	public static PauseMenu Instance
	{
		get
		{
			return GameManager.PauseMenu;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0002EE0E File Offset: 0x0002D00E
	public bool Shown
	{
		get
		{
			return !(this.fadeGroup == null) && this.fadeGroup.IsShown;
		}
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0002EE2B File Offset: 0x0002D02B
	public static void Show()
	{
		PauseMenu.Instance.Open(null, true);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0002EE39 File Offset: 0x0002D039
	public static void Hide()
	{
		PauseMenu.Instance.Close();
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0002EE45 File Offset: 0x0002D045
	public static void Toggle()
	{
		if (PauseMenu.Instance.fadeGroup.IsShown)
		{
			PauseMenu.Hide();
			return;
		}
		PauseMenu.Show();
	}
}
