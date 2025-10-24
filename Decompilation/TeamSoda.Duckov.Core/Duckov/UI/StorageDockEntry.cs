using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem.Data;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003AE RID: 942
	public class StorageDockEntry : MonoBehaviour
	{
		// Token: 0x060021DD RID: 8669 RVA: 0x00075EA4 File Offset: 0x000740A4
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00075EC2 File Offset: 0x000740C2
		private void OnButtonClick()
		{
			if (!PlayerStorage.IsAccessableAndNotFull())
			{
				return;
			}
			this.TakeTask().Forget();
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x00075ED8 File Offset: 0x000740D8
		private UniTask TakeTask()
		{
			StorageDockEntry.<TakeTask>d__13 <TakeTask>d__;
			<TakeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<TakeTask>d__.<>4__this = this;
			<TakeTask>d__.<>1__state = -1;
			<TakeTask>d__.<>t__builder.Start<StorageDockEntry.<TakeTask>d__13>(ref <TakeTask>d__);
			return <TakeTask>d__.<>t__builder.Task;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x00075F1C File Offset: 0x0007411C
		public void Setup(int index, ItemTreeData item)
		{
			this.index = index;
			this.item = item;
			ItemTreeData.DataEntry rootData = item.RootData;
			this.itemDisplay.Setup(rootData.typeID);
			int stackCount = rootData.StackCount;
			if (stackCount > 1)
			{
				this.countText.text = stackCount.ToString();
				this.countDisplay.SetActive(true);
			}
			else
			{
				this.countDisplay.SetActive(false);
			}
			if (PlayerStorage.IsAccessableAndNotFull())
			{
				this.bgImage.color = this.colorNormal;
				this.text.text = this.textKeyNormal.ToPlainText();
			}
			else
			{
				this.bgImage.color = this.colorFull;
				this.text.text = this.textKeyInventoryFull.ToPlainText();
			}
			this.text.gameObject.SetActive(true);
			this.loadingIndicator.SetActive(false);
		}

		// Token: 0x040016D8 RID: 5848
		[SerializeField]
		private ItemMetaDisplay itemDisplay;

		// Token: 0x040016D9 RID: 5849
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040016DA RID: 5850
		[SerializeField]
		private GameObject countDisplay;

		// Token: 0x040016DB RID: 5851
		[SerializeField]
		private TextMeshProUGUI countText;

		// Token: 0x040016DC RID: 5852
		[SerializeField]
		private Image bgImage;

		// Token: 0x040016DD RID: 5853
		[SerializeField]
		private Button button;

		// Token: 0x040016DE RID: 5854
		[SerializeField]
		private GameObject loadingIndicator;

		// Token: 0x040016DF RID: 5855
		[SerializeField]
		private Color colorNormal;

		// Token: 0x040016E0 RID: 5856
		[SerializeField]
		private Color colorFull;

		// Token: 0x040016E1 RID: 5857
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKeyNormal;

		// Token: 0x040016E2 RID: 5858
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKeyInventoryFull;

		// Token: 0x040016E3 RID: 5859
		private int index;

		// Token: 0x040016E4 RID: 5860
		private ItemTreeData item;
	}
}
