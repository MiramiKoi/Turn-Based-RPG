---@type StatusEffect
local StatusEffect = {}

local ATTACK_MULT = 0.8

---@param ctx StatusEffectContext
function StatusEffect.OnApply(ctx)
    local unit = ctx.unit
    unit.Effects:RemoveAllByDescriptionId("healing")
    unit.Stats.Get("attack_damage"):Multiply(ATTACK_MULT)
end

---@param ctx StatusEffectContext
function StatusEffect.OnRemove(ctx)
    local unit = ctx.unit

    unit.Stats.Get("attack_damage"):Multiply(1 / ATTACK_MULT)
end

return StatusEffect