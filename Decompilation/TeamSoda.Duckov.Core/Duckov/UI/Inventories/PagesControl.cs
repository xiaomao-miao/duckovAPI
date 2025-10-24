using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI.Inventories
{
	// Token: 0x020003C9 RID: 969
	public class PagesControl : MonoBehaviour
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x0007B854 File Offset: 0x00079A54
		private PrefabPool<PagesControl_Entry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<PagesControl_Entry>(this.template, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0007B88D File Offset: 0x00079A8D
		private void Start()
		{
			if (this.target != null)
			{
				this.Setup(this.target);
			}
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0007B8A9 File Offset: 0x00079AA9
		public void Setup(InventoryDisplay target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0007B8C4 File Offset: 0x00079AC4
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (this.target == null)
			{
				return;
			}
			this.target.onPageInfoRefreshed += this.OnPageInfoRefreshed;
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0007B8F2 File Offset: 0x00079AF2
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onPageInfoRefreshed -= this.OnPageInfoRefreshed;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0007B91A File Offset: 0x00079B1A
		private void OnPageInfoRefreshed()
		{
			this.Refresh();
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0007B924 File Offset: 0x00079B24
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			if (this.inputIndicators)
			{
				GameObject gameObject = this.inputIndicators;
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			if (this.target == null)
			{
				return;
			}
			if (!this.target.UsePages)
			{
				return;
			}
			if (this.target.MaxPage <= 1)
			{
				return;
			}
			for (int i = 0; i < this.target.MaxPage; i++)
			{
				this.Pool.Get(null).Setup(this, i, this.target.SelectedPage == i);
			}
			if (this.inputIndicators)
			{
				GameObject gameObject2 = this.inputIndicators;
				if (gameObject2 == null)
				{
					return;
				}
				gameObject2.SetActive(true);
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0007B9DC File Offset: 0x00079BDC
		internal void NotifySelect(int i)
		{
			if (this.target == null)
			{
				return;
			}
			this.target.SetPage(i);
		}

		// Token: 0x040017FA RID: 6138
		[SerializeField]
		private InventoryDisplay target;

		// Token: 0x040017FB RID: 6139
		[SerializeField]
		private PagesControl_Entry template;

		// Token: 0x040017FC RID: 6140
		[SerializeField]
		private GameObject inputIndicators;

		// Token: 0x040017FD RID: 6141
		private PrefabPool<PagesControl_Entry> _pool;
	}
}
