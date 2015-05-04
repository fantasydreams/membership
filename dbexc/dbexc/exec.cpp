#include "exec.h"

dbexec::dbexec()
{
	conn = NULL;
	errmsg = NULL;
}
bool dbexec::execute()
{
	if (detectfile() && createfile())//如果文件不存在，那么建立
	{
		return connect();
	}
	else
		exit(0);//文件存在，退出程序
	return true;
}
//连接数据库文件
bool dbexec::connect()
{
	if (sqlite3_open("./DB/data.db", &conn) != SQLITE_OK)
		cout << "failed";
	else
	{
		cout << "yes" << endl;
		if (createTable())
			return true;
		//cout << "yes" << endl;
		else
			return false;
			//cout << "false";
	}
	return true;
}
//创建数据库表格
bool dbexec::createTable()
{
	if (sqlite3_exec(conn, "CREATE TABLE shop('id'  INTEGER NOT NULL,'name'  TEXT(20) NOT NULL,PRIMARY KEY('id'));", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE casher('id'  INTEGER NOT NULL,'shop_id'  INTEGER NOT NULL,'password'  TEXT(20) NOT NULL,PRIMARY KEY('id' ASC, 'shop_id' ASC),FOREIGN KEY('shop_id') REFERENCES 'shop' ('id')	); ",NULL,NULL,&errmsg))
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE goods (\
						   	'shop_id'  INTEGER NOT NULL,\
							'code'  TEXT(50) NOT NULL,\
							'name'  TEXT(30) NOT NULL,\
							'price'  REAL(6, 2) NOT NULL,\
							'score'  INTEGER NOT NULL,\
							'discount'  REAL(3, 2) NOT NULL,\
							'brand'  TEXT(20) NOT NULL,\
							PRIMARY KEY('shop_id', 'code')\
							); ", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE sync (\
						   	'shop_id'  INTEGER NOT NULL,\
							'user_id'  INTEGER NOT NULL,\
							'column'  TEXT(20) NOT NULL,\
							'operate'  TEXT(3) NOT NULL,\
							'value'  REAL(6, 2) NOT NULL\
							); ", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE 'user' (\
						   	'id'  INTEGER NOT NULL,\
							PRIMARY KEY('id')\
							);", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE userinfo (\
							'user_id'  INTEGER NOT NULL,\
							'name'  TEXT(16) NOT NULL,\
							PRIMARY KEY('user_id'),\
							FOREIGN KEY('user_id') REFERENCES 'user' ('id')\
							);", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE utos (\
		'user_id'  INTEGER NOT NULL,\
		'shop_id'  INTEGER NOT NULL,\
		'balance'  REAL(5, 2) NOT NULL DEFAULT 0,\
		'usable'  INTEGER NOT NULL DEFAULT 1,\
		PRIMARY KEY('user_id' ASC),\
		CONSTRAINT 'fkey0' FOREIGN KEY('user_id') REFERENCES 'user' ('id'),\
		CONSTRAINT 'fkey1' FOREIGN KEY('shop_id') REFERENCES 'shop' ('id'));\
		CREATE TRIGGER 'utostrigger' after insert on 'utos' for each row begin insert into utos_insert(user_id, shop_id) values(new.user_id, new.shop_id);\
		end; ",NULL,NULL,&errmsg)!=SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	if (sqlite3_exec(conn, "CREATE TABLE utos_insert(\
		'user_id'  INTEGER NOT NULL,\
		'shop_id'  INTEGER NOT NULL\
		); ", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}

	if (sqlite3_exec(conn, "CREATE TABLE sync_time (\
		'shop_lasttime'  TEXT(20),\
		'null_lasttime'  TEXT(20)); ", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << errmsg;
		return false;
	}
	return true;
}
//检测文件和文件夹是否存在
bool dbexec::detectfile()
{
	ifstream in("./DB/data.db");
	if (in)
	{
		cout << "exist";
		return false;
	}
	else
	{ 
		cout << "not exist";
		return true;
	}
}
//创建文件
bool dbexec::createfile()
{
	WIN32_FIND_DATA wfd;
	HANDLE fhandle = FindFirstFile(_T("./DB"), &wfd);
	if (fhandle == INVALID_HANDLE_VALUE)
	{
		//std::cout << "directory not exsit!";
		while (!CreateDirectory(_T("./DB"), NULL));
	}
	std::ofstream out;
	out.open("./DB/data.db", std::ios::out);
	if (out)
	{
		out.close();
		return true;
	}
	else
	{
		return false;
	}
}