using System;
using Saves;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class SetDoorOpenIfSaveData : MonoBehaviour
{
	// Token: 0x06000721 RID: 1825 RVA: 0x0002001E File Offset: 0x0001E21E
	private void Start()
	{
		if (LevelManager.LevelInited)
		{
			this.OnSet();
			return;
		}
		LevelManager.OnLevelInitialized += this.OnSet;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002003F File Offset: 0x0001E23F
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.OnSet;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00020054 File Offset: 0x0001E254
	private void OnSet()
	{
		bool flag = SavesSystem.Load<bool>(this.key);
		Debug.Log(string.Format("Load door data:{0}  {1}", this.key, flag));
		this.door.ForceSetClosed(flag != this.openIfDataTure, false);
	}

	// Token: 0x040006C4 RID: 1732
	public Door door;

	// Token: 0x040006C5 RID: 1733
	public string key;

	// Token: 0x040006C6 RID: 1734
	public bool openIfDataTure = true;
}
