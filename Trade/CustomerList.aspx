<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerList.aspx.cs" Inherits="CustomerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/ListEdit.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="querypanel">
        <table>
            <tr>
                <td>交易商代码:</td>
                <td>
                    <asp:TextBox ID="txtTraderCode_Begin" runat="server" Width="100"></asp:TextBox>
                    至
                        <asp:TextBox ID="txtTraderCode_End" runat="server" Width="100"></asp:TextBox></td>
                <td>交易商名称</td>
                <td><asp:TextBox ID="txtTraderName" runat="server" Width="200"></asp:TextBox></td>
                <td></td>
            </tr>
            <tr>
                <td>业务员:
                </td>
                <td>
                    <asp:DropDownList ID="ddlSeller" runat="server" DataTextField="UserName" DataValueField="ID" Width="100"></asp:DropDownList>
                </td>
                <td>状态：
                </td>
                <td>
                    <asp:RadioButtonList ID="rblState" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="全部" Value="ALL" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="开户" Value="New"></asp:ListItem>
                        <asp:ListItem Text="活动" Value="Active"></asp:ListItem>
                        <asp:ListItem Text="会员" Value="VIP"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" /></td>
            </tr>
        </table>

    </div>
    <div style="margin-top: 5px; margin-bottom: 5px;">
        <asp:Button ID="btnAdd" runat="server" Text="添加" PostBackUrl="~/CustomerEdit.aspx" />

    </div>
    <asp:ListView ID="ListView_Customer" runat="server" DataKeyNames="ID" DataMember="ID" OnItemCommand="ListView_Customer_ItemCommand">
        <LayoutTemplate>
            <table class="table-border listtable" style="width: 100%;" cellspacing="0">
                <thead>
                    <tr>
                        <th style="width: 10%;">交易商代码
                        </th>
                        <th style="width: 10%;">交易商名称
                        </th>
                        <th style="width: 7%;">客户类型</th>
                        <th style="width: 7%;">客户操作类型</th>
                        <th style="width: 10%;">QQ</th>
                        <th style="width: 10%;">Email</th>
                        <th style="width: 10%;">手机号码</th>
                        <th style="width: 10%;">联系电话</th>
                        <th style="width: 10%;">联系地址</th>
                        <th style="width: 10%;">业务员</th>
                        <th style="width: 6%;"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("CustomerNameCode")%>
                </td>
                <td>
                    <%#Eval("CustomerName")%>
                </td>
                <td>
                    <%# TextLocalization.CustomerTypeFormat( Eval("CustomerType"))%>
                </td>
                <td>
                    <%# TextLocalization.CustomerStateFormat(Eval("State"))%>
                </td>
                <td>
                    <%#Eval("QQ")%>
                </td>
                <td>
                    <%#Eval("Email")%>
                </td>
                <td>
                    <%#Eval("CellPhoneNumber")%>
                </td>
                <td>
                    <%#Eval("ContactPhoneNumber")%>
                </td>
                <td>
                    <%#Eval("Address")%>
                </td>
                <td class="textcenter">
                    <%#Eval("UserName")%>
                </td>
                <td class="textcenter">
                    <a href='CustomerEdit.aspx?action=edit&ID=<%#  Eval("ID") %>'>编辑</a>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            未查询到结果
        </EmptyDataTemplate>

    </asp:ListView>
    <webdiyer:AspNetPager ID="anPager" runat="server" CssClass="pager" CustomInfoClass="info"
        CustomInfoSectionWidth="" NumericButtonCount="10" CustomInfoHTML="<font color=''>%currentPageIndex%</font>/%PageCount% 页 <font color=''>[共%RecordCount%条]</font>" SubmitButtonClass="gobtn"
        PageSize="30" ShowPageIndex="True" OnPageChanged="anPager_PageChanged"
        UrlRewritePattern="TradeTransactionList_{0}.html" PagingButtonSpacing="" NextPageText=">"
        PrevPageText="<" FirstPageText="<<" LastPageText=">>" ShowCustomInfoSection="Right" ShowPageIndexBox="Never">
    </webdiyer:AspNetPager>
</asp:Content>

