---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.OnApply(ctx)
    local unit = ctx.unit
    local effect = ctx.effect
    local stacks = effect.stacks or 1

    local damage = 5 + stacks * 3
    unit.Combat:TakeDamage(damage)
    unit.ActionBlocker:Set(0, true)
end

---@param ctx StatusEffectContext
function StatusEffect.OnRemove(ctx)
    local unit = ctx.unit
    unit.ActionBlocker:Set(0, false)
end

return StatusEffect