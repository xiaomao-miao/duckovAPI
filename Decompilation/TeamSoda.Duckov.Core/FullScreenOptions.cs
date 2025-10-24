using System;
using Duckov.Options;

// Token: 0x020001CA RID: 458
public class FullScreenOptions : OptionsProviderBase
{
	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00037B93 File Offset: 0x00035D93
	public override string Key
	{
		get
		{
			return ResolutionSetter.Key_ScreenMode;
		}
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x00037B9A File Offset: 0x00035D9A
	public override string GetCurrentOption()
	{
		return ResolutionSetter.ScreenModeToName(OptionsManager.Load<ResolutionSetter.screenModes>(this.Key, ResolutionSetter.screenModes.Borderless));
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x00037BAD File Offset: 0x00035DAD
	public override string[] GetOptions()
	{
		return ResolutionSetter.GetScreenModes();
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x00037BB4 File Offset: 0x00035DB4
	public override void Set(int index)
	{
		OptionsManager.Save<ResolutionSetter.screenModes>(this.Key, (ResolutionSetter.screenModes)index);
	}
}
