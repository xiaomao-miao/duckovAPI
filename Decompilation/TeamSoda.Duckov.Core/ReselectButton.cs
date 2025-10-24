using System;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200015B RID: 347
public class ReselectButton : MonoBehaviour
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x0002E463 File Offset: 0x0002C663
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0002E481 File Offset: 0x0002C681
	private void OnEnable()
	{
		this.setActiveGroup.SetActive(LevelManager.Instance && LevelManager.Instance.IsBaseLevel);
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0002E4A7 File Offset: 0x0002C6A7
	private void OnDisable()
	{
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0002E4AC File Offset: 0x0002C6AC
	private void OnButtonClicked()
	{
		SceneLoader.Instance.LoadScene(this.prepareSceneID, null, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
		if (PauseMenu.Instance && PauseMenu.Instance.Shown)
		{
			PauseMenu.Hide();
		}
	}

	// Token: 0x04000948 RID: 2376
	[SerializeField]
	private GameObject setActiveGroup;

	// Token: 0x04000949 RID: 2377
	[SerializeField]
	private Button button;

	// Token: 0x0400094A RID: 2378
	[SerializeField]
	[SceneID]
	private string prepareSceneID = "Prepare";
}
