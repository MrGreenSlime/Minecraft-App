-- Keep the different peripherals loaded
local Monitor = nil
local ColonyIntegrator = nil
local PlayerSideMe = nil
local PlayerSideMeInventory = nil
local ColonySideMeInventory = nil
local ColonySideMe = nil

local MonitorWriter = nil
local SleepTime = 5

local JsonFileHelper = require("JsonFileHelper")
local pretty = require "cc.pretty"

-- Decision table with functions for each different kind of peripheral
local peripheralTable = 
{
    -- This just sets the colonyIntegrator as the peripheral as there is only 1 colonyintegrator
    ["colonyIntegrator"] = function (peripheral, side)
        ColonyIntegrator = peripheral
    end,
    -- idem as above but with the monitor
    ["monitor"] = function (peripheral, side)
        Monitor = peripheral
    end,
    -- Asks which me bridge on which side is either colony side or player side
    ["meBridge"] = function (peripheral, side)
        print("Is the MEBridge with the name " .. side .. " connected to the player system or the colony system? (player/colony)")
        local answer = nil
        answer = io.read()
        while not (answer == "player" or answer == "colony") do
            print("Please answer with 'player' or 'colony' (player/colony)")
            answer = io.read()
        end
        if answer == "player" then
            PlayerSideMe = peripheral
        elseif answer == "colony" then
            ColonySideMe = peripheral
        end
    end,
    ["ae2:interface"] = function (peripheral, name)
        print("Is the interface with the name ".. name .. " connected to the player system or the colony system? (player/colony)")
        local answer = nil
        answer = io.read()
        while not (answer == "player" or answer == "colony") do
            print("Please answer with 'player' or 'colony' (player/colony)")
            answer = io.read()
        end
        if answer == "player" then
            PlayerSideMeInventory = peripheral
        elseif answer == "colony" then
            ColonySideMeInventory = peripheral
        end
    end
}

-- Splits a string based on a given seperator sep
local function split (inputstr, sep)
    if sep == nil then
            sep = "%s"
    end
    local t={}
    for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
            table.insert(t, str)
    end
    return t
end

-- Function to map the different peripherals
local function MapPeripherals()
    -- Get all names of connected peripherals and loop over them
    local peripherals = peripheral.getNames()
        for i, peripheralName in ipairs(peripherals) do
            local func = nil
            -- Wrap the peripheral
            local wrappedPeripheral = peripheral.wrap(peripheralName)
            if not (wrappedPeripheral == nil) then 
                -- Get the type of the peripheral
                local type = peripheral.getType(wrappedPeripheral)
                local splitTypes = split(type, "%s")
                -- pretty.pretty_print(splitTypes)

                -- If the split types has more then 1 value take the second value (THIS IS HARDCODED)
                local i = 1
                if table.getn(splitTypes) > 1 then
                    i = 2
                end

                -- Get the function for the given type from the functiontable
                func = peripheralTable[splitTypes[i]]
                --print(peripheral.getType(wrappedPeripheral))
            end
            -- If the peripheral type has a funtion, run that function to put the wrapped peripheral in the correct variable
            if not (func == nil) then func(wrappedPeripheral, peripheralName) end
        end

        -- Check if the colonyIntegrator is in a colony, if not exit the program
        if not ColonyIntegrator.isInColony() then 
            MonitorWriter.WriteLine("Block is not in a colony", Monitor)
            os.exit()
        end

        -- Ask the user for the sleeptime
        print("Please insert the checkInterval in seconds (default 5 seconds)")
        local answer = io.read()
        local num = tonumber(answer)
        if num ~= nil then SleepTime = num end
end

