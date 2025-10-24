using System;
using Duckov.Quests;
using Duckov.Scenes;
using Duckvo.Beacons;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class RequireBeaconUnlocked : Condition
{
	// Token: 0x06000976 RID: 2422 RVA: 0x000294DC File Offset: 0x000276DC
	public override bool Evaluate()
	{
		return BeaconManager.GetBeaconUnlocked(this.beaconID, this.beaconIndex);
	}

	// Token: 0x04000858 RID: 2136
	[SerializeField]
	[SceneID]
	private string beaconID;

	// Token: 0x04000859 RID: 2137
	[SerializeField]
	private int beaconIndex;
}
