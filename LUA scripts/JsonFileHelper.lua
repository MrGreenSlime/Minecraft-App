-- This function opens the file given in the path parameter and then writes the data serialized as JSON to said file
local function WriteJson(path, data)
    local file = io.open(path, "w+")
    if file == nil then return end
    file.write(file,textutils.serializeJSON(data, {allow_repetitions = true}))
    io.close(file)
end

-- This returns the public functions so other scripts can use them
return {WriteJson = WriteJson}