using System;
using Duckov.MiniGames;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001D2 RID: 466
public class FsrSetting : OptionsProviderBase
{
	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000386B4 File Offset: 0x000368B4
	public override string Key
	{
		get
		{
			return "FsrSetting";
		}
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x000386BC File Offset: 0x000368BC
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.qualityKey.ToPlainText(),
			this.balancedKey.ToPlainText(),
			this.performanceKey.ToPlainText(),
			this.ultraPerformanceKey.ToPlainText()
		};
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x00038718 File Offset: 0x00036918
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 0))
		{
		case 0:
			return this.offKey.ToPlainText();
		case 1:
			return this.qualityKey.ToPlainText();
		case 2:
			return this.balancedKey.ToPlainText();
		case 3:
			return this.performanceKey.ToPlainText();
		case 4:
			return this.ultraPerformanceKey.ToPlainText();
		default:
			return this.offKey.ToPlainText();
		}
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x00038798 File Offset: 0x00036998
	public override void Set(int index)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
		int num = index;
		if (FsrSetting.gameOn)
		{
			num = 0;
		}
		switch (num)
		{
		case 0:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 1f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.Linear;
			}
			break;
		case 1:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.67f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		case 2:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.58f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		case 3:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.5f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		case 4:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.33f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x00038878 File Offset: 0x00036A78
	private void Awake()
	{
		this.RefreshOnLevelInited();
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
		GamingConsole.OnGamingConsoleInteractChanged += this.OnGamingConsoleInteractChanged;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x000388A2 File Offset: 0x00036AA2
	private void OnGamingConsoleInteractChanged(bool _gameOn)
	{
		FsrSetting.gameOn = _gameOn;
		this.SyncSetting();
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x000388B0 File Offset: 0x00036AB0
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x000388C4 File Offset: 0x00036AC4
	private void SyncSetting()
	{
		int index = OptionsManager.Load<int>(this.Key, 0);
		this.Set(index);
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x000388E5 File Offset: 0x00036AE5
	private void RefreshOnLevelInited()
	{
		this.SyncSetting();
	}

	// Token: 0x04000B9C RID: 2972
	[LocalizationKey("Default")]
	public string offKey = "fsr_Off";

	// Token: 0x04000B9D RID: 2973
	[LocalizationKey("Default")]
	public string qualityKey = "fsr_Quality";

	// Token: 0x04000B9E RID: 2974
	[LocalizationKey("Default")]
	public string balancedKey = "fsr_Balanced";

	// Token: 0x04000B9F RID: 2975
	[LocalizationKey("Default")]
	public string performanceKey = "fsr_Performance";

	// Token: 0x04000BA0 RID: 2976
	[LocalizationKey("Default")]
	public string ultraPerformanceKey = "fsr_UltraPerformance";

	// Token: 0x04000BA1 RID: 2977
	private static bool gameOn;
}
