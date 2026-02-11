---@type StatusEffect
local StatusEffect = {}

---@param ctx StatusEffectContext
function StatusEffect.OnApply(ctx)
    local unit = ctx.unit
    unit:SetActionDisabled(0, true)
    unit.Stats:Get("visibility_radius"):Set(0)
end

---@param ctx StatusEffectContext
function StatusEffect.OnRemove(ctx)
    local unit = ctx.unit
    unit:SetActionDisabled(0, false)
    local visibility = unit.Stats:Get("visibility_radius")
    visibility:Set(visibility.Description.MaxValue)
end

return StatusEffect