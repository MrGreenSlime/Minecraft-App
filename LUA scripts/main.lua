local monitorWriter = require("monitorWriter")

local peripherals = require("wrapPeripherals")
peripherals.Initialize(monitorWriter)
monitorWriter.Init(peripherals.GetMonitor())

local colonyExtractor = require("extractTasks")
local colonyData = colonyExtractor.ExtractTasks(peripherals, monitorWriter)
