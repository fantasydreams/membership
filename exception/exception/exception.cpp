
#include "exception.h"


int main(int argc,char agrv[]) //传参数
{

	exception ex;
	if (ex.createDB())
		std::cout << "yes";
	else
	{
		//std::cout << "no";
		

	}
		
	system("pause");
	//Sleep(3000);
//	BOOL WINAPI FreeConsole(void); // 不显示控制台窗口
	return 0;
}


