#include "mercypch.h"
#include "Mercy/Core/Application.h"

#include <GLFW/glfw3.h>

Mercy::Application::Application()
{
  m_Window = std::unique_ptr<Window>( Window::Create() );
  m_Window->SetEventCallback( ME_BIND_EVENT_FN( Application::OnEvent ) );
}

Mercy::Application::~Application()
{
}

void Mercy::Application::Run()
{
  while ( m_Running )
  {
    glClearColor( 1, 1, 0, 1 );
    glClear( GL_COLOR_BUFFER_BIT );
    m_Window->OnUpdate();
  }
}

void Mercy::Application::OnEvent( Event& event )
{
  EventDispatcher dispatcher( event );
  dispatcher.Dispatch<WindowCloseEvent>( ME_BIND_EVENT_FN( Application::OnWindowClose ) );

  ME_CORE_TRACE( "{0}", event );
}

bool Mercy::Application::OnWindowClose( WindowCloseEvent& event )
{
  m_Running = false;
  return true;
}
