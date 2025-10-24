using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A5 RID: 933
	public static class ItemUIUtilities
	{
		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x0600215F RID: 8543 RVA: 0x00074830 File Offset: 0x00072A30
		// (remove) Token: 0x06002160 RID: 8544 RVA: 0x00074864 File Offset: 0x00072A64
		public static event Action OnSelectionChanged;

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06002161 RID: 8545 RVA: 0x00074898 File Offset: 0x00072A98
		// (remove) Token: 0x06002162 RID: 8546 RVA: 0x000748CC File Offset: 0x00072ACC
		public static event Action<Item> OnOrphanRaised;

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002163 RID: 8547 RVA: 0x000748FF File Offset: 0x00072AFF
		public static ItemDisplay SelectedItemDisplayRaw
		{
			get
			{
				return ItemUIUtilities.selectedItemDisplay;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x00074906 File Offset: 0x00072B06
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x00074930 File Offset: 0x00072B30
		public static ItemDisplay SelectedItemDisplay
		{
			get
			{
				if (ItemUIUtilities.selectedItemDisplay == null)
				{
					return null;
				}
				if (ItemUIUtilities.selectedItemDisplay.Target == null)
				{
					return null;
				}
				return ItemUIUtilities.selectedItemDisplay;
			}
			private set
			{
				ItemDisplay itemDisplay = ItemUIUtilities.selectedItemDisplay;
				if (itemDisplay != null)
				{
					itemDisplay.NotifyUnselected();
				}
				ItemUIUtilities.selectedItemDisplay = value;
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					ItemUIUtilities.selectedItemTypeID = -1;
				}
				else
				{
					ItemUIUtilities.selectedItemTypeID = selectedItem.TypeID;
					ItemUIUtilities.cachedSelectedItemMeta = ItemAssetsCollection.GetMetaData(ItemUIUtilities.selectedItemTypeID);
					ItemUIUtilities.cacheGunSelected = selectedItem.Tags.Contains("Gun");
				}
				ItemDisplay itemDisplay2 = ItemUIUtilities.selectedItemDisplay;
				if (itemDisplay2 != null)
				{
					itemDisplay2.NotifySelected();
				}
				Action onSelectionChanged = ItemUIUtilities.OnSelectionChanged;
				if (onSelectionChanged == null)
				{
					return;
				}
				onSelectionChanged();
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000749B8 File Offset: 0x00072BB8
		public static Item SelectedItem
		{
			get
			{
				if (ItemUIUtilities.SelectedItemDisplay == null)
				{
					return null;
				}
				return ItemUIUtilities.SelectedItemDisplay.Target;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002167 RID: 8551 RVA: 0x000749D3 File Offset: 0x00072BD3
		public static bool IsGunSelected
		{
			get
			{
				return !(ItemUIUtilities.SelectedItem == null) && ItemUIUtilities.cacheGunSelected;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000749E9 File Offset: 0x00072BE9
		public static string SelectedItemCaliber
		{
			get
			{
				return ItemUIUtilities.cachedSelectedItemMeta.caliber;
			}
		}

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06002169 RID: 8553 RVA: 0x000749F8 File Offset: 0x00072BF8
		// (remove) Token: 0x0600216A RID: 8554 RVA: 0x00074A2C File Offset: 0x00072C2C
		public static event Action<Item, bool> OnPutItem;

		// Token: 0x0600216B RID: 8555 RVA: 0x00074A5F File Offset: 0x00072C5F
		public static void Select(ItemDisplay itemDisplay)
		{
			ItemUIUtilities.SelectedItemDisplay = itemDisplay;
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x00074A67 File Offset: 0x00072C67
		public static void RaiseOrphan(Item orphan)
		{
			if (orphan == null)
			{
				return;
			}
			Action<Item> onOrphanRaised = ItemUIUtilities.OnOrphanRaised;
			if (onOrphanRaised != null)
			{
				onOrphanRaised(orphan);
			}
			Debug.LogWarning(string.Format("游戏中出现了孤儿Item {0}。", orphan));
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x00074A94 File Offset: 0x00072C94
		public static void NotifyPutItem(Item item, bool pickup = false)
		{
			Action<Item, bool> onPutItem = ItemUIUtilities.OnPutItem;
			if (onPutItem == null)
			{
				return;
			}
			onPutItem(item, pickup);
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x00074AA8 File Offset: 0x00072CA8
		public static string GetPropertiesDisplayText(this Item item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (item.Variables != null)
			{
				foreach (CustomData customData in item.Variables)
				{
					if (customData.Display)
					{
						stringBuilder.AppendLine(customData.DisplayName + "\t" + customData.GetValueDisplayString(""));
					}
				}
			}
			if (item.Constants != null)
			{
				foreach (CustomData customData2 in item.Constants)
				{
					if (customData2.Display)
					{
						stringBuilder.AppendLine(customData2.DisplayName + "\t" + customData2.GetValueDisplayString(""));
					}
				}
			}
			if (item.Stats != null)
			{
				foreach (Stat stat in item.Stats)
				{
					if (stat.Display)
					{
						stringBuilder.AppendLine(string.Format("{0}\t{1}", stat.DisplayName, stat.Value));
					}
				}
			}
			if (item.Modifiers != null)
			{
				foreach (ModifierDescription modifierDescription in item.Modifiers)
				{
					if (modifierDescription.Display)
					{
						stringBuilder.AppendLine(modifierDescription.DisplayName + "\t" + modifierDescription.GetDisplayValueString("0.##"));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00074C80 File Offset: 0x00072E80
		[return: TupleElementNames(new string[]
		{
			"name",
			"value",
			"polarity"
		})]
		public static List<ValueTuple<string, string, Polarity>> GetPropertyValueTextPair(this Item item)
		{
			List<ValueTuple<string, string, Polarity>> list = new List<ValueTuple<string, string, Polarity>>();
			if (item.Variables != null)
			{
				foreach (CustomData customData in item.Variables)
				{
					if (customData.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(customData.DisplayName, customData.GetValueDisplayString(""), Polarity.Neutral));
					}
				}
			}
			if (item.Constants != null)
			{
				foreach (CustomData customData2 in item.Constants)
				{
					if (customData2.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(customData2.DisplayName, customData2.GetValueDisplayString(""), Polarity.Neutral));
					}
				}
			}
			if (item.Stats != null)
			{
				foreach (Stat stat in item.Stats)
				{
					if (stat.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(stat.DisplayName, stat.Value.ToString(), Polarity.Neutral));
					}
				}
			}
			if (item.Modifiers != null)
			{
				foreach (ModifierDescription modifierDescription in item.Modifiers)
				{
					if (modifierDescription.Display)
					{
						Polarity polarity = StatInfoDatabase.GetPolarity(modifierDescription.Key);
						if (modifierDescription.Value < 0f)
						{
							polarity = -polarity;
						}
						list.Add(new ValueTuple<string, string, Polarity>(modifierDescription.DisplayName, modifierDescription.GetDisplayValueString("0.##"), polarity));
					}
				}
			}
			return list;
		}

		// Token: 0x040016A7 RID: 5799
		private static ItemDisplay selectedItemDisplay;

		// Token: 0x040016A8 RID: 5800
		private static bool cacheGunSelected;

		// Token: 0x040016A9 RID: 5801
		private static int selectedItemTypeID;

		// Token: 0x040016AA RID: 5802
		private static ItemMetaData cachedSelectedItemMeta;
	}
}
