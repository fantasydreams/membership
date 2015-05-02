#include"network.h"
#include <iostream>

network::~network()
{
	for (int i = 0; i < 8; i++)
	{
		if (*(IpAddr + i))
			delete[] *(IpAddr + i);
		else
			break;
	}
}

network::network()
{
	memset(IpAddr, NULL, 8);
	err = gethostip();
}

bool network::gethostip()
{
	//加载套接字库  
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
	in_addr addr;
	for (int i = 0;; i++)
	{
		char *p = host->h_addr_list[i];
		if (p == NULL)
			break;
		memcpy(&addr.S_un.S_addr, p, host->h_length);
		*(IpAddr + i) = new char[strlen(inet_ntoa(addr)) + 1];
		strcpy(*(IpAddr + i), inet_ntoa(addr));
		//std::cout << *(IpAddr + i);
	}
	return true;
}