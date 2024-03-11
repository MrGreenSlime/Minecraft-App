local function ExtractTasks(peripherals, monitorWriter)
    monitorWriter.WriteLine("Extracting non builder requests from " .. peripherals.GetColonyIntegrator().getColonyName(), peripherals.GetMonitor())
    local requestData = {}
    requestData["Name"] = peripherals.GetColonyIntegrator().getColonyName()
    requestData["Requests"] = peripherals.GetColonyIntegrator().getRequests()
    for i=#requestData["Requests"],1,-1 do
        if not (string.find(requestData["Requests"][i]["target"], "Builder") == nil) then
            table.remove(requestData["Requests"], i)
        end
    end
    local builderRequests = {}
    local buildings = peripherals.GetColonyIntegrator().getBuildings()
    for i, building in ipairs(buildings) do
        if not (string.find(building["name"], "builder") == nil) then
            local builder = {}
            if not (building["citizens"][1] == nil) then
                builder["name"] = building["citizens"][1]["name"]
                monitorWriter.WriteLine("Extracting requests from builder: " .. builder["name"], peripherals.GetMonitor())
            else
                monitorWriter.WriteLine("Extracting requests from unknown builder.", peripherals.GetMonitor())
            end
            builder["location"] = building["location"]
            builder["Requests"] = peripherals.GetColonyIntegrator().getBuilderResources(building["location"])
            table.insert(builderRequests, builder)
        end
    end
    requestData["BuilderRequests"] = builderRequests
    local colonyData = {}
    --colonyData[peripherals.GetColonyIntegrator().getColonyName()] = requestData
    table.insert(colonyData, requestData)
    return colonyData
end

return {ExtractTasks = ExtractTasks}