#include "Mercy.h"

class Sandbox : public Mercy::Application
{
public:
  Sandbox()
  {

  }

  ~Sandbox()
  {

  }
};

Mercy::Application* Mercy::CreateApplication()
{
  return new Sandbox();
}