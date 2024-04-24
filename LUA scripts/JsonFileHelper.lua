-- This function opens the file given in the path parameter and then writes the data serialized as JSON to said file
local function WriteJson(path, data)
    local file = io.open(path, "w+")
    if file == nil then return end
    file.write(file,textutils.serializeJSON(data, {allow_repetitions = true}))
    io.close(file)
end

local function ReadJson(path)
    local file = io.open(path, "r")
    if file == nil then return end
    local data = file.read(file,"a")
    io.close(file)
    return textutils.unserialiseJSON(data, {allow_repetitions = true})
end

local function file_exists(name)
    local f=io.open(name, "r")
    if f~=nil then io.close(f) return true else return false end
end

-- This returns the public functions so other scripts can use them
return {WriteJson = WriteJson, ReadJson = ReadJson, file_exists = file_exists}