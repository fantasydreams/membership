#include "exception.h"
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//不显示控制台
int main(int argc, char agrv[]) //传参数
{
	//BOOL WINAPI FreeConsole(void); // 不显示控制台窗口
	MysqlServer sql;
	//if (ex.createDB())
	//{
	//	std::cout << "yes";
	//}
	//else
	//{
	//	//std::cout << "no";
	//}

	system("pause");
	//Sleep(3000);
	
	return 0;
}
