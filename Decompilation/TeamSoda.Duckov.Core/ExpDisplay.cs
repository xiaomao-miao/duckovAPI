using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F8 RID: 504
public class ExpDisplay : MonoBehaviour
{
	// Token: 0x06000EBF RID: 3775 RVA: 0x0003ACB8 File Offset: 0x00038EB8
	private void Refresh()
	{
		EXPManager instance = EXPManager.Instance;
		if (instance == null)
		{
			return;
		}
		int num = instance.LevelFromExp(this.displayExp);
		if (this.displayingLevel != num)
		{
			this.displayingLevel = num;
			this.OnDisplayingLevelChanged();
		}
		ValueTuple<long, long> levelExpRange = this.GetLevelExpRange(num);
		long num2 = levelExpRange.Item2 - levelExpRange.Item1;
		this.txtLevel.text = num.ToString();
		this.txtCurrentExp.text = this.displayExp.ToString();
		string text;
		if (levelExpRange.Item2 == 9223372036854775807L)
		{
			text = "∞";
		}
		else
		{
			text = levelExpRange.Item2.ToString();
		}
		this.txtMaxExp.text = text;
		float fillAmount = (float)((double)(this.displayExp - levelExpRange.Item1) / (double)num2);
		this.expBarFill.fillAmount = fillAmount;
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x0003AD8C File Offset: 0x00038F8C
	private void OnDisplayingLevelChanged()
	{
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x0003AD90 File Offset: 0x00038F90
	[return: TupleElementNames(new string[]
	{
		"from",
		"to"
	})]
	private ValueTuple<long, long> GetLevelExpRange(int level)
	{
		ValueTuple<long, long> result;
		if (this.cachedLevelExpRange.TryGetValue(level, out result))
		{
			return result;
		}
		EXPManager instance = EXPManager.Instance;
		if (instance == null)
		{
			return new ValueTuple<long, long>(0L, 0L);
		}
		ValueTuple<long, long> levelExpRange = instance.GetLevelExpRange(level);
		this.cachedLevelExpRange[level] = levelExpRange;
		return levelExpRange;
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0003ADDE File Offset: 0x00038FDE
	private void SnapToCurrent()
	{
		this.displayExp = EXPManager.EXP;
		this.Refresh();
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0003ADF4 File Offset: 0x00038FF4
	private UniTask Animate(long targetExp, float duration, AnimationCurve curve)
	{
		ExpDisplay.<Animate>d__15 <Animate>d__;
		<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Animate>d__.<>4__this = this;
		<Animate>d__.targetExp = targetExp;
		<Animate>d__.duration = duration;
		<Animate>d__.curve = curve;
		<Animate>d__.<>1__state = -1;
		<Animate>d__.<>t__builder.Start<ExpDisplay.<Animate>d__15>(ref <Animate>d__);
		return <Animate>d__.<>t__builder.Task;
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0003AE50 File Offset: 0x00039050
	private long LongLerp(long a, long b, float t)
	{
		long num = b - a;
		return a + (long)(t * (float)num);
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0003AE68 File Offset: 0x00039068
	private void OnEnable()
	{
		if (this.snapToCurrentOnEnable)
		{
			this.SnapToCurrent();
		}
		this.RegisterEvents();
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0003AE7E File Offset: 0x0003907E
	private void OnDisable()
	{
		this.UnregisterEvents();
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0003AE86 File Offset: 0x00039086
	private void RegisterEvents()
	{
		EXPManager.onExpChanged = (Action<long>)Delegate.Combine(EXPManager.onExpChanged, new Action<long>(this.OnExpChanged));
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0003AEA8 File Offset: 0x000390A8
	private void UnregisterEvents()
	{
		EXPManager.onExpChanged = (Action<long>)Delegate.Remove(EXPManager.onExpChanged, new Action<long>(this.OnExpChanged));
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0003AECA File Offset: 0x000390CA
	private void OnExpChanged(long exp)
	{
		this.Animate(exp, this.animationDuration, this.animationCurve).Forget();
	}

	// Token: 0x04000C29 RID: 3113
	[SerializeField]
	private TextMeshProUGUI txtLevel;

	// Token: 0x04000C2A RID: 3114
	[SerializeField]
	private TextMeshProUGUI txtCurrentExp;

	// Token: 0x04000C2B RID: 3115
	[SerializeField]
	private TextMeshProUGUI txtMaxExp;

	// Token: 0x04000C2C RID: 3116
	[SerializeField]
	private Image expBarFill;

	// Token: 0x04000C2D RID: 3117
	[SerializeField]
	private bool snapToCurrentOnEnable;

	// Token: 0x04000C2E RID: 3118
	[SerializeField]
	private float animationDuration = 0.1f;

	// Token: 0x04000C2F RID: 3119
	[SerializeField]
	private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000C30 RID: 3120
	[SerializeField]
	private long displayExp;

	// Token: 0x04000C31 RID: 3121
	private int displayingLevel = -1;

	// Token: 0x04000C32 RID: 3122
	[TupleElementNames(new string[]
	{
		"from",
		"to"
	})]
	private Dictionary<int, ValueTuple<long, long>> cachedLevelExpRange = new Dictionary<int, ValueTuple<long, long>>();

	// Token: 0x04000C33 RID: 3123
	private int currentToken;
}
