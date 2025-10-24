using System;
using Duckov.Options;
using HorizonBasedAmbientOcclusion.Universal;
using SodaCraft.Localizations;
using UnityEngine.Rendering;

// Token: 0x020001D4 RID: 468
public class HBAOSettings : OptionsProviderBase
{
	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x00038A84 File Offset: 0x00036C84
	public override string Key
	{
		get
		{
			return "HBAOSettings";
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x00038A8B File Offset: 0x00036C8B
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.lowKey.ToPlainText(),
			this.normalKey.ToPlainText(),
			this.highKey.ToPlainText()
		};
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x00038ACC File Offset: 0x00036CCC
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 2))
		{
		case 0:
			return this.offKey.ToPlainText();
		case 1:
			return this.lowKey.ToPlainText();
		case 2:
			return this.normalKey.ToPlainText();
		case 3:
			return this.highKey.ToPlainText();
		default:
			return this.offKey.ToPlainText();
		}
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00038B3C File Offset: 0x00036D3C
	public override void Set(int index)
	{
		HBAO hbao;
		if (this.GlobalVolumePorfile.TryGet<HBAO>(out hbao))
		{
			switch (index)
			{
			case 0:
				hbao.EnableHBAO(false);
				break;
			case 1:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Half, false);
				hbao.bias.value = 64f;
				break;
			case 2:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Half, false);
				hbao.bias.value = 128f;
				break;
			case 3:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Full, false);
				hbao.bias.value = 128f;
				break;
			}
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00038BF8 File Offset: 0x00036DF8
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00038C0B File Offset: 0x00036E0B
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00038C20 File Offset: 0x00036E20
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BA5 RID: 2981
	[LocalizationKey("Default")]
	public string offKey = "HBAOSettings_Off";

	// Token: 0x04000BA6 RID: 2982
	[LocalizationKey("Default")]
	public string lowKey = "HBAOSettings_Low";

	// Token: 0x04000BA7 RID: 2983
	[LocalizationKey("Default")]
	public string normalKey = "HBAOSettings_Normal";

	// Token: 0x04000BA8 RID: 2984
	[LocalizationKey("Default")]
	public string highKey = "HBAOSettings_High";

	// Token: 0x04000BA9 RID: 2985
	public VolumeProfile GlobalVolumePorfile;
}
