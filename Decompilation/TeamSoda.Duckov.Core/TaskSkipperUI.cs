using System;
using Duckov.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

// Token: 0x020001EB RID: 491
public class TaskSkipperUI : MonoBehaviour
{
	// Token: 0x06000E76 RID: 3702 RVA: 0x0003A200 File Offset: 0x00038400
	private void Awake()
	{
		UIInputManager.OnInteractInputContext += this.OnInteractInputContext;
		this.anyButtonListener = InputSystem.onAnyButtonPress.Call(new Action<InputControl>(this.OnAnyButton));
		this.skipped = false;
		this.alpha = 0f;
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0003A24C File Offset: 0x0003844C
	private void OnAnyButton(InputControl control)
	{
		this.Show();
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0003A254 File Offset: 0x00038454
	private void OnDestroy()
	{
		UIInputManager.OnInteractInputContext -= this.OnInteractInputContext;
		this.anyButtonListener.Dispose();
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0003A272 File Offset: 0x00038472
	private void OnInteractInputContext(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.pressing = true;
		}
		if (context.canceled)
		{
			this.pressing = false;
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0003A294 File Offset: 0x00038494
	private void Update()
	{
		this.UpdatePressing();
		this.UpdateFill();
		this.UpdateCanvasGroup();
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0003A2A8 File Offset: 0x000384A8
	private void Show()
	{
		this.show = true;
		this.hideTimer = this.hideAfterSeconds;
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0003A2C0 File Offset: 0x000384C0
	private void UpdatePressing()
	{
		if (UIInputManager.Instance == null)
		{
			this.pressing = Keyboard.current.fKey.isPressed;
		}
		if (this.pressing && !this.skipped)
		{
			this.pressTime += Time.deltaTime;
			if (this.pressTime >= this.totalTime)
			{
				this.skipped = true;
				this.target.Skip();
			}
			this.Show();
			return;
		}
		if (!this.skipped)
		{
			this.pressTime = Mathf.MoveTowards(this.pressTime, 0f, Time.deltaTime);
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0003A35C File Offset: 0x0003855C
	private void UpdateFill()
	{
		float fillAmount = this.pressTime / this.totalTime;
		this.fill.fillAmount = fillAmount;
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0003A384 File Offset: 0x00038584
	private void UpdateCanvasGroup()
	{
		if (this.show)
		{
			this.alpha = Mathf.MoveTowards(this.alpha, 1f, 10f * Time.deltaTime);
			this.hideTimer = Mathf.MoveTowards(this.hideTimer, 0f, Time.deltaTime);
			if (this.hideTimer < 0.01f)
			{
				this.show = false;
			}
		}
		else
		{
			this.alpha = Mathf.MoveTowards(this.alpha, 0f, 10f * Time.deltaTime);
		}
		this.canvasGroup.alpha = this.alpha;
	}

	// Token: 0x04000BF7 RID: 3063
	[SerializeField]
	private TaskList target;

	// Token: 0x04000BF8 RID: 3064
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000BF9 RID: 3065
	[SerializeField]
	private Image fill;

	// Token: 0x04000BFA RID: 3066
	[SerializeField]
	private float totalTime = 2f;

	// Token: 0x04000BFB RID: 3067
	[SerializeField]
	private float hideAfterSeconds = 2f;

	// Token: 0x04000BFC RID: 3068
	private float pressTime;

	// Token: 0x04000BFD RID: 3069
	private float alpha;

	// Token: 0x04000BFE RID: 3070
	private float hideTimer;

	// Token: 0x04000BFF RID: 3071
	private bool show;

	// Token: 0x04000C00 RID: 3072
	private IDisposable anyButtonListener;

	// Token: 0x04000C01 RID: 3073
	private bool pressing;

	// Token: 0x04000C02 RID: 3074
	private bool skipped;
}
