using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class CustomFaceInstance : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004AEE File Offset: 0x00002CEE
	private CustomFaceData data
	{
		get
		{
			return GameplayDataSettings.CustomFaceData;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000F7 RID: 247 RVA: 0x00004AF8 File Offset: 0x00002CF8
	// (remove) Token: 0x060000F8 RID: 248 RVA: 0x00004B30 File Offset: 0x00002D30
	public event Action OnLoadFaceData;

	// Token: 0x060000F9 RID: 249 RVA: 0x00004B65 File Offset: 0x00002D65
	private void TestConvert()
	{
		this.convertedData = this.ConvertToSaveData();
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00004B73 File Offset: 0x00002D73
	public void AddRendererToSubVisual(Renderer renderer)
	{
		if (!this.subvisuals)
		{
			return;
		}
		this.subvisuals.AddRenderer(renderer);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00004B90 File Offset: 0x00002D90
	private ValueTuple<CustomFacePartUtility, CustomFacePartUtility, CustomFacePartCollection> GetDataByPartType(CustomFacePartTypes type)
	{
		CustomFacePartUtility item = null;
		CustomFacePartUtility item2 = null;
		CustomFacePartCollection item3 = null;
		switch (type)
		{
		case CustomFacePartTypes.hair:
			item = this.hairPart;
			item3 = this.data.Hairs;
			break;
		case CustomFacePartTypes.eye:
			item = this.eyePart;
			item3 = this.data.Eyes;
			break;
		case CustomFacePartTypes.eyebrow:
			item = this.eyebrowPart;
			item3 = this.data.Eyebrows;
			break;
		case CustomFacePartTypes.mouth:
			item = this.mouthPart;
			item3 = this.data.Mouths;
			break;
		case CustomFacePartTypes.tail:
			item = this.tailPart;
			item3 = this.data.Tails;
			break;
		case CustomFacePartTypes.foot:
			item = this.footLPart;
			item2 = this.footRPart;
			item3 = this.data.Foots;
			break;
		case CustomFacePartTypes.wing:
			item = this.wingLPart;
			item2 = this.wingRPart;
			item3 = this.data.Wings;
			break;
		}
		return new ValueTuple<CustomFacePartUtility, CustomFacePartUtility, CustomFacePartCollection>(item, item2, item3);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00004C74 File Offset: 0x00002E74
	public CustomFacePart SwitchPart(CustomFacePartTypes type, CustomFaceInstance parent, int direction)
	{
		ValueTuple<CustomFacePartUtility, CustomFacePartUtility, CustomFacePartCollection> dataByPartType = this.GetDataByPartType(type);
		CustomFacePartUtility item = dataByPartType.Item1;
		CustomFacePartUtility item2 = dataByPartType.Item2;
		CustomFacePartCollection item3 = dataByPartType.Item3;
		if (item == null || item3 == null || item3.totalCount <= 0)
		{
			return null;
		}
		CustomFacePart nextOrPrevPrefab = item3.GetNextOrPrevPrefab(item.GetCurrentPartID(), direction);
		if (!nextOrPrevPrefab)
		{
			return null;
		}
		CustomFacePart result = this.SwitchOnePartInternal(item, nextOrPrevPrefab, parent, false);
		if (item2 != null)
		{
			this.SwitchOnePartInternal(item2, nextOrPrevPrefab, parent, true);
		}
		return result;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00004CE0 File Offset: 0x00002EE0
	private CustomFacePart ChangePart(CustomFacePartTypes type, CustomFaceInstance parent, int id)
	{
		ValueTuple<CustomFacePartUtility, CustomFacePartUtility, CustomFacePartCollection> dataByPartType = this.GetDataByPartType(type);
		CustomFacePartUtility item = dataByPartType.Item1;
		CustomFacePartUtility item2 = dataByPartType.Item2;
		CustomFacePartCollection item3 = dataByPartType.Item3;
		if (item == null || item3 == null || item3.totalCount <= 0)
		{
			return null;
		}
		CustomFacePart partPrefab = item3.GetPartPrefab(id);
		if (!partPrefab)
		{
			return null;
		}
		CustomFacePart result = this.SwitchOnePartInternal(item, partPrefab, parent, false);
		if (item2 != null)
		{
			this.SwitchOnePartInternal(item2, partPrefab, parent, true);
		}
		return result;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00004D44 File Offset: 0x00002F44
	private CustomFacePart SwitchOnePartInternal(CustomFacePartUtility part, CustomFacePart pfb, CustomFaceInstance parent, bool mirror)
	{
		CustomFacePart customFacePart = this.InstantiatePartFromPrefab(pfb);
		customFacePart.mirror = mirror;
		part.ChangePart(customFacePart);
		part.RefreshThisPart();
		if (parent)
		{
			foreach (Renderer renderer in customFacePart.renderers)
			{
				parent.AddRendererToSubVisual(renderer);
			}
		}
		return customFacePart;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00004DC0 File Offset: 0x00002FC0
	private CustomFacePart InstantiatePartFromPrefab(CustomFacePart pfb)
	{
		if (!pfb)
		{
			return null;
		}
		CustomFacePart result = null;
		if (Application.isPlaying)
		{
			result = UnityEngine.Object.Instantiate<CustomFacePart>(pfb);
		}
		return result;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00004DE8 File Offset: 0x00002FE8
	public void RefreshAll()
	{
		this.UpdateHead();
		this.hairPart.RefreshThisPart();
		this.eyePart.RefreshThisPart();
		this.eyebrowPart.partInfo.heightOffset = this.eyePart.partInfo.height;
		if (this.faceMaskSocket)
		{
			this.faceMaskSocket.localPosition = (this.eyePart.partInfo.height + this.eyePart.partInfo.heightOffset) * Vector3.up;
		}
		this.eyebrowPart.RefreshThisPart();
		this.mouthPart.RefreshThisPart();
		this.tailPart.RefreshThisPart();
		this.footLPart.RefreshThisPart();
		this.footRPart.RefreshThisPart();
		this.wingLPart.RefreshThisPart();
		this.wingRPart.RefreshThisPart();
		this.SetMainColor();
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00004EC7 File Offset: 0x000030C7
	private void OnValidate()
	{
		this.RefreshAll();
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00004ECF File Offset: 0x000030CF
	private void LateUpdate()
	{
		this.UpdateHead();
		this.UpdateHands();
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00004EE0 File Offset: 0x000030E0
	private void UpdateHands()
	{
		CustomFacePart partInstance = this.wingLPart.PartInstance;
		if (!partInstance || !partInstance.centerObject)
		{
			return;
		}
		CustomFacePart partInstance2 = this.wingRPart.PartInstance;
		partInstance.centerObject.transform.position = this.leftHandSocket.transform.position;
		partInstance.centerObject.transform.rotation = this.leftHandSocket.transform.rotation;
		partInstance2.centerObject.transform.position = this.righthandSocket.transform.position;
		partInstance2.centerObject.transform.rotation = this.righthandSocket.transform.rotation;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00004F9C File Offset: 0x0000319C
	private void UpdateHead()
	{
		if (!this.foreheadJoint)
		{
			return;
		}
		this.foreheadJoint.localPosition = Vector3.up * (this.foreheadDefaultHeight + this.headSetting.foreheadHeight);
		this.foreheadJoint.localScale = Vector3.one + Vector3.up * (this.headSetting.foreheadRound - 1f);
		this.headJoint.localScale = Vector3.one * (this.headSetting.headScaleOffset + 1f);
		if (this.hairSocket)
		{
			this.hairSocket.localPosition = this.foreheadJoint.localPosition;
		}
		if (this.helmatSocket)
		{
			this.helmatSocket.localPosition = this.foreheadJoint.localPosition + Vector3.up * this.helmatSocketYoffset;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00005090 File Offset: 0x00003290
	private void SetMainColor()
	{
		if (this.mainBlock == null)
		{
			this.mainBlock = new MaterialPropertyBlock();
		}
		this.mainBlock.SetColor("_Tint", this.headSetting.mainColor);
		foreach (Renderer renderer in this.mainRenderers)
		{
			if (renderer)
			{
				renderer.SetPropertyBlock(this.mainBlock);
			}
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000050F8 File Offset: 0x000032F8
	public CustomFaceSettingData ConvertToSaveData()
	{
		return new CustomFaceSettingData
		{
			headSetting = this.headSetting,
			hairID = this.hairPart.GetCurrentPartID(),
			hairInfo = this.hairPart.partInfo,
			eyeID = this.eyePart.GetCurrentPartID(),
			eyeInfo = this.eyePart.partInfo,
			eyebrowID = this.eyebrowPart.GetCurrentPartID(),
			eyebrowInfo = this.eyebrowPart.partInfo,
			mouthID = this.mouthPart.GetCurrentPartID(),
			mouthInfo = this.mouthPart.partInfo,
			tailID = this.tailPart.GetCurrentPartID(),
			tailInfo = this.tailPart.partInfo,
			footID = this.footLPart.GetCurrentPartID(),
			footInfo = this.footLPart.partInfo,
			wingID = this.wingLPart.GetCurrentPartID(),
			wingInfo = this.wingLPart.partInfo
		};
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00005218 File Offset: 0x00003418
	public void LoadFromData(CustomFaceSettingData saveData)
	{
		this.headSetting = saveData.headSetting;
		this.hairPart.partInfo = saveData.hairInfo;
		this.eyePart.partInfo = saveData.eyeInfo;
		this.eyebrowPart.partInfo = saveData.eyebrowInfo;
		this.mouthPart.partInfo = saveData.mouthInfo;
		this.tailPart.partInfo = saveData.tailInfo;
		this.footLPart.partInfo = saveData.footInfo;
		this.footRPart.partInfo = saveData.footInfo;
		this.wingLPart.partInfo = saveData.wingInfo;
		this.wingRPart.partInfo = saveData.wingInfo;
		this.ChangePart(CustomFacePartTypes.hair, this, saveData.hairID);
		this.ChangePart(CustomFacePartTypes.eye, this, saveData.eyeID);
		this.ChangePart(CustomFacePartTypes.eyebrow, this, saveData.eyebrowID);
		this.ChangePart(CustomFacePartTypes.mouth, this, saveData.mouthID);
		this.ChangePart(CustomFacePartTypes.tail, this, saveData.tailID);
		this.ChangePart(CustomFacePartTypes.foot, this, saveData.footID);
		this.ChangePart(CustomFacePartTypes.wing, this, saveData.wingID);
		this.RefreshAll();
		Action onLoadFaceData = this.OnLoadFaceData;
		if (onLoadFaceData == null)
		{
			return;
		}
		onLoadFaceData();
	}

	// Token: 0x04000066 RID: 102
	public Renderer[] mainRenderers;

	// Token: 0x04000067 RID: 103
	public CustomFaceHeadSetting headSetting;

	// Token: 0x04000068 RID: 104
	public Transform headJoint;

	// Token: 0x04000069 RID: 105
	public Transform foreheadJoint;

	// Token: 0x0400006A RID: 106
	public float foreheadDefaultHeight = 0.19f;

	// Token: 0x0400006B RID: 107
	public Transform helmatSocket;

	// Token: 0x0400006C RID: 108
	public Transform faceMaskSocket;

	// Token: 0x0400006D RID: 109
	public float helmatSocketYoffset = -0.2f;

	// Token: 0x0400006E RID: 110
	public Transform hairSocket;

	// Token: 0x0400006F RID: 111
	[SerializeField]
	public CustomFacePartUtility hairPart;

	// Token: 0x04000070 RID: 112
	[SerializeField]
	public CustomFacePartUtility eyePart;

	// Token: 0x04000071 RID: 113
	[SerializeField]
	public CustomFacePartUtility eyebrowPart;

	// Token: 0x04000072 RID: 114
	[SerializeField]
	public CustomFacePartUtility mouthPart;

	// Token: 0x04000073 RID: 115
	[SerializeField]
	public CustomFacePartUtility tailPart;

	// Token: 0x04000074 RID: 116
	[SerializeField]
	public CustomFacePartUtility wingLPart;

	// Token: 0x04000075 RID: 117
	[SerializeField]
	public CustomFacePartUtility wingRPart;

	// Token: 0x04000076 RID: 118
	[SerializeField]
	public CustomFacePartUtility footRPart;

	// Token: 0x04000077 RID: 119
	[SerializeField]
	public CustomFacePartUtility footLPart;

	// Token: 0x04000078 RID: 120
	public Transform leftHandSocket;

	// Token: 0x04000079 RID: 121
	public Transform righthandSocket;

	// Token: 0x0400007A RID: 122
	private MaterialPropertyBlock mainBlock;

	// Token: 0x0400007C RID: 124
	[SerializeField]
	private CharacterSubVisuals subvisuals;

	// Token: 0x0400007D RID: 125
	[SerializeField]
	private CustomFaceSettingData convertedData;
}
