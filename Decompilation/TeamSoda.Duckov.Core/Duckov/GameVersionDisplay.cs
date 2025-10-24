using System;
using TMPro;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023B RID: 571
	public class GameVersionDisplay : MonoBehaviour
	{
		// Token: 0x060011B1 RID: 4529 RVA: 0x000441C4 File Offset: 0x000423C4
		private void Start()
		{
			this.text.text = string.Format("v{0}", GameMetaData.Instance.Version);
		}

		// Token: 0x04000DAD RID: 3501
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
