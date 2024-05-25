-- Keep track of current y level on monitor and the maximum y height of the monitor
local y = 1
local maxSize = 0

-- Initialize the monitor by clearing it, setting the text scale to 0.5 and setting the cursor on the first line after which the max y height is extracted
local function Init(monitor)
    monitor.clear()
    monitor.setTextScale(0.5)
    y = 1
    monitor.setCursorPos(1,y)
    local x,y1 = monitor.getSize()
    maxSize = y1
    print(maxSize)
end

-- This resets the monitor by clearing it and resetting the cursor to the first line
local function Reset(monitor)
    monitor.clear()
    y = 1
    monitor.setCursorPos(1,y)
end

-- This function writes a line and sets the cursor on the next line. If the next line is off the screen it resets the monitor
local function WriteLine(text, monitor)
    monitor.write(text)
    y = y + 1
    if y > maxSize then
        Reset(monitor)
    end
    monitor.setCursorPos(1,y)
end

-- This simply clears the monitor
local function Clear(monitor)
    monitor.clear()
end

-- This returns the public functions so other scripts can use them
return {Init = Init, WriteLine = WriteLine, Clear = Clear, Reset = Reset}