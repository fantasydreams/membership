#include "sync.h"
extern MysqlServer Sql;
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
int main(char argc,char *argv[]) //������
{
	//std::cout << argv[0] << "" << argv[1];
	if (argv[1] && argv[2])//�������������Ž��й���  //��һ��������shop_id,�ڶ����������ж��ͻ����Ƿ��һ������
	{
		//std::cout << argv[0] << "	" << argv[1];
		Sql.sync(argv[1]);
		while (true)
			Sleep(100000000);//��ͬ�����߳�һֱ����
	}
	Sql.sync("123");
	while (true)
		Sleep(100000000);//��ͬ�����߳�һֱ����
	return 0;
}
