#include "mercypch.h"
#include "Platform/Windows/WInput.h"

#include "Mercy/Core/Application.h"
#include <GLFW/glfw3.h>

Mercy::Input* Mercy::Input::s_Instance = new Mercy::WInput();

bool Mercy::WInput::IsKeyPressedImpl( int keycode )
{
  GLFWwindow* window = static_cast<GLFWwindow*>( Application::Get().GetWindow().GetNativeWindow() );
  int state = glfwGetKey( window, keycode );

  return state == GLFW_PRESS || state == GLFW_REPEAT;
}

bool Mercy::WInput::IsMouseButtonPressedImpl( int button )
{
  GLFWwindow* window = static_cast<GLFWwindow*>( Application::Get().GetWindow().GetNativeWindow() );
  int state = glfwGetMouseButton( window, button );

  return state == GLFW_PRESS;
}

std::pair<float, float> Mercy::WInput::GetMousePositionImpl()
{
  GLFWwindow* window = static_cast<GLFWwindow*>( Application::Get().GetWindow().GetNativeWindow() );
  double xpos;
  double ypos;
  glfwGetCursorPos( window, &xpos, &ypos );

  return { (float)xpos, (float)ypos };
}

float Mercy::WInput::GetMouseXImpl()
{
  auto [x, y] = GetMousePositionImpl();
  return x;
}

float Mercy::WInput::GetMouseYImpl()
{
  auto [x, y] = GetMousePositionImpl();
  return y;
}