/*=====================================================

powered by Carol(luoshengwen)
date : 2015 - 5 - 4

=====================================================*/


#include "sync.h"

MysqlServer Sql;

//fristrun function  Has passed the test
bool MysqlServer::fristrun()
{
	MYSQL_ROW record;
	MYSQL_RES *res = NULL;
	std::string sql = "select id,name from shop;", temp , s_id = shop_id;
	sqliteBeginTransaction();//开始事务
	if (!mysql_query(mysql,sql.c_str()))
	{
		chain_table<MYSQL_ROW> result;
		res = mysql_store_result(mysql);
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		while (record = result.getelement())
		{
			temp = record[0];
			sql = "insert into shop(id,name)values(" + temp + ",'";
			temp = record[1];
			sql += temp + "');";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
		if (res)
			mysql_free_result(res);
	}
	else
	{
		sqliteRollbackTransaction();//回滚事务
		std::cout << mysql_error(mysql);
		return false;
	}
	sql = "select id, password from casher where shop_id = " + s_id + ";";
	if (!mysql_query(mysql, sql.c_str()))
	{
		chain_table<MYSQL_ROW> result;
		res = mysql_store_result(mysql);
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		//sqlite3_exec(conn, "begin;", 0, 0, &err_msg);
		while (record = result.getelement())
		{
			temp = record[0];
			sql = "insert into casher(id,shop_id,password)values(" + temp + "," + s_id + ",";
			temp = record[1];
			sql += temp + ");";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
		//sqlite3_exec(conn, "commit;", 0, 0, &err_msg);
		if (res)
			mysql_free_result(res);
	}
	else
	{
		sqliteRollbackTransaction();//回滚事务
		std::cout << mysql_error(mysql);
		return false;
	}
	sql = "select id,code,name,price,score,discount,brand from goods where shop_id = " + s_id + ";";
	if (!mysql_query(mysql, sql.c_str()))
	{
		chain_table<MYSQL_ROW>result;
		res = mysql_store_result(mysql);
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		//sqlite3_exec(conn, "begin;", 0, 0, &err_msg);
		while (record = result.getelement())
		{
			temp = record[0];
			sql = "insert into goods(goods_id,shop_id,code,name,price,score,discount,brand)values(" + temp + "," + s_id + ",'";
			temp = record[1];
			sql += temp + "','";
			temp = record[2];
			sql += temp + "',";
			temp = record[3];
			sql += temp + ",";
			temp = record[4];
			sql += temp + ",";
			temp = record[5];
			sql += temp + ",'";
			temp = record[6];
			sql += temp + "')";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
		//sqlite3_exec(conn, "commit;", 0, 0, &err_msg);
		if (res)
			mysql_free_result(res);
	}
	else
	{
		sqliteRollbackTransaction();//回滚事务
		std::cout << mysql_error(mysql);
		return false;
	}
	sql = "select id from user";
	if (!mysql_query(mysql, sql.c_str()))
	{
		res = mysql_store_result(mysql);
		chain_table<MYSQL_ROW>result;
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		//sqlite3_exec(conn, "begin;", 0, 0, &err_msg);
		while (record = result.getelement())
		{
			temp = record[0];
			sql = "insert into user(id)values(" + temp +")";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
		//sqlite3_exec(conn, "commit;", 0, 0, &err_msg);
		if (res)
			mysql_free_result(res);
	}
	else
	{
		sqliteRollbackTransaction();//回滚事务
		std::cout << mysql_error(mysql);
		return false;
	}
	sql = "select user_id,name from userinfo;";
	if (!mysql_query(mysql, sql.c_str()))
	{
		res = mysql_store_result(mysql);
		chain_table<MYSQL_ROW>result;
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		//sqlite3_exec(conn, "begin;", 0, 0, &err_msg);
		while (record = result.getelement())
		{
			temp = record[0];
			sql = "insert into userinfo(user_id,name)values(" + temp + ",'";
			if (record[1])
			{
				temp = record[1];
				sql += temp + "');";
			}
			else
				sql += "');";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
		//sqlite3_exec(conn, "commit;", 0, 0, &err_msg);
		if (res)
			mysql_free_result(res);
	}
	else
	{
		sqliteRollbackTransaction();//回滚事务
		std::cout << mysql_error(mysql);
		return false;
	}
	sql = "select user_id,balance,usable from utos where shop_id = " + s_id + ";";
	if (!mysql_query(mysql, sql.c_str()))
	{
		res = mysql_store_result(mysql);
		chain_table<MYSQL_ROW>result;
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		//sqlite3_exec(conn, "begin;", 0, 0, &err_msg);
		while (record = result.getelement())
		{
			temp = record[0];
			sql = "insert into utos(user_id,shop_id,balance,usable)values(" + temp + "," + s_id + ",";
			temp = record[1];
			sql += temp + ",";
			temp = record[2];
			sql += temp + ")";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
		//sqlite3_exec(conn, "commit;", 0, 0, &err_msg);
		if (res)
			mysql_free_result(res);
	}
	else
	{
		sqliteRollbackTransaction();//回滚事务
		std::cout << mysql_error(mysql);
		return false;
	}
	sqliteCommitTransaction();//提交事务
	return true;
}


void MysqlServer::sync(const char * id,const char * frist_run)
{
	shop_id = id;
	while (!connectdata());
	while (!sqlconnect());
		//std::cout << "normal";
	//std::string fristrun_flag = id[2];
	if (!strcmp(frist_run, "1"))
		while (!fristrun());//第一次运行，同步云端所有数据
	std::thread uploadUpdate(&MysqlServer::uploadToMysql,this); //更新mysql服务器线程
	uploadUpdate.detach();
	std::thread downloadUpdate(&MysqlServer::downloadToSqlite,this); //更新本地数据库线程
	downloadUpdate.detach();
}
void MysqlServer::downloadToSqlite()
{
	std::string sql = "select s_id_0,s_table,s_method,time from refresh_log where shop_id = ", temp;
	temp = shop_id;
	sql += temp + ";";
	char buffer[256];
	strcpy(buffer, sql.c_str());
	//std::cout << buffer<<std::endl;
	while (true)
	{
		try
		{
			//先进行user 更新，不然会导致sqlite规则性失败
			DownloadMysqlQuery1(mysql1, "select s_id_0,s_table,s_method,time from refresh_log where shop_id is NULL;");  //update the record which ship_id is null
			DownloadMysqlQuery(mysql1, buffer);  //update have shop_id value
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

bool MysqlServer::DownloadMysqlQuery(MYSQL * mysql,const char * sql)
{
	if (!(mysql_query(mysql, sql)))//执行成功
	{
		MYSQL_RES * res = mysql_store_result(mysql);
		MYSQL_ROW record = NULL;
		std::string time = "0"; //record the last sync time
		//std::string switchon;
		
		chain_table<MYSQL_ROW> result; //防止mysql_query  command失效的问题
		while (record = mysql_fetch_row(res))
			result.push_back(record);
		while (record = result.getelement())
		{
			if (strcmp(record[3], time.c_str()))
				time = record[3];  //将最新同步时间存到time变量中
			if (!strcmp("casher", record[1]))//如果是casher表格
				casherupdate(mysql, record);
			if (!strcmp("goods", record[1]))//goods table
				goodsupdate(mysql, record);
			if (!strcmp("utos", record[1]))//utos table
				utosupdate(mysql, record);
		}
		//printtest(mysql);
		if (res)
			mysql_free_result(res);

		if (time.c_str()!="0")
		{
			std::string sql = "update sync_time set shop_lasttime = '" + time + "';";
			SqliteNoCallbackQuery(conn, sql.c_str());
		}
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
//bool MysqlServer::shopupdate(MYSQL * mysql, MYSQL_ROW record)
//{
//	std::string sql,temp = record[0]; //id:shop_id,sql :sql query , temp : id  //这里的temp就是utos中的user_id
//	std::string tmp; //存储临时信息
//	if (!strcmp("insert", record[2]))//insert method;
//	{
//		sql = "insert into shop(id) values(" + temp + ")";
//		SqliteNoCallbackQuery(conn1, sql.c_str());//执行sql语句
//		return true;
//	}
//	if (!(strcmp("delete", record[2])))//delete method
//	{  //删除本地所有有关shop_id的数据
//		sql = "delete from shop where shop_id = " + temp + ";";
//		SqliteNoCallbackQuery(conn1, sql.c_str());
//		sql = "delete from casher where shop_id = " + temp + ";";
//		SqliteNoCallbackQuery(conn1, sql.c_str());
//		sql = "delete from goods where shop_id = " + temp + ";";
//		SqliteNoCallbackQuery(conn1, sql.c_str());
//		sql = "delete from utos where shop_id = " + temp + ";";
//		SqliteNoCallbackQuery(conn1, sql.c_str());
//		return true;
//	}
//	return false;
//}

//仍有bug存在，尚未找到解决方案
bool MysqlServer::utosupdate(MYSQL *mysql,const MYSQL_ROW record)
{
	std::string id = this->shop_id, sql, temp = record[0]; //id:shop_id,sql :sql query , temp : id  //这里的temp就是utos中的user_id
	std::string tmp; //存储临时信息
	//sqliteBeginTransaction();
	if (!strcmp("insert", record[2]))	//insert method  
	{
		sql = "select balance,usable from utos where shop_id = " + id + " and user_id = " + temp + ";";
		if (!mysql_query(mysql, sql.c_str()))//查询成功
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);
			if (record)//如果查询有结果
			{
				sql = "insert into utos(user_id,shop_id,balance,usable) values(" + temp + "," + id + ",";
				tmp = record[0];
				sql += tmp + ",";
				tmp = record[1];
				sql += tmp + ");";
				//sql = "insert into utos(user_id, shop_id, balance, usable) values(1600006, 101, 8.8, 1);";
				SqliteNoCallbackQuery(conn, sql.c_str());//执行本地sql database操作语句
				/*char * errmsg = NULL;
				if (sqlite3_exec(conn, sql.c_str(), NULL, NULL, &errmsg) == SQLITE_OK);
				else
					std::cout << errmsg << std::endl;*/
			}
			if (res)
				mysql_free_result(res);
			//sqliteCommitTransaction();
			return true;
		}
	}
	if (!strcmp("delete", record[2]))//delete method         //Has passed the test
	{
		sql = "delete form utos where shop_id = " + id + " and user_id = " + temp + ";";
		SqliteNoCallbackQuery(conn, sql.c_str());
		//sqliteCommitTransaction();
		return true;
	}
	if (!strcmp("update", record[2]))//update method
	{
		sql = "select balance,usable from utos where shop_id = " + id + " and user_id = " + temp + ";";
		if (!mysql_query(mysql, sql.c_str()))//查询成功
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);
			if (record)//如果查询有结果
			{
				sql = "update utos set balance = ";
				tmp = record[0];
				sql += tmp + ", usable = ";
				tmp = record[1];
				sql += tmp + " where shop_id = " + id + " and user_id = " + temp + ";";
				std::cout << sql << std::endl;
				SqliteNoCallbackQuery(conn, sql.c_str());//执行本地sql database操作语句
				/*if(sqlite3_exec(conn, sql.c_str(), NULL, NULL, &err_msg)!=SQLITE_OK)
				{
					std::cout << err_msg << std::endl;
					sqlite3_free(err_msg);
				};*/
				
			}
			if (res)
				mysql_free_result(res);
			//sqliteCommitTransaction();
			return true;
		}
	}
	//sqliteRollbackTransaction();
	return false;
}

//goods table data update  Has passed the test
bool MysqlServer::goodsupdate(MYSQL *mysql, const MYSQL_ROW record) 
{
	std::string id = this->shop_id, sql, temp = record[0]; //id:shop_id,sql :sql query , temp : id
	//sqliteBeginTransaction();
	if (!strcmp("update", record[2]))//update method          Has passed the test
	{
		sql = "select code,name,price,score,discount,brand from goods where id = " + temp + ";";
		if (!mysql_query(mysql, sql.c_str()))
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);//因为只有一个结果
			if (record && record[3] >= 0)//如果得到相关的结果并且不是兑换商品
			{
				std::string tmp;
				sql = "update goods set code = '";
				tmp = record[0];
				sql += tmp + "', name = '";
				tmp = record[1];
				sql += tmp + "', price = ";
				tmp = record[2];
				sql += tmp + ", score = ";
				tmp = record[3];
				sql += tmp + ", discount = ";
				tmp = record[4];
				sql += tmp + ", brand = '";
				tmp = record[5];
				sql += tmp + "', shop_id = " + id;
				sql +=" where goods_id = '" + temp + "';";

				SqliteNoCallbackQuery(conn, sql.c_str());//执行成功
			}
			if (res)//释放查询资源
				mysql_free_result(res);
		}
		//sqliteCommitTransaction();
		return true;
	}
	if (!(strcmp("insert", record[2])))//insert method       Has passed the test
	{
		sql = "select code,name,price,score,discount,brand from goods where id = '" + temp + "';";
		if (!mysql_query(mysql, sql.c_str()))
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);//因为只有一个结果
			if (record && record[3] >= 0)//如果得到相关的结果
			{
				std::string tmp;
				sql = "insert into goods(goods_id,shop_id,code,name,price,score,discount,brand) values( '";
				sql += temp + "',";
				sql += id + ",'";
				tmp = record[0];
				sql += tmp + "','";
				tmp = record[1];
				sql += tmp + "',";
				tmp = record[2];
				sql += tmp + ",";
				tmp = record[3];
				sql += tmp + ",";
				tmp = record[4];
				sql += tmp + ",'";
				tmp = record[5];
				sql += tmp + "');";
				SqliteNoCallbackQuery(conn, sql.c_str());//执行成功
			}
			if (res)//释放查询资源
				mysql_free_result(res);
		}
		//sqliteCommitTransaction();
		return true;
	}
	if (!strcmp("delete", record[2]))//delete method       Has passed the test
	{
		sql = "delete from goods where goods_id = '" + temp + "';";
		SqliteNoCallbackQuery(conn, sql.c_str());//执行成功
		//sqliteCommitTransaction();
		return true;
	}
	//sqliteRollbackTransaction();
	return false;
}

//casher table data update //Has passed the test
bool MysqlServer::casherupdate(MYSQL *mysql,MYSQL_ROW record)
{
	std::string id = this->shop_id , sql,temp = record[0];
	//sqliteBeginTransaction();
	if (!strcmp("update", record[2]))//update method   //Has passed the test
	{
		sql = "select password from casher where shop_id = " + id;
		sql += " and id = " + temp;
		if (!mysql_query(mysql, sql.c_str()))
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);//因为只有一个结果
			if (record)//如果得到相关的结果
			{
				std::string passwd = record[0];
				sql = "update casher set password = '" + passwd + "' where shop_id = " + id + " and id = " + temp + ";";
				SqliteNoCallbackQuery(conn, sql.c_str());//执行成功
			}
			if (res)//释放查询资源
				mysql_free_result(res);
		}
		else
			std::cout << mysql_error(mysql);
		//sqliteCommitTransaction();
		return true;
	}
	if (!strcmp("insert", record[2]))//insert method   //Has passed the test
	{
		sql = "select password from casher where shop_id = " + id + " and id = " + temp + ";";
		if (!mysql_query(mysql, sql.c_str()))
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);//因为只有一个结果
			if (record)
			{
				std::string passwd = record[0];
				sql = "insert into casher(id,shop_id,password) values( " + temp + "," + shop_id + ",'" + passwd + "');";
				SqliteNoCallbackQuery(conn, sql.c_str());//执行成功
			}
			if (res)
				mysql_free_result(res);
		}
		else
			std::cout << mysql_error(mysql)<<std::endl;
		if (res)//释放查询资源
			mysql_free_result(res);
		//sqliteCommitTransaction();
		return true;
	}
	if (!strcmp("delete", record[2]))//delete method         Has passed the test
	{
		sql = "delete from casher where shop_id = " + id + " and id = " + temp + ";";
		SqliteNoCallbackQuery(conn, sql.c_str());//执行成功
		//sqliteCommitTransaction();
		return true;
	}
	//sqliteRollbackTransaction();
	return false;
}

