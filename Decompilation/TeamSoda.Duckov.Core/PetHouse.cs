using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class PetHouse : MonoBehaviour
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000942 RID: 2370 RVA: 0x00028EE0 File Offset: 0x000270E0
	public static PetHouse Instance
	{
		get
		{
			return PetHouse.instance;
		}
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00028EE7 File Offset: 0x000270E7
	private void Awake()
	{
		PetHouse.instance = this;
		if (LevelManager.LevelInited)
		{
			this.OnLevelInited();
			return;
		}
		LevelManager.OnLevelInitialized += this.OnLevelInited;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00028F0E File Offset: 0x0002710E
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.OnLevelInited;
		if (this.petTarget)
		{
			this.petTarget.SetStandBy(false, this.petTarget.transform.position);
		}
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00028F4C File Offset: 0x0002714C
	private void OnLevelInited()
	{
		CharacterMainControl petCharacter = LevelManager.Instance.PetCharacter;
		petCharacter.SetPosition(this.petMarker.position);
		this.petTarget = petCharacter.GetComponentInChildren<PetAI>();
		if (this.petTarget != null)
		{
			this.petTarget.SetStandBy(true, this.petMarker.position);
		}
	}

	// Token: 0x0400083F RID: 2111
	private static PetHouse instance;

	// Token: 0x04000840 RID: 2112
	public Transform petMarker;

	// Token: 0x04000841 RID: 2113
	private PetAI petTarget;
}
