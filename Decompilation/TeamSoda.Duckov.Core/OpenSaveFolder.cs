using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001C5 RID: 453
public class OpenSaveFolder : MonoBehaviour
{
	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000D7C RID: 3452 RVA: 0x000379FD File Offset: 0x00035BFD
	private string filePath
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00037A04 File Offset: 0x00035C04
	private void Update()
	{
		if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.lKey.isPressed)
		{
			this.OpenFolder();
		}
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x00037A2E File Offset: 0x00035C2E
	public void OpenFolder()
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = this.filePath,
			UseShellExecute = true
		});
	}
}
