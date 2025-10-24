using System;

// Token: 0x02000098 RID: 152
public static class CharacterMainControlExtensions
{
	// Token: 0x06000529 RID: 1321 RVA: 0x000174DC File Offset: 0x000156DC
	public static bool IsMainCharacter(this CharacterMainControl character)
	{
		if (character == null)
		{
			return false;
		}
		LevelManager instance = LevelManager.Instance;
		return ((instance != null) ? instance.MainCharacter : null) == character;
	}
}
