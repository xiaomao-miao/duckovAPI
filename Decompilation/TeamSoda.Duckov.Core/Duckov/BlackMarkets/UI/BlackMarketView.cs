using System;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000308 RID: 776
	public class BlackMarketView : View
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x0005C15E File Offset: 0x0005A35E
		public static BlackMarketView Instance
		{
			get
			{
				return View.GetViewInstance<BlackMarketView>();
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x0005C165 File Offset: 0x0005A365
		protected override bool ShowOpenCloseButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0005C168 File Offset: 0x0005A368
		protected override void Awake()
		{
			base.Awake();
			this.btn_demandPanel.onClick.AddListener(delegate()
			{
				this.SetMode(BlackMarketView.Mode.Demand);
			});
			this.btn_supplyPanel.onClick.AddListener(delegate()
			{
				this.SetMode(BlackMarketView.Mode.Supply);
			});
			this.btn_refresh.onClick.AddListener(new UnityAction(this.OnRefreshBtnClicked));
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0005C1CF File Offset: 0x0005A3CF
		private void OnEnable()
		{
			BlackMarket.onRefreshChanceChanged += this.OnRefreshChanceChanced;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0005C1E2 File Offset: 0x0005A3E2
		private void OnDisable()
		{
			BlackMarket.onRefreshChanceChanged -= this.OnRefreshChanceChanced;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0005C1F5 File Offset: 0x0005A3F5
		private void OnRefreshChanceChanced(BlackMarket market)
		{
			this.RefreshRefreshButton();
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0005C200 File Offset: 0x0005A400
		private void RefreshRefreshButton()
		{
			if (this.Target == null)
			{
				this.refreshChanceText.text = "ERROR";
				this.refreshInteractableIndicator.SetActive(false);
			}
			int refreshChance = this.Target.RefreshChance;
			int maxRefreshChance = this.Target.MaxRefreshChance;
			this.refreshChanceText.text = string.Format("{0}/{1}", refreshChance, maxRefreshChance);
			this.refreshInteractableIndicator.SetActive(refreshChance > 0);
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0005C27F File Offset: 0x0005A47F
		private void OnRefreshBtnClicked()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.PayAndRegenerate();
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x0005C29B File Offset: 0x0005A49B
		// (set) Token: 0x06001963 RID: 6499 RVA: 0x0005C2A3 File Offset: 0x0005A4A3
		public BlackMarket Target { get; private set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x0005C2AC File Offset: 0x0005A4AC
		private bool ShowDemand
		{
			get
			{
				return (BlackMarketView.Mode.Demand | this.mode) == this.mode;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x0005C2BE File Offset: 0x0005A4BE
		private bool ShowSupply
		{
			get
			{
				return (BlackMarketView.Mode.Supply | this.mode) == this.mode;
			}
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0005C2D0 File Offset: 0x0005A4D0
		public static void Show(BlackMarketView.Mode mode)
		{
			if (BlackMarketView.Instance == null)
			{
				return;
			}
			if (BlackMarket.Instance == null)
			{
				return;
			}
			BlackMarketView.Instance.Setup(BlackMarket.Instance, mode);
			BlackMarketView.Instance.Open(null);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0005C309 File Offset: 0x0005A509
		private void Setup(BlackMarket target, BlackMarketView.Mode mode)
		{
			this.Target = target;
			this.demandPanel.Setup(target);
			this.supplyPanel.Setup(target);
			this.RefreshRefreshButton();
			this.SetMode(mode);
			base.Open(null);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0005C33E File Offset: 0x0005A53E
		private void SetMode(BlackMarketView.Mode mode)
		{
			this.mode = mode;
			this.demandPanel.gameObject.SetActive(this.ShowDemand);
			this.supplyPanel.gameObject.SetActive(this.ShowSupply);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0005C373 File Offset: 0x0005A573
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0005C386 File Offset: 0x0005A586
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0005C39C File Offset: 0x0005A59C
		private void Update()
		{
			if (this.Target == null)
			{
				return;
			}
			int refreshChance = this.Target.RefreshChance;
			int maxRefreshChance = this.Target.MaxRefreshChance;
			string text;
			if (refreshChance < maxRefreshChance)
			{
				TimeSpan remainingTimeBeforeRefresh = this.Target.RemainingTimeBeforeRefresh;
				text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)remainingTimeBeforeRefresh.TotalHours), remainingTimeBeforeRefresh.Minutes, remainingTimeBeforeRefresh.Seconds);
			}
			else
			{
				text = "--:--:--";
			}
			this.refreshETAText.text = text;
		}

		// Token: 0x04001265 RID: 4709
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001266 RID: 4710
		[SerializeField]
		private DemandPanel demandPanel;

		// Token: 0x04001267 RID: 4711
		[SerializeField]
		private SupplyPanel supplyPanel;

		// Token: 0x04001268 RID: 4712
		[SerializeField]
		private TextMeshProUGUI refreshETAText;

		// Token: 0x04001269 RID: 4713
		[SerializeField]
		private TextMeshProUGUI refreshChanceText;

		// Token: 0x0400126A RID: 4714
		[SerializeField]
		private Button btn_demandPanel;

		// Token: 0x0400126B RID: 4715
		[SerializeField]
		private Button btn_supplyPanel;

		// Token: 0x0400126C RID: 4716
		[SerializeField]
		private Button btn_refresh;

		// Token: 0x0400126D RID: 4717
		[SerializeField]
		private GameObject refreshInteractableIndicator;

		// Token: 0x0400126F RID: 4719
		private BlackMarketView.Mode mode;

		// Token: 0x0200059F RID: 1439
		public enum Mode
		{
			// Token: 0x04002020 RID: 8224
			None,
			// Token: 0x04002021 RID: 8225
			Demand,
			// Token: 0x04002022 RID: 8226
			Supply
		}
	}
}
