#include "sqlite3.h"
#include <fstream>
#include <windows.h>

#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"")
#pragma comment(lib,"Shlwapi.lib")
#pragma comment(lib,"sqlite3.lib")