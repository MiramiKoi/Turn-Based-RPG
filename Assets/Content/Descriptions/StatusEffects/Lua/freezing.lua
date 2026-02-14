---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.CanApply(ctx)
    local unit = ctx.unit
    return not unit.Effects:HasStatusEffect("burn")
end

---@param ctx StatusEffectContext
function StatusEffect.CanTick(ctx)
    return StatusEffect.CanApply(ctx)
end

---@param ctx StatusEffectContext
function StatusEffect.OnApply(ctx)
    local unit = ctx.unit
    unit.ActionBlocker:Set(1, true)
end

---@param ctx StatusEffectContext
function StatusEffect.OnRemove(ctx)
    local unit = ctx.unit
    unit.ActionBlocker:Set(1, false)
end

---@param ctx StatusEffectContext
function StatusEffect.OnTick(ctx)
    local unit = ctx.unit
    local effect = ctx.effect
    local stacks = effect.stacks or 1

    local damage = 1 + stacks
    unit.Combat:TakeDamage(damage)
end

return StatusEffect