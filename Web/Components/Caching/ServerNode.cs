﻿namespace mojoPortal.Web.Caching;

public class ServerNode
{
	public string IPAddressOrHostName { get; set; }
	public int Port { get; set; }
	public bool IsAlive { get; set; }


	public ServerNode()
	{
		IsAlive = true;
		Port = 11211;
		IPAddressOrHostName = "127.0.0.1";
	}


	public ServerNode(string ipAddress, int port)
	{
		IPAddressOrHostName = ipAddress;
		Port = port;
		IsAlive = true;
	}


	public string GetFullHostAddress()
	{
		if (Port == 0)
		{
			Port = 11211;
		}

		if (string.IsNullOrWhiteSpace(IPAddressOrHostName))
		{
			IPAddressOrHostName = "127.0.0.1";
		}

		return string.Format("{0}:{1}", IPAddressOrHostName, Port);
	}
}
