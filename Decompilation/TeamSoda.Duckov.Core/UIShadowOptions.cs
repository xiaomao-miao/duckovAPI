using System;
using Duckov.Options;
using LeTai.TrueShadow;
using SodaCraft.Localizations;

// Token: 0x020001DC RID: 476
public class UIShadowOptions : OptionsProviderBase
{
	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0003947F File Offset: 0x0003767F
	public override string Key
	{
		get
		{
			return "UIShadow";
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00039486 File Offset: 0x00037686
	// (set) Token: 0x06000E26 RID: 3622 RVA: 0x00039493 File Offset: 0x00037693
	public static bool Active
	{
		get
		{
			return OptionsManager.Load<bool>("UIShadow", true);
		}
		set
		{
			OptionsManager.Save<bool>("UIShadow", value);
		}
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x000394A0 File Offset: 0x000376A0
	public static void Apply()
	{
		TrueShadow.ExternalActive = UIShadowOptions.Active;
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000E28 RID: 3624 RVA: 0x000394AC File Offset: 0x000376AC
	public string ActiveText
	{
		get
		{
			return "Options_On".ToPlainText();
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000E29 RID: 3625 RVA: 0x000394B8 File Offset: 0x000376B8
	public string InactiveText
	{
		get
		{
			return "Options_Off".ToPlainText();
		}
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x000394C4 File Offset: 0x000376C4
	public override string GetCurrentOption()
	{
		if (UIShadowOptions.Active)
		{
			return this.ActiveText;
		}
		return this.InactiveText;
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x000394DA File Offset: 0x000376DA
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.InactiveText,
			this.ActiveText
		};
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x000394F4 File Offset: 0x000376F4
	public override void Set(int index)
	{
		if (index <= 0)
		{
			UIShadowOptions.Active = false;
			return;
		}
		UIShadowOptions.Active = true;
	}

	// Token: 0x04000BBD RID: 3005
	private const string key = "UIShadow";
}
