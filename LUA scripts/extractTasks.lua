-- This function extracts the tasks from the colony
local function ExtractTasks(peripherals, monitorWriter)
    -- User info
    monitorWriter.WriteLine("Extracting non builder requests from " .. peripherals.GetColonyIntegrator().getColonyName(), peripherals.GetMonitor())

    local requestData = {}
    -- Add the colony name in the data
    requestData["Name"] = peripherals.GetColonyIntegrator().getColonyName()
    -- Add all requests into the data
    requestData["Requests"] = peripherals.GetColonyIntegrator().getRequests()
    -- Remove all requests that are comming from Builders from the requests table
    for i=#requestData["Requests"],1,-1 do
        if not (string.find(requestData["Requests"][i]["target"], "Builder") == nil) then
            table.remove(requestData["Requests"], i)
        end
    end

    -- Get all builder requests
    local builderRequests = {}
    -- Get all buildings
    local buildings = peripherals.GetColonyIntegrator().getBuildings()
    -- For each building check if it is a builder. if so, get the resources from that builder (this is more then the normal requests above)
    for i, building in ipairs(buildings) do
        if not (string.find(building["name"], "builder") == nil) then
            local builder = {}
            -- Add builder name if it has one
            if not (building["citizens"][1] == nil) then
                builder["name"] = building["citizens"][1]["name"]
                monitorWriter.WriteLine("Extracting requests from builder: " .. builder["name"], peripherals.GetMonitor())
            else
                monitorWriter.WriteLine("Extracting requests from unknown builder.", peripherals.GetMonitor())
            end
            -- Add builder location
            builder["location"] = building["location"]
            builder["Requests"] = peripherals.GetColonyIntegrator().getBuilderResources(building["location"])
            table.insert(builderRequests, builder)
        end
    end
    -- Add the builde data into the main data tree
    requestData["BuilderRequests"] = builderRequests
    -- Turn the data into a table
    local colonyData = {}
    table.insert(colonyData, requestData)
    return colonyData
end

-- This returns the public functions so other scripts can use them
return {ExtractTasks = ExtractTasks}