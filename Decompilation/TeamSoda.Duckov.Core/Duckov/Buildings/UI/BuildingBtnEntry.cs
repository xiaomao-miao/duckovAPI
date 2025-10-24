using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031A RID: 794
	public class BuildingBtnEntry : MonoBehaviour
	{
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x0005F7E6 File Offset: 0x0005D9E6
		private string TokenFormat
		{
			get
			{
				return this.tokenFormatKey.ToPlainText();
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x0005F7F3 File Offset: 0x0005D9F3
		public BuildingInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0005F7FB File Offset: 0x0005D9FB
		public bool CostEnough
		{
			get
			{
				return this.info.TokenAmount > 0 || this.info.cost.Enough;
			}
		}

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06001A64 RID: 6756 RVA: 0x0005F824 File Offset: 0x0005DA24
		// (remove) Token: 0x06001A65 RID: 6757 RVA: 0x0005F85C File Offset: 0x0005DA5C
		public event Action<BuildingBtnEntry> onButtonClicked;

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06001A66 RID: 6758 RVA: 0x0005F894 File Offset: 0x0005DA94
		// (remove) Token: 0x06001A67 RID: 6759 RVA: 0x0005F8CC File Offset: 0x0005DACC
		public event Action<BuildingBtnEntry> onRecycleRequested;

		// Token: 0x06001A68 RID: 6760 RVA: 0x0005F901 File Offset: 0x0005DB01
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			this.recycleButton.onPressFullfilled.AddListener(new UnityAction(this.OnRecycleButtonTriggered));
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0005F93B File Offset: 0x0005DB3B
		private void OnRecycleButtonTriggered()
		{
			Action<BuildingBtnEntry> action = this.onRecycleRequested;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0005F94E File Offset: 0x0005DB4E
		private void OnEnable()
		{
			BuildingManager.OnBuildingListChanged += this.Refresh;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0005F961 File Offset: 0x0005DB61
		private void OnDisable()
		{
			BuildingManager.OnBuildingListChanged -= this.Refresh;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0005F974 File Offset: 0x0005DB74
		private void OnButtonClicked()
		{
			Action<BuildingBtnEntry> action = this.onButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0005F987 File Offset: 0x0005DB87
		internal void Setup(BuildingInfo buildingInfo)
		{
			this.info = buildingInfo;
			this.Refresh();
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0005F998 File Offset: 0x0005DB98
		private void Refresh()
		{
			int tokenAmount = this.info.TokenAmount;
			this.nameText.text = this.info.DisplayName;
			this.descriptionText.text = this.info.Description;
			this.tokenText.text = this.TokenFormat.Format(new
			{
				tokenAmount
			});
			this.icon.sprite = this.info.iconReference;
			this.costDisplay.Setup(this.info.cost, 1);
			this.costDisplay.gameObject.SetActive(tokenAmount <= 0);
			bool reachedAmountLimit = this.info.ReachedAmountLimit;
			this.amountText.text = ((this.info.maxAmount > 0) ? string.Format("{0}/{1}", this.info.CurrentAmount, this.info.maxAmount) : string.Format("{0}/∞", this.info.CurrentAmount));
			this.reachedAmountLimitationIndicator.SetActive(reachedAmountLimit);
			bool flag = !this.info.ReachedAmountLimit && this.CostEnough;
			this.backGround.color = (flag ? this.avaliableColor : this.normalColor);
			this.recycleButton.gameObject.SetActive(this.info.CurrentAmount > 0);
		}

		// Token: 0x040012E8 RID: 4840
		[SerializeField]
		private Button button;

		// Token: 0x040012E9 RID: 4841
		[SerializeField]
		private Image icon;

		// Token: 0x040012EA RID: 4842
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040012EB RID: 4843
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x040012EC RID: 4844
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x040012ED RID: 4845
		[SerializeField]
		private LongPressButton recycleButton;

		// Token: 0x040012EE RID: 4846
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x040012EF RID: 4847
		[SerializeField]
		[LocalizationKey("Default")]
		private string tokenFormatKey;

		// Token: 0x040012F0 RID: 4848
		[SerializeField]
		private TextMeshProUGUI tokenText;

		// Token: 0x040012F1 RID: 4849
		[SerializeField]
		private GameObject reachedAmountLimitationIndicator;

		// Token: 0x040012F2 RID: 4850
		[SerializeField]
		private Image backGround;

		// Token: 0x040012F3 RID: 4851
		[SerializeField]
		private Color normalColor;

		// Token: 0x040012F4 RID: 4852
		[SerializeField]
		private Color avaliableColor;

		// Token: 0x040012F5 RID: 4853
		private BuildingInfo info;
	}
}