//user conn1 and mysql1 connection    //
bool MysqlServer::DownloadMysqlQuery1(MYSQL *mysql, const char *sql)
{
	if (!(mysql_query(mysql, sql)))//执行成功
	{
		MYSQL_RES * res = mysql_store_result(mysql);
		MYSQL_ROW record = NULL;// mysql_fetch_row(res);
		//printtest(mysql);
		std::string time = "0";
		chain_table<MYSQL_ROW> result;
		while (record = mysql_fetch_row(res))
		{
			/*std::cout << record[0] << "	"
				<< record[1] << "	"
				<< record[2] << "	"
				<< record[3] << "	" << std::endl;*/
			result.push_back(record);
		}
			
			//sql :  select s_id_0, s_table, s_method, time from refresh_log where shop_id is NULL;
		while (record = result.getelement())
		{
			/*std::cout << record[0] << "	"
				<< record[1] << "	"
				<< record[2] << "	"
				<< record[3] << "	" << std::endl;*/
			if (record[3], time.c_str())
				time = record[3];
			if (!strcmp("user", record[1])) //user table
				userupdate(mysql, record);
			if (!strcmp("userinfo", record[1]))//userinfo table
				userinfoupdate(mysql, record);
		}
		if (res)
			mysql_free_result(res);
		if (time.c_str()!="0")
		{
			std::string sql = "update sync_time set null_lasttime = '" + time + "';";
			SqliteNoCallbackQuery(conn, sql.c_str());
			
		}
		return true;
	}
	else
	{
		std:: cout << mysql_errno(mysql);
		//while (!sqlconnect())
		mysql_ping(mysql);
		Sleep(6000);
	}
	return true;
}

