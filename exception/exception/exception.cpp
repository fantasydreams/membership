
#include "exception.h"


int main(int argc,char agrv[]) //������
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
//	BOOL WINAPI FreeConsole(void); // ����ʾ����̨����
	return 0;
}


