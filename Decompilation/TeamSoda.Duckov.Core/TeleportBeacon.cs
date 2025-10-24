using System;
using Duckov.Scenes;
using Duckvo.Beacons;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class TeleportBeacon : MonoBehaviour
{
	// Token: 0x060005E6 RID: 1510 RVA: 0x0001A4E4 File Offset: 0x000186E4
	private void Start()
	{
		bool beaconUnlocked = BeaconManager.GetBeaconUnlocked(this.beaconScene, this.beaconIndex);
		this.activeByUnlocked.SetActive(beaconUnlocked);
		this.interactable.gameObject.SetActive(!beaconUnlocked);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001A523 File Offset: 0x00018723
	public void ActivateBeacon()
	{
		BeaconManager.UnlockBeacon(this.beaconScene, this.beaconIndex);
		this.activeByUnlocked.SetActive(true);
		this.interactable.gameObject.SetActive(false);
	}

	// Token: 0x0400056D RID: 1389
	[SceneID]
	public string beaconScene;

	// Token: 0x0400056E RID: 1390
	public int beaconIndex;

	// Token: 0x0400056F RID: 1391
	public GameObject activeByUnlocked;

	// Token: 0x04000570 RID: 1392
	public InteractableBase interactable;
}
