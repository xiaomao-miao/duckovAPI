using System;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.SavesRestore
{
	// Token: 0x020003E9 RID: 1001
	public class SavesBackupRestorePanelEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x0007DEDF File Offset: 0x0007C0DF
		public SavesSystem.BackupInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0007DEE7 File Offset: 0x0007C0E7
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.NotifyClicked(this);
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0007DEF8 File Offset: 0x0007C0F8
		internal void Setup(SavesBackupRestorePanel master, SavesSystem.BackupInfo info)
		{
			this.master = master;
			this.info = info;
			if (info.time_raw <= 0L)
			{
				this.timeText.text = "???";
				return;
			}
			this.timeText.text = info.Time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
		}

		// Token: 0x04001896 RID: 6294
		[SerializeField]
		private TextMeshProUGUI timeText;

		// Token: 0x04001897 RID: 6295
		private SavesBackupRestorePanel master;

		// Token: 0x04001898 RID: 6296
		private SavesSystem.BackupInfo info;
	}
}
