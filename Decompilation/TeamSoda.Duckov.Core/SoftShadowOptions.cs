using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D9 RID: 473
public class SoftShadowOptions : OptionsProviderBase
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x000391CC File Offset: 0x000373CC
	public override string Key
	{
		get
		{
			return "SoftShadowSettings";
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x000391D3 File Offset: 0x000373D3
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x000391F8 File Offset: 0x000373F8
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.offKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		return this.onKey.ToPlainText();
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0003923E File Offset: 0x0003743E
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x00039251 File Offset: 0x00037451
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x00039264 File Offset: 0x00037464
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x00039285 File Offset: 0x00037485
	public override void Set(int index)
	{
	}

	// Token: 0x04000BB7 RID: 2999
	[LocalizationKey("Default")]
	public string offKey = "SoftShadowOptions_Off";

	// Token: 0x04000BB8 RID: 3000
	[LocalizationKey("Default")]
	public string onKey = "SoftShadowOptions_On";
}
