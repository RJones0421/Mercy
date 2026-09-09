#pragma once

#include "Mercy/Core/Core.h"
#include "Mercy/Events/Event.h"

namespace Mercy
{
  class MERCY_API Layer
  {
  public:
		Layer( const std::string& name = "Layer" );
		virtual ~Layer();

		virtual void OnAttach() {}
		virtual void OnDetach() {}
		virtual void OnUpdate() {}
		virtual void OnEvent( Event& event ) {}

		inline const std::string& GetName() const { return m_DebugName; }

	protected:
		std::string m_DebugName;
  };
}
