using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B4 RID: 948
	public class ItemDecomposeView : View
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x00077152 File Offset: 0x00075352
		public static ItemDecomposeView Instance
		{
			get
			{
				return View.GetViewInstance<ItemDecomposeView>();
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x00077159 File Offset: 0x00075359
		private Item SelectedItem
		{
			get
			{
				return ItemUIUtilities.SelectedItem;
			}
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00077160 File Offset: 0x00075360
		protected override void Awake()
		{
			base.Awake();
			this.decomposeButton.onClick.AddListener(new UnityAction(this.OnDecomposeButtonClick));
			this.countSlider.OnValueChangedEvent += this.OnSliderValueChanged;
			this.SetupEmpty();
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000771AC File Offset: 0x000753AC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.countSlider.OnValueChangedEvent -= this.OnSliderValueChanged;
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000771CC File Offset: 0x000753CC
		private void OnDecomposeButtonClick()
		{
			if (this.decomposing)
			{
				return;
			}
			if (this.SelectedItem == null)
			{
				return;
			}
			int value = this.countSlider.Value;
			this.DecomposeTask(this.SelectedItem, value).Forget();
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x0007720F File Offset: 0x0007540F
		private void OnFastPick(UIInputEventData data)
		{
			this.OnDecomposeButtonClick();
			data.Use();
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x00077220 File Offset: 0x00075420
		private UniTask DecomposeTask(Item item, int count)
		{
			ItemDecomposeView.<DecomposeTask>d__21 <DecomposeTask>d__;
			<DecomposeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DecomposeTask>d__.<>4__this = this;
			<DecomposeTask>d__.item = item;
			<DecomposeTask>d__.count = count;
			<DecomposeTask>d__.<>1__state = -1;
			<DecomposeTask>d__.<>t__builder.Start<ItemDecomposeView.<DecomposeTask>d__21>(ref <DecomposeTask>d__);
			return <DecomposeTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00077274 File Offset: 0x00075474
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			ItemUIUtilities.Select(null);
			this.detailsFadeGroup.SkipHide();
			if (CharacterMainControl.Main != null)
			{
				this.characterInventoryDisplay.gameObject.SetActive(true);
				this.characterInventoryDisplay.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, (Item e) => e == null || DecomposeDatabase.CanDecompose(e.TypeID), false, null);
			}
			else
			{
				this.characterInventoryDisplay.gameObject.SetActive(false);
			}
			if (PlayerStorage.Inventory != null)
			{
				this.storageDisplay.gameObject.SetActive(true);
				this.storageDisplay.Setup(PlayerStorage.Inventory, null, (Item e) => e == null || DecomposeDatabase.CanDecompose(e.TypeID), false, null);
			}
			else
			{
				this.storageDisplay.gameObject.SetActive(false);
			}
			this.Refresh();
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00077379 File Offset: 0x00075579
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x0007738C File Offset: 0x0007558C
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			UIInputManager.OnFastPick += this.OnFastPick;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000773B0 File Offset: 0x000755B0
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			UIInputManager.OnFastPick -= this.OnFastPick;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000773D4 File Offset: 0x000755D4
		private void OnSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000773DC File Offset: 0x000755DC
		private void OnSliderValueChanged(float value)
		{
			this.RefreshResult(this.SelectedItem, Mathf.RoundToInt(value));
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000773F0 File Offset: 0x000755F0
		private void Refresh()
		{
			if (this.SelectedItem == null)
			{
				this.SetupEmpty();
				return;
			}
			this.Setup(this.SelectedItem);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00077414 File Offset: 0x00075614
		private void SetupEmpty()
		{
			this.detailsFadeGroup.Hide();
			this.targetNameDisplay.text = "-";
			this.resultDisplay.Clear();
			this.cannotDecomposeIndicator.SetActive(false);
			this.decomposeButton.gameObject.SetActive(false);
			this.noItemSelectedIndicator.SetActive(true);
			this.busyIndicator.SetActive(false);
			this.countSlider.SetMinMax(1, 1);
			this.countSlider.Value = 1;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00077498 File Offset: 0x00075698
		private void Setup(Item selectedItem)
		{
			if (selectedItem == null)
			{
				return;
			}
			this.noItemSelectedIndicator.SetActive(false);
			this.detailsDisplay.Setup(selectedItem);
			this.detailsFadeGroup.Show();
			this.targetNameDisplay.text = selectedItem.DisplayName;
			bool valid = DecomposeDatabase.GetDecomposeFormula(selectedItem.TypeID).valid;
			this.decomposeButton.gameObject.SetActive(valid);
			this.cannotDecomposeIndicator.gameObject.SetActive(!valid);
			this.SetupSlider(selectedItem);
			this.RefreshResult(selectedItem, Mathf.RoundToInt((float)this.countSlider.Value));
			this.busyIndicator.SetActive(this.decomposing);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0007754C File Offset: 0x0007574C
		private void SetupSlider(Item selectedItem)
		{
			if (selectedItem.Stackable)
			{
				this.countSlider.SetMinMax(1, selectedItem.StackCount);
				this.countSlider.Value = selectedItem.StackCount;
				return;
			}
			this.countSlider.SetMinMax(1, 1);
			this.countSlider.Value = 1;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000775A0 File Offset: 0x000757A0
		private void RefreshResult(Item selectedItem, int count)
		{
			if (selectedItem == null)
			{
				this.countSlider.SetMinMax(1, 1);
				this.countSlider.Value = 1;
				return;
			}
			DecomposeFormula decomposeFormula = DecomposeDatabase.GetDecomposeFormula(selectedItem.TypeID);
			if (decomposeFormula.valid)
			{
				bool stackable = selectedItem.Stackable;
				this.resultDisplay.Setup(decomposeFormula.result, count);
				return;
			}
			this.resultDisplay.Clear();
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0007760C File Offset: 0x0007580C
		internal static void Show()
		{
			ItemDecomposeView instance = ItemDecomposeView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Open(null);
		}

		// Token: 0x0400172C RID: 5932
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400172D RID: 5933
		[SerializeField]
		private InventoryDisplay characterInventoryDisplay;

		// Token: 0x0400172E RID: 5934
		[SerializeField]
		private InventoryDisplay storageDisplay;

		// Token: 0x0400172F RID: 5935
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04001730 RID: 5936
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001731 RID: 5937
		[SerializeField]
		private DecomposeSlider countSlider;

		// Token: 0x04001732 RID: 5938
		[SerializeField]
		private TextMeshProUGUI targetNameDisplay;

		// Token: 0x04001733 RID: 5939
		[SerializeField]
		private CostDisplay resultDisplay;

		// Token: 0x04001734 RID: 5940
		[SerializeField]
		private GameObject cannotDecomposeIndicator;

		// Token: 0x04001735 RID: 5941
		[SerializeField]
		private GameObject noItemSelectedIndicator;

		// Token: 0x04001736 RID: 5942
		[SerializeField]
		private Button decomposeButton;

		// Token: 0x04001737 RID: 5943
		[SerializeField]
		private GameObject busyIndicator;

		// Token: 0x04001738 RID: 5944
		private bool decomposing;
	}
}
