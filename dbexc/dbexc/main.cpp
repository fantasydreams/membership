


#include "exec.h"
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
using namespace std;


int main()
{
	dbexec exec;
	if (exec.execute())
	{
		//std::cout << "yes";
		return true;
	}
	else
	{
		//std::cout << "no";
		return false;
	}
}