-- Function to make the object to write to the savedState file so the user doesn't have to keep inputting which peripheral is which
local function WriteConfigFile()
    local peripherals = {}
    if Monitor ~= nil then peripherals["Monitor"] = peripheral.getName(Monitor) end
    if ColonyIntegrator ~= nil then peripherals["ColonyIntegrator"] = peripheral.getName(ColonyIntegrator) end
    if PlayerSideMe ~= nil then peripherals["PlayerSideMe"] = peripheral.getName(PlayerSideMe) end
    if PlayerSideMeInventory ~= nil then peripherals["PlayerSideMeInventory"] = peripheral.getName(PlayerSideMeInventory) end
    if ColonySideMeInventory ~= nil then peripherals["ColonySideMeInventory"] = peripheral.getName(ColonySideMeInventory) end
    if ColonySideMe ~= nil then peripherals["ColonySideMe"] = peripheral.getName(ColonySideMe) end
    if SleepTime ~= nil then peripherals["SleepTime"] = SleepTime end

    JsonFileHelper.WriteJson("savedState.json", peripherals)
end

-- Initialize everything to detect the different peripherals
local function Initialize(monitorWriter)
    -- Cache MonitorWriter
    MonitorWriter = monitorWriter

    -- Check if a savedState exists
    if JsonFileHelper.file_exists("savedState.json") then

        -- Ask the user if they want to use the savedState
        print("previous config file found. Use this? (yes/no)")
        local answer = io.read()
        while not (answer == "yes" or answer == "no") do
            print("Please answer with 'yes' or 'no' (yes/no)")
            answer = io.read()
        end

        -- If the answer is yes then map the peripherals according to the savedState
        if answer == "yes" then
            local peripherals = JsonFileHelper.ReadJson("savedState.json")
            if peripherals.Monitor ~= nil then Monitor = peripheral.wrap(peripherals.Monitor) end
            if peripherals.ColonyIntegrator ~= nil then ColonyIntegrator = peripheral.wrap(peripherals.ColonyIntegrator) end
            if peripherals.PlayerSideMe ~= nil then PlayerSideMe = peripheral.wrap(peripherals.PlayerSideMe) end
            if peripherals.PlayerSideMeInventory ~= nil then PlayerSideMeInventory = peripheral.wrap(peripherals.PlayerSideMeInventory) end
            if peripherals.ColonySideMeInventory ~= nil then ColonySideMeInventory = peripheral.wrap(peripherals.ColonySideMeInventory) end
            if peripherals.ColonySideMe ~= nil then ColonySideMe = peripheral.wrap(peripherals.ColonySideMe) end
            if peripherals.SleepTime ~= nil then SleepTime = peripherals.SleepTime end
        elseif answer == "no" then
            -- If the answer is no then map the peripherals like new
            MapPeripherals()
        end
    else
        -- If no savedState exists, map the peripherals like new
        MapPeripherals()
    end
    -- Write the config file away
    WriteConfigFile()
end

-- This is a simple test function to test if it finds all peripherals
local function TestFunction()
    Initialize()
    print(Monitor)
    print(ColonyIntegrator)
    print(PlayerSideMe)
    print(ColonySideMe)
end

-- Returns the monitor as object
local function GetMonitor()
    return Monitor
end

-- Returns the ColonyIntegrator as object
local function GetColonyIntegrator()
    return ColonyIntegrator
end

-- Returns the playerSide MEBridge as object
local function GetPlayerMeBridge()
    return PlayerSideMe
end

-- Returns the colonySide MEBridge as object
local function GetColonyMeBridge()
    return ColonySideMe
end

-- Returns the playerside interface connected to the playerside Me system as object
local function GetPlayerMeBridgeInventory()
    return PlayerSideMeInventory
end

-- Returns the colonyside interface connected to the colonyside Me system as object
local function GetColonyMeBridgeInventory()
    return ColonySideMeInventory
end

-- Returns the SleepTime configured
local function GetSleepTime()
    return SleepTime
end

-- This returns the public functions so other scripts can use them
return {Initialize = Initialize,
GetMonitor = GetMonitor, 
GetColonyIntegrator = GetColonyIntegrator,
GetPlayerMeBridge = GetPlayerMeBridge,
GetColonyMeBridge = GetColonyMeBridge,
TestFunction = TestFunction,
GetPlayerMeBridgeInventory = GetPlayerMeBridgeInventory,
GetColonyMeBridgeInventory = GetColonyMeBridgeInventory,
GetSleepTime = GetSleepTime}

--TestFunction()