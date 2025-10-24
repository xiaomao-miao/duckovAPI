using System;
using Duckov.Options;
using SodaCraft.Localizations;
using Umbra;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001D8 RID: 472
public class ShadowSetting : OptionsProviderBase
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00038F7B File Offset: 0x0003717B
	public override string Key
	{
		get
		{
			return "ShadowSettings";
		}
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00038F82 File Offset: 0x00037182
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.lowKey.ToPlainText(),
			this.middleKey.ToPlainText(),
			this.highKey.ToPlainText()
		};
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00038FC4 File Offset: 0x000371C4
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 2))
		{
		case 0:
			return this.offKey.ToPlainText();
		case 1:
			return this.lowKey.ToPlainText();
		case 2:
			return this.middleKey.ToPlainText();
		case 3:
			return this.highKey.ToPlainText();
		default:
			return this.highKey.ToPlainText();
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00039034 File Offset: 0x00037234
	private void SetShadow(bool on, int res, float shadowDistance, bool softShadow, bool softShadowDownSample, bool contactShadow, int pointLightCount)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.shadowDistance = (on ? shadowDistance : 0f);
			universalRenderPipelineAsset.mainLightShadowmapResolution = res;
			universalRenderPipelineAsset.additionalLightsShadowmapResolution = res;
			universalRenderPipelineAsset.maxAdditionalLightsCount = pointLightCount;
		}
		if (this.umbraProfile)
		{
			this.umbraProfile.shadowSource = (softShadow ? ShadowSource.UmbraShadows : ShadowSource.UnityShadows);
			this.umbraProfile.downsample = softShadowDownSample;
			this.umbraProfile.contactShadows = contactShadow;
		}
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x000390B8 File Offset: 0x000372B8
	public override void Set(int index)
	{
		switch (index)
		{
		case 0:
			this.SetShadow(false, 512, 0f, false, false, false, 0);
			break;
		case 1:
			this.SetShadow(true, 1024, 70f, false, false, false, 0);
			break;
		case 2:
			this.SetShadow(true, 2048, 80f, true, true, true, 5);
			break;
		case 3:
			this.SetShadow(true, 4096, 90f, true, false, true, 6);
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x00039143 File Offset: 0x00037343
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x00039156 File Offset: 0x00037356
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0003916C File Offset: 0x0003736C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 2);
		this.Set(index);
	}

	// Token: 0x04000BB1 RID: 2993
	public UmbraProfile umbraProfile;

	// Token: 0x04000BB2 RID: 2994
	public float onDistance = 100f;

	// Token: 0x04000BB3 RID: 2995
	[LocalizationKey("Default")]
	public string highKey = "Options_High";

	// Token: 0x04000BB4 RID: 2996
	[LocalizationKey("Default")]
	public string middleKey = "Options_Middle";

	// Token: 0x04000BB5 RID: 2997
	[LocalizationKey("Default")]
	public string lowKey = "Options_Low";

	// Token: 0x04000BB6 RID: 2998
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
