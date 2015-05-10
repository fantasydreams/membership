


#include "exec.h"
#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//不显示控制台
using namespace std;


int main()
{
	dbexec exec;
	if (exec.execute())
	{
		//std::cout << "yes";
		//system("pause");
		return true;
	}
	else
	{
		//std::cout << "no";
		//system("pause");
		return false;
	}
}

