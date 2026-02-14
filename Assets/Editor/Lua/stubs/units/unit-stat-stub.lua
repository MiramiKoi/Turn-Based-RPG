---@class StatModelCollection
---@field Stats table<string, StatModel>
---@field Get fun(self:StatModelCollection, id:string):StatModel

---@class StatModel
---@field Description any
---@field Value number
---@field ChangeValue fun(self:StatModel, delta:number)
---@field Set fun(self:StatModel, value:number)
---@field Multiply fun(self:StatModel, factor:number)