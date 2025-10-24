using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000250 RID: 592
	public class PerkTree : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00045C35 File Offset: 0x00043E35
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x00045C33 File Offset: 0x00043E33
		[LocalizationKey("Perks")]
		private string perkTreeName
		{
			get
			{
				return this.displayNameKey;
			}
			set
			{
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00045C3D File Offset: 0x00043E3D
		public string ID
		{
			get
			{
				return this.perkTreeID;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x00045C45 File Offset: 0x00043E45
		private string displayNameKey
		{
			get
			{
				return "PerkTree_" + this.ID;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x00045C57 File Offset: 0x00043E57
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x00045C64 File Offset: 0x00043E64
		public bool Horizontal
		{
			get
			{
				return this.horizontal;
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x0600127A RID: 4730 RVA: 0x00045C6C File Offset: 0x00043E6C
		// (remove) Token: 0x0600127B RID: 4731 RVA: 0x00045CA4 File Offset: 0x00043EA4
		public event Action<PerkTree> onPerkTreeStatusChanged;

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x00045CD9 File Offset: 0x00043ED9
		public ReadOnlyCollection<Perk> Perks
		{
			get
			{
				if (this.perks_ReadOnly == null)
				{
					this.perks_ReadOnly = this.perks.AsReadOnly();
				}
				return this.perks_ReadOnly;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00045CFA File Offset: 0x00043EFA
		public PerkTreeRelationGraphOwner RelationGraphOwner
		{
			get
			{
				return this.relationGraphOwner;
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00045D02 File Offset: 0x00043F02
		private void Awake()
		{
			this.Load();
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00045D2C File Offset: 0x00043F2C
		private void Start()
		{
			foreach (Perk perk in this.perks)
			{
				if (!(perk == null) && perk.DefaultUnlocked)
				{
					perk.ForceUnlock();
				}
			}
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00045D90 File Offset: 0x00043F90
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00045DB4 File Offset: 0x00043FB4
		public object GenerateSaveData()
		{
			return new PerkTree.SaveData(this);
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00045DBC File Offset: 0x00043FBC
		public void SetupSaveData(object data)
		{
			foreach (Perk perk in this.perks)
			{
				perk.Unlocked = false;
			}
			PerkTree.SaveData saveData = data as PerkTree.SaveData;
			if (saveData == null)
			{
				return;
			}
			using (List<Perk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Perk cur = enumerator.Current;
					if (!(cur == null))
					{
						PerkTree.SaveData.Entry entry = saveData.entries.Find((PerkTree.SaveData.Entry e) => e != null && e.perkName == cur.name);
						if (entry != null)
						{
							cur.Unlocked = entry.unlocked;
							cur.unlocking = entry.unlocking;
							cur.unlockingBeginTimeRaw = entry.unlockingBeginTime;
						}
					}
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00045EBC File Offset: 0x000440BC
		private string SaveKey
		{
			get
			{
				return "PerkTree_" + this.perkTreeID;
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00045ECE File Offset: 0x000440CE
		public void Save()
		{
			SavesSystem.Save<PerkTree.SaveData>(this.SaveKey, this.GenerateSaveData() as PerkTree.SaveData);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00045EE8 File Offset: 0x000440E8
		public void Load()
		{
			if (!SavesSystem.KeyExisits(this.SaveKey))
			{
				return;
			}
			PerkTree.SaveData data = SavesSystem.Load<PerkTree.SaveData>(this.SaveKey);
			this.SetupSaveData(data);
			this.loaded = true;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00045F20 File Offset: 0x00044120
		public void ReapplyPerks()
		{
			foreach (Perk perk in this.perks)
			{
				perk.Unlocked = false;
			}
			foreach (Perk perk2 in this.perks)
			{
				perk2.Unlocked = perk2.Unlocked;
			}
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00045FB8 File Offset: 0x000441B8
		internal bool AreAllParentsUnlocked(Perk perk)
		{
			PerkRelationNode relatedNode = this.RelationGraphOwner.GetRelatedNode(perk);
			if (relatedNode == null)
			{
				return false;
			}
			foreach (PerkRelationNode perkRelationNode in this.relationGraphOwner.RelationGraph.GetIncomingNodes(relatedNode))
			{
				Perk relatedNode2 = perkRelationNode.relatedNode;
				if (!(relatedNode2 == null) && !relatedNode2.Unlocked)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00046040 File Offset: 0x00044240
		internal void NotifyChildStateChanged(Perk perk)
		{
			PerkRelationNode relatedNode = this.RelationGraphOwner.GetRelatedNode(perk);
			if (relatedNode == null)
			{
				return;
			}
			foreach (PerkRelationNode perkRelationNode in this.relationGraphOwner.RelationGraph.GetOutgoingNodes(relatedNode))
			{
				perkRelationNode.NotifyIncomingStateChanged();
			}
			Action<PerkTree> action = this.onPerkTreeStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x000460C0 File Offset: 0x000442C0
		private void Collect()
		{
			this.perks.Clear();
			Perk[] componentsInChildren = base.transform.GetComponentsInChildren<Perk>();
			Perk[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Master = this;
			}
			this.perks.AddRange(componentsInChildren);
		}

		// Token: 0x04000E11 RID: 3601
		[SerializeField]
		private string perkTreeID = "DefaultPerkTree";

		// Token: 0x04000E12 RID: 3602
		[SerializeField]
		private bool horizontal;

		// Token: 0x04000E13 RID: 3603
		[SerializeField]
		private PerkTreeRelationGraphOwner relationGraphOwner;

		// Token: 0x04000E14 RID: 3604
		[SerializeField]
		internal List<Perk> perks = new List<Perk>();

		// Token: 0x04000E16 RID: 3606
		private ReadOnlyCollection<Perk> perks_ReadOnly;

		// Token: 0x04000E17 RID: 3607
		private bool loaded;

		// Token: 0x02000534 RID: 1332
		[Serializable]
		private class SaveData
		{
			// Token: 0x060027BF RID: 10175 RVA: 0x00091834 File Offset: 0x0008FA34
			public SaveData(PerkTree perkTree)
			{
				this.entries = new List<PerkTree.SaveData.Entry>();
				for (int i = 0; i < perkTree.perks.Count; i++)
				{
					Perk perk = perkTree.perks[i];
					if (!(perk == null))
					{
						this.entries.Add(new PerkTree.SaveData.Entry(perk));
					}
				}
			}

			// Token: 0x04001E7C RID: 7804
			public List<PerkTree.SaveData.Entry> entries;

			// Token: 0x02000674 RID: 1652
			[Serializable]
			public class Entry
			{
				// Token: 0x06002AAC RID: 10924 RVA: 0x000A1519 File Offset: 0x0009F719
				public Entry(Perk perk)
				{
					this.perkName = perk.name;
					this.unlocked = perk.Unlocked;
					this.unlocking = perk.Unlocking;
					this.unlockingBeginTime = perk.unlockingBeginTimeRaw;
				}

				// Token: 0x04002332 RID: 9010
				public string perkName;

				// Token: 0x04002333 RID: 9011
				public bool unlocking;

				// Token: 0x04002334 RID: 9012
				public long unlockingBeginTime;

				// Token: 0x04002335 RID: 9013
				public bool unlocked;
			}
		}
	}
}
