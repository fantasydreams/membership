#ifndef  _WINSOCK_DEPRECATED_NO_WARNINGS
#define  _WINSOCK_DEPRECATED_NO_WARNINGS
#endif

#ifndef _CRT_SECURE_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#endif


#ifndef _NetWorkAPI_
#define _NetWorkAPI_

#include<WinSock2.h>

#pragma comment(lib,"ws2_32.lib")
class network
{
public:
	network();
	~network();
protected:
	int err;
	char *IpAddr[8];  //存域名的ip地址（可能存在多个ip地址）
	bool gethostip();
	int True;//标记有效地IP地址
};
#endif

