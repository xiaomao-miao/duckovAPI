using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200039A RID: 922
	public class ItemOperationMenu : ManagedUIElement
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00071D85 File Offset: 0x0006FF85
		// (set) Token: 0x060020A4 RID: 8356 RVA: 0x00071D8C File Offset: 0x0006FF8C
		public static ItemOperationMenu Instance { get; private set; }

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x00071D94 File Offset: 0x0006FF94
		private Item TargetItem
		{
			get
			{
				ItemDisplay targetDisplay = this.TargetDisplay;
				if (targetDisplay == null)
				{
					return null;
				}
				return targetDisplay.Target;
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00071DA7 File Offset: 0x0006FFA7
		protected override void Awake()
		{
			base.Awake();
			ItemOperationMenu.Instance = this;
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			this.Initialize();
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00071DD5 File Offset: 0x0006FFD5
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00071DE0 File Offset: 0x0006FFE0
		private void Update()
		{
			if (this.fadeGroup.IsHidingInProgress)
			{
				return;
			}
			if (!this.fadeGroup.IsShown)
			{
				return;
			}
			if (!Mouse.current.leftButton.wasReleasedThisFrame && !(this.targetView == null) && this.targetView.open)
			{
				if (this.fadeGroup.IsShowingInProgress)
				{
					return;
				}
				if (!Mouse.current.rightButton.wasReleasedThisFrame)
				{
					return;
				}
			}
			base.Close();
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00071E5C File Offset: 0x0007005C
		private void Initialize()
		{
			this.btn_Use.onClick.AddListener(new UnityAction(this.Use));
			this.btn_Split.onClick.AddListener(new UnityAction(this.Split));
			this.btn_Dump.onClick.AddListener(new UnityAction(this.Dump));
			this.btn_Equip.onClick.AddListener(new UnityAction(this.Equip));
			this.btn_Modify.onClick.AddListener(new UnityAction(this.Modify));
			this.btn_Unload.onClick.AddListener(new UnityAction(this.Unload));
			this.btn_Wishlist.onClick.AddListener(new UnityAction(this.Wishlist));
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00071F30 File Offset: 0x00070130
		private void Wishlist()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			int typeID = this.TargetItem.TypeID;
			if (ItemWishlist.GetWishlistInfo(typeID).isManuallyWishlisted)
			{
				ItemWishlist.RemoveFromWishlist(typeID);
				return;
			}
			ItemWishlist.AddToWishList(this.TargetItem.TypeID);
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00071F7D File Offset: 0x0007017D
		private void Use()
		{
			LevelManager instance = LevelManager.Instance;
			if (instance != null)
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter != null)
				{
					mainCharacter.UseItem(this.TargetItem);
				}
			}
			InventoryView.Hide();
			base.Close();
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00071FAB File Offset: 0x000701AB
		private void Split()
		{
			SplitDialogue.SetupAndShow(this.TargetItem);
			base.Close();
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00071FBE File Offset: 0x000701BE
		private void Dump()
		{
			LevelManager instance = LevelManager.Instance;
			if ((instance != null) ? instance.MainCharacter : null)
			{
				this.TargetItem.Drop(LevelManager.Instance.MainCharacter, true);
			}
			base.Close();
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00071FF4 File Offset: 0x000701F4
		private void Modify()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			ItemCustomizeView instance = ItemCustomizeView.Instance;
			if (instance == null)
			{
				return;
			}
			List<Inventory> list = new List<Inventory>();
			LevelManager instance2 = LevelManager.Instance;
			Inventory inventory;
			if (instance2 == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance2.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			if (inventory2)
			{
				list.Add(inventory2);
			}
			instance.Setup(this.TargetItem, list);
			instance.Open(null);
			base.Close();
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00072079 File Offset: 0x00070279
		private void Equip()
		{
			LevelManager instance = LevelManager.Instance;
			if (instance != null)
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter != null)
				{
					Item characterItem = mainCharacter.CharacterItem;
					if (characterItem != null)
					{
						characterItem.TryPlug(this.TargetItem, false, null, 0);
					}
				}
			}
			base.Close();
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000720B4 File Offset: 0x000702B4
		private void Unload()
		{
			Item targetItem = this.TargetItem;
			ItemSetting_Gun itemSetting_Gun = (targetItem != null) ? targetItem.GetComponent<ItemSetting_Gun>() : null;
			if (itemSetting_Gun == null)
			{
				return;
			}
			AudioManager.Post("SFX/Combat/Gun/unload");
			itemSetting_Gun.TakeOutAllBullets();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000720EF File Offset: 0x000702EF
		protected override void OnOpen()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000720FC File Offset: 0x000702FC
		protected override void OnClose()
		{
			this.fadeGroup.Hide();
			this.displayingItem = null;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00072110 File Offset: 0x00070310
		public static void Show(ItemDisplay id)
		{
			if (ItemOperationMenu.Instance == null)
			{
				return;
			}
			ItemOperationMenu.Instance.MShow(id);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x0007212B File Offset: 0x0007032B
		private void MShow(ItemDisplay targetDisplay)
		{
			if (targetDisplay == null)
			{
				return;
			}
			this.TargetDisplay = targetDisplay;
			this.targetView = targetDisplay.GetComponentInParent<View>();
			this.Setup();
			base.Open(null);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00072158 File Offset: 0x00070358
		private void Setup()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			this.displayingItem = this.TargetItem;
			this.icon.sprite = this.TargetItem.Icon;
			this.nameText.text = this.TargetItem.DisplayName;
			this.btn_Use.gameObject.SetActive(this.Usable);
			this.btn_Use.interactable = this.UseButtonInteractable;
			this.btn_Split.gameObject.SetActive(this.Splittable);
			this.btn_Dump.gameObject.SetActive(this.Dumpable);
			this.btn_Equip.gameObject.SetActive(this.Equipable);
			this.btn_Modify.gameObject.SetActive(this.Modifyable);
			this.btn_Unload.gameObject.SetActive(this.Unloadable);
			this.RefreshWeightText();
			this.RefreshPosition();
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00072250 File Offset: 0x00070450
		private void RefreshPosition()
		{
			RectTransform rectTransform = this.TargetDisplay.transform as RectTransform;
			Rect rect = rectTransform.rect;
			Vector2 min = rect.min;
			Vector2 max = rect.max;
			Vector3 point = rectTransform.localToWorldMatrix.MultiplyPoint(min);
			Vector3 point2 = rectTransform.localToWorldMatrix.MultiplyPoint(max);
			Vector3 vector = this.rectTransform.worldToLocalMatrix.MultiplyPoint(point);
			Vector3 vector2 = this.rectTransform.worldToLocalMatrix.MultiplyPoint(point2);
			Vector2[] array = new Vector2[]
			{
				new Vector2(vector.x, vector.y),
				new Vector2(vector.x, vector2.y),
				new Vector2(vector2.x, vector.y),
				new Vector2(vector2.x, vector2.y)
			};
			int num = 0;
			float num2 = float.MaxValue;
			Vector2 center = this.rectTransform.rect.center;
			for (int i = 0; i < array.Length; i++)
			{
				float sqrMagnitude = (array[i] - center).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num = i;
					num2 = sqrMagnitude;
				}
			}
			bool flag = (num & 2) > 0;
			bool flag2 = (num & 1) > 0;
			float x = flag ? vector2.x : vector.x;
			float y = flag2 ? vector.y : vector2.y;
			this.contentRectTransform.pivot = new Vector2((float)(flag ? 0 : 1), (float)(flag2 ? 0 : 1));
			this.contentRectTransform.localPosition = new Vector2(x, y);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00072424 File Offset: 0x00070624
		private void RefreshWeightText()
		{
			if (this.displayingItem == null)
			{
				return;
			}
			this.weightText.text = string.Format(this.weightTextFormat, this.displayingItem.TotalWeight);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x0007245B File Offset: 0x0007065B
		public void OnPointerClick(PointerEventData eventData)
		{
			base.Close();
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x00072463 File Offset: 0x00070663
		private bool Usable
		{
			get
			{
				return this.TargetItem.UsageUtilities != null;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x00072476 File Offset: 0x00070676
		private bool UseButtonInteractable
		{
			get
			{
				if (this.TargetItem)
				{
					Item targetItem = this.TargetItem;
					LevelManager instance = LevelManager.Instance;
					return targetItem.IsUsable((instance != null) ? instance.MainCharacter : null);
				}
				return false;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x000724A4 File Offset: 0x000706A4
		private bool Splittable
		{
			get
			{
				CharacterMainControl main = CharacterMainControl.Main;
				return (main == null || main.CharacterItem.Inventory.GetFirstEmptyPosition(0) >= 0) && (this.TargetItem && this.TargetItem.Stackable) && this.TargetItem.StackCount > 1;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060020BC RID: 8380 RVA: 0x00072500 File Offset: 0x00070700
		private bool Dumpable
		{
			get
			{
				if (!this.TargetItem.CanDrop)
				{
					return false;
				}
				LevelManager instance = LevelManager.Instance;
				Item item;
				if (instance == null)
				{
					item = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					item = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
				}
				Item y = item;
				return this.TargetItem.GetRoot() == y;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x00072550 File Offset: 0x00070750
		private bool Equipable
		{
			get
			{
				if (this.TargetItem == null)
				{
					return false;
				}
				if (this.TargetItem.PluggedIntoSlot != null)
				{
					return false;
				}
				LevelManager instance = LevelManager.Instance;
				bool? flag;
				if (instance == null)
				{
					flag = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					if (mainCharacter == null)
					{
						flag = null;
					}
					else
					{
						Item characterItem = mainCharacter.CharacterItem;
						flag = ((characterItem != null) ? new bool?(characterItem.Slots.Any((Slot e) => e.CanPlug(this.TargetItem))) : null);
					}
				}
				bool? flag2 = flag;
				return flag2 != null && flag2.Value;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x000725E6 File Offset: 0x000707E6
		private bool Modifyable
		{
			get
			{
				return this.alwaysModifyable;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x000725F3 File Offset: 0x000707F3
		private bool Unloadable
		{
			get
			{
				return !(this.TargetItem == null) && this.TargetItem.GetComponent<ItemSetting_Gun>();
			}
		}

		// Token: 0x04001639 RID: 5689
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400163A RID: 5690
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x0400163B RID: 5691
		[SerializeField]
		private RectTransform contentRectTransform;

		// Token: 0x0400163C RID: 5692
		[SerializeField]
		private Image icon;

		// Token: 0x0400163D RID: 5693
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x0400163E RID: 5694
		[SerializeField]
		private TextMeshProUGUI weightText;

		// Token: 0x0400163F RID: 5695
		[SerializeField]
		private string weightTextFormat = "{0:0.#}kg";

		// Token: 0x04001640 RID: 5696
		[SerializeField]
		private Button btn_Use;

		// Token: 0x04001641 RID: 5697
		[SerializeField]
		private Button btn_Split;

		// Token: 0x04001642 RID: 5698
		[SerializeField]
		private Button btn_Dump;

		// Token: 0x04001643 RID: 5699
		[SerializeField]
		private Button btn_Equip;

		// Token: 0x04001644 RID: 5700
		[SerializeField]
		private Button btn_Modify;

		// Token: 0x04001645 RID: 5701
		[SerializeField]
		private Button btn_Unload;

		// Token: 0x04001646 RID: 5702
		[SerializeField]
		private Button btn_Wishlist;

		// Token: 0x04001647 RID: 5703
		[SerializeField]
		private bool alwaysModifyable;

		// Token: 0x04001648 RID: 5704
		private View targetView;

		// Token: 0x04001649 RID: 5705
		private ItemDisplay TargetDisplay;

		// Token: 0x0400164A RID: 5706
		private Item displayingItem;
	}
}
