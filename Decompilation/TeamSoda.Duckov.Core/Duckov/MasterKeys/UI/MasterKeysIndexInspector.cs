using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E0 RID: 736
	public class MasterKeysIndexInspector : MonoBehaviour
	{
		// Token: 0x0600178C RID: 6028 RVA: 0x000565AD File Offset: 0x000547AD
		internal void Setup(MasterKeysIndexEntry target)
		{
			if (target == null)
			{
				this.SetupEmpty();
				return;
			}
			this.SetupNormal(target);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000565C8 File Offset: 0x000547C8
		private void SetupNormal(MasterKeysIndexEntry target)
		{
			this.targetItemID = target.ItemID;
			this.placeHolder.SetActive(false);
			this.content.SetActive(true);
			this.nameText.text = target.DisplayName;
			this.descriptionText.text = target.Description;
			this.icon.sprite = target.Icon;
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0005662C File Offset: 0x0005482C
		private void SetupEmpty()
		{
			this.content.gameObject.SetActive(false);
			this.placeHolder.SetActive(true);
		}

		// Token: 0x0400112F RID: 4399
		[SerializeField]
		private int targetItemID;

		// Token: 0x04001130 RID: 4400
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001131 RID: 4401
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x04001132 RID: 4402
		[SerializeField]
		private Image icon;

		// Token: 0x04001133 RID: 4403
		[SerializeField]
		private GameObject content;

		// Token: 0x04001134 RID: 4404
		[SerializeField]
		private GameObject placeHolder;
	}
}
