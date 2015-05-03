#include "sync.h"

MysqlServer Sql;

void MysqlServer::sync()
{

	while (!connectdata());
	while (!sqlconnect());
		//std::cout << "normal";
	std::thread uploadUpdate(&MysqlServer::uploadToMysql,this);
	uploadUpdate.detach();
	std::thread downloadUpdate(&MysqlServer::downloadToSqlite,this);
	downloadUpdate.detach();
		//uploadToMysql();
	//std::thread Mysql_update;

}
void MysqlServer::downloadToSqlite()
{
	while (true)
	{
		try
		{
			DownloadMysqlQuery(mysql1, "select id,s_table,s_method,s_id_0,s_id_1 from refresh_log;");
			//std::cout << "hello word!" << std::endl;
			Sleep(5000);//完成一次提交后线程休眠5S
		}
		catch (...)
		{
			//异常处理代码
			//while (!sqlconnect())
				Sleep(6000);
		}
	}
}

bool MysqlServer::DownloadMysqlQuery(MYSQL * mysql,char * sql)
{
	if (!(mysql_query(mysql, sql)))//执行成功
	{
		//MYSQL_RES * res = mysql_store_result(mysql);
		//MYSQL_ROW record = mysql_fetch_row(res);
		printtest(mysql);
		return true;
	}
	else
	{
		//while (!sqlconnect())
		mysql_ping(mysql);
		Sleep(6000);
	}
		  return true;
}

void MysqlServer::uploadToMysql()
{
	while (true)
	{
		try 
		{
			query(conn,"select shop_id,user_id,column,operate,value from sync;");
			//std::cout << "hello word!" << std::endl;
			Sleep(5000);//完成一次提交后线程休眠5S
		}
		catch (...)
		{
			//异常处理代码
			//while (!sqlconnect())
				Sleep(6000);
		}
	}
}

inline bool MysqlServer::sqlconnect() //
{
	if (conncetsql() && conncetsql1())
	{
		//std::cout << "ok";
		//printtest();
		return true;
	}
	else
		return false;
		//std::cout << "error";
}

MysqlServer::MysqlServer()
{
	conn1 = conn = NULL;
	err_msg = NULL;
	memset(sql, 0, sizeof(char) * 200);
	bit = 0;
	key = NULL;
	sha1 = NULL;
}

MysqlServer::~MysqlServer()
{
	if (res)
		mysql_free_result(res);  //释放查询结果
	if (res1)
		mysql_free_result(res1);
	if (mysql)
		mysql_close(mysql);		//关闭mysql连接
	if (mysql1)
		mysql_close(mysql1);
	if (key)
		delete[] key;
	if (sha1)
		delete[]sha1;
}

bool MysqlServer::conncetsql()
{
	while (!NetworkIsAvliable())
		Sleep(2000);
	if (mysql)
	{
		mysql_close(mysql);
		mysql_free_result(res);
	}
		mysql = mysql_init(mysql);
		mysql_options(mysql, MYSQL_OPT_RECONNECT, &value);

		while (IpAddr[0] == NULL)  //当没有连接互联网
		{
			Sleep(10000);//休眠10s
			gethostip();
			//std::cout << "休眠..." << std::endl;
		}
		for (int i = 0; i < 8; i++)
		{
			if (*(IpAddr + i))
				try
				{
					if (mysql_real_connect(mysql, *IpAddr, "ming", "18883285787", "member", 3306, NULL, NULL))  //连接至数据库
					{
						mysql_query(mysql, "set names gbk");
						return true;  //成功返回真
					}
				}
				catch (...)
				{
					return false;
				}
		}
	return false;  //否则返回假
}

inline bool MysqlServer::conncetsql1()
{
	while (!NetworkIsAvliable())
		Sleep(2000);
	try
	{
		if (mysql1)
			mysql_close(mysql1);
		mysql1 = mysql_init(mysql1);
		mysql_options(mysql1, MYSQL_OPT_RECONNECT, &value1);
		while (!NetworkIsAvliable())
			Sleep(10000);//休眠10s
		for (int i = 0; i < 8; i++)
		{
			if (*(IpAddr + i))
				if (mysql_real_connect(mysql1, *IpAddr, "ming", "18883285787", "member", 3306, NULL, NULL))  //连接至数据库
				{
					mysql_query(mysql1, "set names gbk");
					return true;  //成功返回真
				}
		}
	}
	catch (...)
	{
		return false;
	}
	return false;  //否则返回假
}

