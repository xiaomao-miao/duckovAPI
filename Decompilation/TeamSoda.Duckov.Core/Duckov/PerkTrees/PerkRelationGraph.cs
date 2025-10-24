using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.PerkTrees
{
	// Token: 0x02000253 RID: 595
	public class PerkRelationGraph : Graph
	{
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00046145 File Offset: 0x00044345
		public override Type baseNodeType
		{
			get
			{
				return typeof(PerkRelationNodeBase);
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00046151 File Offset: 0x00044351
		public override bool requiresAgent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x00046154 File Offset: 0x00044354
		public override bool requiresPrimeNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x00046157 File Offset: 0x00044357
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0004615A File Offset: 0x0004435A
		public override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Vertical;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0004615D File Offset: 0x0004435D
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x00046160 File Offset: 0x00044360
		public override bool canAcceptVariableDrops
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00046164 File Offset: 0x00044364
		public PerkRelationNode GetRelatedNode(Perk perk)
		{
			return base.allNodes.Find(delegate(Node node)
			{
				if (node == null)
				{
					return false;
				}
				PerkRelationNode perkRelationNode = node as PerkRelationNode;
				return perkRelationNode != null && perkRelationNode.relatedNode == perk;
			}) as PerkRelationNode;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0004619C File Offset: 0x0004439C
		public List<PerkRelationNode> GetIncomingNodes(PerkRelationNode skillTreeNode)
		{
			List<PerkRelationNode> list = new List<PerkRelationNode>();
			foreach (Connection connection in skillTreeNode.inConnections)
			{
				if (connection != null)
				{
					PerkRelationNode perkRelationNode = connection.sourceNode as PerkRelationNode;
					if (perkRelationNode != null)
					{
						list.Add(perkRelationNode);
					}
				}
			}
			return list;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00046208 File Offset: 0x00044408
		public List<PerkRelationNode> GetOutgoingNodes(PerkRelationNode skillTreeNode)
		{
			List<PerkRelationNode> list = new List<PerkRelationNode>();
			foreach (Connection connection in skillTreeNode.outConnections)
			{
				if (connection != null)
				{
					PerkRelationNode perkRelationNode = connection.targetNode as PerkRelationNode;
					if (perkRelationNode != null)
					{
						list.Add(perkRelationNode);
					}
				}
			}
			return list;
		}
	}
}
