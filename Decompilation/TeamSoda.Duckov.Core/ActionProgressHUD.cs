using System;
using Duckov;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C6 RID: 198
public class ActionProgressHUD : MonoBehaviour
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001C05D File Offset: 0x0001A25D
	public bool InProgress
	{
		get
		{
			return this.inProgress;
		}
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0001C068 File Offset: 0x0001A268
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnActionStartEvent += this.OnActionStart;
				this.characterMainControl.OnActionProgressFinishEvent += this.OnActionFinish;
			}
		}
		this.inProgress = false;
		float num = 0f;
		if (this.currentProgressInterface as UnityEngine.Object != null)
		{
			Progress progress = this.currentProgressInterface.GetProgress();
			this.inProgress = progress.inProgress;
			num = progress.progress;
			if (!this.inProgress)
			{
				this.currentProgressInterface = null;
			}
		}
		if (this.inProgress)
		{
			this.targetAlpha = 1f;
			this.fillImage.fillAmount = num;
			if (num >= 1f)
			{
				this.targetAlpha = 0f;
			}
		}
		else
		{
			this.targetAlpha = 0f;
		}
		this.parentCanvasGroup.alpha = Mathf.MoveTowards(this.parentCanvasGroup.alpha, this.targetAlpha, 8f * Time.deltaTime);
		if (this.stopIndicator && this.characterMainControl)
		{
			bool flag = false;
			CharacterActionBase currentAction = this.characterMainControl.CurrentAction;
			if (currentAction && currentAction.Running && currentAction.IsStopable())
			{
				flag = true;
			}
			if (flag != this.stopIndicator.activeSelf && this.targetAlpha != 0f)
			{
				this.stopIndicator.SetActive(flag);
			}
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0001C1F0 File Offset: 0x0001A3F0
	private void OnDestroy()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.OnActionStartEvent -= this.OnActionStart;
			this.characterMainControl.OnActionProgressFinishEvent -= this.OnActionFinish;
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001C230 File Offset: 0x0001A430
	private void OnActionStart(CharacterActionBase action)
	{
		this.currentProgressInterface = (action as IProgress);
		if (this.specificActionType != CharacterActionBase.ActionPriorities.Whatever && action.ActionPriority() != this.specificActionType)
		{
			this.currentProgressInterface = null;
		}
		if (action && !action.progressHUD)
		{
			this.currentProgressInterface = null;
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0001C27D File Offset: 0x0001A47D
	private void OnActionFinish(CharacterActionBase action)
	{
		UnityEvent onFinishEvent = this.OnFinishEvent;
		if (onFinishEvent != null)
		{
			onFinishEvent.Invoke();
		}
		if (this.fillImage)
		{
			this.fillImage.fillAmount = 1f;
		}
	}

	// Token: 0x040005F3 RID: 1523
	public CharacterActionBase.ActionPriorities specificActionType;

	// Token: 0x040005F4 RID: 1524
	public ProceduralImage fillImage;

	// Token: 0x040005F5 RID: 1525
	public CanvasGroup parentCanvasGroup;

	// Token: 0x040005F6 RID: 1526
	private CharacterMainControl characterMainControl;

	// Token: 0x040005F7 RID: 1527
	private IProgress currentProgressInterface;

	// Token: 0x040005F8 RID: 1528
	private float targetAlpha;

	// Token: 0x040005F9 RID: 1529
	private bool inProgress;

	// Token: 0x040005FA RID: 1530
	public UnityEvent OnFinishEvent;

	// Token: 0x040005FB RID: 1531
	[FormerlySerializedAs("cancleIndicator")]
	public GameObject stopIndicator;
}
