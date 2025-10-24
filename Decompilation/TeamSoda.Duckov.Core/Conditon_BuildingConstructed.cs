using System;
using Duckov.Buildings;
using Duckov.Quests;

// Token: 0x0200011A RID: 282
public class Conditon_BuildingConstructed : Condition
{
	// Token: 0x06000974 RID: 2420 RVA: 0x000294AC File Offset: 0x000276AC
	public override bool Evaluate()
	{
		bool flag = BuildingManager.Any(this.buildingID, false);
		if (this.not)
		{
			flag = !flag;
		}
		return flag;
	}

	// Token: 0x04000856 RID: 2134
	public string buildingID;

	// Token: 0x04000857 RID: 2135
	public bool not;
}
