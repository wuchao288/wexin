namespace Hidistro.UI.Web
{
    using Hidistro.Core.Jobs;
    using System;
    using System.Web;

    public class Global : HttpApplication
    {
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Hidistro.Core.Jobs.Jobs.Instance().Stop();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Hidistro.Core.Jobs.Jobs.Instance().Start();
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }
    }
}

