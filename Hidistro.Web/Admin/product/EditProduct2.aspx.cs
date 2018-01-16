using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using kindeditor.Net;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.HtmlControls;
using Hidistro.UI.Web.Admin;
 


namespace Hidistro.Web.Admin.product
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class EditProduct2 : ProductBasePage
    {
        private int categoryId;
        private int productId;
        
        private string toline = "";

        private void btnSave_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            decimal num3;
            decimal? nullable;
            decimal? nullable2;
            decimal? nullable3;
            if (this.categoryId == 0)
            {
                this.categoryId = (int)this.ViewState["ProductCategoryId"];
            }
            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num, out num3, out nullable, out nullable2, out num2, out nullable3))
            {
                if (!this.chkSkuEnabled.Checked)
                {
                    if (num3 <= 0M)
                    {
                        this.ShowMsg("商品一口价必须大于0", false);
                        return;
                    }
                    if (nullable.HasValue && (nullable.Value >= num3))
                    {
                        this.ShowMsg("商品成本价必须小于商品一口价", false);
                        return;
                    }
                }
                string text = this.fckDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo target = new ProductInfo
                {
                    ProductId = this.productId,
                    CategoryId = this.categoryId,
                    TypeId = this.dropProductTypes.SelectedValue,
                    ProductName = this.txtProductName.Text,
                    ProductCode = this.txtProductCode.Text,
                    DisplaySequence = num,
                    MarketPrice = nullable2,
                    Unit = this.txtUnit.Text,
                    ImageUrl1 = this.uploader1.UploadedImageUrl,
                    ImageUrl2 = this.uploader2.UploadedImageUrl,
                    ImageUrl3 = this.uploader3.UploadedImageUrl,
                    ImageUrl4 = this.uploader4.UploadedImageUrl,
                    ImageUrl5 = this.uploader5.UploadedImageUrl,
                    ThumbnailUrl40 = this.uploader1.ThumbnailUrl40,
                    ThumbnailUrl60 = this.uploader1.ThumbnailUrl60,
                    ThumbnailUrl100 = this.uploader1.ThumbnailUrl100,
                    ThumbnailUrl160 = this.uploader1.ThumbnailUrl160,
                    ThumbnailUrl180 = this.uploader1.ThumbnailUrl180,
                    ThumbnailUrl220 = this.uploader1.ThumbnailUrl220,
                    ThumbnailUrl310 = this.uploader1.ThumbnailUrl310,
                    ThumbnailUrl410 = this.uploader1.ThumbnailUrl410,
                    ShortDescription = this.txtShortDescription.Text,
                    IsfreeShipping = this.ChkisfreeShipping.Checked,
                    Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null,
                    AddedDate = DateTime.Now,
                    BrandId = this.dropBrandCategories.SelectedValue
                };
                ProductSaleStatus onSale = ProductSaleStatus.OnSale;
                if (this.radInStock.Checked)
                {
                    onSale = ProductSaleStatus.OnStock;
                }
                if (this.radUnSales.Checked)
                {
                    onSale = ProductSaleStatus.UnSale;
                }
                if (this.radOnSales.Checked)
                {
                    onSale = ProductSaleStatus.OnSale;
                }
                target.SaleStatus = onSale;
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category != null)
                {
                    target.MainCategoryPath = category.Path + "|";
                }
                Dictionary<string, SKUItem> skus = null;
                Dictionary<int, IList<int>> attrs = null;
                if (this.chkSkuEnabled.Checked)
                {
                    target.HasSKU = true;
                    skus = base.GetSkus(this.txtSkus.Text);
                }
                else
                {
                    Dictionary<string, SKUItem> dictionary3 = new Dictionary<string, SKUItem>();
                    SKUItem item = new SKUItem
                    {
                        SkuId = "0",
                        SKU = this.txtSku.Text,
                        SalePrice = num3,
                        CostPrice = nullable.HasValue ? nullable.Value : 0M,
                        Stock = num2,
                        Weight = nullable3.HasValue ? nullable3.Value : 0M
                    };
                    dictionary3.Add("0", item);
                    skus = dictionary3;
                    if (this.txtMemberPrices.Text.Length > 0)
                    {
                        base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
                    }
                }
                if (!string.IsNullOrEmpty(this.txtAttributes.Text) && (this.txtAttributes.Text.Length > 0))
                {
                    attrs = base.GetAttributes(this.txtAttributes.Text);
                }
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target);
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    IList<int> tagIds = new List<int>();
                    if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                    {
                        string str2 = this.txtProductTag.Text.Trim();
                        string[] strArray = null;
                        if (str2.Contains(","))
                        {
                            strArray = str2.Split(new char[] { ',' });
                        }
                        else
                        {
                            strArray = new string[] { str2 };
                        }
                        foreach (string str3 in strArray)
                        {
                            tagIds.Add(Convert.ToInt32(str3));
                        }
                    }
                    switch (ProductHelper.UpdateProduct(target, skus, attrs, tagIds))
                    {
                        case ProductActionStatus.Success:
                            this.litralProductTag.SelectedValue = tagIds;
                            this.ShowMsg("修改商品成功", true);
                            return;

                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("修改商品失败，保存商品属性时出错", false);
                            return;

                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("修改商品失败，商品名称不能重复", false);
                            return;

                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("修改商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.SKUError:
                            this.ShowMsg("修改商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.OffShelfError:
                            this.ShowMsg("修改商品失败， 子站没在零售价范围内的商品无法下架", false);
                            return;

                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("修改商品失败，保存商品标签时出错", false);
                            return;
                    }
                    this.ShowMsg("修改商品失败，未知错误", false);
                }
            }
        }

        private void LoadProduct(ProductInfo product, Dictionary<int, IList<int>> attrs)
        {
            this.dropProductTypes.SelectedValue = product.TypeId;
            this.dropBrandCategories.SelectedValue = product.BrandId;
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
            this.txtProductCode.Text = product.ProductCode;
            this.txtUnit.Text = product.Unit;
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                this.radOnSales.Checked = true;
            }
            else if (product.SaleStatus == ProductSaleStatus.UnSale)
            {
                this.radUnSales.Checked = true;
            }
            else
            {
                this.radInStock.Checked = true;
            }
            this.ChkisfreeShipping.Checked = product.IsfreeShipping;
            this.uploader1.UploadedImageUrl = product.ImageUrl1;
            this.uploader2.UploadedImageUrl = product.ImageUrl2;
            this.uploader3.UploadedImageUrl = product.ImageUrl3;
            this.uploader4.UploadedImageUrl = product.ImageUrl4;
            this.uploader5.UploadedImageUrl = product.ImageUrl5;
            if ((attrs != null) && (attrs.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<xml><attributes>");
                foreach (int num in attrs.Keys)
                {
                    builder.Append("<item attributeId=\"").Append(num.ToString(CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int)ProductTypeHelper.GetAttribute(num).UsageMode).ToString()).Append("\" >");
                    foreach (int num2 in attrs[num])
                    {
                        builder.Append("<attValue valueId=\"").Append(num2.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</item>");
                }
                builder.Append("</attributes></xml>");
                this.txtAttributes.Text = builder.ToString();
            }
            this.chkSkuEnabled.Checked = product.HasSKU;
            if (product.HasSKU)
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append("<xml><productSkus>");
                foreach (string str in product.Skus.Keys)
                {
                    SKUItem item = product.Skus[str];
                    string str2 = ("<item skuCode=\"" + item.SKU + "\" salePrice=\"" + item.SalePrice.ToString("F2") + "\" costPrice=\"" + ((item.CostPrice > 0M) ? item.CostPrice.ToString("F2") : "") + "\" qty=\"" + item.Stock.ToString(CultureInfo.InvariantCulture) + "\" weight=\"" + ((item.Weight > 0M) ? item.Weight.ToString("F2") : "") + "\">") + "<skuFields>";
                    foreach (int num3 in item.SkuItems.Keys)
                    {
                        string[] strArray2 = new string[] { "<sku attributeId=\"", num3.ToString(CultureInfo.InvariantCulture), "\" valueId=\"", item.SkuItems[num3].ToString(CultureInfo.InvariantCulture), "\" />" };
                        string str3 = string.Concat(strArray2);
                        str2 = str2 + str3;
                    }
                    str2 = str2 + "</skuFields>";
                    if (item.MemberPrices.Count > 0)
                    {
                        str2 = str2 + "<memberPrices>";
                        foreach (int num4 in item.MemberPrices.Keys)
                        {
                            decimal num14 = item.MemberPrices[num4];
                            str2 = str2 + string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", num4.ToString(CultureInfo.InvariantCulture), num14.ToString("F2"));
                        }
                        str2 = str2 + "</memberPrices>";
                    }
                    str2 = str2 + "</item>";
                    builder2.Append(str2);
                }
                builder2.Append("</productSkus></xml>");
                this.txtSkus.Text = builder2.ToString();
            }
            SKUItem defaultSku = product.DefaultSku;
            this.txtSku.Text = product.SKU;
            this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
            this.txtCostPrice.Text = (defaultSku.CostPrice > 0M) ? defaultSku.CostPrice.ToString("F2") : "";
            this.txtStock.Text = defaultSku.Stock.ToString(CultureInfo.InvariantCulture);
            this.txtWeight.Text = (defaultSku.Weight > 0M) ? defaultSku.Weight.ToString("F2") : "";
            if (defaultSku.MemberPrices.Count > 0)
            {
                this.txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int num5 in defaultSku.MemberPrices.Keys)
                {
                    decimal num19 = defaultSku.MemberPrices[num5];
                    this.txtMemberPrices.Text = this.txtMemberPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num5.ToString(CultureInfo.InvariantCulture), num19.ToString("F2"));
                }
                this.txtMemberPrices.Text = this.txtMemberPrices.Text + "</gradePrices></xml>";
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
            if (!this.Page.IsPostBack)
            {
                Dictionary<int, IList<int>> dictionary;
                IList<int> tagsId = null;
                ProductInfo product = ProductHelper.GetProductDetails(this.productId, out dictionary, out tagsId);
                if (product == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                        this.ViewState["ProductCategoryId"] = this.categoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(product.CategoryId);
                        this.ViewState["ProductCategoryId"] = product.CategoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    this.lnkEditCategory.NavigateUrl = this.lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture);
                    this.litralProductTag.SelectedValue = tagsId;
                    if (tagsId.Count > 0)
                    {
                        foreach (int num in tagsId)
                        {
                            this.txtProductTag.Text = this.txtProductTag.Text + num.ToString() + ",";
                        }
                        this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                    }
                    this.dropProductTypes.DataBind();
                    this.dropBrandCategories.DataBind();
                    this.LoadProduct(product, dictionary);
                }
            }
        }

        private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight)
        {
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            displaySequence = stock = 0;
            salePrice = 0M;
            if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写商品排序");
            }
            if (this.txtProductCode.Text.Length > 20)
            {
                str = str + Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
            }
            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtMarketPrice.Text, out num))
                {
                    marketPrice = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的市场价");
                }
            }
            if (!skuEnabled)
            {
                if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品一口价");
                }
                if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
                {
                    decimal num2;
                    if (decimal.TryParse(this.txtCostPrice.Text, out num2))
                    {
                        costPrice = new decimal?(num2);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的库存数量");
                }
                if (!string.IsNullOrEmpty(this.txtWeight.Text))
                {
                    decimal num3;
                    if (decimal.TryParse(this.txtWeight.Text, out num3))
                    {
                        weight = new decimal?(num3);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的重量");
                    }
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
   
    }
}