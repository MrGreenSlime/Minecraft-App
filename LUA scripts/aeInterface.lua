local pretty = require "cc.pretty"

-- This function extract the items from both the playerside AE system and the colonyside AE system
local function ExtractItems(peripherals, monitorWriter)
    --Extract Playerside Items
    monitorWriter.WriteLine("Extracting Item List from playerside AE system", peripherals.GetMonitor())
    local items = peripherals.GetPlayerMeBridge().listItems()
    local aeData = {}
    aeData["playerSide"] = items
    --Extract Colonyside Items
    monitorWriter.WriteLine("Extracting Item List from colonyside AE system", peripherals.GetMonitor())
    items = peripherals.GetColonyMeBridge().listItems()
    aeData["colonySide"] = items
    return aeData
end

--This function extracts the crafting patterns present in the playerside AE system
local function ExtractPatterns(peripherals, monitorWriter)
    monitorWriter.WriteLine("Extracting Pattern List from playerside AE system", peripherals.GetMonitor())
    local patterns = peripherals.GetPlayerMeBridge().listCraftableItems()
    return patterns
end

-- This function moves a given item to the colonySide me system
local function MoveItemToColony(peripherals, monitorWriter, command)
    local playerSide = peripherals.GetPlayerMeBridge()
    local item = {}
    item["name"] = command["Item"]
    --pretty.pretty_print(item)
    -- Check if item exists in me system
    local response = playerSide.getItem(item)
    --pretty.pretty_print(response)
    if type(response) == "table" then
        if response.amount ~= nil and response.amount > 0 then
            -- If item exists and has atleast 1 in the me system, export that item to the colonyside interface
            response.count = command["Amount"]
            playerSide.exportItemToPeripheral(response, peripheral.getName(peripherals.GetColonyMeBridgeInventory()))
            monitorWriter.WriteLine("Exported " .. response.name .. " to colony", peripherals.GetMonitor())
        end
    end
end

-- This function crafts an item on the playerside me system
local function CraftItem(peripherals, monitorWriter, command)
    local playerside = peripherals.GetPlayerMeBridge()
    local item = {}
    item["name"] = command["Item"]
    item["count"] = command["Amount"]
    return playerside.craftItem(item)
end

-- This function loops over all commands and makes them happen
local function ProcessCommands(peripherals, monitorWriter, commands)
    local toRemove = {}
    for index, command in ipairs(commands) do
        if command["NeedsCrafting"] then
            CraftItem(peripherals, monitorWriter, command)
        else
            MoveItemToColony(peripherals, monitorWriter, command)
        end
        table.insert(toRemove, index)
    end
    for i, commandToRemove in pairs(toRemove) do
        table.remove(commands, commandToRemove)
    end
    return commands
end

-- This returns the public functions so other scripts can use them
return {ExtractItems = ExtractItems, ExtractPatterns = ExtractPatterns, MoveItemToColony = MoveItemToColony, CraftItem = CraftItem, ProcessCommands = ProcessCommands}