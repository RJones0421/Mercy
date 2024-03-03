#pragma once

#include "Core.h"

namespace Mercy
{
  class MERCY_API Application
  {
  public:
    Application();
    virtual ~Application();

    void Run();

  private:
    bool m_Running = true;
  };

  // Defined in client
  Application* CreateApplication();
}