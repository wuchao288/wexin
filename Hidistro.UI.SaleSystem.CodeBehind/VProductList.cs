namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private Literal hdProductList;
        private HtmlInputHidden txtTotalPages;
        private Literal lblSortFiled;//排序字段
        private Literal lblSortDirection;//排序方向
        private Literal lblKeyWord; //搜索内容
        
        protected override void AttachChildControls()
        {
            int num;
            int num2;
            int num3;

            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            this.litContent = (Literal) this.FindControl("litContent");
            this.hdProductList = (Literal)this.FindControl("hdProductList");
            this.rptCategories = (VshopTemplatedRepeater) this.FindControl("rptCategories");
            this.txtTotalPages = (HtmlInputHidden) this.FindControl("txtTotal");

            this.lblSortFiled = (Literal)this.FindControl("lblSortFiled");
            this.lblSortDirection = (Literal)this.FindControl("lblSortDirection");
            this.lblKeyWord = (Literal)this.FindControl("lblKeyWord");
            
            string str = this.Page.Request.QueryString["sort"];
            if (string.IsNullOrWhiteSpace(str))
            {
                str = "DisplaySequence";
            }
            string str2 = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }

            this.lblSortFiled.Text = str;
            this.lblSortDirection.Text = str2;

            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 50;
            }

            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxMainCategories(100);
            this.rptCategories.DataSource = maxSubCategories;
            this.rptCategories.DataBind();

            lblKeyWord.Text = keyWord;
            
            DataTable dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, num, num2, out num3, str, str2);

            this.hdProductList.Text = JsonConvert.SerializeObject(dt);
            this.txtTotalPages.SetWhenIsNotNull(num3.ToString());
            PageTitle.AddSiteNameTitle("商品列表");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductList.html";
            }
            base.OnInit(e);
        }
    }
}

