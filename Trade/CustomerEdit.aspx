<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerEdit.aspx.cs" Inherits="CustomerEdit" ValidateRequest="false" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/jQuery/redmond/jquery-ui-1.8.10.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.10.custom.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".date").datepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table>
            <tr>
                <td>客户编号</td>
                <td>
                    <asp:Label ID="txtID" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>交易商代码</td>
                <td>
                    <asp:TextBox ID="txtCustomerNameCode" runat="server"></asp:TextBox><span class="reqired">*</span></td>
            </tr>

            <tr>
                <td>交易商名称</td>
                <td>
                    <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox></td>
            </tr>

            <tr>
                <td>客户类型</td>
                <td>
                    <asp:RadioButtonList ID="rblCustomerType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="个人" Value="Personal" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="公司" Value="Company"></asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>

            <tr>
                <td>客户操作类型</td>
                <td>
                    <asp:RadioButtonList ID="rblState" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblState_SelectedIndexChanged">
                        <asp:ListItem Text="新增" Value="New" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="开户" Value="Open"></asp:ListItem>
                        <asp:ListItem Text="活动" Value="Active"></asp:ListItem>
                        <asp:ListItem Text="会员" Value="VIP"></asp:ListItem>
                    </asp:RadioButtonList>

                </td>
            </tr>
            <tr>
                <td>QQ</td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Email</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>手机号码</td>
                <td>
                    <asp:TextBox ID="txtCellPhoneNumber" runat="server"></asp:TextBox><span id="reqiredCellPhoneNumber" runat="server" class="reqired">*</span></td>
            </tr>
            <tr>
                <td>联系电话</td>
                <td>
                    <asp:TextBox ID="txtContactPhoneNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>身份证号码</td>
                <td>
                    <asp:TextBox ID="txtIDCardNumber" runat="server" Width="250" MaxLength="18"></asp:TextBox><span id="reqiredIDCardNumber" runat="server" class="reqired">*</span></td>
            </tr>
            <tr>
                <td>联系地址</td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" Width="250"></asp:TextBox><span id="reqiredAddress" runat="server" class="reqired">*</span></td>
            </tr>
            <tr>
                <td>业务员</td>
                <td>
                    <asp:DropDownList ID="ddlSeller" runat="server" DataTextField="UserName" DataValueField="ID" Width="150"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>录入日期</td>
                <td>
                    <asp:Label ID="txtInDate" runat="server"></asp:Label></td>
            </tr>
        </table>


        <br />

        <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnOK_Click" />
    </div>

    <hr style="margin: 10px;" />
    <asp:Panel ID="pnlLog" runat="server" Visible="false">
        <div>备注</div>

        <asp:TextBox ID="txtMemo" runat="server" Width="400" Height="100"></asp:TextBox>

        <asp:Button ID="btnAddMemo" runat="server" Text="添加" OnClick="btnAddMemo_Click" />

        <div style="margin-top: 10px;">
            <asp:Repeater ID="rptMemo" runat="server">
                <HeaderTemplate></HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <span style="display: inline-block; width: 120px; color: #9B9B9B;"><%# Eval("InDate","{0:yyyy-MM-dd HH:mm}") %></span>
                        <span style="display: inline-block; width: 80px; color: #2595B7;"><%# Eval("InUser") %></span>
                        <div style="display: inline-block;"><%# HttpUtility.HtmlEncode( Eval("Memo")) %></div>
                    </div>

                </ItemTemplate>
                <SeparatorTemplate>
                    <hr style="border: 0; border-bottom: dashed 1px #ccc; margin-top: 5px;" />
                </SeparatorTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:Repeater>

            <webdiyer:AspNetPager ID="anPager" runat="server" CssClass="pager" CustomInfoClass="info"
                CustomInfoSectionWidth="" NumericButtonCount="10" CustomInfoHTML="<font color=''>%currentPageIndex%</font>/%PageCount% 页 <font color=''>[共%RecordCount%条]</font>" SubmitButtonClass="gobtn"
                PageSize="30" ShowPageIndex="True" OnPageChanged="anPager_PageChanged"
                UrlRewritePattern="TradeTransactionList_{0}.html" PagingButtonSpacing="" NextPageText=">"
                PrevPageText="<" FirstPageText="<<" LastPageText=">>" ShowCustomInfoSection="Right">
            </webdiyer:AspNetPager>

        </div>

    </asp:Panel>

</asp:Content>

