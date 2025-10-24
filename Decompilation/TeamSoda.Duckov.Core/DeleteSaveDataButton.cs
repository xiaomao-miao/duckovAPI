using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Saves;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000165 RID: 357
public class DeleteSaveDataButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0002EA26 File Offset: 0x0002CC26
	private float TimeSinceStartedHolding
	{
		get
		{
			return Time.unscaledTime - this.timeWhenStartedHolding;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0002EA34 File Offset: 0x0002CC34
	private float T
	{
		get
		{
			if (this.totalTime <= 0f)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.TimeSinceStartedHolding / this.totalTime);
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0002EA5B File Offset: 0x0002CC5B
	public void OnPointerDown(PointerEventData eventData)
	{
		this.holding = true;
		this.timeWhenStartedHolding = Time.unscaledTime;
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0002EA6F File Offset: 0x0002CC6F
	public void OnPointerUp(PointerEventData eventData)
	{
		this.holding = false;
		this.timeWhenStartedHolding = float.MaxValue;
		this.RefreshProgressBar();
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0002EA89 File Offset: 0x0002CC89
	private void Start()
	{
		this.barFill.fillAmount = 0f;
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0002EA9B File Offset: 0x0002CC9B
	private void Update()
	{
		if (this.holding)
		{
			this.RefreshProgressBar();
			if (this.T >= 1f)
			{
				this.Execute();
			}
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0002EABE File Offset: 0x0002CCBE
	private void Execute()
	{
		this.holding = false;
		this.DeleteCurrentSaveData();
		this.RefreshProgressBar();
		this.NotifySaveDeleted().Forget();
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0002EAE0 File Offset: 0x0002CCE0
	private UniTask NotifySaveDeleted()
	{
		DeleteSaveDataButton.<NotifySaveDeleted>d__14 <NotifySaveDeleted>d__;
		<NotifySaveDeleted>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<NotifySaveDeleted>d__.<>4__this = this;
		<NotifySaveDeleted>d__.<>1__state = -1;
		<NotifySaveDeleted>d__.<>t__builder.Start<DeleteSaveDataButton.<NotifySaveDeleted>d__14>(ref <NotifySaveDeleted>d__);
		return <NotifySaveDeleted>d__.<>t__builder.Task;
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0002EB23 File Offset: 0x0002CD23
	private void DeleteCurrentSaveData()
	{
		SavesSystem.DeleteCurrentSave();
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0002EB2A File Offset: 0x0002CD2A
	private void RefreshProgressBar()
	{
		this.barFill.fillAmount = this.T;
	}

	// Token: 0x04000959 RID: 2393
	[SerializeField]
	private float totalTime = 3f;

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private Image barFill;

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	private FadeGroup saveDeletedNotifierFadeGroup;

	// Token: 0x0400095C RID: 2396
	private float timeWhenStartedHolding = float.MaxValue;

	// Token: 0x0400095D RID: 2397
	private bool holding;
}
