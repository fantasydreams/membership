


#include "exec.h"
#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//不显示控制台
using namespace std;


int main()
{
	dbexec exec;
	if (exec.execute())
		return true;
	else
		return false;
}

