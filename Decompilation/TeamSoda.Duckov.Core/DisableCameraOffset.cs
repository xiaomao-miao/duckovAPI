using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001CE RID: 462
public class DisableCameraOffset : OptionsProviderBase
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0003825D File Offset: 0x0003645D
	public override string Key
	{
		get
		{
			return "DisableCameraOffset";
		}
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00038264 File Offset: 0x00036464
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.onKey.ToPlainText(),
			this.offKey.ToPlainText()
		};
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x00038288 File Offset: 0x00036488
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.onKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		return this.offKey.ToPlainText();
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x000382CE File Offset: 0x000364CE
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				DisableCameraOffset.disableCameraOffset = false;
			}
		}
		else
		{
			DisableCameraOffset.disableCameraOffset = true;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x000382F3 File Offset: 0x000364F3
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x00038306 File Offset: 0x00036506
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0003831C File Offset: 0x0003651C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000B94 RID: 2964
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000B95 RID: 2965
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";

	// Token: 0x04000B96 RID: 2966
	public static bool disableCameraOffset;
}
