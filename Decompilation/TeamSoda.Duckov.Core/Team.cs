using System;

// Token: 0x02000074 RID: 116
public class Team
{
	// Token: 0x06000443 RID: 1091 RVA: 0x000136AB File Offset: 0x000118AB
	public static bool IsEnemy(Teams selfTeam, Teams targetTeam)
	{
		return selfTeam != Teams.middle && (selfTeam == Teams.all || (targetTeam != Teams.middle && selfTeam != targetTeam));
	}
}
