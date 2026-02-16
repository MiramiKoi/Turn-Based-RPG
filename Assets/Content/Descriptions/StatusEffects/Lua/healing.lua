---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.CanApply(ctx)
    return not ctx.unit.Effects:HasStatusEffect("curse")
end

---@param ctx StatusEffectContext
function StatusEffect.OnTick(ctx)
    local unit = ctx.unit
    local effect = ctx.effect
    local stacks = effect.stacks or 1
    
    local heal = 5 + stacks * 2
    local max = unit.Stats:Get("health").Description.Value;
    local current = unit.Stats:Get("health").Value;
    
    if heal + current < heal then
        unit.Stats:Get("health"):ChangeValue(heal)
    end
end

return StatusEffect