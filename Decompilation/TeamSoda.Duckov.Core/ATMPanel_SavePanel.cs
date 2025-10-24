using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019B RID: 411
public class ATMPanel_SavePanel : MonoBehaviour
{
	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00033485 File Offset: 0x00031685
	private int CashAmount
	{
		get
		{
			if (this._cachedCashAmount < 0)
			{
				this._cachedCashAmount = ItemUtilities.GetItemCount(451);
			}
			return this._cachedCashAmount;
		}
	}

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06000C1B RID: 3099 RVA: 0x000334A8 File Offset: 0x000316A8
	// (remove) Token: 0x06000C1C RID: 3100 RVA: 0x000334E0 File Offset: 0x000316E0
	public event Action<ATMPanel_SavePanel> onQuit;

	// Token: 0x06000C1D RID: 3101 RVA: 0x00033515 File Offset: 0x00031715
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		this.RefreshCash();
		this.Refresh();
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00033534 File Offset: 0x00031734
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00033547 File Offset: 0x00031747
	private void OnPlayerItemOperation()
	{
		this.RefreshCash();
		this.Refresh();
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00033555 File Offset: 0x00031755
	private void RefreshCash()
	{
		this._cachedCashAmount = ItemUtilities.GetItemCount(451);
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00033568 File Offset: 0x00031768
	private void Awake()
	{
		this.inputPanel.onInputFieldValueChanged += this.OnInputValueChanged;
		this.inputPanel.maxFunction = (() => (long)this.CashAmount);
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x000335DB File Offset: 0x000317DB
	private void OnQuitButtonClicked()
	{
		Action<ATMPanel_SavePanel> action = this.onQuit;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x000335F0 File Offset: 0x000317F0
	private void OnConfirmButtonClicked()
	{
		if (this.inputPanel.Value <= 0L)
		{
			this.inputPanel.Clear();
			return;
		}
		if (this.inputPanel.Value > (long)this.CashAmount)
		{
			return;
		}
		if (ATMPanel.Save(this.inputPanel.Value))
		{
			this.inputPanel.Clear();
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0003364A File Offset: 0x0003184A
	private void OnInputValueChanged(string v)
	{
		this.Refresh();
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00033654 File Offset: 0x00031854
	private void Refresh()
	{
		bool flag = (long)this.CashAmount >= this.inputPanel.Value;
		flag &= (this.inputPanel.Value >= 0L);
		this.insufficientIndicator.SetActive(!flag);
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0003369D File Offset: 0x0003189D
	internal void Hide(bool skip = false)
	{
		if (skip)
		{
			this.fadeGroup.SkipHide();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x000336B9 File Offset: 0x000318B9
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x04000A89 RID: 2697
	private const int CashItemTypeID = 451;

	// Token: 0x04000A8A RID: 2698
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000A8B RID: 2699
	[SerializeField]
	private DigitInputPanel inputPanel;

	// Token: 0x04000A8C RID: 2700
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000A8D RID: 2701
	[SerializeField]
	private GameObject insufficientIndicator;

	// Token: 0x04000A8E RID: 2702
	[SerializeField]
	private Button quitButton;

	// Token: 0x04000A8F RID: 2703
	private int _cachedCashAmount = -1;
}
