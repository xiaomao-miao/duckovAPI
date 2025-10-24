using System;
using UnityEngine;

namespace Duckov.MiniGames.Debugging
{
	// Token: 0x020002CD RID: 717
	public class HideAndLockCursor : MonoBehaviour
	{
		// Token: 0x060016AE RID: 5806 RVA: 0x0005316F File Offset: 0x0005136F
		private void OnEnable()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0005317D File Offset: 0x0005137D
		private void OnDisable()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0005318B File Offset: 0x0005138B
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				base.enabled = false;
			}
		}
	}
}
