using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.MiniGames.GoldMiner.UI;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A8 RID: 680
	public class PassivePropsUI : MiniGameBehaviour
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x00051938 File Offset: 0x0004FB38
		private PrefabPool<PassivePropDisplay> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<PassivePropDisplay>(this.entryTemplate, null, new Action<PassivePropDisplay>(this.OnGetEntry), new Action<PassivePropDisplay>(this.OnReleaseEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00051987 File Offset: 0x0004FB87
		private void OnReleaseEntry(PassivePropDisplay display)
		{
			this.navGroup.Remove(display.NavEntry);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0005199A File Offset: 0x0004FB9A
		private void OnGetEntry(PassivePropDisplay display)
		{
			this.navGroup.Add(display.NavEntry);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x000519B0 File Offset: 0x0004FBB0
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = this.master;
			goldMiner2.onArtifactChange = (Action<GoldMiner>)Delegate.Combine(goldMiner2.onArtifactChange, new Action<GoldMiner>(this.OnArtifactChanged));
			GoldMiner goldMiner3 = this.master;
			goldMiner3.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner3.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyTick));
			NavGroup.OnNavGroupChanged = (Action)Delegate.Combine(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00051A52 File Offset: 0x0004FC52
		private void OnDestroy()
		{
			NavGroup.OnNavGroupChanged = (Action)Delegate.Remove(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00051A74 File Offset: 0x0004FC74
		private void OnNavGroupChanged()
		{
			this.changeLock = true;
			if (this.navGroup.active && this.Pool.ActiveEntries.Count <= 0)
			{
				this.upNavGroup.SetAsActiveNavGroup();
			}
			this.RefreshDescription();
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00051AB0 File Offset: 0x0004FCB0
		private void OnEarlyTick(GoldMiner miner)
		{
			this.RefreshDescription();
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00051AC4 File Offset: 0x0004FCC4
		private void SetCoord([TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> coord)
		{
			int navIndex = this.CoordToIndex(coord);
			this.navGroup.NavIndex = navIndex;
			this.RefreshDescription();
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00051AEC File Offset: 0x0004FCEC
		private void RefreshDescription()
		{
			if (!this.navGroup.active)
			{
				this.HideDescription();
				return;
			}
			if (this.Pool.ActiveEntries.Count <= 0)
			{
				this.HideDescription();
				return;
			}
			NavEntry selectedEntry = this.navGroup.GetSelectedEntry();
			if (selectedEntry == null)
			{
				this.HideDescription();
				return;
			}
			if (!selectedEntry.VCT.IsHovering)
			{
				this.HideDescription();
				return;
			}
			PassivePropDisplay component = selectedEntry.GetComponent<PassivePropDisplay>();
			if (component == null)
			{
				this.HideDescription();
				return;
			}
			this.SetupAndShowDescription(component);
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00051B75 File Offset: 0x0004FD75
		private void HideDescription()
		{
			this.descriptionContainer.gameObject.SetActive(false);
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00051B88 File Offset: 0x0004FD88
		private void SetupAndShowDescription(PassivePropDisplay ppd)
		{
			this.descriptionContainer.gameObject.SetActive(true);
			string description = ppd.Target.Description;
			this.descriptionText.text = description;
			this.descriptionContainer.position = ppd.rectTransform.TransformPoint(ppd.rectTransform.rect.max);
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00051BEC File Offset: 0x0004FDEC
		private int CoordToIndex([TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> coord)
		{
			int count = this.navGroup.entries.Count;
			if (count <= 0)
			{
				return 0;
			}
			int constraintCount = this.gridLayout.constraintCount;
			int num = count / constraintCount;
			if (coord.Item2 > num)
			{
				coord.Item2 = num;
			}
			int num2 = constraintCount;
			if (coord.Item2 == num)
			{
				num2 = count % constraintCount;
			}
			if (coord.Item1 < 0)
			{
				coord.Item1 = num2 - 1;
			}
			coord.Item1 %= num2;
			if (coord.Item2 < 0)
			{
				coord.Item2 = num;
			}
			coord.Item2 %= num + 1;
			return constraintCount * coord.Item2 + coord.Item1;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00051C90 File Offset: 0x0004FE90
		[return: TupleElementNames(new string[]
		{
			"x",
			"y"
		})]
		private ValueTuple<int, int> IndexToCoord(int index)
		{
			int constraintCount = this.gridLayout.constraintCount;
			int item = index / constraintCount;
			return new ValueTuple<int, int>(index % constraintCount, item);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00051CB6 File Offset: 0x0004FEB6
		private void OnLevelBegin(GoldMiner miner)
		{
			this.Refresh();
			this.RefreshDescription();
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00051CC4 File Offset: 0x0004FEC4
		private void OnArtifactChanged(GoldMiner miner)
		{
			this.Refresh();
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00051CCC File Offset: 0x0004FECC
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			if (this.master == null)
			{
				return;
			}
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			foreach (IGrouping<string, GoldMinerArtifact> source in from e in run.artifacts
			where e != null
			group e by e.ID)
			{
				GoldMinerArtifact target = source.ElementAt(0);
				this.Pool.Get(null).Setup(target, source.Count<GoldMinerArtifact>());
			}
		}

		// Token: 0x04001067 RID: 4199
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001068 RID: 4200
		[SerializeField]
		private RectTransform descriptionContainer;

		// Token: 0x04001069 RID: 4201
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x0400106A RID: 4202
		[SerializeField]
		private PassivePropDisplay entryTemplate;

		// Token: 0x0400106B RID: 4203
		[SerializeField]
		private NavGroup navGroup;

		// Token: 0x0400106C RID: 4204
		[SerializeField]
		private NavGroup upNavGroup;

		// Token: 0x0400106D RID: 4205
		[SerializeField]
		private GridLayoutGroup gridLayout;

		// Token: 0x0400106E RID: 4206
		private PrefabPool<PassivePropDisplay> _pool;

		// Token: 0x0400106F RID: 4207
		private bool changeLock;
	}
}
