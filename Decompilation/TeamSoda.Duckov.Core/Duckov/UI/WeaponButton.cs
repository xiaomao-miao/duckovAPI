using System;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A2 RID: 930
	public class WeaponButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06002128 RID: 8488 RVA: 0x000739A0 File Offset: 0x00071BA0
		// (remove) Token: 0x06002129 RID: 8489 RVA: 0x000739D4 File Offset: 0x00071BD4
		public static event Action<WeaponButton> OnWeaponButtonSelected;

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x00073A07 File Offset: 0x00071C07
		private CharacterMainControl Character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600212B RID: 8491 RVA: 0x00073A0F File Offset: 0x00071C0F
		private Slot TargetSlot
		{
			get
			{
				return this._targetSlot;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x00073A17 File Offset: 0x00071C17
		private Item TargetItem
		{
			get
			{
				Slot targetSlot = this.TargetSlot;
				if (targetSlot == null)
				{
					return null;
				}
				return targetSlot.Content;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x00073A2C File Offset: 0x00071C2C
		private bool IsSelected
		{
			get
			{
				Item targetItem = this.TargetItem;
				if (((targetItem != null) ? targetItem.ActiveAgent : null) != null)
				{
					UnityEngine.Object activeAgent = this.TargetItem.ActiveAgent;
					ItemAgentHolder agentHolder = this._character.agentHolder;
					return activeAgent == ((agentHolder != null) ? agentHolder.CurrentHoldItemAgent : null);
				}
				return false;
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00073A7C File Offset: 0x00071C7C
		private void Awake()
		{
			this.RegisterStaticEvents();
			LevelManager instance = LevelManager.Instance;
			if (((instance != null) ? instance.MainCharacter : null) != null)
			{
				this.Initialize(LevelManager.Instance.MainCharacter);
			}
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00073AAD File Offset: 0x00071CAD
		private void OnDestroy()
		{
			this.UnregisterStaticEvents();
			this.isBeingDestroyed = true;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00073ABC File Offset: 0x00071CBC
		private void RegisterStaticEvents()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Combine(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeHoldItemAgent));
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00073AEF File Offset: 0x00071CEF
		private void UnregisterStaticEvents()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Remove(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeHoldItemAgent));
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00073B22 File Offset: 0x00071D22
		private void OnMainCharacterChangeHoldItemAgent(CharacterMainControl control, DuckovItemAgent agent)
		{
			if (this._character && control == this._character)
			{
				this.Refresh();
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00073B45 File Offset: 0x00071D45
		private void OnLevelInitialized()
		{
			LevelManager instance = LevelManager.Instance;
			this.Initialize((instance != null) ? instance.MainCharacter : null);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00073B60 File Offset: 0x00071D60
		private void Initialize(CharacterMainControl character)
		{
			this.UnregisterEvents();
			this._character = character;
			if (character == null)
			{
				Debug.LogError("Character 不存在，初始化失败");
			}
			if (character.CharacterItem == null)
			{
				Debug.LogError("Character item 不存在，初始化失败");
			}
			this._targetSlot = character.CharacterItem.Slots.GetSlot(this.targetSlotKey);
			if (this._targetSlot == null)
			{
				Debug.LogError("Slot " + this.targetSlotKey + " 不存在，初始化失败");
			}
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x00073BEF File Offset: 0x00071DEF
		private void RegisterEvents()
		{
			if (this._targetSlot == null)
			{
				return;
			}
			this._targetSlot.onSlotContentChanged += this.OnSlotContentChanged;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00073C11 File Offset: 0x00071E11
		private void UnregisterEvents()
		{
			if (this._targetSlot == null)
			{
				return;
			}
			this._targetSlot.onSlotContentChanged -= this.OnSlotContentChanged;
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00073C33 File Offset: 0x00071E33
		private void OnSlotContentChanged(Slot slot)
		{
			this.Refresh();
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00073C3C File Offset: 0x00071E3C
		private void Refresh()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this.displayParent.SetActive(this.TargetItem);
			bool isSelected = this.IsSelected;
			if (this.TargetItem)
			{
				this.icon.sprite = this.TargetItem.Icon;
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(this.TargetItem.DisplayQuality);
				this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
				this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
				this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				this.selectionFrame.SetActive(isSelected);
			}
			UnityEvent<WeaponButton> unityEvent = this.onRefresh;
			if (unityEvent != null)
			{
				unityEvent.Invoke(this);
			}
			if (isSelected)
			{
				UnityEvent<WeaponButton> unityEvent2 = this.onSelected;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke(this);
				}
				Action<WeaponButton> onWeaponButtonSelected = WeaponButton.OnWeaponButtonSelected;
				if (onWeaponButtonSelected == null)
				{
					return;
				}
				onWeaponButtonSelected(this);
			}
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00073D1E File Offset: 0x00071F1E
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.Character == null)
			{
				return;
			}
			UnityEvent<WeaponButton> unityEvent = this.onClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x0400167B RID: 5755
		[SerializeField]
		private string targetSlotKey = "PrimaryWeapon";

		// Token: 0x0400167C RID: 5756
		[SerializeField]
		private GameObject displayParent;

		// Token: 0x0400167D RID: 5757
		[SerializeField]
		private Image icon;

		// Token: 0x0400167E RID: 5758
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x0400167F RID: 5759
		[SerializeField]
		private GameObject selectionFrame;

		// Token: 0x04001680 RID: 5760
		public UnityEvent<WeaponButton> onClick;

		// Token: 0x04001681 RID: 5761
		public UnityEvent<WeaponButton> onRefresh;

		// Token: 0x04001682 RID: 5762
		public UnityEvent<WeaponButton> onSelected;

		// Token: 0x04001684 RID: 5764
		private CharacterMainControl _character;

		// Token: 0x04001685 RID: 5765
		private Slot _targetSlot;

		// Token: 0x04001686 RID: 5766
		private bool isBeingDestroyed;
	}
}