bool MysqlServer::NetworkIsAvliable()
{
	WORD wVersionRequested;
	WSADATA wsaData;
	wVersionRequested = MAKEWORD(1, 1);    //初始化Socket动态连接库,请求1.1版本的winsocket库  

	err = WSAStartup(wVersionRequested, &wsaData);

	if (LOBYTE(wsaData.wVersion) != 1 ||   //判断请求的winsocket是不是1.1的版本  
		HIBYTE(wsaData.wVersion) != 1)
	{
		WSACleanup();          //清盘  
		return false;                  //终止对winsocket使用  
	}
	//WSADATA ws;  
	//WSAStartup(MAKEWORD(2,2),&ws);//  
	char http[] = "www.myvip6.com";           //访问服务器域名
	SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);//建立socket  
	if (sock == INVALID_SOCKET)
	{
		//std::cout << "建立访问socket套接字失败!" << std::endl;
		return false;
	}
	hostent* host = gethostbyname(http);//取得主机的IP地址  
	if (host == NULL)
	{
		//std::cout << "主机处于没有联网状态;" << std::endl;
		return false;
	}
	return true;
}

bool MysqlServer::queryDatabase()
{
	if (!(mysql_query(mysql, "select id,s_table,s_method,s_id_0,s_id_1 from refresh_log;")))//查询成功
	{
		printtest(mysql);
		return true;
	}
	else
		return false;
}

void MysqlServer::printtest(MYSQL *mysql)
{
	int lineNum = mysql_field_count(mysql), i = 0;
	MYSQL_RES *res = mysql_store_result(mysql);
	MYSQL_ROW record = NULL;// mysql_fetch_row(res);
	while (record = mysql_fetch_row(res))
	{
		for (i = 0; i < lineNum; i++)
		{
			if (record[i])
			std::cout << record[i] << "	";
		}
		std::cout << std::endl;
	}
}

//sqlite3回调函数
int sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames)
{
	//std::cout << "in";
	//std::wstring wstr;
	//std::wcout.imbue(std::locale("chs"));
	//for (int i = 0; i < nColumn; i++)
	//{
	//	wstr = UTF8ToUnicode(colValues[i]);
	//	std::wcout << wstr << "\t";
	//}
	//std::cout << std::endl;
	std::string sql, temp;
	//首先查找是否在服务器上存在相关信息
	sql = "select user_id from utos where shop_id = ";
	temp = colValues[0];
	sql += temp + " and user_id = ";
	temp = colValues[1];
	sql += temp + ";";
	//std::cout << sql << std::endl;
	point:if (!mysql_query(Sql.mysql, sql.c_str()))//查询成功
	{
		Sql.res = mysql_store_result(Sql.mysql);
		if (Sql.record1 = mysql_fetch_row(Sql.res)) //数据存在
		{
			sql = "update utos set ";
			temp = colValues[2];
			sql += temp + " = ";
			sql += temp + " ";
			temp = colValues[3];
			sql += temp + " ";
			temp = colValues[4];
			sql += temp + " where shop_id = ";
			temp = colValues[0];
			sql += temp + " and user_id = ";
			temp = colValues[1];
			sql += temp + ";";
			//std::cout << sql << std::endl;
			char buffer[512];
			strcpy(buffer,sql.c_str());
			if (Sql.Mysqlupdatequery(Sql.mysql, buffer))
			{
				sql = "delete from sync where shop_id = ";
				temp = colValues[0];
				sql += temp + " and user_id = ";
				temp = colValues[1];
				sql += temp + ";";
				strcpy(buffer, sql.c_str());
				std::cout << sql<<std::endl;
				while (!Sql.SqliteNoCallbackQuery(Sql.conn, buffer))
					Sleep(2000);
			}
		}

	}
	else
	{
		//while (!mysql_options(Sql.mysql, MYSQL_OPT_RECONNECT, NULL)) //尝试重新连接数据库
		mysql_ping(Sql.mysql1);
		Sleep(6000);//休眠6S
		goto point;
	}
	return 0;
}

inline bool MysqlServer::SqliteNoCallbackQuery(sqlite3 * sqlite, char * sql)
{
	char * errmsg = NULL;
	if (sqlite3_exec(sqlite, sql, NULL, NULL, &errmsg) == SQLITE_OK)
	{
		return true;
	}
	else
	{
		std::cout << errmsg<<std::endl;
		return false;
	}
	
}

