using System;
using System.Collections.Generic;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000315 RID: 789
	public class BuildingEffect : MonoBehaviour
	{
		// Token: 0x06001A0F RID: 6671 RVA: 0x0005DFC6 File Offset: 0x0005C1C6
		private void Awake()
		{
			BuildingManager.OnBuildingListChanged += this.OnBuildingStatusChanged;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0005DFEA File Offset: 0x0005C1EA
		private void OnDestroy()
		{
			this.DisableEffects();
			BuildingManager.OnBuildingListChanged -= this.OnBuildingStatusChanged;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005E014 File Offset: 0x0005C214
		private void OnLevelInitialized()
		{
			this.Refresh();
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0005E01C File Offset: 0x0005C21C
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0005E024 File Offset: 0x0005C224
		private void OnBuildingStatusChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0005E02C File Offset: 0x0005C22C
		private void Refresh()
		{
			this.DisableEffects();
			if (this.IsBuildingConstructed())
			{
				this.EnableEffects();
			}
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005E042 File Offset: 0x0005C242
		private bool IsBuildingConstructed()
		{
			return BuildingManager.Any(this.buildingID, false);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0005E050 File Offset: 0x0005C250
		private void DisableEffects()
		{
			foreach (Modifier modifier in this.modifiers)
			{
				if (modifier != null)
				{
					modifier.RemoveFromTarget();
				}
			}
			this.modifiers.Clear();
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0005E0B0 File Offset: 0x0005C2B0
		private void EnableEffects()
		{
			this.DisableEffects();
			if (CharacterMainControl.Main == null)
			{
				return;
			}
			foreach (BuildingEffect.ModifierDescription description in this.modifierDescriptions)
			{
				this.Apply(description);
			}
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0005E118 File Offset: 0x0005C318
		private void Apply(BuildingEffect.ModifierDescription description)
		{
			CharacterMainControl main = CharacterMainControl.Main;
			Stat stat;
			if (main == null)
			{
				stat = null;
			}
			else
			{
				Item characterItem = main.CharacterItem;
				stat = ((characterItem != null) ? characterItem.GetStat(description.stat) : null);
			}
			Stat stat2 = stat;
			if (stat2 == null)
			{
				return;
			}
			Modifier modifier = new Modifier(description.type, description.value, this);
			stat2.AddModifier(modifier);
			this.modifiers.Add(modifier);
		}

		// Token: 0x040012BB RID: 4795
		[SerializeField]
		private string buildingID;

		// Token: 0x040012BC RID: 4796
		[SerializeField]
		private List<BuildingEffect.ModifierDescription> modifierDescriptions = new List<BuildingEffect.ModifierDescription>();

		// Token: 0x040012BD RID: 4797
		private List<Modifier> modifiers = new List<Modifier>();

		// Token: 0x020005A7 RID: 1447
		[Serializable]
		public struct ModifierDescription
		{
			// Token: 0x04002037 RID: 8247
			public string stat;

			// Token: 0x04002038 RID: 8248
			public ModifierType type;

			// Token: 0x04002039 RID: 8249
			public float value;
		}
	}
}
