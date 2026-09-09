#include "Mercy.h"
#include "Mercy/Core/Log.h"

class ExampleLayer : public Mercy::Layer
{
public:
  ExampleLayer()
    : Layer( "Example" )
  {

  }

  void OnUpdate() override
  {

  }

  void OnEvent( Mercy::Event& event ) override
  {

  }
};

class Sandbox : public Mercy::Application
{
public:
  Sandbox()
  {
    PushLayer( new ExampleLayer() );
  }

  ~Sandbox()
  {

  }
};

Mercy::Application* Mercy::CreateApplication()
{
  ME_INFO( "Application created" );

  return new Sandbox();
}