


#include "exec.h"
#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
using namespace std;


int main()
{
	dbexec exec;
	if (exec.execute())
		return true;
	else
		return false;
}

