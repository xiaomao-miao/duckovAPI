using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001DB RID: 475
public class SunFogSettings : OptionsProviderBase
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0003937E File Offset: 0x0003757E
	public override string Key
	{
		get
		{
			return "SunFogSetting";
		}
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00039385 File Offset: 0x00037585
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000393AC File Offset: 0x000375AC
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

	// Token: 0x06000E1F RID: 3615 RVA: 0x000393F2 File Offset: 0x000375F2
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				SunFogEntry.SetEnabled(true);
			}
		}
		else
		{
			SunFogEntry.SetEnabled(false);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x00039417 File Offset: 0x00037617
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0003942A File Offset: 0x0003762A
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x00039440 File Offset: 0x00037640
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BBB RID: 3003
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000BBC RID: 3004
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
