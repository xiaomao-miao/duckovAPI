using System;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000390 RID: 912
	public class ItemDetailsDisplay : MonoBehaviour
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x0006FFFB File Offset: 0x0006E1FB
		private string DurabilityToolTipsFormat
		{
			get
			{
				return this.durabilityToolTipsFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x00070008 File Offset: 0x0006E208
		public ItemSlotCollectionDisplay SlotCollectionDisplay
		{
			get
			{
				return this.slotCollectionDisplay;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x00070010 File Offset: 0x0006E210
		private PrefabPool<ItemVariableEntry> VariablePool
		{
			get
			{
				if (this._variablePool == null)
				{
					this._variablePool = new PrefabPool<ItemVariableEntry>(this.variableEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._variablePool;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600200B RID: 8203 RVA: 0x00070050 File Offset: 0x0006E250
		private PrefabPool<ItemStatEntry> StatPool
		{
			get
			{
				if (this._statPool == null)
				{
					this._statPool = new PrefabPool<ItemStatEntry>(this.statEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._statPool;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x00070090 File Offset: 0x0006E290
		private PrefabPool<ItemModifierEntry> ModifierPool
		{
			get
			{
				if (this._modifierPool == null)
				{
					this._modifierPool = new PrefabPool<ItemModifierEntry>(this.modifierEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._modifierPool;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x000700D0 File Offset: 0x0006E2D0
		private PrefabPool<ItemEffectEntry> EffectPool
		{
			get
			{
				if (this._effectPool == null)
				{
					this._effectPool = new PrefabPool<ItemEffectEntry>(this.effectEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._effectPool;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x0007010E File Offset: 0x0006E30E
		public Item Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00070118 File Offset: 0x0006E318
		internal void Setup(Item target)
		{
			this.UnregisterEvents();
			this.Clear();
			if (target == null)
			{
				return;
			}
			this.target = target;
			this.icon.sprite = target.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.displayName.text = target.DisplayName;
			this.itemID.text = string.Format("#{0}", target.TypeID);
			this.description.text = target.Description;
			this.countContainer.SetActive(target.Stackable);
			this.count.text = target.StackCount.ToString();
			this.tagsDisplay.Setup(target);
			this.usageUtilitiesDisplay.Setup(target);
			this.usableIndicator.gameObject.SetActive(target.UsageUtilities != null);
			this.RefreshDurability();
			this.slotCollectionDisplay.Setup(target, false);
			this.registeredIndicator.SetActive(target.IsRegistered());
			this.RefreshWeightText();
			this.SetupGunDisplays();
			this.SetupVariables();
			this.SetupConstants();
			this.SetupStats();
			this.SetupModifiers();
			this.SetupEffects();
			this.RegisterEvents();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00070297 File Offset: 0x0006E497
		private void Awake()
		{
			this.SlotCollectionDisplay.onElementDoubleClicked += this.OnElementDoubleClicked;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000702B0 File Offset: 0x0006E4B0
		private void OnElementDoubleClicked(ItemSlotCollectionDisplay collectionDisplay, SlotDisplay slotDisplay)
		{
			if (!collectionDisplay.Editable)
			{
				return;
			}
			Item item = slotDisplay.GetItem();
			if (item == null)
			{
				return;
			}
			ItemUtilities.SendToPlayer(item, false, PlayerStorage.Instance != null);
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000702E9 File Offset: 0x0006E4E9
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000702F1 File Offset: 0x0006E4F1
		private void Clear()
		{
			this.tagsDisplay.Clear();
			this.VariablePool.ReleaseAll();
			this.StatPool.ReleaseAll();
			this.ModifierPool.ReleaseAll();
			this.EffectPool.ReleaseAll();
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0007032C File Offset: 0x0006E52C
		private void SetupGunDisplays()
		{
			Item item = this.Target;
			ItemSetting_Gun itemSetting_Gun = (item != null) ? item.GetComponent<ItemSetting_Gun>() : null;
			if (itemSetting_Gun == null)
			{
				this.bulletTypeDisplay.gameObject.SetActive(false);
				return;
			}
			this.bulletTypeDisplay.gameObject.SetActive(true);
			this.bulletTypeDisplay.Setup(itemSetting_Gun.TargetBulletID);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0007038C File Offset: 0x0006E58C
		private void SetupVariables()
		{
			if (this.target.Variables == null)
			{
				return;
			}
			foreach (CustomData customData in this.target.Variables)
			{
				if (customData.Display)
				{
					ItemVariableEntry itemVariableEntry = this.VariablePool.Get(this.propertiesParent);
					itemVariableEntry.Setup(customData);
					itemVariableEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x00070410 File Offset: 0x0006E610
		private void SetupConstants()
		{
			if (this.target.Constants == null)
			{
				return;
			}
			foreach (CustomData customData in this.target.Constants)
			{
				if (customData.Display)
				{
					ItemVariableEntry itemVariableEntry = this.VariablePool.Get(this.propertiesParent);
					itemVariableEntry.Setup(customData);
					itemVariableEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00070494 File Offset: 0x0006E694
		private void SetupStats()
		{
			if (this.target.Stats == null)
			{
				return;
			}
			foreach (Stat stat in this.target.Stats)
			{
				if (stat.Display)
				{
					ItemStatEntry itemStatEntry = this.StatPool.Get(this.propertiesParent);
					itemStatEntry.Setup(stat);
					itemStatEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x00070520 File Offset: 0x0006E720
		private void SetupModifiers()
		{
			if (this.target.Modifiers == null)
			{
				return;
			}
			foreach (ModifierDescription modifierDescription in this.target.Modifiers)
			{
				if (modifierDescription.Display)
				{
					ItemModifierEntry itemModifierEntry = this.ModifierPool.Get(this.propertiesParent);
					itemModifierEntry.Setup(modifierDescription);
					itemModifierEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000705AC File Offset: 0x0006E7AC
		private void SetupEffects()
		{
			foreach (Effect effect in this.target.Effects)
			{
				if (effect.Display)
				{
					ItemEffectEntry itemEffectEntry = this.EffectPool.Get(this.propertiesParent);
					itemEffectEntry.Setup(effect);
					itemEffectEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x00070628 File Offset: 0x0006E828
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onDestroy += this.OnTargetDestroy;
			this.target.onChildChanged += this.OnTargetChildChanged;
			this.target.onSetStackCount += this.OnTargetSetStackCount;
			this.target.onDurabilityChanged += this.OnTargetDurabilityChanged;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000706A0 File Offset: 0x0006E8A0
		private void RefreshWeightText()
		{
			this.weightText.text = string.Format(this.weightFormat, this.target.TotalWeight);
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000706C8 File Offset: 0x0006E8C8
		private void OnTargetSetStackCount(Item item)
		{
			this.RefreshWeightText();
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000706D0 File Offset: 0x0006E8D0
		private void OnTargetChildChanged(Item obj)
		{
			this.RefreshWeightText();
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000706D8 File Offset: 0x0006E8D8
		internal void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onDestroy -= this.OnTargetDestroy;
			this.target.onChildChanged -= this.OnTargetChildChanged;
			this.target.onSetStackCount -= this.OnTargetSetStackCount;
			this.target.onDurabilityChanged -= this.OnTargetDurabilityChanged;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x00070750 File Offset: 0x0006E950
		private void OnTargetDurabilityChanged(Item item)
		{
			this.RefreshDurability();
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00070758 File Offset: 0x0006E958
		private void RefreshDurability()
		{
			bool useDurability = this.target.UseDurability;
			this.durabilityContainer.SetActive(useDurability);
			if (useDurability)
			{
				float durability = this.target.Durability;
				float maxDurability = this.target.MaxDurability;
				float maxDurabilityWithLoss = this.target.MaxDurabilityWithLoss;
				string lossPercentage = string.Format("{0:0}%", this.target.DurabilityLoss * 100f);
				float num = durability / maxDurability;
				this.durabilityText.text = string.Format("{0:0} / {1:0}", durability, maxDurabilityWithLoss);
				this.durabilityToolTips.text = this.DurabilityToolTipsFormat.Format(new
				{
					curDurability = durability,
					maxDurability = maxDurability,
					maxDurabilityWithLoss = maxDurabilityWithLoss,
					lossPercentage = lossPercentage
				});
				this.durabilityFill.fillAmount = num;
				this.durabilityFill.color = this.durabilityColorOverT.Evaluate(num);
				this.durabilityLoss.fillAmount = this.target.DurabilityLoss;
			}
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0007084A File Offset: 0x0006EA4A
		private void OnTargetDestroy(Item item)
		{
		}

		// Token: 0x040015D0 RID: 5584
		[SerializeField]
		private Image icon;

		// Token: 0x040015D1 RID: 5585
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x040015D2 RID: 5586
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040015D3 RID: 5587
		[SerializeField]
		private TextMeshProUGUI itemID;

		// Token: 0x040015D4 RID: 5588
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x040015D5 RID: 5589
		[SerializeField]
		private GameObject countContainer;

		// Token: 0x040015D6 RID: 5590
		[SerializeField]
		private TextMeshProUGUI count;

		// Token: 0x040015D7 RID: 5591
		[SerializeField]
		private GameObject durabilityContainer;

		// Token: 0x040015D8 RID: 5592
		[SerializeField]
		private TextMeshProUGUI durabilityText;

		// Token: 0x040015D9 RID: 5593
		[SerializeField]
		private TooltipsProvider durabilityToolTips;

		// Token: 0x040015DA RID: 5594
		[SerializeField]
		[LocalizationKey("Default")]
		private string durabilityToolTipsFormatKey = "UI_DurabilityToolTips";

		// Token: 0x040015DB RID: 5595
		[SerializeField]
		private Image durabilityFill;

		// Token: 0x040015DC RID: 5596
		[SerializeField]
		private Image durabilityLoss;

		// Token: 0x040015DD RID: 5597
		[SerializeField]
		private Gradient durabilityColorOverT;

		// Token: 0x040015DE RID: 5598
		[SerializeField]
		private TextMeshProUGUI weightText;

		// Token: 0x040015DF RID: 5599
		[SerializeField]
		private ItemSlotCollectionDisplay slotCollectionDisplay;

		// Token: 0x040015E0 RID: 5600
		[SerializeField]
		private RectTransform propertiesParent;

		// Token: 0x040015E1 RID: 5601
		[SerializeField]
		private BulletTypeDisplay bulletTypeDisplay;

		// Token: 0x040015E2 RID: 5602
		[SerializeField]
		private TagsDisplay tagsDisplay;

		// Token: 0x040015E3 RID: 5603
		[SerializeField]
		private GameObject usableIndicator;

		// Token: 0x040015E4 RID: 5604
		[SerializeField]
		private UsageUtilitiesDisplay usageUtilitiesDisplay;

		// Token: 0x040015E5 RID: 5605
		[SerializeField]
		private GameObject registeredIndicator;

		// Token: 0x040015E6 RID: 5606
		[SerializeField]
		private ItemVariableEntry variableEntryPrefab;

		// Token: 0x040015E7 RID: 5607
		[SerializeField]
		private ItemStatEntry statEntryPrefab;

		// Token: 0x040015E8 RID: 5608
		[SerializeField]
		private ItemModifierEntry modifierEntryPrefab;

		// Token: 0x040015E9 RID: 5609
		[SerializeField]
		private ItemEffectEntry effectEntryPrefab;

		// Token: 0x040015EA RID: 5610
		[SerializeField]
		private string weightFormat = "{0:0.#} kg";

		// Token: 0x040015EB RID: 5611
		private Item target;

		// Token: 0x040015EC RID: 5612
		private PrefabPool<ItemVariableEntry> _variablePool;

		// Token: 0x040015ED RID: 5613
		private PrefabPool<ItemStatEntry> _statPool;

		// Token: 0x040015EE RID: 5614
		private PrefabPool<ItemModifierEntry> _modifierPool;

		// Token: 0x040015EF RID: 5615
		private PrefabPool<ItemEffectEntry> _effectPool;
	}
}
