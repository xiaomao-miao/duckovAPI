using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001C7 RID: 455
public static class ScrollWheelBehaviour
{
	// Token: 0x06000D85 RID: 3461 RVA: 0x00037AD6 File Offset: 0x00035CD6
	public static string GetDisplayName(ScrollWheelBehaviour.Behaviour behaviour)
	{
		return string.Format("ScrollWheelBehaviour_{0}", behaviour).ToPlainText();
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00037AED File Offset: 0x00035CED
	// (set) Token: 0x06000D87 RID: 3463 RVA: 0x00037AFA File Offset: 0x00035CFA
	public static ScrollWheelBehaviour.Behaviour CurrentBehaviour
	{
		get
		{
			return OptionsManager.Load<ScrollWheelBehaviour.Behaviour>("ScrollWheelBehaviour", ScrollWheelBehaviour.Behaviour.AmmoAndInteract);
		}
		set
		{
			OptionsManager.Save<ScrollWheelBehaviour.Behaviour>("ScrollWheelBehaviour", value);
		}
	}

	// Token: 0x020004D6 RID: 1238
	public enum Behaviour
	{
		// Token: 0x04001CF9 RID: 7417
		AmmoAndInteract,
		// Token: 0x04001CFA RID: 7418
		Weapon
	}
}
