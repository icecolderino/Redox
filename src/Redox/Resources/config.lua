--[==[
Created by ice cold at 29-02-2020
This api allows lua plugins to create config files in json
<!--]==]

--Global table
Config = {}
--The instance of the Configuration class in redox
local config
local created = false;

function Config.Exists(name)
	local exists = Plugin:HasConfig(name)
	return exists
end

function Config.Create(name)
	if(not created) then
		config = Plugin:CreateConfig(name)
		created = true
	else
		error("[Redox] " .. Plugin.Title .. " tried to create a second configuration file")
	end
end

function Config.Save(name)
	Plugin:SaveConfig(name)
end

function Config.Load(name)
	Plugin:LoadConfig(name)
end

function Config.AddSetting(key, value)
	if(type(value) == "table") then			
	    local tbl = Plugin:CreateDictionary()
	    local arr = Plugin:CreateArray()
	    local isarray = false
	    local count = 1
		for k,v in ipairs(value) do      	
		    if(tonumber(k)) then
		    	isarray = true
		    	arr[count - 1] = v
		    else
		    	tbl:Add(tostring(key), v)
		    end
	    end
	    if(isarray) then 
	    	config:AddSetting(key, arr)
	    else
	    	config:AddSetting(key, tbl)
	    end
	else
       config:AddSetting(key, value)
    end
end

function Config.GetSetting(key)
	if(Config.HasSetting(key)) then
		local value = config:GetSetting(key)
		local tbl = Plugin.Util:TableFromDictionary(value)

		if(tbl ~= nil) then return tbl end
		return value
	end
end