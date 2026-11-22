using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Tile
{
    [CreateAssetMenu(menuName = "Owaribound/World/Tile/Ground Rule Tile")]
    public sealed class GroundRuleTile : RuleTile
    {
        public override bool RuleMatch(int neighbor, TileBase other)
        {
            return neighbor switch
            {
                TilingRuleOutput.Neighbor.This    => other is GroundRuleTile,
                TilingRuleOutput.Neighbor.NotThis => other is not GroundRuleTile,
                _ => base.RuleMatch(neighbor, other)
            };
        }
    }
}