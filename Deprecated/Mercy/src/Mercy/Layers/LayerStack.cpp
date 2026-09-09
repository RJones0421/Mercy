#include "mercypch.h"
#include "LayerStack.h"

Mercy::LayerStack::LayerStack()
{
}

Mercy::LayerStack::~LayerStack()
{
	for ( Layer* layer : m_Layers )
	{
		delete layer;
	}
}

void Mercy::LayerStack::PushLayer( Layer* layer )
{
	m_Layers.emplace( m_Layers.begin() + m_LayerInsertIndex, layer );
	m_LayerInsertIndex++;
	layer->OnAttach();
}

void Mercy::LayerStack::PushOverlay( Layer* overlay )
{
	m_Layers.emplace_back( overlay );
	overlay->OnAttach();
}

void Mercy::LayerStack::PopLayer( Layer* layer )
{
	auto it = std::find( m_Layers.begin(), m_Layers.end(), layer );
	if ( it != m_Layers.end() )
	{
		m_Layers.erase( it );
		m_LayerInsertIndex--;
		layer->OnDetach();
	}
}

void Mercy::LayerStack::PopOverlay( Layer* overlay )
{
	auto it = std::find( m_Layers.begin(), m_Layers.end(), overlay );
	if ( it != m_Layers.end() )
	{
		m_Layers.erase( it );
		overlay->OnDetach();
	}
}
