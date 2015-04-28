#include "exception.h"

MysqlServer::MysqlServer()
{
	mysql = mysql_init(mysql);
	if (conncetsql())
		std::cout << "ok";
	else
		std::cout << "error";

}

MysqlServer::~MysqlServer()
{
	if (res)
		mysql_free_result(res);  //释放查询结果
	if (mysql)
		mysql_close(mysql);		//关闭mysql连接
}

inline bool MysqlServer::conncetsql()
{
	for (int i = 0; i < 8; i++)
	{
		if (*(IpAddr+i))
			if (mysql_real_connect(mysql, *IpAddr, "ming", "18883285787", "member", 3306, NULL, NULL))  //连接至数据库
				return true;  //成功返回真
	}
	return false;  //否则返回假
}
