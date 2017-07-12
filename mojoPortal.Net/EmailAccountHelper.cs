using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Text;
using anmar.SharpMimeTools;
using OpenPOP.POP3;


namespace mojoPortal.Net
{
    /// <summary>
    /// Author:					
    /// Created:				2006-08-02
    /// Last Modified:			2007-06-17
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public class EmailAccountHelper
    {
        //private POP3Connection mailConnection;
        private POPClient mailClient = null;
        private String userName = String.Empty;
        private String password = String.Empty;
        private String popServerName = String.Empty;
        private int popServerPort = 110;
        private bool useSSL = true;

        public EmailAccountHelper(
            String name, 
            String userPassword, 
            String server, 
            int port)
        {
            userName = name;
            password = userPassword;
            popServerName = server;
            popServerPort = port;

            //mailConnection 
            //    = new POP3Connection(
            //    userName, 
            //    password, 
            //    popServerName, 
            //    popServerPort);

            mailClient = new POPClient();
            mailClient.UseSSL = useSSL;

        }

        public EmailAccountHelper()
        {
            // temporary testing
            userName = ConfigurationManager.AppSettings["testEmailUserName"];
            password = ConfigurationManager.AppSettings["testEmailPassword"];
            popServerName = ConfigurationManager.AppSettings["testEmailPopServerName"];


            //mailConnection = new POP3Connection(userName, password, popServerName, popServerPort);


        }

        public int MessageCount
        {
            get
            {
                //if (mailConnection != null)
                //{
                //    mailConnection.Open();
                //    int count = mailConnection.MessageCount();
                //    mailConnection.Close();
                //    return count;
                //}
                if (mailClient != null)
                {
                    //Utility.Log = true;
                    mailClient.Disconnect();
                    mailClient.UseSSL = this.useSSL;
                    mailClient.Connect(this.popServerName, this.popServerPort);
                    mailClient.Authenticate(this.userName, this.password);
                    int Count = mailClient.GetMessageCount();
                    mailClient.Disconnect();
                    return Count;

                }
                return 0;

            }
        }

        public List<SharpMessage> GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;

            List<SharpMessage> inboxMessages
                = new List<SharpMessage>();

            mailClient.Disconnect();
            mailClient.UseSSL = this.useSSL;
            mailClient.Connect(this.popServerName, this.popServerPort);

            mailClient.Authenticate(
                this.userName,
                this.password,
                AuthenticationMethod.TRYBOTH);

            int totalMessages = mailClient.GetMessageCount();

            if (pageSize > 0) totalPages = totalMessages / pageSize;

            if (totalMessages <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalMessages, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            int lastMessageToGet;
            if (pageNumber == 1)
            {
                lastMessageToGet = totalMessages;
            }
            else
            {
                lastMessageToGet = totalMessages - (pageSize * pageNumber);
            }
            int firstMessageToGet = lastMessageToGet - pageSize;

            for (int i = lastMessageToGet; i >= firstMessageToGet; i -= 1)
            {
                SharpMessage message
                    = mailClient.GetMessage(i, false);

                if (message != null)
                {
                    inboxMessages.Add(message);
                }

                //message.

            }

            mailClient.Disconnect();


            return inboxMessages;

        }

        //public List<Message> GetPage(
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    totalPages = 1;

        //    List<Message> inboxMessages
        //        = new List<Message>();

        //    mailClient.Disconnect();
        //    mailClient.UseSSL = this.useSSL;
        //    mailClient.Connect(this.popServerName, this.popServerPort);

        //    mailClient.Authenticate(
        //        this.userName,
        //        this.password,
        //        AuthenticationMethod.TRYBOTH);

        //    int totalMessages = mailClient.GetMessageCount();

        //    if (pageSize > 0) totalPages = totalMessages / pageSize;

        //    if (totalMessages <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else
        //    {
        //        int remainder;
        //        Math.DivRem(totalMessages, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    int lastMessageToGet;
        //    if (pageNumber == 1)
        //    {
        //        lastMessageToGet = totalMessages;
        //    }
        //    else
        //    {
        //        lastMessageToGet = totalMessages - (pageSize * pageNumber);
        //    }
        //    int firstMessageToGet = lastMessageToGet - pageSize;

        //    for (int i = lastMessageToGet; i >= firstMessageToGet; i -= 1)
        //    {
        //        OpenPOP.MIMEParser.Message message
        //            = mailClient.GetMessage(i, false);

        //        if (message != null)
        //        {
        //            inboxMessages.Add(message);
        //        }

        //    }

        //    mailClient.Disconnect();


        //    return inboxMessages;

        //}

        public DataTable GetInBox()
        {
            DataTable dtInBox = new DataTable();
            dtInBox.Columns.Add("MessageID", typeof(String));
            dtInBox.Columns.Add("From", typeof(String));
            dtInBox.Columns.Add("Subject", typeof(String));
            dtInBox.Columns.Add("Date", typeof(String));
            
			//Utility.Log=true;
            mailClient.Disconnect();
            mailClient.UseSSL = this.useSSL;
            mailClient.Connect(this.popServerName, this.popServerPort);

            mailClient.Authenticate(
                this.userName, 
                this.password,
                AuthenticationMethod.TRYBOTH);

            int Count = mailClient.GetMessageCount();
            if (Count > 20) Count = 20;

			for(int i=Count;i>=1;i-=1)
			{
                SharpMessage message 
                    = mailClient.GetMessage(i, false);

                if (message != null)
				{
                    DataRow row = dtInBox.NewRow();
                    row["MessageID"] = message.MessageID;
                    row["From"] = message.From;
                    row["Subject"] = message.Subject;
                    row["Date"] = message.Date;
                    dtInBox.Rows.Add(row);
				}
				
		    }

            mailClient.Disconnect();

            return dtInBox;

        }










        //public DataTable GetInBox()
        //{

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("MessageID", typeof(String));
        //    dt.Columns.Add("From", typeof(String));
        //    dt.Columns.Add("Subject", typeof(String));
        //    dt.Columns.Add("Date", typeof(DateTime));


        //    if (mailConnection != null)
        //    {
        //        mailConnection.Open();

        //        int messageCount = mailConnection.MessageCount();
        //        int minRange = messageCount - 21;
        //        int maxRange = messageCount - 1;
        //        bool deleteFromServer = false;

        //        SharpMessage[] messages
        //            = mailConnection.GetMessageRange(
        //            minRange,
        //            maxRange,
        //            deleteFromServer);

        //        //mailConnection.GetMessageRange(

        //        foreach (SharpMessage message in messages)
        //        {

        //            DataRow row = dt.NewRow();

        //            //message.MessageID
        //            row["MessageID"] = message.MessageID;
        //            row["From"] = message.From;
        //            row["Subject"] = message.Subject;
        //            row["Date"] = message.Date;

                  
        //            dt.Rows.Add(row);
                
        //        }


        //        mailConnection.Close();
        //    }

        //    //short[] q = mails.List();
        //    //POP3Message msg = mails.Retr(q[q.GetUpperBound(0)]);
        //    //foreach (DictionaryEntry de in msg.Headers)
        //    //    Console.WriteLine("\n***********\n{0}\n-----------\n{1}", de.Key, de.Value);

        //    //mails.Close();
        //    return dt;


        //}

    }
}
