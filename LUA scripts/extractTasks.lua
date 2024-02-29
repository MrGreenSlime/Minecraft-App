local y = 1
function WriteLine(text, monitor)
    monitor.write(text)
    y = y + 1
    monitor.setCursorPos(1,y)
end


local colony = peripheral.find("colonyIntegrator")
local monitor = peripheral.find("monitor")
monitor.setTextScale(0.8)
monitor.clear()
monitor.setCursorPos(1,y)
WriteLine("Initializing", monitor)

if colony == nil then 
    WriteLine("colonyIntegrator not found", monitor)
else
    if not colony.isInColony then 
        WriteLine("Block is not in a colony", monitor)
    else
        WriteLine("Extracting requests from " .. colony.getColonyName(), monitor)
        local logfile = "requests.log"
        local file = fs.open(logfile, "w+")
        local requestData = {}
        requestData["Requests"] = colony.getRequests()
        for i=#requestData["Requests"],1,-1 do
            -- print(not (string.find(requestData["Requests"][i]["target"], "Builder") == nil))
            if not (string.find(requestData["Requests"][i]["target"], "Builder") == nil) then
                table.remove(requestData["Requests"], i)
                -- print("removed builder")
            end
        end
        local builderRequests = {}
        local buildings = colony.getBuildings()
        for i, building in ipairs(buildings) do
            --print(building["name"])
            if not (string.find(building["name"], "builder") == nil) then
                local builder = {}
                --textutils.pagedPrint(textutils.serialise(building))
                if not (building["citizens"][1] == nil) then
                    builder["name"] = building["citizens"][1]["name"]
                    --print(textutils.serialise(building["citizens"]))
                end
                builder["location"] = building["location"]
                builder["Requests"] = colony.getBuilderResources(building["location"])
                --print(colony.getBuilderResources(building["location"]))
                table.insert(builderRequests, builder)
            end
        end
        requestData["builderRequests"] = builderRequests
        local colonyData = {}
        colonyData[colony.getColonyName()] = requestData
        file.write(textutils.serialiseJSON(colonyData, {allow_repetitions = true}))
    end
end


