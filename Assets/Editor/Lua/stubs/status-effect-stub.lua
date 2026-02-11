---@class EffectTable
---@field stacks number
---@field remaining_turns number

---@class StatusEffectContext
---@field unit UnitModel
---@field world World
---@field effect EffectTable

---@class StatusEffect
---@field CanTick fun(ctx:StatusEffectContext):boolean
---@field CanApply fun(ctx:StatusEffectContext):boolean
---@field OnTick fun(ctx:StatusEffectContext)
---@field OnApply fun(ctx:StatusEffectContext)
---@field OnRemove fun(ctx:StatusEffectContext)

---@class ActiveEffects
---@field TryApply fun(self:ActiveEffects, descriptionKey:string)
---@field RemoveById fun(self:ActiveEffects, id:string)
---@field RemoveAllByDescriptionId fun(self:ActiveEffects, descriptionKey:string)
---@field HasStatusEffect fun(self:ActiveEffects, descriptionKey:string):boolean