using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D7 RID: 471
public class RunInputOptions : OptionsProviderBase
{
	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00038E7B File Offset: 0x0003707B
	public override string Key
	{
		get
		{
			return "RunInputModeSettings";
		}
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00038E82 File Offset: 0x00037082
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.holdModeKey.ToPlainText(),
			this.switchModeKey.ToPlainText()
		};
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x00038EA8 File Offset: 0x000370A8
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 0);
		if (num == 0)
		{
			return this.holdModeKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.holdModeKey.ToPlainText();
		}
		return this.switchModeKey.ToPlainText();
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00038EEE File Offset: 0x000370EE
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				InputManager.useRunInputBuffer = true;
			}
		}
		else
		{
			InputManager.useRunInputBuffer = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00038F13 File Offset: 0x00037113
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x00038F26 File Offset: 0x00037126
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x00038F3C File Offset: 0x0003713C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BAF RID: 2991
	[LocalizationKey("Default")]
	public string holdModeKey = "RunInputMode_Hold";

	// Token: 0x04000BB0 RID: 2992
	[LocalizationKey("Default")]
	public string switchModeKey = "RunInputMode_Switch";
}
