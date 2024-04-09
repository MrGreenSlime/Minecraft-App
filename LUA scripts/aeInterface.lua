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

-- This returns the public functions so other scripts can use them
return {ExtractItems = ExtractItems, ExtractPatterns = ExtractPatterns}