#pragma once

#include "Mercy/Events/Event.h"

namespace Mercy
{
  class MERCY_API KeyEvent : public Event
  {
  public:
    inline int GetKeycode() const { return m_Keycode; }

    EVENT_CLASS_CATEGORY( EventCategoryInput | EventCategoryKeyboard );

  protected:
    KeyEvent( int keycode )
      : m_Keycode( keycode )
    {
    }

    int m_Keycode;
  };

  class MERCY_API KeyPressedEvent : public KeyEvent
  {
  public:
    KeyPressedEvent( int keycode, int repeatCount )
      : KeyEvent( keycode )
      , m_RepeatCount( repeatCount )
    {
    }

    inline int GetRepeatCount() const { return m_RepeatCount; }

    std::string ToString() const override
    {
      std::stringstream ss;
      ss << "KeyPressedEvent: " << m_Keycode << " (" << m_RepeatCount << " repeats)";
      return ss.str();
    }

    EVENT_CLASS_TYPE( KeyPressed );

  private:
    int m_RepeatCount;
  };

  class MERCY_API KeyReleasedEvent : public KeyEvent
  {
  public:
    KeyReleasedEvent( int keycode )
      : KeyEvent( keycode )
    {
    }

    std::string ToString() const override
    {
      std::stringstream ss;
      ss << "KeyReleasedEvent: " << m_Keycode;
      return ss.str();
    }

    EVENT_CLASS_TYPE( KeyReleased );
  };

  class MERCY_API KeyTypedEvent : public KeyEvent
  {
  public:
    KeyTypedEvent( int keycode )
      : KeyEvent( keycode )
    {
    }

    std::string ToString() const override
    {
      std::stringstream ss;
      ss << "KeyTypedEvent: " << m_Keycode;
      return ss.str();
    }

    EVENT_CLASS_TYPE( KeyTyped )
  };
}
