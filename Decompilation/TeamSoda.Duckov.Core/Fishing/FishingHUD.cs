using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
	// Token: 0x02000216 RID: 534
	public class FishingHUD : MonoBehaviour
	{
		// Token: 0x06000FDE RID: 4062 RVA: 0x0003E2FD File Offset: 0x0003C4FD
		private void Awake()
		{
			Action_Fishing.OnPlayerStartCatching += this.OnStartCatching;
			Action_Fishing.OnPlayerStopCatching += this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing += this.OnStopFishing;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0003E332 File Offset: 0x0003C532
		private void OnDestroy()
		{
			Action_Fishing.OnPlayerStartCatching -= this.OnStartCatching;
			Action_Fishing.OnPlayerStopCatching -= this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing -= this.OnStopFishing;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0003E367 File Offset: 0x0003C567
		private void OnStopFishing(Action_Fishing fishing)
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0003E374 File Offset: 0x0003C574
		private void OnStopCatching(Action_Fishing fishing, Item item, Action<bool> action)
		{
			this.StopCatchingTask(item, action).Forget();
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0003E383 File Offset: 0x0003C583
		private void OnStartCatching(Action_Fishing fishing, float totalTime, Func<float> currentTimeGetter)
		{
			this.CatchingTask(fishing, totalTime, currentTimeGetter).Forget();
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0003E394 File Offset: 0x0003C594
		private UniTask CatchingTask(Action_Fishing fishing, float totalTime, Func<float> currentTimeGetter)
		{
			FishingHUD.<CatchingTask>d__9 <CatchingTask>d__;
			<CatchingTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CatchingTask>d__.<>4__this = this;
			<CatchingTask>d__.fishing = fishing;
			<CatchingTask>d__.totalTime = totalTime;
			<CatchingTask>d__.currentTimeGetter = currentTimeGetter;
			<CatchingTask>d__.<>1__state = -1;
			<CatchingTask>d__.<>t__builder.Start<FishingHUD.<CatchingTask>d__9>(ref <CatchingTask>d__);
			return <CatchingTask>d__.<>t__builder.Task;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0003E3F0 File Offset: 0x0003C5F0
		private void UpdateBar(float totalTime, float currentTime)
		{
			if (totalTime <= 0f)
			{
				return;
			}
			float fillAmount = 1f - currentTime / totalTime;
			this.countDownFill.fillAmount = fillAmount;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0003E41C File Offset: 0x0003C61C
		private UniTask StopCatchingTask(Item item, Action<bool> confirmCallback)
		{
			FishingHUD.<StopCatchingTask>d__11 <StopCatchingTask>d__;
			<StopCatchingTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<StopCatchingTask>d__.<>4__this = this;
			<StopCatchingTask>d__.item = item;
			<StopCatchingTask>d__.<>1__state = -1;
			<StopCatchingTask>d__.<>t__builder.Start<FishingHUD.<StopCatchingTask>d__11>(ref <StopCatchingTask>d__);
			return <StopCatchingTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000CBE RID: 3262
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CBF RID: 3263
		[SerializeField]
		private Image countDownFill;

		// Token: 0x04000CC0 RID: 3264
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04000CC1 RID: 3265
		[SerializeField]
		private FadeGroup failIndicator;
	}
}
