local y = 1
local maxSize = 0

local function Init(monitor)
    monitor.clear()
    y = 1
    monitor.setCursorPos(1,y)
    maxSize = monitor.getSize()[2]
end

local function Reset(monitor)
    monitor.clear()
    y = 1
    monitor.setCursorPos(1,y)
end

local function WriteLine(text, monitor)
    monitor.write(text)
    y = y + 1
    if y > maxSize then
        Reset(monitor)
    end
    monitor.setCursorPos(1,y)
end

local function Clear(monitor)
    monitor.clear()
end



return {Init = Init, WriteLine = WriteLine, Clear = Clear, Reset = Reset}