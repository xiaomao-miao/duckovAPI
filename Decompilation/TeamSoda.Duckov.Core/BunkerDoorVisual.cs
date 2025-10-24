using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class BunkerDoorVisual : MonoBehaviour
{
	// Token: 0x06000B5D RID: 2909 RVA: 0x000302AC File Offset: 0x0002E4AC
	private void Awake()
	{
		this.animator.SetBool("InRange", this.inRange);
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x000302C4 File Offset: 0x0002E4C4
	public void OnEnter()
	{
		if (this.inRange)
		{
			return;
		}
		this.inRange = true;
		this.animator.SetBool("InRange", this.inRange);
		this.PopText(this.welcomeText.ToPlainText(), 0.5f, this.inRange).Forget();
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00030318 File Offset: 0x0002E518
	public void OnExit()
	{
		if (!this.inRange)
		{
			return;
		}
		this.inRange = false;
		this.animator.SetBool("InRange", this.inRange);
		this.PopText(this.leaveText.ToPlainText(), 0f, this.inRange).Forget();
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0003036C File Offset: 0x0002E56C
	private UniTask PopText(string text, float delay, bool _inRange)
	{
		BunkerDoorVisual.<PopText>d__8 <PopText>d__;
		<PopText>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<PopText>d__.<>4__this = this;
		<PopText>d__.text = text;
		<PopText>d__.delay = delay;
		<PopText>d__._inRange = _inRange;
		<PopText>d__.<>1__state = -1;
		<PopText>d__.<>t__builder.Start<BunkerDoorVisual.<PopText>d__8>(ref <PopText>d__);
		return <PopText>d__.<>t__builder.Task;
	}

	// Token: 0x040009AC RID: 2476
	[LocalizationKey("Dialogues")]
	public string welcomeText;

	// Token: 0x040009AD RID: 2477
	[LocalizationKey("Dialogues")]
	public string leaveText;

	// Token: 0x040009AE RID: 2478
	public Transform textBubblePoint;

	// Token: 0x040009AF RID: 2479
	public bool inRange = true;

	// Token: 0x040009B0 RID: 2480
	public Animator animator;
}
