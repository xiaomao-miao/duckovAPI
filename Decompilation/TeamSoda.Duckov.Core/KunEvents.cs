using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000A8 RID: 168
public class KunEvents : MonoBehaviour
{
	// Token: 0x060005B0 RID: 1456 RVA: 0x0001967E File Offset: 0x0001787E
	private void Awake()
	{
		this.setActiveObject.SetActive(false);
		if (!this.dialogueBubbleProxy)
		{
			this.dialogueBubbleProxy.GetComponent<DialogueBubbleProxy>();
		}
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x000196A8 File Offset: 0x000178A8
	public void Check()
	{
		bool flag = false;
		if (CharacterMainControl.Main == null)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		CharacterModel characterModel = main.characterModel;
		if (!characterModel)
		{
			return;
		}
		CustomFaceInstance customFace = characterModel.CustomFace;
		if (!customFace)
		{
			return;
		}
		bool flag2 = customFace.ConvertToSaveData().hairID == this.hairID;
		Item armorItem = main.GetArmorItem();
		if (armorItem != null && armorItem.TypeID == this.armorID)
		{
			flag = true;
		}
		if (!flag2 && !flag)
		{
			this.dialogueBubbleProxy.textKey = this.notRight;
		}
		else if (flag2 && !flag)
		{
			this.dialogueBubbleProxy.textKey = this.onlyRightFace;
		}
		else if (!flag2 && flag)
		{
			this.dialogueBubbleProxy.textKey = this.onlyRightCloth;
		}
		else
		{
			this.dialogueBubbleProxy.textKey = this.allRight;
			this.setActiveObject.SetActive(true);
		}
		this.dialogueBubbleProxy.Pop();
	}

	// Token: 0x04000529 RID: 1321
	[SerializeField]
	private int hairID = 6;

	// Token: 0x0400052A RID: 1322
	[ItemTypeID]
	[SerializeField]
	private int armorID;

	// Token: 0x0400052B RID: 1323
	public DialogueBubbleProxy dialogueBubbleProxy;

	// Token: 0x0400052C RID: 1324
	[LocalizationKey("Dialogues")]
	public string notRight;

	// Token: 0x0400052D RID: 1325
	[LocalizationKey("Dialogues")]
	public string onlyRightFace;

	// Token: 0x0400052E RID: 1326
	[LocalizationKey("Dialogues")]
	public string onlyRightCloth;

	// Token: 0x0400052F RID: 1327
	[LocalizationKey("Dialogues")]
	public string allRight;

	// Token: 0x04000530 RID: 1328
	[FormerlySerializedAs("SetActiveObject")]
	public GameObject setActiveObject;
}
