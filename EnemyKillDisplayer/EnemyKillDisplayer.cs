using System;
using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;
using ItemChanger;

namespace EnemyKillDisplayer
{
    public class EnemyKillDisplayer : Mod, ITogglableMod
    {
        internal static EnemyKillDisplayer instance;
        
        public EnemyKillDisplayer() : base(null)
        {
            instance = this;
        }
        
        
        public override void Initialize()
        {
            Log("Initializing Mod...");

            ModHooks.RecordKillForJournalHook += ModHooks_RecordKillForJournalHook;
        }
        public void Unload()
        {
            ModHooks.RecordKillForJournalHook -= ModHooks_RecordKillForJournalHook;
        }

        private void ModHooks_RecordKillForJournalHook(EnemyDeathEffects enemyDeathEffects, string playerDataName, string killedBoolPlayerDataLookupKey, string killCountIntPlayerDataLookupKey, string newDataBoolPlayerDataLookupKey)
        {
            JournalEntryStats jes = GameCameras.instance.hudCamera.GetComponentsInChildren<JournalEntryStats>(true)
                .Where(j => j.playerDataName == playerDataName)
                .FirstOrDefault();

            string message = $"{Language.Language.Get(jes.nameConvo, "Journal")}\nin {RecentItemsDisplay.AreaName.CleanAreaName(GameManager.instance.sceneName)}";
            ISprite sprite = new InventoryObjectSprite($"Inventory/Journal/Enemy List/{jes.name}/Portrait");

            RecentItemsDisplay.ItemDisplayMethods.ShowItem(message, sprite);
        }
    }
}