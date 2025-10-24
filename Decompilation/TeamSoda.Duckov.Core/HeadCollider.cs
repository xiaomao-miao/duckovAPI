using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class HeadCollider : MonoBehaviour
{
	// Token: 0x060003A1 RID: 929 RVA: 0x0000FE81 File Offset: 0x0000E081
	public void Init(CharacterMainControl _character)
	{
		this.character = _character;
		this.character.OnTeamChanged += this.OnSetTeam;
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0000FEA1 File Offset: 0x0000E0A1
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.OnTeamChanged -= this.OnSetTeam;
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000FEC8 File Offset: 0x0000E0C8
	private void OnSetTeam(Teams team)
	{
		bool enabled = Team.IsEnemy(Teams.player, team);
		this.sphereCollider.enabled = enabled;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0000FEEC File Offset: 0x0000E0EC
	private void OnDrawGizmos()
	{
		Color yellow = Color.yellow;
		yellow.a = 0.3f;
		Gizmos.color = yellow;
		Gizmos.DrawSphere(base.transform.position, this.sphereCollider.radius * base.transform.lossyScale.x);
	}

	// Token: 0x040002C1 RID: 705
	private CharacterMainControl character;

	// Token: 0x040002C2 RID: 706
	[SerializeField]
	private SphereCollider sphereCollider;
}
