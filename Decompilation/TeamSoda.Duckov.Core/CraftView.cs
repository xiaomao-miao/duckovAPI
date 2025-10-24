using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001AA RID: 426
public class CraftView : View, ISingleSelectionMenu<CraftView_ListEntry>
{
	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00034C84 File Offset: 0x00032E84
	private static CraftView Instance
	{
		get
		{
			return View.GetViewInstance<CraftView>();
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00034C8C File Offset: 0x00032E8C
	private PrefabPool<CraftView_ListEntry> ListEntryPool
	{
		get
		{
			if (this._listEntryPool == null)
			{
				this._listEntryPool = new PrefabPool<CraftView_ListEntry>(this.listEntryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._listEntryPool;
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00034CC5 File Offset: 0x00032EC5
	private string NotificationFormat
	{
		get
		{
			return this.notificationFormatKey.ToPlainText();
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00034CD4 File Offset: 0x00032ED4
	private PrefabPool<CraftViewFilterBtnEntry> FilterBtnPool
	{
		get
		{
			if (this._filterBtnPool == null)
			{
				this._filterBtnPool = new PrefabPool<CraftViewFilterBtnEntry>(this.filterBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._filterBtnPool;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00034D0D File Offset: 0x00032F0D
	private CraftView.FilterInfo CurrentFilter
	{
		get
		{
			if (this.currentFilterIndex < 0 || this.currentFilterIndex >= this.filters.Length)
			{
				this.currentFilterIndex = 0;
			}
			return this.filters[this.currentFilterIndex];
		}
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x00034D40 File Offset: 0x00032F40
	public void SetFilter(int index)
	{
		if (index < 0 || index >= this.filters.Length)
		{
			return;
		}
		this.currentFilterIndex = index;
		this.selectedEntry = null;
		this.RefreshDetails();
		this.RefreshList(this.predicate);
		this.RefreshFilterButtons();
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00034D78 File Offset: 0x00032F78
	private static bool CheckFilter(CraftingFormula formula, CraftView.FilterInfo filter)
	{
		if (formula.result.id < 0)
		{
			return false;
		}
		if (filter.requireTags.Length == 0)
		{
			return true;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(formula.result.id);
		foreach (Tag value in filter.requireTags)
		{
			if (metaData.tags.Contains(value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00034DDB File Offset: 0x00032FDB
	protected override void Awake()
	{
		base.Awake();
		this.listEntryTemplate.gameObject.SetActive(false);
		this.craftButton.onClick.AddListener(new UnityAction(this.OnCraftButtonClicked));
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x00034E10 File Offset: 0x00033010
	private void OnCraftButtonClicked()
	{
		this.CraftTask().Forget();
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00034E20 File Offset: 0x00033020
	private UniTask CraftTask()
	{
		CraftView.<CraftTask>d__33 <CraftTask>d__;
		<CraftTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CraftTask>d__.<>4__this = this;
		<CraftTask>d__.<>1__state = -1;
		<CraftTask>d__.<>t__builder.Start<CraftView.<CraftTask>d__33>(ref <CraftTask>d__);
		return <CraftTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x00034E64 File Offset: 0x00033064
	private void OnCraftFinished(Item item)
	{
		if (item == null)
		{
			return;
		}
		string displayName = item.DisplayName;
		NotificationText.Push(this.NotificationFormat.Format(new
		{
			itemDisplayName = displayName
		}));
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00034E98 File Offset: 0x00033098
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.SetFilter(0);
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00034EB2 File Offset: 0x000330B2
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00034EC5 File Offset: 0x000330C5
	public static void SetupAndOpenView(Predicate<CraftingFormula> predicate)
	{
		if (!CraftView.Instance)
		{
			return;
		}
		CraftView.Instance.SetupAndOpen(predicate);
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00034EE0 File Offset: 0x000330E0
	public void SetupAndOpen(Predicate<CraftingFormula> predicate)
	{
		this.predicate = predicate;
		this.detailsFadeGroup.SkipHide();
		this.loadingIndicator.SkipHide();
		this.placeHolderFadeGroup.SkipShow();
		this.selectedEntry = null;
		this.RefreshDetails();
		this.RefreshList(predicate);
		this.RefreshFilterButtons();
		base.Open(null);
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00034F38 File Offset: 0x00033138
	private void RefreshList(Predicate<CraftingFormula> predicate)
	{
		this.ListEntryPool.ReleaseAll();
		IEnumerable<string> unlockedFormulaIDs = CraftingManager.UnlockedFormulaIDs;
		CraftView.FilterInfo currentFilter = this.CurrentFilter;
		bool flag = currentFilter.requireTags != null && currentFilter.requireTags.Length != 0;
		using (IEnumerator<string> enumerator = unlockedFormulaIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CraftingFormula craftingFormula;
				if (CraftingFormulaCollection.TryGetFormula(enumerator.Current, out craftingFormula) && predicate(craftingFormula) && (!flag || CraftView.CheckFilter(craftingFormula, currentFilter)))
				{
					this.ListEntryPool.Get(null).Setup(this, craftingFormula);
				}
			}
		}
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00034FD8 File Offset: 0x000331D8
	private int CountFilter(CraftView.FilterInfo filter)
	{
		IEnumerable<string> unlockedFormulaIDs = CraftingManager.UnlockedFormulaIDs;
		bool flag = filter.requireTags != null && filter.requireTags.Length != 0;
		int num = 0;
		using (IEnumerator<string> enumerator = unlockedFormulaIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CraftingFormula craftingFormula;
				if (CraftingFormulaCollection.TryGetFormula(enumerator.Current, out craftingFormula) && this.predicate(craftingFormula) && (!flag || CraftView.CheckFilter(craftingFormula, filter)))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0003505C File Offset: 0x0003325C
	private void RefreshFilterButtons()
	{
		this.FilterBtnPool.ReleaseAll();
		int num = 0;
		foreach (CraftView.FilterInfo filterInfo in this.filters)
		{
			if (this.CountFilter(filterInfo) < 1)
			{
				num++;
			}
			else
			{
				this.FilterBtnPool.Get(null).Setup(this, filterInfo, num, num == this.currentFilterIndex);
				num++;
			}
		}
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x000350C4 File Offset: 0x000332C4
	public CraftView_ListEntry GetSelection()
	{
		return this.selectedEntry;
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x000350CC File Offset: 0x000332CC
	public bool SetSelection(CraftView_ListEntry selection)
	{
		if (this.selectedEntry != null)
		{
			CraftView_ListEntry craftView_ListEntry = this.selectedEntry;
			this.selectedEntry = null;
			craftView_ListEntry.NotifyUnselected();
		}
		this.selectedEntry = selection;
		this.selectedEntry.NotifySelected();
		this.RefreshDetails();
		return true;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00035107 File Offset: 0x00033307
	private void RefreshDetails()
	{
		this.RefreshTask(this.NewRefreshToken()).Forget();
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0003511C File Offset: 0x0003331C
	private int NewRefreshToken()
	{
		int num;
		do
		{
			num = UnityEngine.Random.Range(0, int.MaxValue);
		}
		while (num == this.refreshTaskToken);
		this.refreshTaskToken = num;
		return num;
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00035148 File Offset: 0x00033348
	private UniTask RefreshTask(int token)
	{
		CraftView.<RefreshTask>d__50 <RefreshTask>d__;
		<RefreshTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<RefreshTask>d__.<>4__this = this;
		<RefreshTask>d__.token = token;
		<RefreshTask>d__.<>1__state = -1;
		<RefreshTask>d__.<>t__builder.Start<CraftView.<RefreshTask>d__50>(ref <RefreshTask>d__);
		return <RefreshTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00035193 File Offset: 0x00033393
	private void TestShow()
	{
		CraftingManager.UnlockFormula("Biscuit");
		CraftingManager.UnlockFormula("Character");
		this.SetupAndOpen((CraftingFormula e) => true);
	}

	// Token: 0x04000ADF RID: 2783
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private CraftView_ListEntry listEntryTemplate;

	// Token: 0x04000AE1 RID: 2785
	private PrefabPool<CraftView_ListEntry> _listEntryPool;

	// Token: 0x04000AE2 RID: 2786
	[SerializeField]
	private FadeGroup detailsFadeGroup;

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private FadeGroup loadingIndicator;

	// Token: 0x04000AE4 RID: 2788
	[SerializeField]
	private FadeGroup placeHolderFadeGroup;

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	private ItemDetailsDisplay detailsDisplay;

	// Token: 0x04000AE6 RID: 2790
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000AE7 RID: 2791
	[SerializeField]
	private Color crafableColor;

	// Token: 0x04000AE8 RID: 2792
	[SerializeField]
	private Color notCraftableColor;

	// Token: 0x04000AE9 RID: 2793
	[SerializeField]
	private Image buttonImage;

	// Token: 0x04000AEA RID: 2794
	[SerializeField]
	private Button craftButton;

	// Token: 0x04000AEB RID: 2795
	[LocalizationKey("Default")]
	[SerializeField]
	private string notificationFormatKey;

	// Token: 0x04000AEC RID: 2796
	[SerializeField]
	private CraftViewFilterBtnEntry filterBtnTemplate;

	// Token: 0x04000AED RID: 2797
	[SerializeField]
	private CraftView.FilterInfo[] filters;

	// Token: 0x04000AEE RID: 2798
	private PrefabPool<CraftViewFilterBtnEntry> _filterBtnPool;

	// Token: 0x04000AEF RID: 2799
	private int currentFilterIndex;

	// Token: 0x04000AF0 RID: 2800
	private bool crafting;

	// Token: 0x04000AF1 RID: 2801
	private Predicate<CraftingFormula> predicate;

	// Token: 0x04000AF2 RID: 2802
	private CraftView_ListEntry selectedEntry;

	// Token: 0x04000AF3 RID: 2803
	private int refreshTaskToken;

	// Token: 0x04000AF4 RID: 2804
	private Item tempItem;

	// Token: 0x020004C7 RID: 1223
	[Serializable]
	public struct FilterInfo
	{
		// Token: 0x04001CB6 RID: 7350
		[LocalizationKey("Default")]
		[SerializeField]
		public string displayNameKey;

		// Token: 0x04001CB7 RID: 7351
		[SerializeField]
		public Sprite icon;

		// Token: 0x04001CB8 RID: 7352
		[SerializeField]
		public Tag[] requireTags;
	}
}
