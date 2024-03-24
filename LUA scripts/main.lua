local jsonHelper = require("JsonFileHelper")
local monitorWriter = require("monitorWriter")

local peripherals = require("wrapPeripherals")
peripherals.Initialize(monitorWriter)
monitorWriter.Init(peripherals.GetMonitor())
local colonyExtractor = require("extractTasks")
local aeInterface = require("aeInterface")

local colonyData = colonyExtractor.ExtractTasks(peripherals, monitorWriter)
jsonHelper.WriteJson("requests.json", colonyData)


local aeItems = aeInterface.ExtractItems(peripherals, monitorWriter)
local aeCraftable = aeInterface.ExtractPatterns(peripherals, monitorWriter)
local aeData = {}
aeData["items"] = aeItems
aeData["patterns"] = aeCraftable
aeData["colony"] = peripherals.GetColonyIntegrator().getColonyName()
local colonyAeData = {}
table.insert(colonyAeData, aeData)
jsonHelper.WriteJson("aeData.json", colonyAeData)