#include "sync.h"
extern MysqlServer Sql;
//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )//不显示控制台
int main(char argc,char *argv[]) //传参数
{
	//std::cout << argv[0] << "" << argv[1];
	if (argv[1] && argv[2])//仅当传进参数才进行工作  //第一个参数是shop_id,第二个参数是判定客户端是否第一次启动
	{
		//std::cout << argv[0] << "	" << argv[1];
		Sql.sync(argv[1]);
		while (true)
			Sleep(100000000);//等同让主线程一直休眠
	}
	Sql.sync("123");
	while (true)
		Sleep(100000000);//等同让主线程一直休眠
	return 0;
}
