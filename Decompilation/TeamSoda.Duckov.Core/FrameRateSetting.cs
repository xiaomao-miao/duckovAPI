using System;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class FrameRateSetting : OptionsProviderBase
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00038533 File Offset: 0x00036733
	public override string Key
	{
		get
		{
			return "FrameRateSetting";
		}
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0003853A File Offset: 0x0003673A
	public override string[] GetOptions()
	{
		return new string[]
		{
			"60",
			"90",
			"120",
			"144",
			"240",
			this.optionUnlimitKey.ToPlainText()
		};
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00038578 File Offset: 0x00036778
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 1))
		{
		case 0:
			return "60";
		case 1:
			return "90";
		case 2:
			return "120";
		case 3:
			return "144";
		case 4:
			return "240";
		case 5:
			return this.optionUnlimitKey.ToPlainText();
		default:
			return "60";
		}
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x000385E4 File Offset: 0x000367E4
	public override void Set(int index)
	{
		switch (index)
		{
		case 0:
			Application.targetFrameRate = 60;
			break;
		case 1:
			Application.targetFrameRate = 90;
			break;
		case 2:
			Application.targetFrameRate = 120;
			break;
		case 3:
			Application.targetFrameRate = 144;
			break;
		case 4:
			Application.targetFrameRate = 240;
			break;
		case 5:
			Application.targetFrameRate = 500;
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0003865A File Offset: 0x0003685A
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x0003866D File Offset: 0x0003686D
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00038680 File Offset: 0x00036880
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000B9B RID: 2971
	[LocalizationKey("Default")]
	public string optionUnlimitKey = "FrameRateUnlimit";
}
