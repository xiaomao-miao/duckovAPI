using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E0 RID: 992
	public class ChangeGraphicsColorToggle : ToggleComponent
	{
		// Token: 0x060023F8 RID: 9208 RVA: 0x0007D5E9 File Offset: 0x0007B7E9
		protected override void OnSetToggle(ToggleAnimation master, bool value)
		{
			this.image.DOKill(false);
			this.image.DOColor(value ? this.trueColor : this.falseColor, this.duration);
		}

		// Token: 0x04001864 RID: 6244
		[SerializeField]
		private Image image;

		// Token: 0x04001865 RID: 6245
		[SerializeField]
		private Color trueColor;

		// Token: 0x04001866 RID: 6246
		[SerializeField]
		private Color falseColor;

		// Token: 0x04001867 RID: 6247
		[SerializeField]
		private float duration = 0.1f;
	}
}
