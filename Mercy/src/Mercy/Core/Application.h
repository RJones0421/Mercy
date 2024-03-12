#pragma once

#include "Mercy/Core/Core.h"
#include "Mercy/Core/Window.h"

#include "Mercy/Events/Event.h"
#include "Mercy/Events/ApplicationEvent.h"
#include "Mercy/Layers/LayerStack.h"

namespace Mercy
{
  class MERCY_API Application
  {
  public:
    Application();
    virtual ~Application();

    void Run();

    void OnEvent( Event& event );

    void PushLayer( Layer* layer );
    void PushOverlay( Layer* overlay );

  private:
    bool OnWindowClose( WindowCloseEvent& event );

    LayerStack m_LayerStack;
    std::unique_ptr<Window> m_Window;
    bool m_Running = true;
  };

  // Defined in client
  Application* CreateApplication();
}