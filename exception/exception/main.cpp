#include "sync.h"
extern MysqlServer Sql;
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
int main() //������
{
	//BOOL WINAPI FreeConsole(void); // ����ʾ����̨����
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
		Sleep(100000000);//��ͬ�����߳�һֱ����
	return 0;
}
