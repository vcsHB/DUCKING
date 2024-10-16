using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/RelativeRuleTile")]
public class RelativeRuleTile : RuleTile
{
    public enum SiblingGroup
    {
        Poles,
        Terrain,
    }
    public SiblingGroup siblingGroup;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return other is RelativeRuleTile
                        && (other as RelativeRuleTile).siblingGroup == this.siblingGroup;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !(other is RelativeRuleTile
                       && (other as RelativeRuleTile).siblingGroup == this.siblingGroup);
                }
        }

        return base.RuleMatch(neighbor, other);
    }
}
