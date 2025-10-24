using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class AimTargetFinder : MonoBehaviour
{
	// Token: 0x0600045C RID: 1116 RVA: 0x00014181 File Offset: 0x00012381
	private void Start()
	{
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00014184 File Offset: 0x00012384
	public Transform Find(bool search, Vector3 findPoint, ref CharacterMainControl foundCharacter)
	{
		Transform result = null;
		if (search)
		{
			result = this.Search(findPoint, ref foundCharacter);
		}
		return result;
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000141A0 File Offset: 0x000123A0
	private Transform Search(Vector3 findPoint, ref CharacterMainControl character)
	{
		character = null;
		if (this.overlapcColliders == null)
		{
			this.overlapcColliders = new Collider[6];
			this.damageReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
		}
		int num = Physics.OverlapSphereNonAlloc(findPoint, this.searchRadius, this.overlapcColliders, this.damageReceiverLayers);
		Collider collider = null;
		if (num > 0)
		{
			int i = 0;
			while (i < num)
			{
				DamageReceiver component = this.overlapcColliders[i].GetComponent<DamageReceiver>();
				if (!(component == null) && component.Team != Teams.player)
				{
					collider = this.overlapcColliders[i];
					if (component.health != null)
					{
						character = component.health.GetComponent<CharacterMainControl>();
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		if (collider)
		{
			return collider.transform;
		}
		return null;
	}

	// Token: 0x040003C4 RID: 964
	private Vector3 searchPoint;

	// Token: 0x040003C5 RID: 965
	public float searchRadius;

	// Token: 0x040003C6 RID: 966
	private LayerMask damageReceiverLayers;

	// Token: 0x040003C7 RID: 967
	private Collider[] overlapcColliders;
}
