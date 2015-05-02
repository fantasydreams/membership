#include "sync.h"
extern MysqlServer Sql;
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//不显示控制台
int main() //传参数
{
	//BOOL WINAPI FreeConsole(void); // 不显示控制台窗口
	Sql.sync();
	//if (ex.createDB())
	//{
	//	std::cout << "yes";
	//}
	//else
	//{
	//	//std::cout << "no";
	//}
	//system("pause");
	while (true)
		Sleep(100000000);//等同让主线程一直休眠
	return 0;
}