inline bool MysqlServer::Mysqlupdatequery(MYSQL * mysql, char *sql)
{
	if (!mysql_query(mysql, sql))
		return true;
	else
		return false;
}

void MysqlServer::query(sqlite3 * sqlite,char * sql)
{
	if (sqlite3_exec(sqlite, sql, &sqlite3_exec_callback, NULL, &err_msg) != SQLITE_OK)
		std::cout << "err_msg:" << err_msg << std::endl;
}

void MysqlServer::ConvertCharToTCHAR(char * Char)
{
#ifdef UNICODE  
	MultiByteToWideChar(CP_ACP, 0, Char, -1, Name, 100);
#else  
	strcpy(Name, Char);
#endif
}

char * MysqlServer::ConvertLPWSTRToLPSTR(LPWSTR lpwszStrIn)
{
	LPSTR pszOut = NULL;
	if (lpwszStrIn != NULL)
	{
		int nInputStrLen = wcslen(lpwszStrIn);

		// Double NULL Termination  
		int nOutputStrLen = WideCharToMultiByte(CP_ACP, 0, lpwszStrIn, nInputStrLen, NULL, 0, 0, 0) + 2;
		pszOut = new char[nOutputStrLen];

		if (pszOut)
		{
			memset(pszOut, 0x00, nOutputStrLen);
			WideCharToMultiByte(CP_ACP, 0, lpwszStrIn, nInputStrLen, pszOut, nOutputStrLen, 0, 0);
		}
	}
	return pszOut;
}

inline bool MysqlServer::detect_file(char * path)
{
	std::ifstream in(path);
	if (in)
		return true;
	else
		return false;
}

bool MysqlServer::connectdata()
{
	if (detect_file("./DB/data.db"))
	{
		if (!(sqlite3_open("./DB/data.db", &conn) != SQLITE_OK))
		{
			//填写连接密码代码
			return true;
		}
		else
			return false;
		//while (sqlite3_key(conn, "sqlite3", 7) !=SQLITE_OK);
		/*if (sqlite3_key(conn, "sql", 3) != SQLITE_OK)
		std::cout << "error msg:密码错误";*/
		//CreateTable(conn);
		/*if (sqlite3_rekey(conn, "sql", 3) != SQLITE_OK)
		std::cout << "error msg:修改密码错误";*/
	}
	else
	{
		//std::cout << "无法打开数据库，请尝试重启程序或者删除与此程序同一目录下的RSG.db文件！" << std::endl;
		//exit(-1);
		if (detect_file("./dbexc.exe"))
		{
			getFilesha1("./dbexc.exe");
			char * csha1;
			if (!(strcmp("47BD32537817DD1AC15C0722788D33936442C3B7", (csha1 = ConvertLPWSTRToLPSTR(sha1)))))
			{
				delete csha1;
				unsigned int pro = WinExec("./dbexc.exe", SW_RESTORE);
				if (!(pro == 0))
					if (connectdata())
						return true;
					else  //调用失败或者内存耗尽
						return false;
			}
			else
			{
				delete csha1;
				exit(0);
			}

		}
	}
	return true;
}

void MysqlServer::makekey()
{
	bit = ramdom(16, 32);
	while (!(key = new char[bit + 1])); //apply key memory
	memset(key, 0, sizeof(char)*(bit + 1));
	srand(time_t(NULL));
	for (int i = 0; i < bit; i++)
		key[i] = char(ramdom(65, 112));

	std::cout << key << std::endl;
}

//produce ramdom number between x and y
inline int MysqlServer::ramdom(int x, int y)
{
	//	srand(time(NULL));
	return rand() % abs(y - x) + ((x > y) ? y : x);
}

bool MysqlServer::getFilesha1(char * name)
{
	CSHA1 sha;
	ConvertCharToTCHAR(name);
	const bool bSuccess = sha.HashFile(Name);
	sha.Final();
	if (sha1)
		delete[] sha1;
	sha1 = new TCHAR[41];
	sha.ReportHash(sha1, CSHA1::REPORT_HEX_SHORT);
	if (bSuccess)
	{
		//std::cout << sha1 << std::endl;
		return true;
	}
	else
		return false;
}