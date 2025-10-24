using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000394 RID: 916
	public class ItemStatEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x0600203C RID: 8252 RVA: 0x00070ABD File Offset: 0x0006ECBD
		public void NotifyPooled()
		{
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00070ABF File Offset: 0x0006ECBF
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00070ACE File Offset: 0x0006ECCE
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00070AD6 File Offset: 0x0006ECD6
		internal void Setup(Stat target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00070AF4 File Offset: 0x0006ECF4
		private void Refresh()
		{
			StatInfoDatabase.Entry entry = StatInfoDatabase.Get(this.target.Key);
			this.displayName.text = this.target.DisplayName;
			this.value.text = this.target.Value.ToString(entry.DisplayFormat);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00070B4D File Offset: 0x0006ED4D
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty += this.OnTargetSetDirty;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00070B6F File Offset: 0x0006ED6F
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty -= this.OnTargetSetDirty;
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x00070B91 File Offset: 0x0006ED91
		private void OnTargetSetDirty(Stat stat)
		{
			if (stat != this.target)
			{
				Debug.LogError("ItemStatEntry.target与事件触发者不匹配。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x040015FE RID: 5630
		private Stat target;

		// Token: 0x040015FF RID: 5631
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001600 RID: 5632
		[SerializeField]
		private TextMeshProUGUI value;
	}
}
