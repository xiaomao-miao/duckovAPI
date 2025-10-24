using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000199 RID: 409
public class ATMPanel : MonoBehaviour
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00032F8F File Offset: 0x0003118F
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

	// Token: 0x06000BFC RID: 3068 RVA: 0x00032FB0 File Offset: 0x000311B0
	private void Awake()
	{
		this.btnSelectSave.onClick.AddListener(new UnityAction(this.ShowSavePanel));
		this.btnSelectDraw.onClick.AddListener(new UnityAction(this.ShowDrawPanel));
		this.savePanel.onQuit += this.SavePanel_onQuit;
		this.drawPanel.onQuit += this.DrawPanel_onQuit;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00033023 File Offset: 0x00031223
	private void DrawPanel_onQuit(ATMPanel_DrawPanel panel)
	{
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0003302C File Offset: 0x0003122C
	private void SavePanel_onQuit(ATMPanel_SavePanel obj)
	{
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00033035 File Offset: 0x00031235
	private void HideAllPanels(bool skip = false)
	{
		if (skip)
		{
			this.selectPanel.SkipHide();
		}
		else
		{
			this.selectPanel.Hide();
		}
		this.savePanel.Hide(skip);
		this.drawPanel.Hide(skip);
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x0003306A File Offset: 0x0003126A
	public void ShowSelectPanel(bool skipHideOthers = false)
	{
		this.HideAllPanels(skipHideOthers);
		this.selectPanel.Show();
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x0003307E File Offset: 0x0003127E
	public void ShowDrawPanel()
	{
		this.HideAllPanels(false);
		this.drawPanel.Show();
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00033092 File Offset: 0x00031292
	public void ShowSavePanel()
	{
		this.HideAllPanels(false);
		this.savePanel.Show();
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000330A6 File Offset: 0x000312A6
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		this.RefreshCash();
		this.RefreshBalance();
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x000330DD File Offset: 0x000312DD
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00033101 File Offset: 0x00031301
	private void OnPlayerItemOperation()
	{
		this.RefreshCash();
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x00033109 File Offset: 0x00031309
	private void OnMoneyChanged(long oldMoney, long changedMoney)
	{
		this.RefreshBalance();
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00033111 File Offset: 0x00031311
	private void RefreshCash()
	{
		this._cachedCashAmount = ItemUtilities.GetItemCount(451);
		this.cashAmountText.text = string.Format("{0:n0}", this.CashAmount);
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00033143 File Offset: 0x00031343
	private void RefreshBalance()
	{
		this.balanceAmountText.text = string.Format("{0:n0}", EconomyManager.Money);
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00033164 File Offset: 0x00031364
	public static UniTask<bool> Draw(long amount)
	{
		ATMPanel.<Draw>d__26 <Draw>d__;
		<Draw>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Draw>d__.amount = amount;
		<Draw>d__.<>1__state = -1;
		<Draw>d__.<>t__builder.Start<ATMPanel.<Draw>d__26>(ref <Draw>d__);
		return <Draw>d__.<>t__builder.Task;
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x000331A8 File Offset: 0x000313A8
	public static bool Save(long amount)
	{
		Cost cost = new Cost(0L, new ValueTuple<int, long>[]
		{
			new ValueTuple<int, long>(451, amount)
		});
		if (!cost.Pay(false, true))
		{
			return false;
		}
		EconomyManager.Add(amount);
		return true;
	}

	// Token: 0x04000A78 RID: 2680
	private const int CashItemTypeID = 451;

	// Token: 0x04000A79 RID: 2681
	[SerializeField]
	private TextMeshProUGUI balanceAmountText;

	// Token: 0x04000A7A RID: 2682
	[SerializeField]
	private TextMeshProUGUI cashAmountText;

	// Token: 0x04000A7B RID: 2683
	[SerializeField]
	private Button btnSelectSave;

	// Token: 0x04000A7C RID: 2684
	[SerializeField]
	private Button btnSelectDraw;

	// Token: 0x04000A7D RID: 2685
	[SerializeField]
	private FadeGroup selectPanel;

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private ATMPanel_SavePanel savePanel;

	// Token: 0x04000A7F RID: 2687
	[SerializeField]
	private ATMPanel_DrawPanel drawPanel;

	// Token: 0x04000A80 RID: 2688
	private int _cachedCashAmount = -1;

	// Token: 0x04000A81 RID: 2689
	private static bool drawingMoney;

	// Token: 0x04000A82 RID: 2690
	public const long MaxDrawAmount = 10000000L;
}
