using System;
using Saves;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class SaveDataBoolProxy : MonoBehaviour
{
	// Token: 0x060005B9 RID: 1465 RVA: 0x00019974 File Offset: 0x00017B74
	public void Save()
	{
		SavesSystem.Save<bool>(this.key, this.value);
		Debug.Log(string.Format("SetSaveData:{0} to {1}", this.key, this.value));
	}

	// Token: 0x04000538 RID: 1336
	public string key;

	// Token: 0x04000539 RID: 1337
	public bool value;
}
