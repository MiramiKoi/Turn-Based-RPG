---@meta

--------------------------------------------------
-- Effect table (MoonSharp Table)
--------------------------------------------------

---@class EffectTable
---@field stacks number
---@field remaining_turns number

--------------------------------------------------
-- UnitModel (UserData from C#)
--------------------------------------------------

---@class Vector2Int
---@field x number
---@field y number

---@class UnitModel
---@field Id string
---@field Health number
---@field IsDead boolean
---@field Position Vector2Int
---@field Direction number
---@field Flags table<string, boolean>
---@field PointOfInterest table<string, Vector2Int>
---@field ActiveEffects any
---@field Stats any
---@field Description any
---@field OnAttacked fun()
---@field OnDamaging fun()
---@field MoveTo fun(self:UnitModel, position:Vector2Int)
---@field CanMove fun(self:UnitModel):boolean
---@field Rotate fun(self:UnitModel, direction:number)
---@field GetDamage fun(self:UnitModel):number
---@field CanAttack fun(self:UnitModel, position:Vector2Int):boolean
---@field TakeDamage fun(self:UnitModel, damage:number)
---@field SetFlag fun(self:UnitModel, key:string, value:boolean)
---@field SetPointOfInterest fun(self:UnitModel, key:string, value:Vector2Int)
---@field GetPointOfInterest fun(self:UnitModel, key:string):Vector2Int
---@field SetActionDisabled fun(self:UnitModel, action:number, disabled:boolean)
---@field IsActionDisabled fun(self:UnitModel, action:number):boolean
---@field ResetActionDisables fun(self:UnitModel)


--------------------------------------------------
-- World (UserData from C#)
--------------------------------------------------

---@class World
---@field AddressableModel any
---@field MainCamera any
---@field CameraControlModel any
---@field TurnBaseModel any
---@field PlayerControls any
---@field GridModel any
---@field InventoryModel any
---@field GridInteractionModel any
---@field WorldDescription any
---@field GameSystems any
---@field UnitCollection any
---@field AgentCollection any


--------------------------------------------------
-- Status Effect Context (Lua Table)
--------------------------------------------------

---@class StatusEffectContext
---@field unit UnitModel
---@field world World
---@field effect EffectTable

--------------------------------------------------
-- Status Effect Script Contract
--------------------------------------------------

---@class StatusEffect
---@field CanTick fun(ctx:StatusEffectContext):boolean
---@field CanApply fun(ctx:StatusEffectContext):boolean
---@field OnTick fun(ctx:StatusEffectContext)
---@field OnApply fun(ctx:StatusEffectContext)
---@field OnRemove fun(ctx:StatusEffectContext)