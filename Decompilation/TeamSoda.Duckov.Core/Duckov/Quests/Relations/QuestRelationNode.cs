using System;
using System.Collections.Generic;
using Duckov.Utilities;
using NodeCanvas.Framework;

namespace Duckov.Quests.Relations
{
	// Token: 0x0200035F RID: 863
	public class QuestRelationNode : QuestRelationNodeBase
	{
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0006A87A File Offset: 0x00068A7A
		private static QuestCollection QuestCollection
		{
			get
			{
				if (QuestRelationNode._questCollection == null)
				{
					QuestRelationNode._questCollection = GameplayDataSettings.QuestCollection;
				}
				return QuestRelationNode._questCollection;
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0006A898 File Offset: 0x00068A98
		private void SelectQuest()
		{
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0006A89C File Offset: 0x00068A9C
		public List<int> GetParents()
		{
			List<int> list = new List<int>();
			foreach (Connection connection in base.inConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					list.Add(questRelationNode.questID);
				}
				else
				{
					QuestRelationProxyNode questRelationProxyNode = connection.sourceNode as QuestRelationProxyNode;
					if (questRelationProxyNode != null)
					{
						list.Add(questRelationProxyNode.questID);
					}
				}
			}
			return list;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0006A92C File Offset: 0x00068B2C
		public List<int> GetChildren()
		{
			List<int> list = new List<int>();
			foreach (Connection connection in base.outConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					list.Add(questRelationNode.questID);
				}
			}
			return list;
		}

		// Token: 0x0400149C RID: 5276
		public int questID;

		// Token: 0x0400149D RID: 5277
		private static QuestCollection _questCollection;

		// Token: 0x0400149E RID: 5278
		internal bool isDuplicate;
	}
}
