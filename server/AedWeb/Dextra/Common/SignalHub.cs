using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR.Hubs;
using System.Net.Http;
using System.Security.Claims;

namespace Dextra.Common.SignalR.Hubs
{

    public class MyUserType
    {
        public string ConnectionId { get; set; }
        // Can have whatever you want here
    }

    public delegate void SignalCallback(string msg, string command, int start, int end, int number);


    [HubName("SignalHub")]
    //[Authorize(RequireOutgoing = true)]
    public class SignalHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

        public void Echo(string msg, decimal num)
        {
            Clients.Caller.sendMessage(num);
        }

        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();


        public override Task OnConnected()
        {
            //var newCookie = new HttpCookie("SessionID", Context.ConnectionId);
            //Context.Request.GetHttpContext().Response.Cookies.Add(newCookie);
            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { ConnectionId = Context.ConnectionId });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            MyUserType garbage;
            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            return base.OnDisconnected(stopCalled);
        }

        public void ProgresBarrSetting(string connectionId, string msg,string command,int start,int end,int percent)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
            hubContext.Clients.Client(connectionId).ProgresBarrSetting(msg, command,start, end,percent);
            
            //hubContext.Clients.All.updateProgressBar(taskId, percentage);
            //Clients.All.updateProgressBar(taskId, percentage);
        }


        public void DownloadFile(string connectionId, string url,string filename)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
            hubContext.Clients.Client(connectionId).DownloadFile(url, filename);

        }
        public void ServerError(string connectionId,string msg1, string msg2)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
                //hubContext.Clients.Clients(MyUsers.Keys.ToList()).ServerError(msg1, msg2);
                hubContext.Clients.Client(connectionId).ServerError(msg1, msg2);
            } catch
            {

            }
            //hubContext.Clients.All.updateProgressBar(taskId, percentage);
            //Clients.All.updateProgressBar(taskId, percentage);
        }




    }
}