using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D6 RID: 470
public class MoveDirectionOptions : OptionsProviderBase
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x00038D73 File Offset: 0x00036F73
	public override string Key
	{
		get
		{
			return "MoveDirModeSettings";
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00038D7A File Offset: 0x00036F7A
	public static bool MoveViaCharacterDirection
	{
		get
		{
			return MoveDirectionOptions.moveViaCharacterDirection;
		}
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x00038D81 File Offset: 0x00036F81
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.cameraModeKey.ToPlainText(),
			this.aimModeKey.ToPlainText()
		};
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00038DA8 File Offset: 0x00036FA8
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 0);
		if (num == 0)
		{
			return this.cameraModeKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.cameraModeKey.ToPlainText();
		}
		return this.aimModeKey.ToPlainText();
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00038DEE File Offset: 0x00036FEE
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				MoveDirectionOptions.moveViaCharacterDirection = true;
			}
		}
		else
		{
			MoveDirectionOptions.moveViaCharacterDirection = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00038E13 File Offset: 0x00037013
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x00038E26 File Offset: 0x00037026
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00038E3C File Offset: 0x0003703C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 0);
		this.Set(index);
	}

	// Token: 0x04000BAC RID: 2988
	[LocalizationKey("Default")]
	public string cameraModeKey = "MoveDirectionMode_Camera";

	// Token: 0x04000BAD RID: 2989
	[LocalizationKey("Default")]
	public string aimModeKey = "MoveDirectionMode_Aim";

	// Token: 0x04000BAE RID: 2990
	private static bool moveViaCharacterDirection;
}
