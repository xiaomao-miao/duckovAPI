using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class CustomFacePreset : ScriptableObject
{
	// Token: 0x06000115 RID: 277 RVA: 0x00005877 File Offset: 0x00003A77
	private void CopyJsonToClipBoard()
	{
		GUIUtility.systemCopyBuffer = this.DataToJson();
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00005884 File Offset: 0x00003A84
	private void PastyFromJsonData()
	{
		CustomFaceSettingData customFaceSettingData;
		if (CustomFaceSettingData.JsonToData(GUIUtility.systemCopyBuffer, out customFaceSettingData))
		{
			this.settings = customFaceSettingData;
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000058A6 File Offset: 0x00003AA6
	private string DataToJson()
	{
		return Regex.Replace(JsonUtility.ToJson(this.settings, false), "\\d+\\.\\d+", (Match match) => float.Parse(match.Value).ToString("0.###"));
	}

	// Token: 0x040000A9 RID: 169
	public CustomFaceSettingData settings;
}