//userinfo table data update function      //测试通过
bool MysqlServer::userinfoupdate(MYSQL *mysql, const MYSQL_ROW record)
{
	std::string sql,temp = record[0]; //temp is user_id
	//sqliteBeginTransaction();
	if (!strcmp("delete",record[2]))//delete method      //Has passed the test
	{
		sql = "delete from userinfo where user_id = " + temp + ";";
		SqliteNoCallbackQuery(conn, sql.c_str());
		//sqliteCommitTransaction();
		return true;
	}
	if (!strcmp("insert", record[2]))//insert method    //Has passed the test
	{
		sql = "select name from userinfo where user_id = " + temp + ";";
		if (!mysql_query(mysql, sql.c_str()))
		{
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);
			std::string tmp = record[0];
			sql = "insert into userinfo(user_id,name) values(" + temp + ",'" + tmp + "');";//record[0] here represent user_id name instead of user_id
			SqliteNoCallbackQuery(conn, sql.c_str());
			if (res)
				mysql_free_result(res);
			//sqliteCommitTransaction();
			return true;
		}
		//sqliteRollbackTransaction();
		return false;
	}
	if (!strcmp("update", record[2]))//update method        //已通过测试
	{
		sql = "select name from userinfo where user_id = " + temp + ";";
		if (!mysql_query(mysql, sql.c_str()))
		{
			std::string tmp;
			MYSQL_RES * res = mysql_store_result(mysql);
			MYSQL_ROW record = mysql_fetch_row(res);
			sql = "update userinfo set name = '";
			if (record) //当查询结果不为空
			{
				//std::wcout<<UTF8ToUnicode(record[0]);
				tmp = record[0];
				//record[0] here represent user_id name instead of user_id
				sql += tmp + "' where user_id = " + temp + ";";
				std::cout << sql << std::endl;
				SqliteNoCallbackQuery(conn, sql.c_str());
			}
			else//当查询结果为空（null）
			{
				sql += "';";
				SqliteNoCallbackQuery(conn, sql.c_str());
			}
			if (res)
				mysql_free_result(res);
			//sqliteCommitTransaction();
			return true;
		}
		//sqliteRollbackTransaction();
		return false;
	}
	//sqliteRollbackTransaction();
	return false;
}

