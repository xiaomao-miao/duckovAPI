using System;
using Duckov.Options;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class ResolutionOptions : OptionsProviderBase
{
	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00037BCA File Offset: 0x00035DCA
	public override string Key
	{
		get
		{
			return ResolutionSetter.Key_Resolution;
		}
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x00037BD4 File Offset: 0x00035DD4
	public override string GetCurrentOption()
	{
		return OptionsManager.Load<DuckovResolution>(this.Key, new DuckovResolution(Screen.resolutions[Screen.resolutions.Length - 1])).ToString();
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x00037C14 File Offset: 0x00035E14
	public override string[] GetOptions()
	{
		this.avaliableResolutions = ResolutionSetter.GetResolutions();
		string[] array = new string[this.avaliableResolutions.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.avaliableResolutions[i].ToString();
		}
		return array;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x00037C64 File Offset: 0x00035E64
	public override void Set(int index)
	{
		if (this.avaliableResolutions == null || index >= this.avaliableResolutions.Length)
		{
			Debug.Log("设置分辨率流程错误");
			index = 0;
		}
		DuckovResolution obj = this.avaliableResolutions[index];
		OptionsManager.Save<DuckovResolution>(this.Key, obj);
	}

	// Token: 0x04000B88 RID: 2952
	private DuckovResolution[] avaliableResolutions;
}
