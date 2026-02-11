---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.OnTick(ctx)
    local unit = ctx.unit
    local effect = ctx.effect
    local stacks = effect.stacks or 1
    
    local damage = 3 + stacks * 2
    unit:TakeDamage(damage)

    unit:TakeDamage(damage)
end

return StatusEffect