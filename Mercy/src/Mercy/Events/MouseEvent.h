#pragma once

#include "Mercy/Events/Event.h"

#include <sstream>

namespace Mercy
{
  class MERCY_API MouseMovedEvent : public Event
  {
  public:
    MouseMovedEvent( float x, float y )
      : m_MouseX( x )
      , m_MouseY( y )
    {
    }

    inline float GetMouseX() const { return m_MouseX; }
    inline float GetMouseY() const { return m_MouseY; }

    std::string ToString() const override
    {
      std::stringstream ss;
      ss << "MouseMovedEvent: " << m_MouseX << ", " << m_MouseY;
      return ss.str();
    }

    EVENT_CLASS_TYPE( MouseMoved );
    EVENT_CLASS_CATEGORY( EventCategoryInput | EventCategoryMouse );

  private:
    float m_MouseX;
    float m_MouseY;
  };

  class MERCY_API MouseScrolledEvent : public Event
  {
  public:
    MouseScrolledEvent( float xOffset, float yOffset )
      : m_XOffset( xOffset )
      , m_YOffset( yOffset )
    {
    }

    inline float GetXOffset() const { return m_XOffset; }
    inline float GetYOffset() const { return m_YOffset; }

    std::string ToString() const override
    {
      std::stringstream ss;
      ss << "MouseScrolledEvent: " << m_XOffset << ", " << m_YOffset;
      return ss.str();
    }

    EVENT_CLASS_TYPE( MouseScrolled );
    EVENT_CLASS_CATEGORY( EventCategoryInput | EventCategoryMouse );

  private:
    float m_XOffset;
    float m_YOffset;
  };
}
