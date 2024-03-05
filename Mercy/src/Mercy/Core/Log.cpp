#include "mercypch.h"
#include "Log.h"
#include <spdlog/sinks/stdout_color_sinks.h>

std::shared_ptr<spdlog::logger> Mercy::Log::s_CoreLogger;
std::shared_ptr<spdlog::logger> Mercy::Log::s_ClientLogger;

void Mercy::Log::Init()
{
  spdlog::set_pattern( "%^[%T] %n: %v%$" );

  // Create loggers
  // Core/Engine
  s_CoreLogger = spdlog::stdout_color_mt( "MERCY" );
  s_CoreLogger->set_level( spdlog::level::trace );
  // Client
  s_ClientLogger = spdlog::stdout_color_mt( "APP" );
  s_ClientLogger->set_level( spdlog::level::trace );
}