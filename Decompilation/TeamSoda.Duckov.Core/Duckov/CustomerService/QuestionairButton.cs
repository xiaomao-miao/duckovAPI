using System;
using Duckov.Rules;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.CustomerService
{
	// Token: 0x020003F5 RID: 1013
	public class QuestionairButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600249C RID: 9372 RVA: 0x0007EED8 File Offset: 0x0007D0D8
		public string GenerateQuestionair()
		{
			SystemLanguage currentLanguage = LocalizationManager.CurrentLanguage;
			string address;
			if (currentLanguage != SystemLanguage.Japanese)
			{
				if (currentLanguage == SystemLanguage.ChineseSimplified)
				{
					address = this.addressCN;
				}
				else
				{
					address = this.addressEN;
				}
			}
			else
			{
				address = this.addressJP;
			}
			int currentSlot = SavesSystem.CurrentSlot;
			string id = string.Format("{0}_{1}", PlatformInfo.Platform, PlatformInfo.GetID());
			string time = string.Format("{0:0}", GameClock.GetRealTimePlayedOfSaveSlot(currentSlot).TotalMinutes);
			string level = string.Format("{0}", EXPManager.Level);
			RuleIndex ruleIndexOfSaveSlot = GameRulesManager.GetRuleIndexOfSaveSlot(currentSlot);
			int num = 0;
			if (ruleIndexOfSaveSlot <= RuleIndex.Easy)
			{
				if (ruleIndexOfSaveSlot != RuleIndex.Standard)
				{
					if (ruleIndexOfSaveSlot != RuleIndex.Custom)
					{
						if (ruleIndexOfSaveSlot == RuleIndex.Easy)
						{
							num = 2;
						}
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 3;
				}
			}
			else if (ruleIndexOfSaveSlot != RuleIndex.ExtraEasy)
			{
				if (ruleIndexOfSaveSlot != RuleIndex.Hard)
				{
					if (ruleIndexOfSaveSlot == RuleIndex.ExtraHard)
					{
						num = 5;
					}
				}
				else
				{
					num = 4;
				}
			}
			else
			{
				num = 1;
			}
			string difficulty = string.Format("{0}", num);
			return this.format.Format(new
			{
				address,
				id,
				time,
				level,
				difficulty
			});
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0007EFE2 File Offset: 0x0007D1E2
		public void OnPointerClick(PointerEventData eventData)
		{
			Application.OpenURL(this.GenerateQuestionair());
		}

		// Token: 0x040018EC RID: 6380
		private string addressCN = "rsmTLx1";

		// Token: 0x040018ED RID: 6381
		private string addressJP = "mHE3yAa";

		// Token: 0x040018EE RID: 6382
		private string addressEN = "YdoJpod";

		// Token: 0x040018EF RID: 6383
		private string format = "https://usersurvey.biligame.com/vm/{address}.aspx?sojumpparm={id}|{difficulty}|{time}|{level}";
	}
}
