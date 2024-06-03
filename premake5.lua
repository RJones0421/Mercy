workspace "Mercy"
	startproject "Sandbox"
	
	configurations
	{
		"Debug",
		"Release",
		"Dist"
	}

outputdir = "%{cfg.buildcfg}-%{cfg.system}-%{cfg.architecture}"

-- Include directories relative to root folder (solution directory)
IncludeDir = {}
IncludeDir["Glad"] = "Mercy/Vendor/Glad/include"
IncludeDir["GLFW"] = "Mercy/Vendor/GLFW/include"
IncludeDir["glm"] = "Mercy/Vendor/glm"
IncludeDir["spdlog"] = "Mercy/Vendor/spdlog/include"

include "Mercy/Vendor/Glad"
include "Mercy/Vendor/GLFW"

project "Mercy"
	location "Mercy"
	kind "SharedLib"
	language "C++"
	cppdialect "C++17"
	architecture "x86_64"
	staticruntime "off"
	
	targetdir ("bin/" .. outputdir .. "/%{prj.name}")
	objdir ("bin-int/" .. outputdir .. "/%{prj.name}")
	
	pchheader "mercypch.h"
	pchsource "Mercy/src/mercypch.cpp"
	
	files
	{
		"%{prj.name}/src/**.h",
		"%{prj.name}/src/**.cpp",
		"%{prj.name}/Vendor/glm/**.hpp",
		"%{prj.name}/Vendor/glm/**.inl"
	}
	
	defines
	{
		"_CRT_SECURE_NO_WARNINGS"
	}
	
	includedirs
	{
		"%{prj.name}/src",
		"%{IncludeDir.Glad}",
		"%{IncludeDir.GLFW}",
		"%{IncludeDir.glm}",
		"%{IncludeDir.spdlog}"
	}
	
	links
	{
		"Glad",
		"GLFW",
		"opengl32.lib"
	}
	
	filter "system:windows"
		systemversion "latest"
		
		defines
		{
			"ME_PLATFORM_WINDOWS",
			"ME_DYNAMIC_LINK",
			"ME_BUILD_DLL",
			"GLFW_INCLUDE_NONE"
		}
		
		postbuildcommands
		{
			("{COPY} %{cfg.buildtarget.relpath} \"../bin/" .. outputdir .. "/MercyEditor/\""),
			("{COPY} %{cfg.buildtarget.relpath} \"../bin/" .. outputdir .. "/Sandbox/\"")
		}
		
	filter "configurations:Debug"
		defines "ME_DEBUG"
		runtime "Debug"
		symbols "on"
		
	filter "configurations:Release"
		defines "ME_RELEASE"
		runtime "Release"
		optimize "on"
		
	filter "configurations:Dist"
		defines "ME_DIST"
		runtime "Release"
		optimize "on"
		
project "Sandbox"
	location "Sandbox"
	kind "ConsoleApp"
	language "C++"
	cppdialect "C++17"
	architecture "x86_64"
	staticruntime "off"
	
	targetdir ("bin/" .. outputdir .. "/%{prj.name}")
	objdir ("bin-int/" .. outputdir .. "/%{prj.name}")
	
	files
	{
		"%{prj.name}/src/**.h",
		"%{prj.name}/src/**.cpp"
	}
	
	includedirs
	{
		"Mercy/src",
		"%{IncludeDir.glm}",
		"%{IncludeDir.spdlog}"
	}
	
	links
	{
		"Mercy"
	}
	
	filter "system:windows"
		systemversion "latest"
		
		defines
		{
			"ME_PLATFORM_WINDOWS",
			"ME_DYNAMIC_LINK"
		}
		
	filter "configurations:Debug"
		defines "ME_DEBUG"
		runtime "Debug"
		symbols "on"
		
	filter "configurations:Release"
		defines "ME_RELEASE"
		runtime "Release"
		optimize "on"
		
	filter "configurations:Dist"
		defines "ME_DIST"
		runtime "Release"
		optimize "on"

project "MercyEditor"
	location "MercyEditor"
	kind "WindowedApp"
	language "C#"

	dotnetframework "net8.0-windows"

	targetdir ("bin/" .. outputdir .. "x86_64/%{prj.name}")
	objdir ("bin-int/" .. outputdir .. "x86_64/%{prj.name}")

	flags
	{
		"WPF"
	}

	files
	{
		"%{prj.name}/*.cs",
	}

	links
	{
		"Mercy",
		"Sandbox"
	}
