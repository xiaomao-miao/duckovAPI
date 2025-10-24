using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D5 RID: 469
public class HurtVisualSettings : OptionsProviderBase
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00038C75 File Offset: 0x00036E75
	public override string Key
	{
		get
		{
			return "HurtVisualSettings";
		}
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x00038C7C File Offset: 0x00036E7C
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00038CA0 File Offset: 0x00036EA0
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

	// Token: 0x06000DED RID: 3565 RVA: 0x00038CE6 File Offset: 0x00036EE6
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				PlayerHurtVisual.hurtVisualOn = true;
			}
		}
		else
		{
			PlayerHurtVisual.hurtVisualOn = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x00038D0B File Offset: 0x00036F0B
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x00038D1E File Offset: 0x00036F1E
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00038D34 File Offset: 0x00036F34
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BAA RID: 2986
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000BAB RID: 2987
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
