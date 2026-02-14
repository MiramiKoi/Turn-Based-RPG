---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.OnTick(ctx)
    local unit = ctx.unit
    local stacks = ctx.effect.stacks or 1
    local damage = 4 * stacks
    unit.Combat:TakeDamage(damage)
end

return StatusEffect