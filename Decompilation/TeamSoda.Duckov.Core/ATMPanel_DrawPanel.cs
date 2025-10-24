using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019A RID: 410
public class ATMPanel_DrawPanel : MonoBehaviour
{
	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06000C0C RID: 3084 RVA: 0x000331FC File Offset: 0x000313FC
	// (remove) Token: 0x06000C0D RID: 3085 RVA: 0x00033234 File Offset: 0x00031434
	public event Action<ATMPanel_DrawPanel> onQuit;

	// Token: 0x06000C0E RID: 3086 RVA: 0x00033269 File Offset: 0x00031469
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		this.Refresh();
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00033282 File Offset: 0x00031482
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00033298 File Offset: 0x00031498
	private void Awake()
	{
		this.inputPanel.onInputFieldValueChanged += this.OnInputValueChanged;
		this.inputPanel.maxFunction = delegate()
		{
			long num = EconomyManager.Money;
			if (num > 10000000L)
			{
				num = 10000000L;
			}
			return num;
		};
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x0003331E File Offset: 0x0003151E
	private void OnQuitButtonClicked()
	{
		Action<ATMPanel_DrawPanel> action = this.onQuit;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00033331 File Offset: 0x00031531
	private void OnMoneyChanged(long arg1, long arg2)
	{
		this.Refresh();
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x0003333C File Offset: 0x0003153C
	private void OnConfirmButtonClicked()
	{
		if (this.inputPanel.Value <= 0L)
		{
			this.inputPanel.Clear();
			return;
		}
		long num = EconomyManager.Money;
		if (num > 10000000L)
		{
			num = 10000000L;
		}
		if (this.inputPanel.Value > num)
		{
			return;
		}
		this.DrawTask(this.inputPanel.Value).Forget();
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000333A0 File Offset: 0x000315A0
	private UniTask DrawTask(long value)
	{
		ATMPanel_DrawPanel.<DrawTask>d__14 <DrawTask>d__;
		<DrawTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DrawTask>d__.<>4__this = this;
		<DrawTask>d__.value = value;
		<DrawTask>d__.<>1__state = -1;
		<DrawTask>d__.<>t__builder.Start<ATMPanel_DrawPanel.<DrawTask>d__14>(ref <DrawTask>d__);
		return <DrawTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x000333EB File Offset: 0x000315EB
	private void OnInputValueChanged(string v)
	{
		this.Refresh();
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x000333F4 File Offset: 0x000315F4
	private void Refresh()
	{
		bool flag = EconomyManager.Money >= this.inputPanel.Value;
		flag &= (this.inputPanel.Value <= 10000000L);
		flag &= (this.inputPanel.Value >= 0L);
		this.insufficientIndicator.SetActive(!flag);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00033454 File Offset: 0x00031654
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00033461 File Offset: 0x00031661
	internal void Hide(bool skip)
	{
		if (skip)
		{
			this.fadeGroup.SkipHide();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x04000A83 RID: 2691
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private DigitInputPanel inputPanel;

	// Token: 0x04000A85 RID: 2693
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000A86 RID: 2694
	[SerializeField]
	private GameObject insufficientIndicator;

	// Token: 0x04000A87 RID: 2695
	[SerializeField]
	private Button quitButton;
}
