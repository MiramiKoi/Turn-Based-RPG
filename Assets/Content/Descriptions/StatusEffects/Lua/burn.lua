---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.OnTick(ctx)
    local unit = ctx.unit
    local effect = ctx.effect

    local stacks = effect.stacks or 1
    local percent = 0.05
    local per_stack = 2

    local current_health = unit.Health
    local damage = math.floor(current_health * percent + stacks * per_stack)

    unit:TakeDamage(damage)
end

return StatusEffect