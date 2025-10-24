using System;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class vSyncSetting : OptionsProviderBase
{
	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0003950F File Offset: 0x0003770F
	public override string Key
	{
		get
		{
			return "GSyncSetting";
		}
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00039516 File Offset: 0x00037716
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.onKey.ToPlainText(),
			this.offKey.ToPlainText()
		};
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0003953C File Offset: 0x0003773C
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			this.SyncObjectActive(true);
			return this.onKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		this.SyncObjectActive(false);
		return this.offKey.ToPlainText();
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x00039590 File Offset: 0x00037790
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				QualitySettings.vSyncCount = 0;
				this.SyncObjectActive(false);
			}
		}
		else
		{
			QualitySettings.vSyncCount = 1;
			this.SyncObjectActive(true);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x000395C3 File Offset: 0x000377C3
	private void SyncObjectActive(bool active)
	{
		if (this.setActiveIfOn)
		{
			this.setActiveIfOn.SetActive(active);
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x000395DE File Offset: 0x000377DE
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x000395F1 File Offset: 0x000377F1
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00039604 File Offset: 0x00037804
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BBE RID: 3006
	[LocalizationKey("Default")]
	public string onKey = "gSync_On";

	// Token: 0x04000BBF RID: 3007
	[LocalizationKey("Default")]
	public string offKey = "gSync_Off";

	// Token: 0x04000BC0 RID: 3008
	public GameObject setActiveIfOn;
}
