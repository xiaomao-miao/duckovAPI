using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.UI.SavesRestore
{
	// Token: 0x020003E8 RID: 1000
	public class SavesBackupRestorePanel : MonoBehaviour
	{
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x0007DD48 File Offset: 0x0007BF48
		private PrefabPool<SavesBackupRestorePanelEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<SavesBackupRestorePanelEntry>(this.template, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0007DD81 File Offset: 0x0007BF81
		private void Awake()
		{
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0007DD83 File Offset: 0x0007BF83
		public void Open(int savesSlot)
		{
			this.slot = savesSlot;
			this.Refresh();
			this.fadeGroup.Show();
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0007DD9D File Offset: 0x0007BF9D
		public void Close()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0007DDAA File Offset: 0x0007BFAA
		public void Confirm()
		{
			this.confirm = true;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0007DDB3 File Offset: 0x0007BFB3
		public void Cancel()
		{
			this.cancel = true;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0007DDBC File Offset: 0x0007BFBC
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			List<SavesSystem.BackupInfo> list = SavesSystem.GetBackupList(this.slot).ToList<SavesSystem.BackupInfo>();
			list.Sort(delegate(SavesSystem.BackupInfo a, SavesSystem.BackupInfo b)
			{
				if (a.Time < b.Time)
				{
					return 1;
				}
				return -1;
			});
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				SavesSystem.BackupInfo backupInfo = list[i];
				if (backupInfo.exists)
				{
					this.Pool.Get(null).Setup(this, backupInfo);
					num++;
				}
			}
			this.noBackupIndicator.SetActive(num <= 0);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0007DE58 File Offset: 0x0007C058
		internal void NotifyClicked(SavesBackupRestorePanelEntry button)
		{
			if (this.recovering)
			{
				return;
			}
			SavesSystem.BackupInfo info = button.Info;
			if (!info.exists)
			{
				return;
			}
			this.RecoverTask(info).Forget();
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0007DE8C File Offset: 0x0007C08C
		private UniTask RecoverTask(SavesSystem.BackupInfo info)
		{
			SavesBackupRestorePanel.<RecoverTask>d__21 <RecoverTask>d__;
			<RecoverTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RecoverTask>d__.<>4__this = this;
			<RecoverTask>d__.info = info;
			<RecoverTask>d__.<>1__state = -1;
			<RecoverTask>d__.<>t__builder.Start<SavesBackupRestorePanel.<RecoverTask>d__21>(ref <RecoverTask>d__);
			return <RecoverTask>d__.<>t__builder.Task;
		}

		// Token: 0x0400188A RID: 6282
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400188B RID: 6283
		[SerializeField]
		private FadeGroup confirmFadeGroup;

		// Token: 0x0400188C RID: 6284
		[SerializeField]
		private FadeGroup resultFadeGroup;

		// Token: 0x0400188D RID: 6285
		[SerializeField]
		private TextMeshProUGUI[] slotIndexTexts;

		// Token: 0x0400188E RID: 6286
		[SerializeField]
		private TextMeshProUGUI[] backupTimeTexts;

		// Token: 0x0400188F RID: 6287
		[SerializeField]
		private SavesBackupRestorePanelEntry template;

		// Token: 0x04001890 RID: 6288
		[SerializeField]
		private GameObject noBackupIndicator;

		// Token: 0x04001891 RID: 6289
		private PrefabPool<SavesBackupRestorePanelEntry> _pool;

		// Token: 0x04001892 RID: 6290
		private int slot;

		// Token: 0x04001893 RID: 6291
		private bool recovering;

		// Token: 0x04001894 RID: 6292
		private bool confirm;

		// Token: 0x04001895 RID: 6293
		private bool cancel;
	}
}
