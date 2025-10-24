using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001EE RID: 494
public class ColorPunch : MonoBehaviour
{
	// Token: 0x06000E82 RID: 3714 RVA: 0x0003A44B File Offset: 0x0003864B
	private void Awake()
	{
		if (this.graphic == null)
		{
			this.graphic = base.GetComponent<Graphic>();
		}
		this.resetColor = this.graphic.color;
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0003A478 File Offset: 0x00038678
	public void Punch()
	{
		this.DoTask().Forget();
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0003A485 File Offset: 0x00038685
	private int NewToken()
	{
		this.activeToken = UnityEngine.Random.Range(1, int.MaxValue);
		return this.activeToken;
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x0003A4A0 File Offset: 0x000386A0
	private UniTask DoTask()
	{
		ColorPunch.<DoTask>d__9 <DoTask>d__;
		<DoTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DoTask>d__.<>4__this = this;
		<DoTask>d__.<>1__state = -1;
		<DoTask>d__.<>t__builder.Start<ColorPunch.<DoTask>d__9>(ref <DoTask>d__);
		return <DoTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000C03 RID: 3075
	[SerializeField]
	private Graphic graphic;

	// Token: 0x04000C04 RID: 3076
	[SerializeField]
	private float duration;

	// Token: 0x04000C05 RID: 3077
	[SerializeField]
	private Gradient gradient;

	// Token: 0x04000C06 RID: 3078
	[SerializeField]
	private Color tint = Color.white;

	// Token: 0x04000C07 RID: 3079
	private Color resetColor;

	// Token: 0x04000C08 RID: 3080
	private int activeToken;
}
