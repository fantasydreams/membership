/*=====================================================

powered by Carol(luoshengwen)
date : 2015 - 5 - 4

=====================================================*/

#include "sync.h"
extern MysqlServer Sql;
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
int main(char argc,char *argv[]) //������
{
	//std::cout << argv[0] << "" << argv[1];
	if (argv[1])//�������������Ž��й���  //��һ��������shop_id,�ڶ����������ж��ͻ����Ƿ��һ������
	{
		if (argv[2] != "1" || argv[2] != "0")
			exit(0);
		//std::cout << argv[0] << "	" << argv[1];
		Sql.sync(argv[1],argv[2]);
		while (true)
			Sleep(100000000);//��ͬ�����߳�һֱ����
	}
	Sql.sync("101");
	while (true)
		Sleep(100000000);//��ͬ�����߳�һֱ����
	return 0;
}
