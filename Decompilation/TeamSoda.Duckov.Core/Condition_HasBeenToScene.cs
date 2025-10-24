using System;
using Duckov.Quests;
using Duckov.Scenes;

// Token: 0x02000116 RID: 278
public class Condition_HasBeenToScene : Condition
{
	// Token: 0x0600096B RID: 2411 RVA: 0x0002937D File Offset: 0x0002757D
	public override bool Evaluate()
	{
		return MultiSceneCore.GetVisited(this.sceneID);
	}

	// Token: 0x04000851 RID: 2129
	[SceneID]
	public string sceneID;
}
