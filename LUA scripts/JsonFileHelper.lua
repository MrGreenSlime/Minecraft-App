local function WriteJson(path, data)
    local file = io.open(path, "w+")
    if file == nil then return end
    file.write(file,textutils.serializeJSON(data, {allow_repetitions = true}))
    io.close(file)
end

return {WriteJson = WriteJson}