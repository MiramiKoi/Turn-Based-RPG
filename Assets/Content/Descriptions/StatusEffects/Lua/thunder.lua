---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.OnApply(ctx)
    local unit = ctx.unit
    local effect = ctx.effect
    local stacks = effect.stacks or 1

    local damage = 5 + stacks * 3
    unit:TakeDamage(damage)
    unit:SetActionDisabled(0, true)
end

---@param ctx StatusEffectContext
function StatusEffect.OnRemove(ctx)
    local unit = ctx.unit
    unit:SetActionDisabled(0, false)
end

return StatusEffect