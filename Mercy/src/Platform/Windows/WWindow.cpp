#include "mercypch.h"
#include "Platform/Windows/WWindow.h"

#include "Mercy/Events/ApplicationEvent.h"
#include "Mercy/Events/KeyEvent.h"
#include "Mercy/Events/MouseEvent.h"

#include "glad/glad.h"

static bool s_GLFWInitialized = false;

static void GLFWErrorCallback( int error, const char* description )
{
  ME_CORE_ERROR( "GLFW Error: ({0}): {1}", error, description );
}

Mercy::Window* Mercy::Window::Create( const WindowProps& props )
{
  return new WWindow( props );
}

Mercy::WWindow::WWindow( const WindowProps& props )
{
  Init( props );
}

Mercy::WWindow::~WWindow()
{
  Shutdown();
}

void Mercy::WWindow::Init( const WindowProps& props )
{
  m_Data.Title = props.Title;
  m_Data.Width = props.Width;
  m_Data.Height = props.Height;

  ME_CORE_INFO( "Creating window {0} ({1}, {2})", props.Title, props.Width, props.Height );

  if ( !s_GLFWInitialized )
  {
    // TODO: glfwTerminate on system shutdown
    int success = glfwInit();
    ME_CORE_ASSERT( success, "Could not initialize GLFW!" );
    glfwSetErrorCallback( GLFWErrorCallback );
    s_GLFWInitialized = true;
  }

  m_Window = glfwCreateWindow( (int)props.Width, (int)props.Height, m_Data.Title.c_str(), nullptr, nullptr );
  glfwMakeContextCurrent( m_Window );

  int status = gladLoadGLLoader( (GLADloadproc)glfwGetProcAddress );
  ME_CORE_ASSERT( status, "Failed to initialize Glad!" );

  glfwSetWindowUserPointer( m_Window, &m_Data );
  SetVSync( true );

  // Set GLFW callbacks
  glfwSetWindowSizeCallback( m_Window, []( GLFWwindow* window, int width, int height )
    {
      WindowData* data = (WindowData*)glfwGetWindowUserPointer( window );
      data->Width = width;
      data->Height = height;

      WindowResizeEvent event( width, height );
      data->EventCallback( event );
    } );

  glfwSetWindowCloseCallback( m_Window, []( GLFWwindow* window )
    {
      WindowData* data = (WindowData*)glfwGetWindowUserPointer( window );
      WindowCloseEvent event;
      data->EventCallback( event );
    } );

  glfwSetKeyCallback( m_Window, []( GLFWwindow* window, int key, int scancode, int action, int mods )
    {
      WindowData* data = (WindowData*)glfwGetWindowUserPointer( window );

      switch ( action )
      {
      case GLFW_PRESS:
      {
        KeyPressedEvent event( key, 0 );
        data->EventCallback( event );
        break;
      } // GLFW_PRESS
      case GLFW_RELEASE:
      {
        KeyReleasedEvent event( key );
        data->EventCallback( event );
        break;
      } // GLFW_RELEASE
      case GLFW_REPEAT:
      {
        KeyPressedEvent event( key, 1 );
        data->EventCallback( event );
        break;
      } // GLFW_REPEAT
      } // switch
    } );

  glfwSetMouseButtonCallback( m_Window, []( GLFWwindow* window, int button, int action, int mods )
    {
      WindowData* data = (WindowData*)glfwGetWindowUserPointer( window );

      switch ( action )
      {
      case GLFW_PRESS:
      {
        MouseButtonPressedEvent event( button );
        data->EventCallback( event );
        break;
      } // GLFW_PRESS
      case GLFW_RELEASE:
      {
        MouseButtonReleasedEvent event( button );
        data->EventCallback( event );
        break;
      } // GLFW_RELEASE
      } // switch
    } );

  glfwSetScrollCallback( m_Window, []( GLFWwindow* window, double xOffset, double yOffset )
    {
      WindowData* data = (WindowData*)glfwGetWindowUserPointer( window );
      MouseScrolledEvent event( (float)xOffset, (float)yOffset );
      data->EventCallback( event );
    } );

  glfwSetCursorPosCallback( m_Window, []( GLFWwindow* window, double xPos, double yPos )
    {
      WindowData* data = (WindowData*)glfwGetWindowUserPointer( window );
      MouseMovedEvent event( (float)xPos, (float)yPos );
      data->EventCallback( event );
    } );
}

void Mercy::WWindow::Shutdown()
{
  glfwDestroyWindow( m_Window );
}

void Mercy::WWindow::OnUpdate()
{
  glfwPollEvents();
  glfwSwapBuffers( m_Window );
}

void Mercy::WWindow::SetVSync( bool enabled )
{
  if ( enabled )
  {
    glfwSwapInterval( 1 );
  }
  else
  {
    glfwSwapInterval( 0 );
  }

  m_Data.VSync = enabled;
}

bool Mercy::WWindow::IsVSync() const
{
  return m_Data.VSync;
}
