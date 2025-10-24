using System;
using Duckov.PerkTrees;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BC RID: 956
	public class RequireItemEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x060022B8 RID: 8888 RVA: 0x0007967B File Offset: 0x0007787B
		public void NotifyPooled()
		{
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0007967D File Offset: 0x0007787D
		public void NotifyReleased()
		{
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x00079680 File Offset: 0x00077880
		public void Setup(PerkRequirement.RequireItemEntry target)
		{
			int id = target.id;
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(id);
			this.icon.sprite = metaData.icon;
			string displayName = metaData.DisplayName;
			int itemCount = ItemUtilities.GetItemCount(id);
			this.text.text = string.Format(this.textFormat, displayName, target.amount, itemCount);
		}

		// Token: 0x040017A3 RID: 6051
		[SerializeField]
		private Image icon;

		// Token: 0x040017A4 RID: 6052
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040017A5 RID: 6053
		[SerializeField]
		private string textFormat = "{0} x{1}";
	}
}
