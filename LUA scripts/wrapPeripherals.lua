-- Keep the different peripherals loaded
local Monitor = nil
local ColonyIntegrator = nil
local PlayerSideMe = nil
local ColonySideMe = nil

local MonitorWriter = nil

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
        print("Is the MEBridge on the " .. side .. " side connected to the player system or the colony system? (player/colony)")
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
    end
}

-- Initialize everything to detect the different peripherals
local function Initialize(monitorWriter)
    MonitorWriter = monitorWriter

    -- wrap all peripherals on each side
    local topPeripheral = peripheral.wrap("top")
    local bottomPeripheral = peripheral.wrap("bottom")
    local leftPeripheral = peripheral.wrap("left")
    local rightPeripheral = peripheral.wrap("right")
    local frontPeripheral = peripheral.wrap("front")
    local backPeripheral = peripheral.wrap("back")

    -- Check each peripheral against the peripheralTable
    local func = nil
    if not (topPeripheral == nil) then func = peripheralTable[peripheral.getType(topPeripheral)] end
    if not (func == nil) then func(topPeripheral, "top") end
    func = nil
    if not (bottomPeripheral == nil) then func = peripheralTable[peripheral.getType(bottomPeripheral)] end
    if not (func == nil) then func(bottomPeripheral, "bottom") end
    func = nil
    if not (leftPeripheral == nil) then func = peripheralTable[peripheral.getType(leftPeripheral)] end
    if not (func == nil) then func(leftPeripheral, "left") end
    func = nil
    if not (rightPeripheral == nil) then func = peripheralTable[peripheral.getType(rightPeripheral)] end
    if not (func == nil) then func(rightPeripheral, "right") end
    func = nil
    if not (frontPeripheral == nil) then func = peripheralTable[peripheral.getType(frontPeripheral)] end
    if not (func == nil) then func(frontPeripheral, "front") end
    func = nil
    if not (backPeripheral == nil) then func = peripheralTable[peripheral.getType(backPeripheral)] end
    if not (func == nil) then func(backPeripheral, "back") end
    func = nil

    -- Check if the colonyIntegrator is in a colony, if not exit the program
    if not ColonyIntegrator.isInColony() then 
        MonitorWriter.WriteLine("Block is not in a colony", Monitor)
        os.exit()
    end
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

-- This returns the public functions so other scripts can use them
return {Initialize = Initialize,
GetMonitor = GetMonitor, 
GetColonyIntegrator = GetColonyIntegrator,
GetPlayerMeBridge = GetPlayerMeBridge,
GetColonyMeBridge = GetColonyMeBridge,
TestFunction = TestFunction}

--TestFunction()