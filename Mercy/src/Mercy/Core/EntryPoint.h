#pragma once

#ifdef ME_PLATFORM_WINDOWS

extern Mercy::Application* Mercy::CreateApplication();

int main( int argc, char** argv )
{
  Mercy::Application* app = Mercy::CreateApplication();
  app->Run();
  delete app;
}

#endif // ME_PLATFORM_WINDOWS