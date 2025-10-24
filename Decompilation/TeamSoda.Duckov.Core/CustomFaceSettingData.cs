using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200002F RID: 47
[Serializable]
public struct CustomFaceSettingData
{
	// Token: 0x0600010F RID: 271 RVA: 0x0000545D File Offset: 0x0000365D
	public string DataToJson()
	{
		return Regex.Replace(JsonUtility.ToJson(this, false), "\\d+\\.\\d+", (Match match) => float.Parse(match.Value).ToString("0.###"));
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000549C File Offset: 0x0000369C
	public static bool JsonToData(string jsonData, out CustomFaceSettingData data)
	{
		try
		{
			data = JsonUtility.FromJson<CustomFaceSettingData>(jsonData);
		}
		catch (Exception)
		{
			Debug.LogError("捏脸参数违法");
			data = default(CustomFaceSettingData);
			return false;
		}
		return true;
	}

	// Token: 0x04000087 RID: 135
	[HideInInspector]
	public bool savedSetting;

	// Token: 0x04000088 RID: 136
	public CustomFaceHeadSetting headSetting;

	// Token: 0x04000089 RID: 137
	public int hairID;

	// Token: 0x0400008A RID: 138
	public CustomFacePartInfo hairInfo;

	// Token: 0x0400008B RID: 139
	public int eyeID;

	// Token: 0x0400008C RID: 140
	public CustomFacePartInfo eyeInfo;

	// Token: 0x0400008D RID: 141
	public int eyebrowID;

	// Token: 0x0400008E RID: 142
	public CustomFacePartInfo eyebrowInfo;

	// Token: 0x0400008F RID: 143
	public int mouthID;

	// Token: 0x04000090 RID: 144
	public CustomFacePartInfo mouthInfo;

	// Token: 0x04000091 RID: 145
	public int tailID;

	// Token: 0x04000092 RID: 146
	public CustomFacePartInfo tailInfo;

	// Token: 0x04000093 RID: 147
	public int footID;

	// Token: 0x04000094 RID: 148
	public CustomFacePartInfo footInfo;

	// Token: 0x04000095 RID: 149
	public int wingID;

	// Token: 0x04000096 RID: 150
	public CustomFacePartInfo wingInfo;
}
