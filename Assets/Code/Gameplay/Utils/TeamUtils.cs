using SpaceInvaders.Utils;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Utils {

    [Flags]
    public enum ETeamRelationType {

        None = 0,
        Ally,
        Enemy,
    }

    public static class TeamUtils {

        // very lazy team relations implementation based on game object tags (never do this in real project)
        public static ETeamRelationType GetTeamRelationType(GameObject go1, GameObject go2) {
            // at least one object must belong to a team
            if(!go1.CompareTag(TagsHelper.TAG_TEAM_1) && !go1.CompareTag(TagsHelper.TAG_TEAM_2)) {
                return ETeamRelationType.None;
            }
            return go1.CompareTag(go2.tag) ? ETeamRelationType.Ally : ETeamRelationType.Enemy;
        }
    }
}
