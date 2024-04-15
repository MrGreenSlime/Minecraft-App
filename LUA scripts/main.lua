-- Instanciate both helper classes to help with jsonWriting and monitorWriting
local jsonHelper = require("JsonFileHelper")
local monitorWriter = require("monitorWriter")
local pretty = require "cc.pretty"

-- Instanciate and initialize the peripheral wrapper
local peripherals = require("wrapPeripherals")
peripherals.Initialize(monitorWriter)
monitorWriter.Init(peripherals.GetMonitor())

-- Instanciate colonyExtractor helper and ae helper
local colonyExtractor = require("extractTasks")
local aeInterface = require("aeInterface")

-- Infinite loop
while true do
    -- Extract colonyData and write it to requests.json
    local colonyData = colonyExtractor.ExtractTasks(peripherals, monitorWriter)
    jsonHelper.WriteJson("requests.json", colonyData)

    -- Extract ae data and write it to aeData.json
    local aeItems = aeInterface.ExtractItems(peripherals, monitorWriter)
    local aeCraftable = aeInterface.ExtractPatterns(peripherals, monitorWriter)
    local aeData = {}
    aeData["items"] = aeItems
    aeData["patterns"] = aeCraftable
    aeData["colony"] = peripherals.GetColonyIntegrator().getColonyName()
    local colonyAeData = {}
    table.insert(colonyAeData, aeData)
    jsonHelper.WriteJson("aeData.json", colonyAeData)

    -- Read in the commands and execute them
    local commands = jsonHelper.ReadJson("commands.json")
    aeInterface.ProcessCommands(peripherals, monitorWriter, commands)

    -- Sleep for the given sleeptime
    os.sleep(peripherals.GetSleepTime())
end