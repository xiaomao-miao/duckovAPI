using System;
using System.Text.RegularExpressions;
using Duckov.Utilities;
using ItemStatsSystem.Stats;
using SodaCraft.Localizations;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200001E RID: 30
	[Serializable]
	public class ModifierDescription
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00006FBC File Offset: 0x000051BC
		private Item Master
		{
			get
			{
				ModifierDescriptionCollection modifierDescriptionCollection = this.collection;
				if (modifierDescriptionCollection == null)
				{
					return null;
				}
				return modifierDescriptionCollection.Master;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006FCF File Offset: 0x000051CF
		private StringList referenceStatKeys
		{
			get
			{
				return StringLists.StatKeys;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006FD6 File Offset: 0x000051D6
		private string displayNamekey
		{
			get
			{
				return "Stat_" + this.key;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006FE8 File Offset: 0x000051E8
		private int ResultOrder
		{
			get
			{
				if (this.overrideOrder)
				{
					return this.order;
				}
				return (int)this.type;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00006FFF File Offset: 0x000051FF
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007007 File Offset: 0x00005207
		public ModifierType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000700F File Offset: 0x0000520F
		public float Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007017 File Offset: 0x00005217
		public bool IsOverrideOrder
		{
			get
			{
				return this.overrideOrder;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000701F File Offset: 0x0000521F
		public int Order
		{
			get
			{
				return this.order;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007027 File Offset: 0x00005227
		public bool Display
		{
			get
			{
				return this.display;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000702F File Offset: 0x0000522F
		public string DisplayName
		{
			get
			{
				return this.displayNamekey.ToPlainText();
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000703C File Offset: 0x0000523C
		public Modifier CreateModifier(object source)
		{
			return new Modifier(this.type, this.value, this.overrideOrder, this.order, source);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000705C File Offset: 0x0000525C
		public void ReapplyModifier(ModifierDescriptionCollection collection)
		{
			if (this.collection != null && collection != this.collection)
			{
				Debug.LogWarning("One Modifier Description seem to be used in different collections! This could cause errors in the future.");
			}
			this.collection = collection;
			if (this.activeModifier != null)
			{
				this.activeModifier.RemoveFromTarget();
			}
			Item targetItem = this.GetTargetItem();
			if (targetItem == null)
			{
				return;
			}
			if (targetItem.Stats == null)
			{
				return;
			}
			Stat stat = targetItem.Stats.GetStat(this.key);
			if (stat == null)
			{
				Stat stat2 = new Stat(this.key, 0f, false);
				targetItem.Stats.Add(stat2);
				stat = stat2;
			}
			Modifier modifier = this.CreateModifier(this.Master);
			stat.AddModifier(modifier);
			this.activeModifier = modifier;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000711C File Offset: 0x0000531C
		public Item GetTargetItem()
		{
			if (this.Master == null)
			{
				return null;
			}
			if (this.target == ModifierTarget.Self)
			{
				return this.Master;
			}
			if (!this.enableInInventory)
			{
				if (this.target == ModifierTarget.Character && !this.Master.IsInCharacterSlot())
				{
					return null;
				}
				if (this.target == ModifierTarget.Parent && this.Master.PluggedIntoSlot == null)
				{
					return null;
				}
			}
			ModifierTarget modifierTarget = this.target;
			if (modifierTarget == ModifierTarget.Parent)
			{
				return this.Master.ParentItem;
			}
			if (modifierTarget != ModifierTarget.Character)
			{
				Debug.LogWarning("Invalid Modifier Target Type!");
				return null;
			}
			return this.Master.GetCharacterItem();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000071B4 File Offset: 0x000053B4
		public ModifierDescription(ModifierTarget target, string key, ModifierType type, float value, bool overrideOrder = false, int overrideOrderValue = 0)
		{
			this.target = target;
			this.key = key;
			this.type = type;
			this.value = value;
			this.overrideOrder = overrideOrder;
			this.order = overrideOrderValue;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000071F0 File Offset: 0x000053F0
		public ModifierDescription()
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007200 File Offset: 0x00005400
		public static ModifierDescription FromString(string str)
		{
			ModifierDescription modifierDescription = new ModifierDescription();
			string text = str;
			str = str.Trim();
			Match match = ModifierDescription.reg.Match(str);
			if (!match.Success)
			{
				Debug.LogError("无法解析Modifier: " + text);
				return null;
			}
			GroupCollection groups = match.Groups;
			string text2 = groups["instructions"].Value;
			string text3 = groups["Target"].Value;
			string text4 = groups["Key"].Value;
			string text5 = groups["Operation"].Value;
			string text6 = groups["Value"].Value;
			modifierDescription.display = true;
			foreach (string text7 in text2.Split(']', StringSplitOptions.None))
			{
				if (!string.IsNullOrWhiteSpace(text7) && text7.Trim(new char[]
				{
					'[',
					']'
				}) == "hide")
				{
					modifierDescription.display = false;
				}
			}
			if (!(text3 == "Self"))
			{
				if (!(text3 == "Parent"))
				{
					if (!(text3 == "Character"))
					{
						Debug.LogError("无法解析Modifier目标 " + text3 + "\n" + text);
						return null;
					}
					modifierDescription.target = ModifierTarget.Character;
				}
				else
				{
					modifierDescription.target = ModifierTarget.Parent;
				}
			}
			else
			{
				modifierDescription.target = ModifierTarget.Self;
			}
			modifierDescription.key = text4;
			bool flag = false;
			if (!(text5 == "+"))
			{
				if (!(text5 == "-"))
				{
					if (!(text5 == "*+"))
					{
						if (!(text5 == "*-"))
						{
							if (!(text5 == "*"))
							{
								Debug.LogError("无法解析Modifier的operation: " + text5 + " \n" + text);
								return null;
							}
							modifierDescription.type = ModifierType.PercentageMultiply;
						}
						else
						{
							modifierDescription.type = ModifierType.PercentageAdd;
							flag = true;
						}
					}
					else
					{
						modifierDescription.type = ModifierType.PercentageAdd;
					}
				}
				else
				{
					modifierDescription.type = ModifierType.Add;
					flag = true;
				}
			}
			else
			{
				modifierDescription.type = ModifierType.Add;
			}
			float num;
			if (!float.TryParse(text6, out num))
			{
				Debug.LogError("无法解析Modifier的Value: " + text6 + " \n" + text);
			}
			if (flag)
			{
				num = -num;
			}
			modifierDescription.value = num;
			return modifierDescription;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007439 File Offset: 0x00005639
		internal void Release()
		{
			if (this.activeModifier == null)
			{
				return;
			}
			this.activeModifier.RemoveFromTarget();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007450 File Offset: 0x00005650
		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3} {4}", new object[]
			{
				this.target,
				this.key,
				this.type,
				this.value,
				this.overrideOrder ? "" : string.Format(" override order:{0}", this.order)
			});
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000074C8 File Offset: 0x000056C8
		public string GetDisplayValueString(string format = "0.##")
		{
			ModifierType modifierType = this.type;
			if (modifierType == ModifierType.Add)
			{
				return ((this.Value > 0f) ? "+" : "") + this.Value.ToString(format);
			}
			if (modifierType == ModifierType.PercentageAdd)
			{
				return string.Format("{0}{1:0.##}%", (this.Value > 0f) ? "+" : "", this.Value * 100f);
			}
			if (modifierType != ModifierType.PercentageMultiply)
			{
				return string.Format("?{0}", this.Value);
			}
			return string.Format("x{0:0.##}%", 100f + this.Value * 100f);
		}

		// Token: 0x0400008C RID: 140
		[NonSerialized]
		private ModifierDescriptionCollection collection;

		// Token: 0x0400008D RID: 141
		[SerializeField]
		private ModifierTarget target = ModifierTarget.Parent;

		// Token: 0x0400008E RID: 142
		[Tooltip("在背包中是否生效")]
		[SerializeField]
		private bool enableInInventory;

		// Token: 0x0400008F RID: 143
		[Tooltip("Target Stat Key")]
		[SerializeField]
		private string key;

		// Token: 0x04000090 RID: 144
		[Tooltip("Target Stat Key")]
		[SerializeField]
		private bool display;

		// Token: 0x04000091 RID: 145
		[SerializeField]
		private ModifierType type;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		private float value;

		// Token: 0x04000093 RID: 147
		[Tooltip("Order Override")]
		[SerializeField]
		private bool overrideOrder;

		// Token: 0x04000094 RID: 148
		[SerializeField]
		private int order;

		// Token: 0x04000095 RID: 149
		private Modifier activeModifier;

		// Token: 0x04000096 RID: 150
		private static Regex reg = new Regex("(?'instructions'(\\[\\w+\\])*)(?'Target'[a-zA-Z]+)/(?'Key'[a-zA-Z_]+)\\s*(?'Operation'[*+-]+)\\s*(?'Value'[-\\d\\.]+)");
	}
}
