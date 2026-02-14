---@class UnitModel
---@field Id string
---@field Health number
---@field IsDead boolean
---@field Description any
---@field State UnitStateModel
---@field Combat UnitCombatModel
---@field Movement UnitMovementModel
---@field ActionBlocker ActionBlockerModel
---@field Stats StatModelCollection
---@field Inventory InventoryModel
---@field Equipment EquipmentModel
---@field Effects StatusEffectApplierModel
---@field Flags table<string, boolean>
---@field SetFlag fun(self:UnitModel, key:string, value:boolean)

---@class UnitStateModel
---@field Position ReactivePropertyVector2Int
---@field Direction ReactivePropertyUnitDirection
---@field Visible ReactivePropertyBoolean

---@class UnitCombatModel
---@field GetDamage fun(self:UnitCombatModel):number
---@field CanAttack fun(self:UnitCombatModel, target:Vector2Int):boolean
---@field TakeDamage fun(self:UnitCombatModel, damage:number)

---@class UnitMovementModel
---@field MoveTo fun(self:UnitMovementModel, position:Vector2Int)
---@field SetPosition fun(self:UnitMovementModel, position:Vector2Int)
---@field Rotate fun(self:UnitMovementModel, direction:UnitDirection)

---@class ActionBlockerModel
---@field Set fun(self:ActionBlockerModel, type:UnitActionType, value:boolean)
---@field CanExecute fun(self:ActionBlockerModel, type:UnitActionType):boolean

---@alias UnitDirection number | "Right" | "Left"
---@alias UnitActionType number | "All" | "Move" | "Attack"
