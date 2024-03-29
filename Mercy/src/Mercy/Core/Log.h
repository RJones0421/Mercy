#pragma once

#include "Core.h"

#include <memory>
#include <spdlog/spdlog.h>
#include <spdlog/fmt/ostr.h>

namespace Mercy
{
  class MERCY_API Log
  {
  public:
    static void Init();

    inline static std::shared_ptr<spdlog::logger>& GetCoreLogger() { return s_CoreLogger; }
    inline static std::shared_ptr<spdlog::logger>& GetClientLogger() { return s_ClientLogger; }

  private:
    static std::shared_ptr<spdlog::logger> s_CoreLogger;
    static std::shared_ptr<spdlog::logger> s_ClientLogger;
  };
}

// Core log macros
#define ME_CORE_TRACE(...)    ::Mercy::Log::GetCoreLogger()->trace(__VA_ARGS__)
#define ME_CORE_INFO(...)     ::Mercy::Log::GetCoreLogger()->info(__VA_ARGS__)
#define ME_CORE_WARN(...)     ::Mercy::Log::GetCoreLogger()->warn(__VA_ARGS__)
#define ME_CORE_ERROR(...)    ::Mercy::Log::GetCoreLogger()->error(__VA_ARGS__)
#define ME_CORE_CRITICAL(...) ::Mercy::Log::GetCoreLogger()->critical(__VA_ARGS__)

// Client log macros
#define ME_TRACE(...)         ::Mercy::Log::GetClientLogger()->trace(__VA_ARGS__)
#define ME_INFO(...)          ::Mercy::Log::GetClientLogger()->info(__VA_ARGS__)
#define ME_WARN(...)          ::Mercy::Log::GetClientLogger()->warn(__VA_ARGS__)
#define ME_ERROR(...)         ::Mercy::Log::GetClientLogger()->error(__VA_ARGS__)
#define ME_CRITICAL(...)      ::Mercy::Log::GetClientLogger()->critical(__VA_ARGS__)
