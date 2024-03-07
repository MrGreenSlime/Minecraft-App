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

return {ExtractItems = ExtractItems}