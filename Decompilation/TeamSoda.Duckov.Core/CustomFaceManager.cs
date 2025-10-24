using System;
using Duckov.Utilities;
using Saves;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class CustomFaceManager : MonoBehaviour
{
	// Token: 0x06000872 RID: 2162 RVA: 0x0002593D File Offset: 0x00023B3D
	public void SaveSettingToMainCharacter(CustomFaceSettingData setting)
	{
		this.SaveSetting("CustomFace_MainCharacter", setting);
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0002594B File Offset: 0x00023B4B
	public CustomFaceSettingData LoadMainCharacterSetting()
	{
		return this.LoadSetting("CustomFace_MainCharacter");
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00025958 File Offset: 0x00023B58
	private void SaveSetting(string key, CustomFaceSettingData setting)
	{
		setting.savedSetting = true;
		SavesSystem.Save<CustomFaceSettingData>(key, setting);
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0002596C File Offset: 0x00023B6C
	private CustomFaceSettingData LoadSetting(string key)
	{
		CustomFaceSettingData customFaceSettingData = SavesSystem.Load<CustomFaceSettingData>(key);
		if (!customFaceSettingData.savedSetting)
		{
			customFaceSettingData = GameplayDataSettings.CustomFaceData.DefaultPreset.settings;
		}
		return customFaceSettingData;
	}
}
