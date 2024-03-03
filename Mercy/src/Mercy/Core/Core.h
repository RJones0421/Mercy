#pragma once

#ifdef ME_PLATFORM_WINDOWS
#if ME_DYNAMIC_LINK
  #ifdef ME_BUILD_DLL
    #define MERCY_API __declspec(dllexport)
  #else
    #define MERCY_API __declspec(dllimport)
  #endif // ME_BUILD_DLL
#else
  #define MERCY_API
#endif // ME_DYNAMIC_LINK
#endif // ME_PLATFORM_WINDOWS