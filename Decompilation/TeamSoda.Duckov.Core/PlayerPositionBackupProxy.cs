using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class PlayerPositionBackupProxy : MonoBehaviour
{
	// Token: 0x0600092B RID: 2347 RVA: 0x00028B2A File Offset: 0x00026D2A
	public void StartRecoverInteract()
	{
		PauseMenu.Instance.Close();
		PlayerPositionBackupManager.StartRecover();
	}
}
