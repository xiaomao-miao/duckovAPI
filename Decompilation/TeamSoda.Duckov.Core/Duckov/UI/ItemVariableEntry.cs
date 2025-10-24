using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000395 RID: 917
	public class ItemVariableEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002045 RID: 8261 RVA: 0x00070BB5 File Offset: 0x0006EDB5
		public void NotifyPooled()
		{
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00070BB7 File Offset: 0x0006EDB7
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00070BC6 File Offset: 0x0006EDC6
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00070BCE File Offset: 0x0006EDCE
		internal void Setup(CustomData target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00070BE9 File Offset: 0x0006EDE9
		private void Refresh()
		{
			this.displayName.text = this.target.DisplayName;
			this.value.text = this.target.GetValueDisplayString("");
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00070C1C File Offset: 0x0006EE1C
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetData += this.OnTargetSetData;
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00070C3E File Offset: 0x0006EE3E
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetData -= this.OnTargetSetData;
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00070C60 File Offset: 0x0006EE60
		private void OnTargetSetData(CustomData data)
		{
			this.Refresh();
		}

		// Token: 0x04001601 RID: 5633
		private CustomData target;

		// Token: 0x04001602 RID: 5634
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001603 RID: 5635
		[SerializeField]
		private TextMeshProUGUI value;
	}
}
