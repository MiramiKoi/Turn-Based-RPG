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
    unit.Stats:Get("health"):ChangeValue(heal)
end

return StatusEffect