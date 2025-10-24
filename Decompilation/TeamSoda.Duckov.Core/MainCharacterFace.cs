using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class MainCharacterFace : MonoBehaviour
{
	// Token: 0x06000B86 RID: 2950 RVA: 0x00030C10 File Offset: 0x0002EE10
	private void Start()
	{
		CustomFaceSettingData saveData = this.customFaceManager.LoadMainCharacterSetting();
		this.customFace.LoadFromData(saveData);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00030C35 File Offset: 0x0002EE35
	private void Update()
	{
	}

	// Token: 0x040009D0 RID: 2512
	public CustomFaceManager customFaceManager;

	// Token: 0x040009D1 RID: 2513
	public CustomFaceInstance customFace;
}
