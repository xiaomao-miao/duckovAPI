using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

// Token: 0x02000169 RID: 361
public class Title : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000AEF RID: 2799 RVA: 0x0002ECE4 File Offset: 0x0002CEE4
	private void Start()
	{
		this.StartTask().Forget();
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0002ECF4 File Offset: 0x0002CEF4
	private UniTask StartTask()
	{
		Title.<StartTask>d__5 <StartTask>d__;
		<StartTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<StartTask>d__.<>4__this = this;
		<StartTask>d__.<>1__state = -1;
		<StartTask>d__.<>t__builder.Start<Title.<StartTask>d__5>(ref <StartTask>d__);
		return <StartTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0002ED38 File Offset: 0x0002CF38
	private UniTask ContinueTask()
	{
		Title.<ContinueTask>d__6 <ContinueTask>d__;
		<ContinueTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<ContinueTask>d__.<>4__this = this;
		<ContinueTask>d__.<>1__state = -1;
		<ContinueTask>d__.<>t__builder.Start<Title.<ContinueTask>d__6>(ref <ContinueTask>d__);
		return <ContinueTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0002ED7C File Offset: 0x0002CF7C
	private UniTask WaitForTimeline(PlayableDirector timeline)
	{
		Title.<WaitForTimeline>d__7 <WaitForTimeline>d__;
		<WaitForTimeline>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<WaitForTimeline>d__.timeline = timeline;
		<WaitForTimeline>d__.<>1__state = -1;
		<WaitForTimeline>d__.<>t__builder.Start<Title.<WaitForTimeline>d__7>(ref <WaitForTimeline>d__);
		return <WaitForTimeline>d__.<>t__builder.Task;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0002EDBF File Offset: 0x0002CFBF
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.fadeGroup.IsShown)
		{
			this.ContinueTask().Forget();
		}
	}

	// Token: 0x04000969 RID: 2409
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x0400096A RID: 2410
	[SerializeField]
	private PlayableDirector timelineToTitle;

	// Token: 0x0400096B RID: 2411
	[SerializeField]
	private PlayableDirector timelineToMainMenu;

	// Token: 0x0400096C RID: 2412
	private string sfx_PressStart = "UI/game_start";
}
