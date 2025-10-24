using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class CustomFaceSaveLoad : MonoBehaviour
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000122 RID: 290 RVA: 0x00005A1F File Offset: 0x00003C1F
	private CustomFaceData faceData
	{
		get
		{
			return GameplayDataSettings.CustomFaceData;
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00005A28 File Offset: 0x00003C28
	private void Awake()
	{
		this.slotButtons = new List<CustomFaceLoadSaveButton>();
		for (int i = 0; i < 5; i++)
		{
			CustomFaceLoadSaveButton item = this.CreateAButton(i, i.ToString(), this.slotButtonParent);
			this.slotButtons.Add(item);
		}
		this.buttonPfb.gameObject.SetActive(false);
		this.SetSlotAndLoad(0);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00005A85 File Offset: 0x00003C85
	private CustomFaceLoadSaveButton CreateAButton(int index, string name, Transform parent)
	{
		CustomFaceLoadSaveButton customFaceLoadSaveButton = UnityEngine.Object.Instantiate<CustomFaceLoadSaveButton>(this.buttonPfb);
		customFaceLoadSaveButton.Init(this, index, name);
		customFaceLoadSaveButton.transform.SetParent(parent, false);
		return customFaceLoadSaveButton;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00005AA8 File Offset: 0x00003CA8
	public void SetSlotAndLoad(int slot)
	{
		this.currentSlot = slot;
		this.UpdateSelection();
		this.LoadData(slot);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00005AC0 File Offset: 0x00003CC0
	private void UpdateSelection()
	{
		foreach (CustomFaceLoadSaveButton customFaceLoadSaveButton in this.slotButtons)
		{
			customFaceLoadSaveButton.SetSelection(customFaceLoadSaveButton.index == this.currentSlot);
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00005B20 File Offset: 0x00003D20
	private void LoadData(int slot)
	{
		if (!ES3.KeyExists(string.Format("CustomFaceData_{0}", slot)))
		{
			this.LoadDefault();
			return;
		}
		CustomFaceSettingData saveData = (CustomFaceSettingData)ES3.Load(string.Format("CustomFaceData_{0}", slot));
		this.instance.LoadFromData(saveData);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00005B74 File Offset: 0x00003D74
	public void LoadDefault()
	{
		CustomFaceSettingData settings = this.faceData.DefaultPreset.settings;
		this.instance.LoadFromData(settings);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00005B9E File Offset: 0x00003D9E
	public void SaveDataToCurrentSlot()
	{
		this.SaveData(this.currentSlot);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00005BAC File Offset: 0x00003DAC
	private void SaveData(int slot)
	{
		CustomFaceSettingData value = this.instance.ConvertToSaveData();
		ES3.Save<CustomFaceSettingData>(string.Format("CustomFaceData_{0}", slot), value);
	}

	// Token: 0x040000B2 RID: 178
	public CustomFaceInstance instance;

	// Token: 0x040000B3 RID: 179
	public CustomFaceLoadSaveButton buttonPfb;

	// Token: 0x040000B4 RID: 180
	public Transform slotButtonParent;

	// Token: 0x040000B5 RID: 181
	private List<CustomFaceLoadSaveButton> slotButtons;

	// Token: 0x040000B6 RID: 182
	private int currentSlot;
}
