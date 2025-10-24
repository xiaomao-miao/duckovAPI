using System;
using System.Collections.Generic;
using NodeCanvas.Framework;

namespace Duckov.PerkTrees
{
	// Token: 0x02000256 RID: 598
	public class PerkTreeRelationGraphOwner : GraphOwner<PerkRelationGraph>
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x00046370 File Offset: 0x00044570
		public PerkRelationGraph RelationGraph
		{
			get
			{
				if (this._relationGraph == null)
				{
					this._relationGraph = (this.graph as PerkRelationGraph);
				}
				return this._relationGraph;
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00046398 File Offset: 0x00044598
		public List<Perk> GetRequiredNodes(Perk node)
		{
			PerkRelationNode relatedNode = this.RelationGraph.GetRelatedNode(node);
			if (relatedNode == null)
			{
				return null;
			}
			List<PerkRelationNode> incomingNodes = this.RelationGraph.GetIncomingNodes(relatedNode);
			if (incomingNodes == null)
			{
				return null;
			}
			if (incomingNodes.Count < 1)
			{
				return null;
			}
			List<Perk> list = new List<Perk>();
			foreach (PerkRelationNode perkRelationNode in incomingNodes)
			{
				Perk relatedNode2 = perkRelationNode.relatedNode;
				if (!(relatedNode2 == null))
				{
					list.Add(relatedNode2);
				}
			}
			return list;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00046430 File Offset: 0x00044630
		internal PerkRelationNode GetRelatedNode(Perk perk)
		{
			if (this.RelationGraph == null)
			{
				return null;
			}
			return this.RelationGraph.GetRelatedNode(perk);
		}

		// Token: 0x04000E1E RID: 3614
		private PerkRelationGraph _relationGraph;
	}
}
