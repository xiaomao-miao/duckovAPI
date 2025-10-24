using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace Duckov.Quests.Relations
{
	// Token: 0x0200035D RID: 861
	[CreateAssetMenu(menuName = "Quests/Relations")]
	public class QuestRelationGraph : Graph
	{
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0006A73A File Offset: 0x0006893A
		public override Type baseNodeType
		{
			get
			{
				return typeof(QuestRelationNodeBase);
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x0006A746 File Offset: 0x00068946
		public override bool requiresAgent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x0006A749 File Offset: 0x00068949
		public override bool requiresPrimeNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x0006A74C File Offset: 0x0006894C
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x0006A74F File Offset: 0x0006894F
		public override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Vertical;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001E28 RID: 7720 RVA: 0x0006A752 File Offset: 0x00068952
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x0006A755 File Offset: 0x00068955
		public override bool canAcceptVariableDrops
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0006A758 File Offset: 0x00068958
		public QuestRelationNode GetNode(int questID)
		{
			return base.allNodes.Find(delegate(Node node)
			{
				QuestRelationNode questRelationNode = node as QuestRelationNode;
				return questRelationNode != null && questRelationNode.questID == questID;
			}) as QuestRelationNode;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0006A790 File Offset: 0x00068990
		public List<int> GetRequiredIDs(int targetID)
		{
			List<int> list = new List<int>();
			QuestRelationNode node = this.GetNode(targetID);
			if (node == null)
			{
				return list;
			}
			foreach (Connection connection in node.inConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					int questID = questRelationNode.questID;
					list.Add(questID);
				}
				else
				{
					QuestRelationProxyNode questRelationProxyNode = connection.sourceNode as QuestRelationProxyNode;
					if (questRelationProxyNode != null)
					{
						int questID2 = questRelationProxyNode.questID;
						list.Add(questID2);
					}
				}
			}
			return list;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0006A838 File Offset: 0x00068A38
		protected override void OnGraphValidate()
		{
			this.CheckDuplicates();
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0006A840 File Offset: 0x00068A40
		internal void CheckDuplicates()
		{
		}

		// Token: 0x0400149B RID: 5275
		public static int selectedQuestID = -1;
	}
}
