/*=====================================================

powered by Carol
date : 2015 - 5 - 4

=====================================================*/

#include "sync.h"
extern MysqlServer Sql;
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//����ʾ����̨
int main(char argc,char *argv[]) //������
{
	//std::cout << argv[0] << "" << argv[1];
	//if (argv[1])//�������������Ž��й���  //��һ��������shop_id,�ڶ����������ж��ͻ����Ƿ��һ������
	//{
	//	if (argv[2] != "1" || argv[2] != "0")
	//		exit(0);
	//	//std::cout << argv[0] << "	" << argv[1];
	//	Sql.sync(argv[1],argv[2]);
	//}
	//Sql.sync("123","0");
	//while (true)
	//	Sleep(100000000);//��ͬ�����߳�һֱ����

	sqlite3 * conn;
	sqlite3_open("./DB/data.db", &conn);
	char * err;
	if (sqlite3_exec(conn, "insert into utos(shop_id,user_id,balance,changesLimit,usable) values(2,2,2,2,2);", NULL, NULL, &err) != SQLITE_OK)
	//if (sqlite3_exec(conn,"update utos set user_id = 5 where user_id = 1 and shop_id = 1",NULL,NULL,&err))
	{
		std::cout << err << std::endl;
	}
	std::cout << std::endl << sqlite3_changes(conn)<<std::endl;
	system("pause");
	return 0;
}
