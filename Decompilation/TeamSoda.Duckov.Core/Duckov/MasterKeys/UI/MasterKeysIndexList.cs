using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E1 RID: 737
	public class MasterKeysIndexList : MonoBehaviour, ISingleSelectionMenu<MasterKeysIndexEntry>
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x00056654 File Offset: 0x00054854
		private PrefabPool<MasterKeysIndexEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<MasterKeysIndexEntry>(this.entryPrefab, this.entryContainer, new Action<MasterKeysIndexEntry>(this.OnGetEntry), new Action<MasterKeysIndexEntry>(this.OnReleaseEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06001791 RID: 6033 RVA: 0x000566A8 File Offset: 0x000548A8
		// (remove) Token: 0x06001792 RID: 6034 RVA: 0x000566E0 File Offset: 0x000548E0
		internal event Action<MasterKeysIndexEntry> onEntryPointerClicked;

		// Token: 0x06001793 RID: 6035 RVA: 0x00056715 File Offset: 0x00054915
		private void OnGetEntry(MasterKeysIndexEntry entry)
		{
			entry.onPointerClicked += this.OnEntryPointerClicked;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x00056729 File Offset: 0x00054929
		private void OnReleaseEntry(MasterKeysIndexEntry entry)
		{
			entry.onPointerClicked -= this.OnEntryPointerClicked;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0005673D File Offset: 0x0005493D
		private void OnEntryPointerClicked(MasterKeysIndexEntry entry)
		{
			Action<MasterKeysIndexEntry> action = this.onEntryPointerClicked;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00056750 File Offset: 0x00054950
		private void Awake()
		{
			this.entryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x00056764 File Offset: 0x00054964
		internal void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (int itemID in MasterKeysManager.AllPossibleKeys)
			{
				this.Populate(itemID);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x000567C4 File Offset: 0x000549C4
		private void Populate(int itemID)
		{
			MasterKeysIndexEntry masterKeysIndexEntry = this.Pool.Get(this.entryContainer);
			masterKeysIndexEntry.gameObject.SetActive(true);
			masterKeysIndexEntry.Setup(itemID, this);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000567EA File Offset: 0x000549EA
		public MasterKeysIndexEntry GetSelection()
		{
			return this.selection;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000567F2 File Offset: 0x000549F2
		public bool SetSelection(MasterKeysIndexEntry selection)
		{
			this.selection = selection;
			return true;
		}

		// Token: 0x04001135 RID: 4405
		[SerializeField]
		private MasterKeysIndexEntry entryPrefab;

		// Token: 0x04001136 RID: 4406
		[SerializeField]
		private RectTransform entryContainer;

		// Token: 0x04001137 RID: 4407
		private PrefabPool<MasterKeysIndexEntry> _pool;

		// Token: 0x04001139 RID: 4409
		private MasterKeysIndexEntry selection;
	}
}
