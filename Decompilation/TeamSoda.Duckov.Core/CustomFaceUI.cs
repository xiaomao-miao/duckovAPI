using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000038 RID: 56
public class CustomFaceUI : MonoBehaviour
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600013B RID: 315 RVA: 0x00005D90 File Offset: 0x00003F90
	public static CustomFaceUI ActiveView
	{
		get
		{
			if (CustomFaceUI.activeView && CustomFaceUI.activeView.gameObject.activeInHierarchy)
			{
				return CustomFaceUI.activeView;
			}
			CustomFaceUI.activeView = null;
			return null;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600013C RID: 316 RVA: 0x00005DBC File Offset: 0x00003FBC
	// (remove) Token: 0x0600013D RID: 317 RVA: 0x00005DF0 File Offset: 0x00003FF0
	public static event Action OnCustomUIViewChanged;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600013E RID: 318 RVA: 0x00005E24 File Offset: 0x00004024
	// (remove) Token: 0x0600013F RID: 319 RVA: 0x00005E5C File Offset: 0x0000405C
	private event Action onLoadValues;

	// Token: 0x06000140 RID: 320 RVA: 0x00005E94 File Offset: 0x00004094
	public void SetFace(CustomFaceInstance face)
	{
		if (this.faceInstance != null)
		{
			this.faceInstance.OnLoadFaceData -= this.OnLoadFaceData;
		}
		this.faceInstance = face;
		this.faceInstance.OnLoadFaceData += this.OnLoadFaceData;
		this.Init();
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00005EEA File Offset: 0x000040EA
	private void OnLoadFaceData()
	{
		this.LoadValues();
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00005EF2 File Offset: 0x000040F2
	private void Start()
	{
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00005EF4 File Offset: 0x000040F4
	private void OnDestroy()
	{
		if (this.faceInstance != null)
		{
			this.faceInstance.OnLoadFaceData -= this.OnLoadFaceData;
		}
		if (CustomFaceUI.activeView == this)
		{
			CustomFaceUI.activeView = null;
		}
		Action onCustomUIViewChanged = CustomFaceUI.OnCustomUIViewChanged;
		if (onCustomUIViewChanged == null)
		{
			return;
		}
		onCustomUIViewChanged();
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00005F48 File Offset: 0x00004148
	private void OnEnable()
	{
		this.SelectTab(this.tabs[0]);
		CustomFaceUI.activeView = this;
		Action onCustomUIViewChanged = CustomFaceUI.OnCustomUIViewChanged;
		if (onCustomUIViewChanged == null)
		{
			return;
		}
		onCustomUIViewChanged();
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00005F71 File Offset: 0x00004171
	private void OnDisable()
	{
		if (CustomFaceUI.activeView == this)
		{
			CustomFaceUI.activeView = null;
		}
		Action onCustomUIViewChanged = CustomFaceUI.OnCustomUIViewChanged;
		if (onCustomUIViewChanged == null)
		{
			return;
		}
		onCustomUIViewChanged();
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00005F98 File Offset: 0x00004198
	private void Init()
	{
		this.skinColorPicker.Init(this, "UI_CustomFace_SkinColor");
		this.headSizeSlider.Init(0.6f, 1.4f, this, "UI_CustomFace_HeadSize");
		this.headHeightSlider.Init(0f, 0.6f, this, "UI_CustomFace_HeadHeight");
		this.headRoundnessSlider.Init(0.35f, 1f, this, "UI_CustomFace_HeadRoundness");
		this.hairSwitch.Init(this, CustomFacePartTypes.hair, "UI_CustomFace_HairType");
		this.hairColorPicker.Init(this, "UI_CustomFace_HairColor");
		this.eyeSwitch.Init(this, CustomFacePartTypes.eye, "UI_CustomFace_EyeType");
		this.eyeColorPicker.Init(this, "UI_CustomFace_EyeColor");
		this.eyeDistanceSlider.Init(0f, 90f, this, "UI_CustomFace_EyeSpace");
		this.eyeHeightSlider.Init(-0.3f, 0.3f, this, "UI_CustomFace_EyeHeight");
		this.eyeSizeSlider.Init(0.3f, 4f, this, "UI_CustomFace_EyeSize");
		this.eyeTwistSlider.Init(-90f, 90f, this, "UI_CustomFace_EyeRotate");
		this.eyebrowSwitch.Init(this, CustomFacePartTypes.eyebrow, "UI_CustomFace_EyebrowType");
		this.eyebrowColorPicker.Init(this, "UI_CustomFace_EyebrowColor");
		this.eyebrowDistanceSlider.Init(0f, 90f, this, "UI_CustomFace_EyebrowSpace");
		this.eyebrowHeightSlider.Init(-0.3f, 0.3f, this, "UI_CustomFace_EyebrowHeight");
		this.eyebrowSizeSlider.Init(0.3f, 4f, this, "UI_CustomFace_EyebrowSize");
		this.eyebrowTwistSlider.Init(-90f, 90f, this, "UI_CustomFace_EyebrowRotate");
		this.mouthSwitch.Init(this, CustomFacePartTypes.mouth, "UI_CustomFace_MouthType");
		this.mouthColorPicker.Init(this, "UI_CustomFace_MouthColor");
		this.mouthSizeSlider.Init(0.3f, 4f, this, "UI_CustomFace_MouthSize");
		this.mouthHeightSlider.Init(-0.3f, 0.3f, this, "UI_CustomFace_MouthHeight");
		this.mouthLeftRightSlider.Init(-50f, 50f, this, "UI_CustomFace_MouthOffset");
		this.mouthTwistSlider.Init(-90f, 90f, this, "UI_CustomFace_MouthRotate");
		this.wingSwitch.Init(this, CustomFacePartTypes.wing, "UI_CustomFace_WingType");
		this.wingColorPicker.Init(this, "UI_CustomFace_WingColor");
		this.wingSizeSlider.Init(0.5f, 2f, this, "UI_CustomFace_WingSize");
		this.tailSwitch.Init(this, CustomFacePartTypes.tail, "UI_CustomFace_TailType");
		this.tailColorPicker.Init(this, "UI_CustomFace_TailColor");
		this.tailSizeSlider.Init(0.3f, 2f, this, "UI_CustomFace_TailSize");
		this.footSwitch.Init(this, CustomFacePartTypes.foot, "UI_CustomFace_FootType");
		this.footSizeSlider.Init(0.5f, 1.5f, this, "UI_CustomFace_FootSize");
		this.LoadValues();
		foreach (CustomFaceTabs customFaceTabs in this.tabs)
		{
			customFaceTabs.master = this;
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x000062CC File Offset: 0x000044CC
	private void LoadValues()
	{
		this.skinColorPicker.SetColor(this.faceInstance.headSetting.mainColor);
		this.headSizeSlider.SetValue(1f + this.faceInstance.headSetting.headScaleOffset);
		this.headHeightSlider.SetValue(this.faceInstance.headSetting.foreheadHeight);
		this.headRoundnessSlider.SetValue(this.faceInstance.headSetting.foreheadRound);
		this.hairSwitch.SetName(this.faceInstance.hairPart.GetCurrentPartName());
		this.hairColorPicker.SetColor(this.faceInstance.hairPart.partInfo.color);
		this.eyeSwitch.SetName(this.faceInstance.eyePart.GetCurrentPartName());
		this.eyeColorPicker.SetColor(this.faceInstance.eyePart.partInfo.color);
		this.eyeDistanceSlider.SetValue(this.faceInstance.eyePart.partInfo.distanceAngle);
		this.eyeHeightSlider.SetValue(this.faceInstance.eyePart.partInfo.height);
		this.eyeSizeSlider.SetValue(this.faceInstance.eyePart.partInfo.scale);
		this.eyeTwistSlider.SetValue(this.faceInstance.eyePart.partInfo.twist);
		this.eyebrowSwitch.SetName(this.faceInstance.eyebrowPart.GetCurrentPartName());
		this.eyebrowColorPicker.SetColor(this.faceInstance.eyebrowPart.partInfo.color);
		this.eyebrowDistanceSlider.SetValue(this.faceInstance.eyebrowPart.partInfo.distanceAngle);
		this.eyebrowHeightSlider.SetValue(this.faceInstance.eyebrowPart.partInfo.height);
		this.eyebrowSizeSlider.SetValue(this.faceInstance.eyebrowPart.partInfo.scale);
		this.eyebrowTwistSlider.SetValue(this.faceInstance.eyebrowPart.partInfo.twist);
		this.mouthSwitch.SetName(this.faceInstance.mouthPart.GetCurrentPartName());
		this.mouthColorPicker.SetColor(this.faceInstance.mouthPart.partInfo.color);
		this.mouthSizeSlider.SetValue(this.faceInstance.mouthPart.partInfo.scale);
		this.mouthHeightSlider.SetValue(this.faceInstance.mouthPart.partInfo.height);
		this.mouthLeftRightSlider.SetValue(this.faceInstance.mouthPart.partInfo.leftRightAngle);
		this.mouthTwistSlider.SetValue(this.faceInstance.mouthPart.partInfo.twist);
		this.wingSwitch.SetName(this.faceInstance.wingLPart.GetCurrentPartName());
		this.wingColorPicker.SetColor(this.faceInstance.wingLPart.partInfo.color);
		this.wingSizeSlider.SetValue(this.faceInstance.wingLPart.partInfo.scale);
		this.tailSwitch.SetName(this.faceInstance.tailPart.GetCurrentPartName());
		this.tailColorPicker.SetColor(this.faceInstance.tailPart.partInfo.color);
		this.tailSizeSlider.SetValue(this.faceInstance.tailPart.partInfo.scale);
		this.footSwitch.SetName(this.faceInstance.footLPart.GetCurrentPartName());
		this.footSizeSlider.SetValue(this.faceInstance.footLPart.partInfo.scale);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x000066A8 File Offset: 0x000048A8
	public void SelectTab(CustomFaceTabs tab)
	{
		foreach (GameObject gameObject in this.panels)
		{
			gameObject.SetActive(tab.panels.Contains(gameObject));
		}
		foreach (CustomFaceTabs customFaceTabs in this.tabs)
		{
			customFaceTabs.SetSelectVisual(tab == customFaceTabs);
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00006750 File Offset: 0x00004950
	public void SetDirty()
	{
		this.dirty = true;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00006759 File Offset: 0x00004959
	private void LateUpdate()
	{
		if (this.dirty && this.faceInstance)
		{
			this.RefreshInfos();
			this.dirty = false;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00006780 File Offset: 0x00004980
	public void RefreshInfos()
	{
		if (this.faceInstance == null)
		{
			return;
		}
		this.faceInstance.headSetting.mainColor = this.skinColorPicker.CurrentColor;
		this.faceInstance.headSetting.headScaleOffset = this.headSizeSlider.Value - 1f;
		this.faceInstance.headSetting.foreheadHeight = this.headHeightSlider.Value;
		this.faceInstance.headSetting.foreheadRound = this.headRoundnessSlider.Value;
		this.faceInstance.hairPart.partInfo.color = this.hairColorPicker.CurrentColor;
		this.faceInstance.eyePart.partInfo.color = this.eyeColorPicker.CurrentColor;
		this.faceInstance.eyePart.partInfo.distanceAngle = this.eyeDistanceSlider.Value;
		this.faceInstance.eyePart.partInfo.height = this.eyeHeightSlider.Value;
		this.faceInstance.eyePart.partInfo.scale = this.eyeSizeSlider.Value;
		this.faceInstance.eyePart.partInfo.twist = this.eyeTwistSlider.Value;
		this.faceInstance.eyebrowPart.partInfo.color = this.eyebrowColorPicker.CurrentColor;
		this.faceInstance.eyebrowPart.partInfo.distanceAngle = this.eyebrowDistanceSlider.Value;
		this.faceInstance.eyebrowPart.partInfo.height = this.eyebrowHeightSlider.Value;
		this.faceInstance.eyebrowPart.partInfo.scale = this.eyebrowSizeSlider.Value;
		this.faceInstance.eyebrowPart.partInfo.twist = this.eyebrowTwistSlider.Value;
		this.faceInstance.mouthPart.partInfo.color = this.mouthColorPicker.CurrentColor;
		this.faceInstance.mouthPart.partInfo.scale = this.mouthSizeSlider.Value;
		this.faceInstance.mouthPart.partInfo.height = this.mouthHeightSlider.Value;
		this.faceInstance.mouthPart.partInfo.leftRightAngle = this.mouthLeftRightSlider.Value;
		this.faceInstance.mouthPart.partInfo.twist = this.mouthTwistSlider.Value;
		this.faceInstance.wingLPart.partInfo.color = this.wingColorPicker.CurrentColor;
		this.faceInstance.wingRPart.partInfo.color = this.wingColorPicker.CurrentColor;
		this.faceInstance.wingLPart.partInfo.scale = this.wingSizeSlider.Value;
		this.faceInstance.wingRPart.partInfo.scale = this.wingSizeSlider.Value;
		this.faceInstance.tailPart.partInfo.color = this.tailColorPicker.CurrentColor;
		this.faceInstance.tailPart.partInfo.scale = this.tailSizeSlider.Value;
		this.faceInstance.footLPart.partInfo.scale = this.footSizeSlider.Value;
		this.faceInstance.footRPart.partInfo.scale = this.footSizeSlider.Value;
		this.faceInstance.RefreshAll();
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00006B1C File Offset: 0x00004D1C
	public string SwitchPart(CustomFacePartTypes type, int direction)
	{
		if (this.faceInstance == null)
		{
			return "";
		}
		CustomFacePart customFacePart = this.faceInstance.SwitchPart(type, this.faceInstance, direction);
		if (customFacePart == null)
		{
			return "";
		}
		return customFacePart.id.ToString();
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00006B6C File Offset: 0x00004D6C
	public void SaveToMainCharacter()
	{
		if (!this.faceInstance)
		{
			return;
		}
		if (!this.canControl)
		{
			return;
		}
		CustomFaceSettingData setting = this.faceInstance.ConvertToSaveData();
		LevelManager.Instance.CustomFaceManager.SaveSettingToMainCharacter(setting);
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00006BAC File Offset: 0x00004DAC
	public void OnDrag(PointerEventData eventData)
	{
		if (this.faceInstance == null)
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		float angle = -eventData.delta.x * this.rotateSpeed * Time.deltaTime;
		this.faceInstance.transform.Rotate(Vector3.up, angle);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00006C01 File Offset: 0x00004E01
	public void RandomPreset()
	{
		this.faceInstance.LoadFromData(this.presets.GetRandom<CustomFacePreset>().settings);
	}

	// Token: 0x040000C1 RID: 193
	private static CustomFaceUI activeView;

	// Token: 0x040000C3 RID: 195
	public List<CustomFaceTabs> tabs;

	// Token: 0x040000C4 RID: 196
	public List<GameObject> panels;

	// Token: 0x040000C5 RID: 197
	private CustomFaceInstance faceInstance;

	// Token: 0x040000C6 RID: 198
	[SerializeField]
	private float rotateSpeed = 15f;

	// Token: 0x040000C7 RID: 199
	public CustomFaceUIColorPicker skinColorPicker;

	// Token: 0x040000C8 RID: 200
	public CustomFaceSlider headSizeSlider;

	// Token: 0x040000C9 RID: 201
	public CustomFaceSlider headHeightSlider;

	// Token: 0x040000CA RID: 202
	public CustomFaceSlider headRoundnessSlider;

	// Token: 0x040000CB RID: 203
	public CustomFaceUISwitch hairSwitch;

	// Token: 0x040000CC RID: 204
	public CustomFaceUIColorPicker hairColorPicker;

	// Token: 0x040000CD RID: 205
	public CustomFaceUISwitch eyeSwitch;

	// Token: 0x040000CE RID: 206
	public CustomFaceUIColorPicker eyeColorPicker;

	// Token: 0x040000CF RID: 207
	public CustomFaceSlider eyeDistanceSlider;

	// Token: 0x040000D0 RID: 208
	public CustomFaceSlider eyeHeightSlider;

	// Token: 0x040000D1 RID: 209
	public CustomFaceSlider eyeSizeSlider;

	// Token: 0x040000D2 RID: 210
	public CustomFaceSlider eyeTwistSlider;

	// Token: 0x040000D3 RID: 211
	public CustomFaceUISwitch eyebrowSwitch;

	// Token: 0x040000D4 RID: 212
	public CustomFaceUIColorPicker eyebrowColorPicker;

	// Token: 0x040000D5 RID: 213
	public CustomFaceSlider eyebrowDistanceSlider;

	// Token: 0x040000D6 RID: 214
	public CustomFaceSlider eyebrowHeightSlider;

	// Token: 0x040000D7 RID: 215
	public CustomFaceSlider eyebrowSizeSlider;

	// Token: 0x040000D8 RID: 216
	public CustomFaceSlider eyebrowTwistSlider;

	// Token: 0x040000D9 RID: 217
	public CustomFaceUISwitch mouthSwitch;

	// Token: 0x040000DA RID: 218
	public CustomFaceUIColorPicker mouthColorPicker;

	// Token: 0x040000DB RID: 219
	public CustomFaceSlider mouthSizeSlider;

	// Token: 0x040000DC RID: 220
	public CustomFaceSlider mouthHeightSlider;

	// Token: 0x040000DD RID: 221
	public CustomFaceSlider mouthLeftRightSlider;

	// Token: 0x040000DE RID: 222
	public CustomFaceSlider mouthTwistSlider;

	// Token: 0x040000DF RID: 223
	public CustomFaceUISwitch wingSwitch;

	// Token: 0x040000E0 RID: 224
	public CustomFaceUIColorPicker wingColorPicker;

	// Token: 0x040000E1 RID: 225
	public CustomFaceSlider wingSizeSlider;

	// Token: 0x040000E2 RID: 226
	public CustomFaceUISwitch tailSwitch;

	// Token: 0x040000E3 RID: 227
	public CustomFaceUIColorPicker tailColorPicker;

	// Token: 0x040000E4 RID: 228
	public CustomFaceSlider tailSizeSlider;

	// Token: 0x040000E5 RID: 229
	public CustomFaceUISwitch footSwitch;

	// Token: 0x040000E6 RID: 230
	public CustomFaceSlider footSizeSlider;

	// Token: 0x040000E8 RID: 232
	private bool dirty;

	// Token: 0x040000E9 RID: 233
	public bool canControl;

	// Token: 0x040000EA RID: 234
	public List<CustomFacePreset> presets;
}
