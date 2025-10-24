using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F9 RID: 505
public class HealthBar_DamageBar : MonoBehaviour
{
	// Token: 0x06000ECB RID: 3787 RVA: 0x0003AF33 File Offset: 0x00039133
	private void Awake()
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = (base.transform as RectTransform);
		}
		if (this.image == null)
		{
			this.image = base.GetComponent<Image>();
		}
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0003AF70 File Offset: 0x00039170
	public UniTask Animate(float damageBarPostion, float damageBarWidth, Action onComplete)
	{
		HealthBar_DamageBar.<Animate>d__7 <Animate>d__;
		<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Animate>d__.<>4__this = this;
		<Animate>d__.damageBarPostion = damageBarPostion;
		<Animate>d__.damageBarWidth = damageBarWidth;
		<Animate>d__.onComplete = onComplete;
		<Animate>d__.<>1__state = -1;
		<Animate>d__.<>t__builder.Start<HealthBar_DamageBar.<Animate>d__7>(ref <Animate>d__);
		return <Animate>d__.<>t__builder.Task;
	}

	// Token: 0x04000C34 RID: 3124
	[SerializeField]
	internal RectTransform rectTransform;

	// Token: 0x04000C35 RID: 3125
	[SerializeField]
	internal Image image;

	// Token: 0x04000C36 RID: 3126
	[SerializeField]
	private float duration;

	// Token: 0x04000C37 RID: 3127
	[SerializeField]
	private float targetSizeDelta = 4f;

	// Token: 0x04000C38 RID: 3128
	[SerializeField]
	private AnimationCurve curve;

	// Token: 0x04000C39 RID: 3129
	[SerializeField]
	private Gradient colorOverTime;
}
