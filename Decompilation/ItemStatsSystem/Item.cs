using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem.Items;
using ItemStatsSystem.Stats;
using Sirenix.OdinInspector;
using SodaCraft.Localizations;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200001C RID: 28
	public class Item : MonoBehaviour, ISelfValidator
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004F4E File Offset: 0x0000314E
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004F56 File Offset: 0x00003156
		public int TypeID
		{
			get
			{
				return this.typeID;
			}
			internal set
			{
				this.typeID = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004F5F File Offset: 0x0000315F
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004F67 File Offset: 0x00003167
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004F70 File Offset: 0x00003170
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004F7D File Offset: 0x0000317D
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004F85 File Offset: 0x00003185
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
			set
			{
				this.displayName = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004F8E File Offset: 0x0000318E
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004FA0 File Offset: 0x000031A0
		[LocalizationKey("Items")]
		private string description
		{
			get
			{
				return this.displayName + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004FA2 File Offset: 0x000031A2
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004FAA File Offset: 0x000031AA
		public string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004FB7 File Offset: 0x000031B7
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004FBF File Offset: 0x000031BF
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004FC8 File Offset: 0x000031C8
		private string MaxStackCountSuffixLabel
		{
			get
			{
				if (this.MaxStackCount <= 1)
				{
					return "不可堆叠";
				}
				return "可堆叠";
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004FDE File Offset: 0x000031DE
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004FE6 File Offset: 0x000031E6
		public int MaxStackCount
		{
			get
			{
				return this.maxStackCount;
			}
			set
			{
				this.maxStackCount = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004FEF File Offset: 0x000031EF
		public bool Stackable
		{
			get
			{
				return this.MaxStackCount > 1;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004FFA File Offset: 0x000031FA
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005002 File Offset: 0x00003202
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000500B File Offset: 0x0000320B
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005013 File Offset: 0x00003213
		public int Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000501C File Offset: 0x0000321C
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005024 File Offset: 0x00003224
		public DisplayQuality DisplayQuality
		{
			get
			{
				return this.displayQuality;
			}
			set
			{
				this.displayQuality = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000502D File Offset: 0x0000322D
		public float UnitSelfWeight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005035 File Offset: 0x00003235
		public float SelfWeight
		{
			get
			{
				return this.weight * (float)this.StackCount;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005045 File Offset: 0x00003245
		public bool Sticky
		{
			get
			{
				return this.Tags.Contains("Sticky");
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005057 File Offset: 0x00003257
		public bool CanBeSold
		{
			get
			{
				return !this.Sticky && !this.Tags.Contains("NotSellable");
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005076 File Offset: 0x00003276
		public bool CanDrop
		{
			get
			{
				return !this.Sticky;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005084 File Offset: 0x00003284
		public float TotalWeight
		{
			get
			{
				if (this._cachedTotalWeight == null || this._cachedWeight != this.SelfWeight)
				{
					this._cachedWeight = this.SelfWeight;
					this._cachedTotalWeight = new float?(this.RecalculateTotalWeight());
				}
				return this._cachedTotalWeight.Value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000050D4 File Offset: 0x000032D4
		public bool HasHandHeldAgent
		{
			get
			{
				return this.AgentUtilities.GetPrefab(this.handheldHash) != null;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000050F0 File Offset: 0x000032F0
		private string TagsLabelText
		{
			get
			{
				string text = "Tags: ";
				bool flag = true;
				foreach (Tag tag in this.tags)
				{
					text = text + (flag ? "" : ", ") + tag.DisplayName;
					flag = false;
				}
				return text;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005160 File Offset: 0x00003360
		public ItemAgentUtilities AgentUtilities
		{
			get
			{
				return this.agentUtilities;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00005168 File Offset: 0x00003368
		public ItemAgent ActiveAgent
		{
			get
			{
				return this.agentUtilities.ActiveAgent;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005175 File Offset: 0x00003375
		public ItemGraphicInfo ItemGraphic
		{
			get
			{
				return this.itemGraphic;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000517D File Offset: 0x0000337D
		private string StatsTabLabelText
		{
			get
			{
				if (!this.stats)
				{
					return "No Stats";
				}
				return string.Format("Stats({0})", this.stats.Count);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000051AC File Offset: 0x000033AC
		[SerializeField]
		private void CreateStatsComponent()
		{
			StatCollection statCollection = base.gameObject.AddComponent<StatCollection>();
			this.stats = statCollection;
			statCollection.Master = this;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000051D3 File Offset: 0x000033D3
		private string SlotsTabLabelText
		{
			get
			{
				if (!this.slots)
				{
					return "No Slots";
				}
				return string.Format("Slots({0})", this.slots.Count);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005204 File Offset: 0x00003404
		[SerializeField]
		public void CreateSlotsComponent()
		{
			if (this.slots != null)
			{
				Debug.LogError("Slot component已存在");
				return;
			}
			SlotCollection slotCollection = base.gameObject.AddComponent<SlotCollection>();
			this.slots = slotCollection;
			slotCollection.Master = this;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005244 File Offset: 0x00003444
		private string ModifiersTabLabelText
		{
			get
			{
				if (!this.modifiers)
				{
					return "No Modifiers";
				}
				return string.Format("Modifiers({0})", this.modifiers.Count);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005274 File Offset: 0x00003474
		[SerializeField]
		public void CreateModifiersComponent()
		{
			if (this.modifiers == null)
			{
				ModifierDescriptionCollection modifierDescriptionCollection = base.gameObject.AddComponent<ModifierDescriptionCollection>();
				this.modifiers = modifierDescriptionCollection;
			}
			this.modifiers.Master = this;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000052B0 File Offset: 0x000034B0
		[SerializeField]
		private void CreateInventoryComponent()
		{
			Inventory inventory = base.gameObject.AddComponent<Inventory>();
			this.inventory = inventory;
			inventory.AttachedToItem = this;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000052D7 File Offset: 0x000034D7
		public UsageUtilities UsageUtilities
		{
			get
			{
				return this.usageUtilities;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000052DF File Offset: 0x000034DF
		public bool IsUsable(object user)
		{
			return this.usageUtilities != null && this.usageUtilities.IsUsable(this, user);
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000052FE File Offset: 0x000034FE
		public float UseTime
		{
			get
			{
				if (this.usageUtilities != null)
				{
					return this.usageUtilities.UseTime;
				}
				return 0f;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000531F File Offset: 0x0000351F
		public void AddUsageUtilitiesComponent()
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005321 File Offset: 0x00003521
		public StatCollection Stats
		{
			get
			{
				return this.stats;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00005329 File Offset: 0x00003529
		public ModifierDescriptionCollection Modifiers
		{
			get
			{
				return this.modifiers;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005331 File Offset: 0x00003531
		public SlotCollection Slots
		{
			get
			{
				return this.slots;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00005339 File Offset: 0x00003539
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00005341 File Offset: 0x00003541
		public Inventory Inventory
		{
			get
			{
				return this.inventory;
			}
			internal set
			{
				this.inventory = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000534A File Offset: 0x0000354A
		public List<Effect> Effects
		{
			get
			{
				return this.effects;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005352 File Offset: 0x00003552
		public Slot PluggedIntoSlot
		{
			get
			{
				if (this.pluggedIntoSlot == null)
				{
					return null;
				}
				if (this.pluggedIntoSlot.Master == null)
				{
					return null;
				}
				return this.pluggedIntoSlot;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00005379 File Offset: 0x00003579
		public Inventory InInventory
		{
			get
			{
				return this.inInventory;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005384 File Offset: 0x00003584
		public Item ParentItem
		{
			get
			{
				UnityEngine.Object parentObject = this.ParentObject;
				if (parentObject == null)
				{
					return null;
				}
				Item item = this.ParentObject as Item;
				if (item != null)
				{
					return item;
				}
				Inventory inventory = parentObject as Inventory;
				if (inventory == null)
				{
					Debug.LogError("侦测到不合法的Parent Object。需要检查ParentObject代码。");
					return null;
				}
				Item attachedToItem = inventory.AttachedToItem;
				if (attachedToItem != null)
				{
					return attachedToItem;
				}
				return null;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000053EC File Offset: 0x000035EC
		public UnityEngine.Object ParentObject
		{
			get
			{
				if (this.PluggedIntoSlot != null && this.InInventory != null)
				{
					Debug.LogError(string.Format("物品 {0} ({1})同时存在于Slot和Inventory中。", this.DisplayName, base.GetInstanceID()));
				}
				if (this.PluggedIntoSlot != null)
				{
					Slot slot = this.PluggedIntoSlot;
					if (slot == null)
					{
						return null;
					}
					return slot.Master;
				}
				else
				{
					if (this.InInventory != null)
					{
						return this.InInventory;
					}
					return null;
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000545F File Offset: 0x0000365F
		public TagCollection Tags
		{
			get
			{
				return this.tags;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005467 File Offset: 0x00003667
		public CustomDataCollection Variables
		{
			get
			{
				return this.variables;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000546F File Offset: 0x0000366F
		public CustomDataCollection Constants
		{
			get
			{
				return this.constants;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005477 File Offset: 0x00003677
		public bool IsCharacter
		{
			get
			{
				return this.tags.Any((Tag e) => e != null && e.name == "Character");
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600012A RID: 298 RVA: 0x000054A4 File Offset: 0x000036A4
		// (remove) Token: 0x0600012B RID: 299 RVA: 0x000054DC File Offset: 0x000036DC
		public event Action<Item> onItemTreeChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600012C RID: 300 RVA: 0x00005514 File Offset: 0x00003714
		// (remove) Token: 0x0600012D RID: 301 RVA: 0x0000554C File Offset: 0x0000374C
		public event Action<Item> onDestroy;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600012E RID: 302 RVA: 0x00005584 File Offset: 0x00003784
		// (remove) Token: 0x0600012F RID: 303 RVA: 0x000055BC File Offset: 0x000037BC
		public event Action<Item> onSetStackCount;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000130 RID: 304 RVA: 0x000055F4 File Offset: 0x000037F4
		// (remove) Token: 0x06000131 RID: 305 RVA: 0x0000562C File Offset: 0x0000382C
		public event Action<Item> onDurabilityChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000132 RID: 306 RVA: 0x00005664 File Offset: 0x00003864
		// (remove) Token: 0x06000133 RID: 307 RVA: 0x0000569C File Offset: 0x0000389C
		public event Action<Item> onInspectionStateChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000134 RID: 308 RVA: 0x000056D4 File Offset: 0x000038D4
		// (remove) Token: 0x06000135 RID: 309 RVA: 0x0000570C File Offset: 0x0000390C
		public event Action<Item, object> onUse;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000136 RID: 310 RVA: 0x00005744 File Offset: 0x00003944
		// (remove) Token: 0x06000137 RID: 311 RVA: 0x00005778 File Offset: 0x00003978
		public static event Action<Item, object> onUseStatic;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000138 RID: 312 RVA: 0x000057AC File Offset: 0x000039AC
		// (remove) Token: 0x06000139 RID: 313 RVA: 0x000057E4 File Offset: 0x000039E4
		public event Action<Item> onChildChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600013A RID: 314 RVA: 0x0000581C File Offset: 0x00003A1C
		// (remove) Token: 0x0600013B RID: 315 RVA: 0x00005854 File Offset: 0x00003A54
		public event Action<Item> onParentChanged;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600013C RID: 316 RVA: 0x0000588C File Offset: 0x00003A8C
		// (remove) Token: 0x0600013D RID: 317 RVA: 0x000058C4 File Offset: 0x00003AC4
		public event Action<Item> onPluggedIntoSlot;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600013E RID: 318 RVA: 0x000058FC File Offset: 0x00003AFC
		// (remove) Token: 0x0600013F RID: 319 RVA: 0x00005934 File Offset: 0x00003B34
		public event Action<Item> onUnpluggedFromSlot;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000140 RID: 320 RVA: 0x0000596C File Offset: 0x00003B6C
		// (remove) Token: 0x06000141 RID: 321 RVA: 0x000059A4 File Offset: 0x00003BA4
		public event Action<Item, Slot> onSlotContentChanged;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000142 RID: 322 RVA: 0x000059DC File Offset: 0x00003BDC
		// (remove) Token: 0x06000143 RID: 323 RVA: 0x00005A14 File Offset: 0x00003C14
		public event Action<Item> onSlotTreeChanged;

		// Token: 0x06000144 RID: 324 RVA: 0x00005A49 File Offset: 0x00003C49
		private void Awake()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (this.inventory)
			{
				this.inventory.onContentChanged += this.OnInventoryContentChanged;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005A7D File Offset: 0x00003C7D
		private void OnInventoryContentChanged(Inventory inventory, int index)
		{
			this.NotifyChildChanged();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005A88 File Offset: 0x00003C88
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.agentUtilities.Initialize(this);
			StatCollection statCollection = this.Stats;
			if (statCollection != null)
			{
				statCollection.Initialize();
			}
			SlotCollection slotCollection = this.Slots;
			if (slotCollection != null)
			{
				slotCollection.Initialize();
			}
			ModifierDescriptionCollection modifierDescriptionCollection = this.Modifiers;
			if (modifierDescriptionCollection != null)
			{
				modifierDescriptionCollection.Initialize();
			}
			ModifierDescriptionCollection modifierDescriptionCollection2 = this.modifiers;
			if (modifierDescriptionCollection2 != null)
			{
				modifierDescriptionCollection2.ReapplyModifiers();
			}
			this.HandleEffectsActive();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005AFC File Offset: 0x00003CFC
		public Item GetCharacterItem()
		{
			Item item = this;
			while (item != null)
			{
				if (item.IsCharacter)
				{
					return item;
				}
				item = item.ParentItem;
			}
			return null;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005B28 File Offset: 0x00003D28
		public bool IsInCharacterSlot()
		{
			Item item = null;
			Item item2 = this;
			if (item2.IsCharacter)
			{
				return false;
			}
			while (item2 != null)
			{
				if (item2.IsCharacter)
				{
					return item.PluggedIntoSlot != null;
				}
				item = item2;
				item2 = item2.ParentItem;
			}
			return false;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005B6A File Offset: 0x00003D6A
		public Item CreateInstance()
		{
			Item item = UnityEngine.Object.Instantiate<Item>(this);
			item.Initialize();
			return item;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005B78 File Offset: 0x00003D78
		public void Detach()
		{
			Slot slot = this.PluggedIntoSlot;
			if (slot != null)
			{
				slot.Unplug();
			}
			Inventory inventory = this.InInventory;
			if (inventory == null)
			{
				return;
			}
			inventory.RemoveItem(this);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005B9E File Offset: 0x00003D9E
		internal void NotifyPluggedTo(Slot slot)
		{
			this.pluggedIntoSlot = slot;
			Action<Item> action = this.onPluggedIntoSlot;
			if (action != null)
			{
				action(this);
			}
			Action<Item> action2 = this.onParentChanged;
			if (action2 == null)
			{
				return;
			}
			action2(this);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005BCC File Offset: 0x00003DCC
		internal void NotifyUnpluggedFrom(Slot slot)
		{
			if (this.pluggedIntoSlot != slot)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"物品 ",
					this.DisplayName,
					" 被通知从Slot移除，但当前Slot ",
					(this.pluggedIntoSlot != null) ? (this.pluggedIntoSlot.Master.DisplayName + "/" + this.pluggedIntoSlot.Key) : "空",
					" 与通知Slot ",
					(slot != null) ? (slot.Master.DisplayName + "/" + slot.Key) : "空",
					" 不匹配。"
				}));
				return;
			}
			this.pluggedIntoSlot = null;
			Action<Item> action = this.onUnpluggedFromSlot;
			if (action != null)
			{
				action(this);
			}
			Action<Item> action2 = this.onParentChanged;
			if (action2 == null)
			{
				return;
			}
			action2(this);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005CA5 File Offset: 0x00003EA5
		internal void NotifySlotPlugged(Slot slot)
		{
			this.NotifyChildChanged();
			this.NotifySlotTreeChanged();
			Action<Item, Slot> action = this.onSlotContentChanged;
			if (action == null)
			{
				return;
			}
			action(this, slot);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005CC5 File Offset: 0x00003EC5
		internal void NotifySlotUnplugged(Slot slot)
		{
			this.NotifyChildChanged();
			this.NotifySlotTreeChanged();
			Action<Item, Slot> action = this.onSlotContentChanged;
			if (action == null)
			{
				return;
			}
			action(this, slot);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005CE8 File Offset: 0x00003EE8
		internal void NotifyRemovedFromInventory(Inventory inventory)
		{
			if (!(inventory == this.InInventory))
			{
				if (this.InInventory != null)
				{
					Debug.LogError("尝试从不是当前的Inventory中移除，已取消。");
				}
				return;
			}
			this.inInventory = null;
			Action<Item> action = this.onParentChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005D34 File Offset: 0x00003F34
		internal void NotifyAddedToInventory(Inventory inventory)
		{
			this.inInventory = inventory;
			Action<Item> action = this.onParentChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005D4E File Offset: 0x00003F4E
		internal void NotifyItemTreeChanged()
		{
			Action<Item> action = this.onItemTreeChanged;
			if (action != null)
			{
				action(this);
			}
			this.HandleEffectsActive();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005D68 File Offset: 0x00003F68
		private void HandleEffectsActive()
		{
			if (this.effects == null)
			{
				return;
			}
			bool active = this.IsCharacter || this.PluggedIntoSlot != null;
			if (this.UseDurability && this.Durability <= 0f)
			{
				active = false;
			}
			foreach (Effect effect in this.effects)
			{
				if (!(effect == null))
				{
					effect.gameObject.SetActive(active);
				}
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005E00 File Offset: 0x00004000
		internal void InitiateNotifyItemTreeChanged()
		{
			List<Item> allConnected = this.GetAllConnected();
			if (allConnected == null)
			{
				return;
			}
			foreach (Item item in allConnected)
			{
				item.NotifyItemTreeChanged();
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005E58 File Offset: 0x00004058
		internal void NotifyChildChanged()
		{
			this.RecalculateTotalWeight();
			Action<Item> action = this.onChildChanged;
			if (action != null)
			{
				action(this);
			}
			Item parentItem = this.ParentItem;
			if (parentItem != null)
			{
				parentItem.NotifyChildChanged();
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005E94 File Offset: 0x00004094
		internal void NotifySlotTreeChanged()
		{
			Action<Item> action = this.onSlotTreeChanged;
			if (action != null)
			{
				action(this);
			}
			Item parentItem = this.ParentItem;
			if (parentItem != null)
			{
				parentItem.NotifySlotTreeChanged();
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005EC9 File Offset: 0x000040C9
		public void Use(object user)
		{
			Action<Item, object> action = this.onUse;
			if (action != null)
			{
				action(this, user);
			}
			Action<Item, object> action2 = Item.onUseStatic;
			if (action2 != null)
			{
				action2(this, user);
			}
			this.usageUtilities.Use(this, user);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00005EFD File Offset: 0x000040FD
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00005F18 File Offset: 0x00004118
		public int StackCount
		{
			get
			{
				if (this.Stackable)
				{
					return this.GetInt(Item.StackCountVariableHash, 1);
				}
				return 1;
			}
			set
			{
				if (!this.Stackable)
				{
					if (value != 1)
					{
						Debug.LogError("该物品 " + this.DisplayName + " 不可堆叠。无法设置数量。");
					}
					return;
				}
				int num = value;
				if (value >= 1 && value > this.MaxStackCount)
				{
					Debug.LogWarning(string.Format("尝试将数量设为{0},但该物品 {1} 的数量最多为{2}。将改为设为{3}。", new object[]
					{
						value,
						this.DisplayName,
						this.MaxStackCount,
						this.MaxStackCount
					}));
					num = this.MaxStackCount;
				}
				this.SetInt("Count", num, true);
				Action<Item> action = this.onSetStackCount;
				if (action != null)
				{
					action(this);
				}
				this.NotifyChildChanged();
				if (this.InInventory)
				{
					this.InInventory.NotifyContentChanged(this);
				}
				if (this.StackCount < 1)
				{
					this.DestroyTree();
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00005FF2 File Offset: 0x000041F2
		public bool UseDurability
		{
			get
			{
				return this.MaxDurability > 0f;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00006001 File Offset: 0x00004201
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00006018 File Offset: 0x00004218
		public float MaxDurability
		{
			get
			{
				return this.Constants.GetFloat(Item.MaxDurabilityConstantHash, 0f);
			}
			set
			{
				this.Constants.SetFloat("MaxDurability", value, true);
				Action<Item> action = this.onDurabilityChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000603D File Offset: 0x0000423D
		public float MaxDurabilityWithLoss
		{
			get
			{
				return this.MaxDurability * (1f - this.DurabilityLoss);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006052 File Offset: 0x00004252
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000606E File Offset: 0x0000426E
		public float DurabilityLoss
		{
			get
			{
				return Mathf.Clamp01(this.Variables.GetFloat("DurabilityLoss", 0f));
			}
			set
			{
				this.Variables.SetFloat("DurabilityLoss", value, true);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00006082 File Offset: 0x00004282
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00006094 File Offset: 0x00004294
		public float Durability
		{
			get
			{
				return this.GetFloat(Item.DurabilityVariableHash, 0f);
			}
			set
			{
				float num = Mathf.Min(this.MaxDurability, value);
				if (num < 0f)
				{
					num = 0f;
				}
				this.SetFloat("Durability", num, true);
				Action<Item> action = this.onDurabilityChanged;
				if (action != null)
				{
					action(this);
				}
				this.HandleEffectsActive();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000060E1 File Offset: 0x000042E1
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000060F4 File Offset: 0x000042F4
		public bool Inspected
		{
			get
			{
				return this.Variables.GetBool(Item.InspectedVariableHash, false);
			}
			set
			{
				this.Variables.SetBool("Inspected", value, true);
				if (this.slots != null)
				{
					foreach (Slot slot in this.slots)
					{
						if (slot != null)
						{
							Item content = slot.Content;
							if (!(content == null))
							{
								content.Inspected = value;
							}
						}
					}
				}
				Action<Item> action = this.onInspectionStateChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006188 File Offset: 0x00004388
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00006190 File Offset: 0x00004390
		public bool Inspecting
		{
			get
			{
				return this._inspecting;
			}
			set
			{
				this._inspecting = value;
				Action<Item> action = this.onInspectionStateChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000061AA File Offset: 0x000043AA
		public bool NeedInspection
		{
			get
			{
				return !this.Inspected && !(this.InInventory == null) && this.InInventory.NeedInspection;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000061D6 File Offset: 0x000043D6
		public CustomData GetVariableEntry(string variableKey)
		{
			return this.Variables.GetEntry(variableKey);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000061E4 File Offset: 0x000043E4
		public CustomData GetVariableEntry(int hash)
		{
			return this.Variables.GetEntry(hash);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000061F2 File Offset: 0x000043F2
		public float GetFloat(string key, float defaultResult = 0f)
		{
			return this.Variables.GetFloat(key, defaultResult);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006201 File Offset: 0x00004401
		public int GetInt(string key, int defaultResult = 0)
		{
			return this.Variables.GetInt(key, defaultResult);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006210 File Offset: 0x00004410
		public bool GetBool(string key, bool defaultResult = false)
		{
			return this.Variables.GetBool(key, defaultResult);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000621F File Offset: 0x0000441F
		public string GetString(string key, string defaultResult = null)
		{
			return this.Variables.GetString(key, defaultResult);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000622E File Offset: 0x0000442E
		public float GetFloat(int hash, float defaultResult = 0f)
		{
			return this.Variables.GetFloat(hash, defaultResult);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000623D File Offset: 0x0000443D
		public int GetInt(int hash, int defaultResult = 0)
		{
			return this.Variables.GetInt(hash, defaultResult);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000624C File Offset: 0x0000444C
		public bool GetBool(int hash, bool defaultResult = false)
		{
			return this.Variables.GetBool(hash, defaultResult);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000625B File Offset: 0x0000445B
		public string GetString(int hash, string defaultResult = null)
		{
			return this.Variables.GetString(hash, defaultResult);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000626A File Offset: 0x0000446A
		public void SetFloat(string key, float value, bool createNewIfNotExist = true)
		{
			this.Variables.Set(key, value, createNewIfNotExist);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000627A File Offset: 0x0000447A
		public void SetInt(string key, int value, bool createNewIfNotExist = true)
		{
			this.Variables.Set(key, value, createNewIfNotExist);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000628A File Offset: 0x0000448A
		public void SetBool(string key, bool value, bool createNewIfNotExist = true)
		{
			this.Variables.Set(key, value, createNewIfNotExist);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000629A File Offset: 0x0000449A
		public void SetString(string key, string value, bool createNewIfNotExist = true)
		{
			this.Variables.Set(key, value, createNewIfNotExist);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000062AA File Offset: 0x000044AA
		public void SetFloat(int hash, float value)
		{
			this.Variables.Set(hash, value);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000062B9 File Offset: 0x000044B9
		public void SetInt(int hash, int value)
		{
			this.Variables.Set(hash, value);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000062C8 File Offset: 0x000044C8
		public void SetBool(int hash, bool value)
		{
			this.Variables.Set(hash, value);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000062D7 File Offset: 0x000044D7
		public void SetString(int hash, string value)
		{
			this.Variables.Set(hash, value);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000062E6 File Offset: 0x000044E6
		internal void ForceSetStackCount(int value)
		{
			Debug.LogWarning(string.Format("正在强制将物品 {0} 的 Stack Count 设置为 {1}。", this.DisplayName, value));
			this.SetInt(Item.StackCountVariableHash, value);
			Action<Item> action = this.onSetStackCount;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006320 File Offset: 0x00004520
		public void Combine(Item incomingItem)
		{
			if (incomingItem == null)
			{
				return;
			}
			if (incomingItem == this)
			{
				return;
			}
			if (!this.Stackable)
			{
				Debug.LogError("正在尝试组合物品，但物品 " + this.DisplayName + " 不能堆叠。");
				return;
			}
			if (this.TypeID != incomingItem.TypeID)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"物品 ",
					this.DisplayName,
					" 与 ",
					incomingItem.DisplayName,
					" 类型不同，无法组合。"
				}));
				return;
			}
			int num = this.MaxStackCount - this.StackCount;
			if (num <= 0)
			{
				return;
			}
			int stackCount = this.StackCount;
			int stackCount2 = incomingItem.StackCount;
			int num2 = (incomingItem.StackCount >= num) ? num : incomingItem.StackCount;
			int num3 = incomingItem.StackCount - num2;
			this.StackCount += num2;
			incomingItem.StackCount = num3;
			if (num3 <= 0)
			{
				incomingItem.Detach();
				if (Application.isPlaying)
				{
					incomingItem.DestroyTree();
					return;
				}
				UnityEngine.Object.DestroyImmediate(incomingItem);
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000641D File Offset: 0x0000461D
		public void CombineInto(Item otherItem)
		{
			otherItem.Combine(this);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006428 File Offset: 0x00004628
		public UniTask<Item> Split(int count)
		{
			Item.<Split>d__241 <Split>d__;
			<Split>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Split>d__.<>4__this = this;
			<Split>d__.count = count;
			<Split>d__.<>1__state = -1;
			<Split>d__.<>t__builder.Start<Item.<Split>d__241>(ref <Split>d__);
			return <Split>d__.<>t__builder.Task;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006473 File Offset: 0x00004673
		public override string ToString()
		{
			return this.displayName + " (物品)";
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006485 File Offset: 0x00004685
		public bool IsBeingDestroyed
		{
			get
			{
				return this.isBeingDestroyed;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000648D File Offset: 0x0000468D
		public bool Repairable
		{
			get
			{
				return this.UseDurability && this.Tags.Contains("Repairable");
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000064A9 File Offset: 0x000046A9
		public string SoundKey
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.soundKey))
				{
					return "default";
				}
				return this.soundKey;
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000064C4 File Offset: 0x000046C4
		public void MarkDestroyed()
		{
			this.isBeingDestroyed = true;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000064CD File Offset: 0x000046CD
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
			this.Detach();
			this.agentUtilities.ReleaseActiveAgent();
			Action<Item> action = this.onDestroy;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000064F8 File Offset: 0x000046F8
		public Stat GetStat(int hash)
		{
			if (this.Stats == null)
			{
				return null;
			}
			StatCollection statCollection = this.Stats;
			if (statCollection == null)
			{
				return null;
			}
			return statCollection.GetStat(hash);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000651C File Offset: 0x0000471C
		public Stat GetStat(string key)
		{
			StatCollection statCollection = this.Stats;
			if (statCollection == null)
			{
				return null;
			}
			return statCollection.GetStat(key);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006530 File Offset: 0x00004730
		public float GetStatValue(int hash)
		{
			Stat stat = this.GetStat(hash);
			if (stat == null)
			{
				return 0f;
			}
			return stat.Value;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006554 File Offset: 0x00004754
		public static Stat GetStat(Item item, int hash)
		{
			if (item == null)
			{
				return null;
			}
			return item.GetStat(hash);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006568 File Offset: 0x00004768
		public static float GetStatValue(Item item, int hash)
		{
			if (item == null)
			{
				return 0f;
			}
			Stat stat = Item.GetStat(item, hash);
			if (stat == null)
			{
				return 0f;
			}
			return stat.Value;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000659B File Offset: 0x0000479B
		private void OnValidate()
		{
			base.transform.hideFlags = HideFlags.HideInInspector;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000065AC File Offset: 0x000047AC
		public void Validate(SelfValidationResult result)
		{
			if (this.Stats != null && this.Stats.gameObject != base.gameObject)
			{
				result.AddError("引用了其他物体上的Stats组件。").WithFix("改为引用本物体的Stats组件", delegate()
				{
					this.stats = base.GetComponent<StatCollection>();
				}, true);
			}
			if (this.Slots != null && this.Slots.gameObject != base.gameObject)
			{
				result.AddError("引用了其他物体上的Slots组件。").WithFix("改为引用本物体的Slots组件", delegate()
				{
					this.slots = base.GetComponent<SlotCollection>();
				}, true);
			}
			if (this.Modifiers != null && this.Modifiers.gameObject != base.gameObject)
			{
				result.AddError("引用了其他物体上的Modifiers组件。").WithFix("改为引用本物体的Modifiers组件", delegate()
				{
					this.modifiers = base.GetComponent<ModifierDescriptionCollection>();
				}, true);
			}
			if (this.Inventory != null && this.Inventory.gameObject != base.gameObject)
			{
				result.AddError("引用了其他物体上的Inventory组件。").WithFix("改为引用本物体的Inventory组件", delegate()
				{
					this.inventory = base.GetComponent<Inventory>();
				}, true);
			}
			if (this.Effects.Any((Effect e) => e == null))
			{
				result.AddError("Effects列表中有空物体。").WithFix("移除空Effect项目", delegate()
				{
					this.Effects.RemoveAll((Effect e) => e == null);
				}, true);
			}
			if (this.Effects.Any((Effect e) => !e.transform.IsChildOf(base.transform)))
			{
				result.AddError("引用了其他物体上的Effects。").WithFix("移除不正确的Effects", delegate()
				{
					this.Effects.RemoveAll((Effect e) => !e.transform.IsChildOf(base.transform));
				}, true);
			}
			if (this.Stackable)
			{
				if (this.Slots != null || this.Inventory != null)
				{
					result.AddError("可堆叠物体不应包含Slot、Inventory等独特信息。").WithFix("变为不可堆叠物体", delegate()
					{
						this.maxStackCount = 1;
					}, true);
				}
				if (this.Variables.Any((CustomData e) => e.Key != "Count"))
				{
					result.AddError("可堆叠物体不应包含特殊变量。");
				}
				if (!this.Variables.Any((CustomData e) => e.Key == "Count"))
				{
					result.AddWarning("可堆叠物体应包含Count变量，记录当前具体数量。(默认数量)").WithFix("添加Count变量。", delegate()
					{
						this.variables.Add(new CustomData("Count", this.MaxStackCount));
					}, true);
					return;
				}
			}
			else if (this.Variables.Any((CustomData e) => e.Key == "Count"))
			{
				result.AddWarning("不可堆叠物体包含了Count变量。建议删除。").WithFix("删除Count变量。", delegate()
				{
					this.variables.Remove(this.variables.GetEntry("Count"));
				}, true);
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000688C File Offset: 0x00004A8C
		public float RecalculateTotalWeight()
		{
			float num = 0f;
			num += this.SelfWeight;
			if (this.inventory != null)
			{
				this.inventory.RecalculateWeight();
				float cachedWeight = this.inventory.CachedWeight;
				num += cachedWeight;
			}
			if (this.slots != null)
			{
				foreach (Slot slot in this.slots)
				{
					if (slot != null && slot.Content != null)
					{
						float totalWeight = slot.Content.TotalWeight;
						num += totalWeight;
					}
				}
			}
			this._cachedTotalWeight = new float?(num);
			return num;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006948 File Offset: 0x00004B48
		public void AddEffect(Effect instance)
		{
			instance.SetItem(this);
			if (!this.effects.Contains(instance))
			{
				this.effects.Add(instance);
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000696C File Offset: 0x00004B6C
		private void CreateNewEffect()
		{
			GameObject gameObject = new GameObject("New Effect");
			gameObject.transform.SetParent(base.transform, false);
			Effect instance = gameObject.AddComponent<Effect>();
			this.AddEffect(instance);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000069A4 File Offset: 0x00004BA4
		public int GetTotalRawValue()
		{
			float num = (float)this.Value;
			if (this.UseDurability && this.MaxDurability > 0f)
			{
				if (this.MaxDurability > 0f)
				{
					num *= this.Durability / this.MaxDurability;
				}
				else
				{
					num = 0f;
				}
			}
			int num2 = Mathf.FloorToInt(num) * (this.Stackable ? this.StackCount : 1);
			if (this.Slots != null)
			{
				foreach (Slot slot in this.Slots)
				{
					if (slot != null)
					{
						Item content = slot.Content;
						if (!(content == null))
						{
							num2 += content.GetTotalRawValue();
						}
					}
				}
			}
			if (this.Inventory != null)
			{
				foreach (Item item in this.Inventory)
				{
					if (!(item == null))
					{
						num2 += item.GetTotalRawValue();
					}
				}
			}
			return num2;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006AD0 File Offset: 0x00004CD0
		public int RemoveAllModifiersFrom(object endowmentEntry)
		{
			if (this.stats == null)
			{
				return 0;
			}
			int num = 0;
			foreach (Stat stat in this.stats)
			{
				if (stat != null)
				{
					num += stat.RemoveAllModifiersFromSource(endowmentEntry);
				}
			}
			return num;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006B38 File Offset: 0x00004D38
		public bool AddModifier(string statKey, Modifier modifier)
		{
			if (this.stats == null)
			{
				return false;
			}
			Stat stat = this.stats[statKey];
			if (stat == null)
			{
				return false;
			}
			stat.AddModifier(modifier);
			return true;
		}

		// Token: 0x0400005A RID: 90
		[SerializeField]
		private int typeID;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		private int order;

		// Token: 0x0400005C RID: 92
		[LocalizationKey("Items")]
		[SerializeField]
		private string displayName;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private int maxStackCount = 1;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		private int value;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private int quality;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private DisplayQuality displayQuality;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		private float weight;

		// Token: 0x04000063 RID: 99
		private float _cachedWeight;

		// Token: 0x04000064 RID: 100
		private float? _cachedTotalWeight;

		// Token: 0x04000065 RID: 101
		private int handheldHash = "Handheld".GetHashCode();

		// Token: 0x04000066 RID: 102
		[SerializeField]
		private TagCollection tags = new TagCollection();

		// Token: 0x04000067 RID: 103
		[SerializeField]
		private ItemAgentUtilities agentUtilities = new ItemAgentUtilities();

		// Token: 0x04000068 RID: 104
		[SerializeField]
		private ItemGraphicInfo itemGraphic;

		// Token: 0x04000069 RID: 105
		[SerializeField]
		private StatCollection stats;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private SlotCollection slots;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		private ModifierDescriptionCollection modifiers;

		// Token: 0x0400006C RID: 108
		[SerializeField]
		private CustomDataCollection variables = new CustomDataCollection();

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private CustomDataCollection constants = new CustomDataCollection();

		// Token: 0x0400006E RID: 110
		[SerializeField]
		private Inventory inventory;

		// Token: 0x0400006F RID: 111
		[SerializeField]
		private List<Effect> effects = new List<Effect>();

		// Token: 0x04000070 RID: 112
		[SerializeField]
		private UsageUtilities usageUtilities;

		// Token: 0x04000071 RID: 113
		private Slot pluggedIntoSlot;

		// Token: 0x04000072 RID: 114
		private Inventory inInventory;

		// Token: 0x04000080 RID: 128
		private bool initialized;

		// Token: 0x04000081 RID: 129
		private const string StackCountVariableKey = "Count";

		// Token: 0x04000082 RID: 130
		private static readonly int StackCountVariableHash = "Count".GetHashCode();

		// Token: 0x04000083 RID: 131
		private const string InspectedVariableKey = "Inspected";

		// Token: 0x04000084 RID: 132
		private static readonly int InspectedVariableHash = "Inspected".GetHashCode();

		// Token: 0x04000085 RID: 133
		private const string MaxDurabilityConstantKey = "MaxDurability";

		// Token: 0x04000086 RID: 134
		private const string DurabilityVariableKey = "Durability";

		// Token: 0x04000087 RID: 135
		private static readonly int MaxDurabilityConstantHash = "MaxDurability".GetHashCode();

		// Token: 0x04000088 RID: 136
		private static readonly int DurabilityVariableHash = "Durability".GetHashCode();

		// Token: 0x04000089 RID: 137
		private bool _inspecting;

		// Token: 0x0400008A RID: 138
		public string soundKey;

		// Token: 0x0400008B RID: 139
		private bool isBeingDestroyed;
	}
}
