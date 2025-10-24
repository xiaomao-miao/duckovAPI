using System;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using Saves;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003EA RID: 1002
	public class ContinueButton : MonoBehaviour
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x0007DF5D File Offset: 0x0007C15D
		[SerializeField]
		private string Text_NewGame
		{
			get
			{
				return this.text_NewGame.ToPlainText();
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x0007DF6A File Offset: 0x0007C16A
		[SerializeField]
		private string Text_Continue
		{
			get
			{
				return this.text_Continue.ToPlainText();
			}
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0007DF78 File Offset: 0x0007C178
		private void Awake()
		{
			SavesSystem.OnSetFile += this.Refresh;
			SavesSystem.OnSaveDeleted += this.Refresh;
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0007DFD4 File Offset: 0x0007C1D4
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Refresh;
			SavesSystem.OnSaveDeleted -= this.Refresh;
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0007E009 File Offset: 0x0007C209
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0007E014 File Offset: 0x0007C214
		private void OnButtonClicked()
		{
			GameManager.newBoot = true;
			if (MultiSceneCore.GetVisited("Base"))
			{
				SceneLoader.Instance.LoadBaseScene(null, true).Forget();
				return;
			}
			SavesSystem.Save<VersionData>("CreatedWithVersion", GameMetaData.Instance.Version);
			SceneLoader.Instance.LoadScene(GameplayDataSettings.SceneManagement.PrologueScene, this.overrideCurtainScene, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0007E087 File Offset: 0x0007C287
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0007E090 File Offset: 0x0007C290
		private void Refresh()
		{
			bool flag = SavesSystem.IsOldGame();
			this.text.text = (flag ? this.Text_Continue : this.Text_NewGame);
		}

		// Token: 0x04001899 RID: 6297
		[SerializeField]
		private Button button;

		// Token: 0x0400189A RID: 6298
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400189B RID: 6299
		[LocalizationKey("Default")]
		[SerializeField]
		private string text_NewGame = "新游戏";

		// Token: 0x0400189C RID: 6300
		[LocalizationKey("Default")]
		[SerializeField]
		private string text_Continue = "继续";

		// Token: 0x0400189D RID: 6301
		[SerializeField]
		private SceneReference overrideCurtainScene;
	}
}
