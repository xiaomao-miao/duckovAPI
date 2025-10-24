using System;
using Duckov.Options;
using SodaCraft.Localizations;
using SymmetryBreakStudio.TastyGrassShader;
using UnityEngine.Rendering.Universal;

// Token: 0x020001D3 RID: 467
public class GrassOptions : OptionsProviderBase
{
	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0003892C File Offset: 0x00036B2C
	public override string Key
	{
		get
		{
			return "GrassSettings";
		}
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x00038933 File Offset: 0x00036B33
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x00038958 File Offset: 0x00036B58
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.offKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		return this.onKey.ToPlainText();
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0003899E File Offset: 0x00036B9E
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x000389B1 File Offset: 0x00036BB1
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x000389C4 File Offset: 0x00036BC4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x000389E8 File Offset: 0x00036BE8
	public override void Set(int index)
	{
		ScriptableRendererFeature scriptableRendererFeature = this.rendererData.rendererFeatures.Find((ScriptableRendererFeature e) => e is TastyGrassShaderGlobalSettings);
		if (scriptableRendererFeature != null)
		{
			TastyGrassShaderGlobalSettings tastyGrassShaderGlobalSettings = scriptableRendererFeature as TastyGrassShaderGlobalSettings;
			if (index != 0)
			{
				if (index == 1)
				{
					tastyGrassShaderGlobalSettings.SetActive(true);
					TgsManager.Enable = true;
				}
			}
			else
			{
				tastyGrassShaderGlobalSettings.SetActive(false);
				TgsManager.Enable = false;
			}
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x04000BA2 RID: 2978
	[LocalizationKey("Default")]
	public string offKey = "GrassOptions_Off";

	// Token: 0x04000BA3 RID: 2979
	[LocalizationKey("Default")]
	public string onKey = "GrassOptions_On";

	// Token: 0x04000BA4 RID: 2980
	public UniversalRendererData rendererData;
}
