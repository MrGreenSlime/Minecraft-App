local jsonHelper = require("JsonFileHelper")
local monitorWriter = require("monitorWriter")

local peripherals = require("wrapPeripherals")
peripherals.Initialize(monitorWriter)
monitorWriter.Init(peripherals.GetMonitor())
local colonyExtractor = require("extractTasks")
local aeInterface = require("aeInterface")

local colonyData = colonyExtractor.ExtractTasks(peripherals, monitorWriter)
jsonHelper.WriteJson("requests.json", colonyData)


local aeData = aeInterface.ExtractItems(peripherals, monitorWriter)
jsonHelper.WriteJson("aeData.json", aeData)