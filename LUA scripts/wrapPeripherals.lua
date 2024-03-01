local Monitor = nil
local ColonyIntegrator = nil
local PlayerSideMe = nil
local ColonySideMe = nil

local MonitorWriter = nil

local peripheralTable = 
{
    ["colonyIntegrator"] = function (peripheral, side)
        ColonyIntegrator = peripheral
    end,
    ["monitor"] = function (peripheral, side)
        Monitor = peripheral
    end,
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

local function Initialize(monitorWriter)
    MonitorWriter = monitorWriter
    local topPeripheral = peripheral.wrap("top")
    local bottomPeripheral = peripheral.wrap("bottom")
    local leftPeripheral = peripheral.wrap("left")
    local rightPeripheral = peripheral.wrap("right")
    local frontPeripheral = peripheral.wrap("front")
    local backPeripheral = peripheral.wrap("back")

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

    if not ColonyIntegrator.isInColony() then 
        MonitorWriter.WriteLine("Block is not in a colony", Monitor)
        os.exit()
    end
end

local function TestFunction()
    Initialize()
    print(Monitor)
    print(ColonyIntegrator)
    print(PlayerSideMe)
    print(ColonySideMe)
end

local function GetMonitor()
    return Monitor
end

local function GetColonyIntegrator()
    return ColonyIntegrator
end

local function GetPlayerMeBridge()
    return PlayerSideMe
end

local function GetColonyMeBridge()
    return ColonySideMe
end

return {Initialize = Initialize,
GetMonitor = GetMonitor, 
GetColonyIntegrator = GetColonyIntegrator,
GetPlayerMeBridge = GetPlayerMeBridge,
GetColonyMeBridge = GetColonyMeBridge,
TestFunction = TestFunction}

--TestFunction()