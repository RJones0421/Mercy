#include "mercypch.h"
#include "Mercy/Core/Application.h"

#include <glad/glad.h>

Mercy::Application* Mercy::Application::s_Instance = nullptr;

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

    for ( Layer* layer : m_LayerStack )
    {
      layer->OnUpdate();
    }

    m_Window->OnUpdate();
  }
}

void Mercy::Application::PushLayer( Layer* layer )
{
  m_LayerStack.PushLayer( layer );
}

void Mercy::Application::PushOverlay( Layer* overlay )
{
  m_LayerStack.PushLayer( overlay );
}

void Mercy::Application::OnEvent( Event& event )
{
  EventDispatcher dispatcher( event );
  dispatcher.Dispatch<WindowCloseEvent>( ME_BIND_EVENT_FN( Application::OnWindowClose ) );

  for ( auto it = m_LayerStack.end(); it != m_LayerStack.begin(); )
  {
    ( *--it )->OnEvent( event );
    if ( event.Handled )
    {
      break;
    }
  }
}

bool Mercy::Application::OnWindowClose( WindowCloseEvent& event )
{
  m_Running = false;
  return true;
}