//user table data update   //测试通过
bool MysqlServer::userupdate(MYSQL *mysql, const MYSQL_ROW record)
{
	std::string sql ,temp = record[0];//temp is user_id
	//sqliteBeginTransaction();
	if (!strcmp("delete", record[2]))//delete method
	{
		sql = "delete from user where id = " + temp + ";";
		SqliteNoCallbackQuery(conn, sql.c_str());
		//sqliteCommitTransaction();
		return true;
	}
	if (!strcmp("insert", record[2]))//insert method
	{
		sql = "insert into user(id) values(" + temp + ")";
		SqliteNoCallbackQuery(conn, sql.c_str());
		//sqliteCommitTransaction();
		return true;
	}
	//sqliteRollbackTransaction();
	return false;
}

//use conn and mysql connection
void MysqlServer::uploadToMysql()
{
	while (true)
	{
		try 
		{
			//先调用query_utos_insert ,否则会发生mysql不存在内容的错误
			query_utos_insert(conn, "select shop_id,user_id from utos_insert;");
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
	conn = NULL;
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
	if (conn)              //释放sqlite连接
		sqlite3_close(conn);
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
						mysql_query(mysql, "set names utf8"); //设置传输编码为utf8
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
					mysql_query(mysql1, "set names utf8");//设置传输编码为utf8
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
				Sql.sqliteBeginTransaction();
				while (!Sql.SqliteNoCallbackQuery(Sql.conn, buffer))
					Sleep(2000);
				Sql.sqliteCommitTransaction();
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

int sqlite3_exec_callback_utos(void *data, int nColumn, char **colValues, char **colNames)
{
	if (colValues)//存在结果   其实这一步也没哟必要写，回调函数是有值的时候才进行调用。
	{
		std::string sql = "select user_id from utos where shop_id = ", shop_id, user_id;
		shop_id = colValues[0];
		sql += shop_id + " and user_id = ";
		user_id = colValues[1];
		sql += user_id + ";";
		if (mysql_query(Sql.mysql, sql.c_str()))
		{
			MYSQL_RES * res = mysql_store_result(Sql.mysql);
			MYSQL_ROW record = NULL;
			record = mysql_fetch_row(res);
			if (!record)//如果没有得到查询结果
			{
				Sql.mysqlBeginTransaction(Sql.mysql);
				sql = "insert into utos(user_id,shop_id) values(" + user_id + "," + shop_id + ");";
				if (!mysql_query(Sql.mysql, sql.c_str()))//执行失败
				{
					Sql.mysalRollbackTransaction(Sql.mysql);
					return 1;
				}
				Sql.mysqlCommitTransaction(Sql.mysql);

			}
			Sql.sqliteBeginTransaction();
			sql = "delete from utos where shop_id = " + shop_id + " and user_id = " + user_id + ";";
			Sql.SqliteNoCallbackQuery(Sql.conn, sql.c_str());//删除本地记录
			Sql.sqliteCommitTransaction();
		}
	}
	return 0;
}

inline void MysqlServer::query_utos_insert(sqlite3 * sqlite, const char * sql)
{
	if (sqlite3_exec(sqlite, sql, &sqlite3_exec_callback_utos, NULL, &err_msg) != SQLITE_OK)
	{
		std::cout << err_msg << std::endl;
		sqlite3_free(err_msg);
	}
}

inline bool MysqlServer::SqliteNoCallbackQuery(sqlite3 * sqlite, const char * sql)
{
	char * errmsg = NULL;
	if (sqlite3_exec(sqlite, sql, NULL, NULL, &errmsg) == SQLITE_OK)
	{
		return true;
	}
	else
	{
		std::cout << errmsg<<std::endl;
		sqlite3_free(errmsg);
		return false;
	}
	
}

inline bool MysqlServer::Mysqlupdatequery(MYSQL * mysql, const char *sql)
{
	mysqlBeginTransaction(mysql);
	if (!mysql_query(mysql, sql))
	{
		mysqlCommitTransaction(mysql);
		return true;
	}
	else
	{
		mysalRollbackTransaction(mysql);
		return false;
	}
}

inline void MysqlServer::query(sqlite3 * sqlite,const char * sql)
{
	if (sqlite3_exec(sqlite, sql, &sqlite3_exec_callback, NULL, &err_msg) != SQLITE_OK)
	{
		std::cout << "err_msg:" << err_msg << std::endl;
		sqlite3_free(err_msg);
	}
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

inline bool MysqlServer::detect_file(const char * path)
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
			sqlite3_busy_timeout(conn, 6000);//设置busy时查询时间
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

bool MysqlServer::sqliteBeginTransaction()
{
	char * errmsg;
	if (sqlite3_exec(conn, "begin transaction", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << "begin transaction failed ：" << errmsg << std::endl;
		sqlite3_free(errmsg);
		return false;
	}
	return true;
}

bool MysqlServer::sqliteCommitTransaction()
{
	char * errmsg;
	if (sqlite3_exec(conn, "commit transaction", NULL, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << "commit transaction failed ：" << errmsg << std::endl;
		sqlite3_free(errmsg);
		if (sqlite3_exec(conn, "rollback transaction", NULL, NULL, &errmsg) != SQLITE_OK)
		{
			std::cout << "rollback transaction failed ：" << errmsg << std::endl;
			sqlite3_free(errmsg);
		}
		return false;
	}
	return true;
}

bool MysqlServer::sqliteRollbackTransaction()
{
	char * errmsg = NULL;
	if (sqlite3_exec(conn, "rollback transaction", NULL, NULL, &errmsg))
	{
		std::cout << "rollback transaction failed ：" << errmsg << std::endl;
		sqlite3_free(errmsg);
		return false;
	}
	return true;
}

bool MysqlServer::mysqlBeginTransaction(MYSQL *mysql)
{
	if (!mysql_query(mysql, "begin transaction;"))
	{
		std::cout <<"begin transaction failed:"<< mysql_error(mysql);
		return false;
	}
	return true;
}

bool MysqlServer::mysqlCommitTransaction(MYSQL * mysql)
{
	if (!mysql_query(mysql, "commit transaction;"))
	{
		std::cout << "commit transaction failed:" << mysql_error(mysql);
		return false;
	}
	return true;
}

bool MysqlServer::mysalRollbackTransaction(MYSQL * mysql)
{
	if (!mysql_query(mysql, "rollback transaction;"))
	{
		std::cout << "rollback transaction failed:" << mysql_error(mysql);
		return false;
	}
	return true;
}
