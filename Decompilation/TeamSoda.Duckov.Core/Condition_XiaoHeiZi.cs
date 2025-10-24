using System;
using Duckov.Quests;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class Condition_XiaoHeiZi : Condition
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x0002940D File Offset: 0x0002760D
	public override string DisplayText
	{
		get
		{
			return "看看你是不是小黑子";
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00029414 File Offset: 0x00027614
	public override bool Evaluate()
	{
		if (CharacterMainControl.Main == null)
		{
			return false;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		CharacterModel characterModel = main.characterModel;
		if (!characterModel)
		{
			return false;
		}
		CustomFaceInstance customFace = characterModel.CustomFace;
		if (!customFace)
		{
			return false;
		}
		if (customFace.ConvertToSaveData().hairID != this.hairID)
		{
			return false;
		}
		Item armorItem = main.GetArmorItem();
		return !(armorItem == null) && armorItem.TypeID == this.armorID;
	}

	// Token: 0x04000854 RID: 2132
	[SerializeField]
	private int hairID = 6;

	// Token: 0x04000855 RID: 2133
	[ItemTypeID]
	[SerializeField]
	private int armorID = 379;
}
