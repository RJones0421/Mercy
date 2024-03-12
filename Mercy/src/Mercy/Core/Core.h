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

#ifdef ME_ENABLE_ASSERTS
  #define ME_ASSERT( x, ... ) { if ( !(x) ) { ME_ERROR( "Assertion Failed: {0}", __VA_ARGS__ ); __debugbreak(); } }
  #define ME_CORE_ASSERT( x, ... ) { if ( !(x) ) { ME_CORE_ERROR( "Assertion Failed: {0}", __VA_ARGS__ ); __debugbreak(); } }
#else
  #define ME_ASSERT( x, ... )
  #define ME_CORE_ASSERT( x, ... )
#endif // HZ_ENABLE_ASSERTS

#define BIT(x) ( 1 << x )

#define ME_BIND_EVENT_FN(fn) std::bind(&fn, this, std::placeholders::_1)

#pragma warning( disable: 4251 ) // Disables STL dll export warnings
#pragma warning( disable: 4996 ) // Disables STL dll export warnings
