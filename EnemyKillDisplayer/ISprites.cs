using System;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using UnityEngine;

namespace EnemyKillDisplayer
{
    [Serializable]
    public class InventoryObjectSprite : ISprite
    {
        public string objectPath;
        public InventoryObjectSprite(string objectPath)
        {
            this.objectPath = objectPath;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Sprite Value
        {
            get
            {
                try
                {
                    return GameCameras.instance.hudCamera.transform.Find(objectPath).GetComponent<SpriteRenderer>().sprite;
                }
                catch
                {
                    return Modding.CanvasUtil.NullSprite();
                }
            }
        }
        public ISprite Clone() => (ISprite)MemberwiseClone();
    }

    [Serializable]
    public class InventoryFsmSprite : ISprite
    {
        public string objectPath;
        public string fsmName;
        public string fsmState;

        public Sprite Value
        {
            get
            {
                try
                {
                    return GameCameras.instance.hudCamera.transform
                        .Find(objectPath)
                        .gameObject
                        .LocateMyFSM(fsmName)
                        .GetState(fsmState)
                        .GetFirstActionOfType<SetSpriteRendererSprite>()
                        .sprite.Value as Sprite;
                }
                catch
                {
                    return Modding.CanvasUtil.NullSprite();
                }
            }
        }

        public ISprite Clone() => (ISprite)MemberwiseClone();
    }

    [Serializable]
    public class CharmSprite : ISprite
    {
        public int charmNum;
        public GrimmchildLevel grimmchildLevel = GrimmchildLevel.Playerdata;
        public FragileState fragileState = FragileState.Playerdata;
        public KingsoulState kingsoulState = KingsoulState.Playerdata;

        private static PlayMakerFSM charmFsm => GameCameras.instance.hudCamera.transform.Find("Inventory/Charms/Collected Charms/36")
            .gameObject.LocateMyFSM("charm_show_if_collected");

        [Newtonsoft.Json.JsonIgnore]
        public Sprite Value
        {
            get
            {
                switch (charmNum)
                {
                    case 40:
                        switch (grimmchildLevel)
                        {
                            case GrimmchildLevel.Level1: return CharmIconList.Instance.grimmchildLevel1;
                            case GrimmchildLevel.Level2: return CharmIconList.Instance.grimmchildLevel2;
                            case GrimmchildLevel.Level3: return CharmIconList.Instance.grimmchildLevel3;
                            case GrimmchildLevel.Level4: return CharmIconList.Instance.grimmchildLevel4;
                            case GrimmchildLevel.Nymm: return CharmIconList.Instance.nymmCharm;
                        }
                        goto default;
                    case 36:
                        switch (kingsoulState)
                        {
                            case KingsoulState.Queen:
                            case KingsoulState.Playerdata when PlayerData.instance.GetInt(nameof(PlayerData.royalCharmState)) == 1:
                                return charmFsm.GetState("R Queen").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                            case KingsoulState.King:
                            case KingsoulState.Playerdata when PlayerData.instance.GetInt(nameof(PlayerData.royalCharmState)) == 2:
                                return charmFsm.GetState("R King").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                            case KingsoulState.Final:
                            case KingsoulState.Playerdata when PlayerData.instance.GetInt(nameof(PlayerData.royalCharmState)) == 3:
                                return charmFsm.GetState("R Final").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                            case KingsoulState.Voidheart:
                            case KingsoulState.Playerdata when PlayerData.instance.GetInt(nameof(PlayerData.royalCharmState)) == 4:
                                return charmFsm.GetState("R Shade").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                        }
                        goto default;
                    case 23:
                        switch (fragileState)
                        {
                            case FragileState.Fragile:
                            case FragileState.Playerdata when !PlayerData.instance.GetBool(nameof(PlayerData.brokenCharm_23)) && !PlayerData.instance.GetBool(nameof(PlayerData.fragileHealth_unbreakable)):
                                return CharmIconList.Instance.GetSprite(charmNum);
                            case FragileState.Unbreakable:
                            case FragileState.Playerdata when PlayerData.instance.GetBool(nameof(PlayerData.fragileHealth_unbreakable)):
                                return CharmIconList.Instance.unbreakableHeart;
                            case FragileState.Broken:
                            case FragileState.Playerdata when PlayerData.instance.GetBool(nameof(PlayerData.brokenCharm_23)):
                                return charmFsm.GetState("Glass HP").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                        }
                        goto default;
                    case 24:
                        switch (fragileState)
                        {
                            case FragileState.Fragile:
                            case FragileState.Playerdata when !PlayerData.instance.GetBool(nameof(PlayerData.brokenCharm_24)) && !PlayerData.instance.GetBool(nameof(PlayerData.fragileGreed_unbreakable)):
                                return CharmIconList.Instance.GetSprite(charmNum);
                            case FragileState.Unbreakable:
                            case FragileState.Playerdata when PlayerData.instance.GetBool(nameof(PlayerData.fragileGreed_unbreakable)):
                                return CharmIconList.Instance.unbreakableGreed;
                            case FragileState.Broken:
                            case FragileState.Playerdata when PlayerData.instance.GetBool(nameof(PlayerData.brokenCharm_24)):
                                return charmFsm.GetState("Glass Geo").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                        }
                        goto default;
                    case 25:
                        switch (fragileState)
                        {
                            case FragileState.Fragile:
                            case FragileState.Playerdata when !PlayerData.instance.GetBool(nameof(PlayerData.brokenCharm_25)) && !PlayerData.instance.GetBool(nameof(PlayerData.fragileStrength_unbreakable)):
                                return CharmIconList.Instance.GetSprite(charmNum);
                            case FragileState.Unbreakable:
                            case FragileState.Playerdata when PlayerData.instance.GetBool(nameof(PlayerData.fragileStrength_unbreakable)):
                                return CharmIconList.Instance.unbreakableGreed;
                            case FragileState.Broken:
                            case FragileState.Playerdata when PlayerData.instance.GetBool(nameof(PlayerData.brokenCharm_25)):
                                return charmFsm.GetState("Glass Attack").GetFirstActionOfType<SetSpriteRendererSprite>().sprite.Value as Sprite;
                        }
                        goto default;
                    default:
                        return CharmIconList.Instance.GetSprite(charmNum);
                }
            }
        }
        public ISprite Clone() => (ISprite)MemberwiseClone();
    }

    public enum GrimmchildLevel
    {
        Playerdata,
        Level1,
        Level2,
        Level3,
        Level4,
        Nymm
    }
    public enum FragileState
    {
        Playerdata,
        Fragile,
        Broken,
        Unbreakable
    }
    public enum KingsoulState
    {
        Playerdata,
        Queen,
        King,
        Final,
        Voidheart
    }
}
