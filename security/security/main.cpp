
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
#include "security.h"

int main()
{

	security sec;
	
	//sec.getFilesha1("./dbexc.exe");

	if (sec.exec())
	{
		std::cout << 1;
		//return true;
	}
	else
	{
		std::cout << 2;
		//return false;
	}
		
	system("pause");
}