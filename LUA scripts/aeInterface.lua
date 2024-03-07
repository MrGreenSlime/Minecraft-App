local function ExtractItems(peripherals, monitorWriter)
    monitorWriter.WriteLine("Extracting Item List from playerside AE system", peripherals.GetMonitor())
    local items = peripherals.GetPlayerMeBridge().listItems()
    local aeData = {}
    aeData["playerSide"] = items
    monitorWriter.WriteLine("Extracting Item List from colonyside AE system", peripherals.GetMonitor())
    items = peripherals.GetColonyMeBridge().listItems()
    aeData["colonySide"] = items
    return aeData
end

local function ExtractPatterns(peripherals, monitorWriter)
    monitorWriter.WriteLine("Extracting Pattern List from playerside AE system", peripherals.GetMonitor())
    local patterns = peripherals.GetPlayerMeBridge().listCraftableItems()
    return patterns
end

return {ExtractItems = ExtractItems, ExtractPatterns = ExtractPatterns}