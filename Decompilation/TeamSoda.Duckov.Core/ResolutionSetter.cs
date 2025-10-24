using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Options;
using Sirenix.Utilities;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class ResolutionSetter : MonoBehaviour
{
	// Token: 0x06000D9C RID: 3484 RVA: 0x00037CB4 File Offset: 0x00035EB4
	private void Test()
	{
		this.debugDisplayRes = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
		this.debugmMaxRes = new Vector2Int(ResolutionSetter.MaxResolution.width, ResolutionSetter.MaxResolution.height);
		this.debugScreenRes = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
		this.testRes = ResolutionSetter.GetResolutions();
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00037D30 File Offset: 0x00035F30
	public static DuckovResolution MaxResolution
	{
		get
		{
			Resolution[] resolutions = Screen.resolutions;
			resolutions.Sort(delegate(Resolution A, Resolution B)
			{
				if (A.height > B.height)
				{
					return -1;
				}
				if (A.height < B.height)
				{
					return 1;
				}
				if (A.width > B.width)
				{
					return -1;
				}
				if (A.width < B.width)
				{
					return 1;
				}
				return 0;
			});
			Resolution res = default(Resolution);
			res.width = Screen.currentResolution.width;
			res.height = Screen.currentResolution.height;
			Resolution res2 = Screen.resolutions[resolutions.Length - 1];
			DuckovResolution duckovResolution;
			if (res.width > res2.width)
			{
				duckovResolution = new DuckovResolution(res);
			}
			else
			{
				duckovResolution = new DuckovResolution(res2);
			}
			if ((float)duckovResolution.width / (float)duckovResolution.height < 1.4f)
			{
				duckovResolution.width = Mathf.RoundToInt((float)(duckovResolution.height * 16 / 9));
			}
			return duckovResolution;
		}
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x00037DFC File Offset: 0x00035FFC
	public static Resolution GetResByHeight(int height, DuckovResolution maxRes)
	{
		return new Resolution
		{
			height = height,
			width = (int)((float)maxRes.width * (float)height / (float)maxRes.height)
		};
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x00037E34 File Offset: 0x00036034
	public static DuckovResolution[] GetResolutions()
	{
		DuckovResolution maxResolution = ResolutionSetter.MaxResolution;
		List<Resolution> list = Screen.resolutions.ToList<Resolution>();
		list.Add(ResolutionSetter.GetResByHeight(1080, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(900, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(720, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(540, maxResolution));
		List<DuckovResolution> list2 = new List<DuckovResolution>();
		bool flag = OptionsManager.Load<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.screenModes.Window) != ResolutionSetter.screenModes.Window;
		foreach (Resolution res in list)
		{
			DuckovResolution duckovResolution = new DuckovResolution(res);
			if (!list2.Contains(duckovResolution) && (float)duckovResolution.width / (float)duckovResolution.height >= 1.4f && (!flag || duckovResolution.CheckRotioFit(duckovResolution, maxResolution)))
			{
				list2.Add(duckovResolution);
			}
		}
		list2.Sort(delegate(DuckovResolution A, DuckovResolution B)
		{
			if (A.height > B.height)
			{
				return -1;
			}
			if (A.height < B.height)
			{
				return 1;
			}
			if (A.width > B.width)
			{
				return -1;
			}
			if (A.width < B.width)
			{
				return 1;
			}
			return 0;
		});
		return list2.ToArray();
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x00037F54 File Offset: 0x00036154
	private void Update()
	{
		this.UpdateFullScreenCheck();
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x00037F5C File Offset: 0x0003615C
	private void UpdateFullScreenCheck()
	{
		ResolutionSetter.fullScreenChangeCheckCoolTimer -= Time.unscaledDeltaTime;
		if (ResolutionSetter.fullScreenChangeCheckCoolTimer > 0f)
		{
			return;
		}
		if (ResolutionSetter.currentFullScreen != (Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen))
		{
			ResolutionSetter.currentFullScreen = !ResolutionSetter.currentFullScreen;
			OptionsManager.Save<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.currentFullScreen ? ResolutionSetter.screenModes.Borderless : ResolutionSetter.screenModes.Window);
			ResolutionSetter.fullScreenChangeCheckCoolTimer = ResolutionSetter.fullScreenChangeCheckCoolTime;
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00037FCC File Offset: 0x000361CC
	public static void UpdateResolutionAndScreenMode()
	{
		ResolutionSetter.fullScreenChangeCheckCoolTimer = ResolutionSetter.fullScreenChangeCheckCoolTime;
		DuckovResolution duckovResolution = OptionsManager.Load<DuckovResolution>(ResolutionSetter.Key_Resolution, new DuckovResolution(Screen.resolutions[Screen.resolutions.Length - 1]));
		if ((float)duckovResolution.width / (float)duckovResolution.height < 1.3666667f)
		{
			duckovResolution.width = Mathf.RoundToInt((float)(duckovResolution.height * 16 / 9));
		}
		ResolutionSetter.screenModes screenModes = OptionsManager.Load<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.screenModes.Borderless);
		ResolutionSetter.currentFullScreen = (screenModes == ResolutionSetter.screenModes.Borderless);
		Screen.SetResolution(duckovResolution.width, duckovResolution.height, ResolutionSetter.ScreenModeToFullScreenMode(screenModes));
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00038061 File Offset: 0x00036261
	private static FullScreenMode ScreenModeToFullScreenMode(ResolutionSetter.screenModes screenMode)
	{
		if (screenMode == ResolutionSetter.screenModes.Borderless)
		{
			return FullScreenMode.FullScreenWindow;
		}
		if (screenMode != ResolutionSetter.screenModes.Window)
		{
			return FullScreenMode.ExclusiveFullScreen;
		}
		return FullScreenMode.Windowed;
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00038074 File Offset: 0x00036274
	public static string[] GetScreenModes()
	{
		return new string[]
		{
			("Option_ScreenMode_" + ResolutionSetter.screenModes.Borderless.ToString()).ToPlainText(),
			("Option_ScreenMode_" + ResolutionSetter.screenModes.Window.ToString()).ToPlainText()
		};
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x000380C9 File Offset: 0x000362C9
	public static string ScreenModeToName(ResolutionSetter.screenModes mode)
	{
		return ("Option_ScreenMode_" + mode.ToString()).ToPlainText();
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x000380E7 File Offset: 0x000362E7
	private void Awake()
	{
		ResolutionSetter.UpdateResolutionAndScreenMode();
		OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x000380FF File Offset: 0x000362FF
	private void OnDestroy()
	{
		OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x00038112 File Offset: 0x00036312
	private void OnOptionsChanged(string key)
	{
		if (key == ResolutionSetter.Key_Resolution || key == ResolutionSetter.Key_ScreenMode)
		{
			ResolutionSetter.UpdateResolutionAndScreenMode();
		}
	}

	// Token: 0x04000B89 RID: 2953
	public static string Key_Resolution = "Resolution";

	// Token: 0x04000B8A RID: 2954
	public static string Key_ScreenMode = "ScreenMode";

	// Token: 0x04000B8B RID: 2955
	public static bool currentFullScreen = false;

	// Token: 0x04000B8C RID: 2956
	private static float fullScreenChangeCheckCoolTimer = 1f;

	// Token: 0x04000B8D RID: 2957
	private static float fullScreenChangeCheckCoolTime = 1f;

	// Token: 0x04000B8E RID: 2958
	public Vector2Int debugDisplayRes = new Vector2Int(0, 0);

	// Token: 0x04000B8F RID: 2959
	public Vector2Int debugScreenRes = new Vector2Int(0, 0);

	// Token: 0x04000B90 RID: 2960
	public Vector2Int debugmMaxRes = new Vector2Int(0, 0);

	// Token: 0x04000B91 RID: 2961
	public DuckovResolution[] testRes;

	// Token: 0x020004D7 RID: 1239
	public enum screenModes
	{
		// Token: 0x04001CFC RID: 7420
		Borderless,
		// Token: 0x04001CFD RID: 7421
		Window
	}
}
