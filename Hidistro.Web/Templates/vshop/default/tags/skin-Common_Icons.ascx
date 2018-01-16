<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<script runat="server">
    public string GetImageUrl(string url)
    {
        string str = url;
        if (!url.ToLower().Contains("storage/master/navigate") && !url.ToLower().Contains("templates"))
        {
            return "class='" + url + "'";
        }
        else {
            return "style='background-image:url(" + url + ")'";
        }
    }

    public string GetUrl(string locationType,string url,string location_url)
    {
        string str = url;
        
        if(locationType == "9")
        {
            str = url;
        }
        else {
            str = location_url;
        }
        return str;
    }
</script>

<li>
    <a href="<%# GetUrl(Eval("LocationType").ToString(),Eval("Url").ToString(),Eval("LoctionUrl").ToString())%>">
        <span class="ico_span">
            <div <%# GetImageUrl(Eval("ImageUrl").ToString())%>></div>
        </span>
        <label><%# Eval("ShortDesc")%></label>
    </a>
    <div class="line"></div>
</li>