using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AC RID: 940
	public class ItemShortcutPanel : MonoBehaviour
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x00075C3C File Offset: 0x00073E3C
		// (set) Token: 0x060021C9 RID: 8649 RVA: 0x00075C44 File Offset: 0x00073E44
		public Inventory Target { get; private set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x00075C4D File Offset: 0x00073E4D
		// (set) Token: 0x060021CB RID: 8651 RVA: 0x00075C55 File Offset: 0x00073E55
		public CharacterMainControl Character { get; internal set; }

		// Token: 0x060021CC RID: 8652 RVA: 0x00075C5E File Offset: 0x00073E5E
		private void Awake()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.Initialize();
			}
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x00075C7E File Offset: 0x00073E7E
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00075C91 File Offset: 0x00073E91
		private void OnLevelInitialized()
		{
			this.Initialize();
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00075C9C File Offset: 0x00073E9C
		private void Initialize()
		{
			LevelManager instance = LevelManager.Instance;
			this.Character = ((instance != null) ? instance.MainCharacter : null);
			if (this.Character == null)
			{
				return;
			}
			LevelManager instance2 = LevelManager.Instance;
			Inventory target;
			if (instance2 == null)
			{
				target = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance2.MainCharacter;
				if (mainCharacter == null)
				{
					target = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					target = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			this.Target = target;
			if (this.Target == null)
			{
				return;
			}
			for (int i = 0; i < this.buttons.Length; i++)
			{
				ItemShortcutButton itemShortcutButton = this.buttons[i];
				if (!(itemShortcutButton == null))
				{
					itemShortcutButton.Initialize(this, i);
				}
			}
			this.initialized = true;
		}

		// Token: 0x040016D0 RID: 5840
		[SerializeField]
		private ItemShortcutButton[] buttons;

		// Token: 0x040016D3 RID: 5843
		private bool initialized;
	}
}
