#include "mercypch.h"
#include "Mercy/Core/Application.h"
#include "Mercy/Core/Log.h"

Mercy::Application::Application()
{
}

Mercy::Application::~Application()
{
}

void Mercy::Application::Run()
{
  while ( m_Running )
  {
    ME_CORE_TRACE( "Running frame" );
  }
}