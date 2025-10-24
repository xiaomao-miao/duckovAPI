using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D0 RID: 464
public class EdgeLightSettings : OptionsProviderBase
{
	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00038432 File Offset: 0x00036632
	public override string Key
	{
		get
		{
			return "EdgeLightSetting";
		}
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x00038439 File Offset: 0x00036639
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x00038460 File Offset: 0x00036660
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.offKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.onKey.ToPlainText();
		}
		return this.onKey.ToPlainText();
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x000384A6 File Offset: 0x000366A6
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				EdgeLightEntry.SetEnabled(true);
			}
		}
		else
		{
			EdgeLightEntry.SetEnabled(false);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x000384CB File Offset: 0x000366CB
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x000384DE File Offset: 0x000366DE
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x000384F4 File Offset: 0x000366F4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000B99 RID: 2969
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000B9A RID: 2970
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
