using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
[Serializable]
public class CharacterSkillKeeper
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0002A266 File Offset: 0x00028466
	public SkillBase Skill
	{
		get
		{
			return this.skill;
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0002A26E File Offset: 0x0002846E
	public void SetSkill(SkillBase _skill, GameObject _bindingObject)
	{
		this.skill = null;
		this.skillBindingObject = null;
		if (_skill != null && _bindingObject != null)
		{
			this.skill = _skill;
			this.skillBindingObject = _bindingObject;
		}
		Action onSkillChanged = this.OnSkillChanged;
		if (onSkillChanged == null)
		{
			return;
		}
		onSkillChanged();
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0002A2AE File Offset: 0x000284AE
	public bool CheckSkillAndBinding()
	{
		if (this.skill != null && this.skillBindingObject != null)
		{
			return true;
		}
		this.skill = null;
		this.skillBindingObject = null;
		return false;
	}

	// Token: 0x0400088E RID: 2190
	private SkillBase skill;

	// Token: 0x0400088F RID: 2191
	private GameObject skillBindingObject;

	// Token: 0x04000890 RID: 2192
	public Action OnSkillChanged;
}
