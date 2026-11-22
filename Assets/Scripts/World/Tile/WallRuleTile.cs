using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Tile
{
    [CreateAssetMenu(menuName = "Owaribound/World/Tile/Wall Rule Tile")]
    public sealed class WallRuleTile : RuleTile
    {
        public override bool RuleMatch(int neighbor, TileBase other)
        {
            return neighbor switch
            {
                TilingRuleOutput.Neighbor.This    => other is WallRuleTile,
                TilingRuleOutput.Neighbor.NotThis => other is not WallRuleTile,
                _ => base.RuleMatch(neighbor, other)
            };
        }
    }
}