using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem.Data;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AD RID: 941
	public class StorageDock : View
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x00075D49 File Offset: 0x00073F49
		public static StorageDock Instance
		{
			get
			{
				return View.GetViewInstance<StorageDock>();
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x00075D50 File Offset: 0x00073F50
		private PrefabPool<StorageDockEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<StorageDockEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x00075D89 File Offset: 0x00073F89
		protected override void Awake()
		{
			base.Awake();
			this.entryTemplate.gameObject.SetActive(false);
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00075DA2 File Offset: 0x00073FA2
		private void OnEnable()
		{
			PlayerStorage.OnTakeBufferItem += this.OnTakeBufferItem;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00075DB5 File Offset: 0x00073FB5
		private void OnDisable()
		{
			PlayerStorage.OnTakeBufferItem -= this.OnTakeBufferItem;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00075DC8 File Offset: 0x00073FC8
		private void OnTakeBufferItem()
		{
			this.Refresh();
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00075DD0 File Offset: 0x00073FD0
		protected override void OnOpen()
		{
			base.OnOpen();
			if (PlayerStorage.Instance == null)
			{
				base.Close();
				return;
			}
			this.fadeGroup.Show();
			this.Setup();
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x00075DFD File Offset: 0x00073FFD
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00075E10 File Offset: 0x00074010
		private void Setup()
		{
			this.Refresh();
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00075E18 File Offset: 0x00074018
		private void Refresh()
		{
			this.EntryPool.ReleaseAll();
			List<ItemTreeData> incomingItemBuffer = PlayerStorage.IncomingItemBuffer;
			for (int i = 0; i < incomingItemBuffer.Count; i++)
			{
				ItemTreeData itemTreeData = incomingItemBuffer[i];
				if (itemTreeData != null)
				{
					this.EntryPool.Get(null).Setup(i, itemTreeData);
				}
			}
			this.placeHolder.gameObject.SetActive(incomingItemBuffer.Count <= 0);
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00075E81 File Offset: 0x00074081
		internal static void Show()
		{
			if (StorageDock.Instance == null)
			{
				return;
			}
			StorageDock.Instance.Open(null);
		}

		// Token: 0x040016D4 RID: 5844
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040016D5 RID: 5845
		[SerializeField]
		private StorageDockEntry entryTemplate;

		// Token: 0x040016D6 RID: 5846
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x040016D7 RID: 5847
		private PrefabPool<StorageDockEntry> _entryPool;
	}
}
