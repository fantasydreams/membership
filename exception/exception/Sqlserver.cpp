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
		mysql_free_result(res);  //�ͷŲ�ѯ���
	if (mysql)
		mysql_close(mysql);		//�ر�mysql����
}

inline bool MysqlServer::conncetsql()
{
	for (int i = 0; i < 8; i++)
	{
		if (*(IpAddr+i))
			if (mysql_real_connect(mysql, *IpAddr, "ming", "18883285787", "member", 3306, NULL, NULL))  //���������ݿ�
				return true;  //�ɹ�������
	}
	return false;  //���򷵻ؼ�
}
