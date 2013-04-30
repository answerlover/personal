<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/ListEdit.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top:5px; margin-bottom:5px;">
        <asp:Button ID="btnAdd" runat="server" Text="添加" PostBackUrl="~/UserEdit.aspx?action=Add" />
        <asp:Button ID="btnHR" runat="server" Text="人力资源关系图" PostBackUrl="~/HumanResource.aspx" />
    </div>
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="ID" DataMember="ID" OnItemCommand="ListView_Customer_ItemCommand" OnItemDataBound="ListView1_ItemDataBound">
        <LayoutTemplate>
            <table class="table-border listtable" style="width: 100%;" cellspacing="0">
                <thead>
                    <tr>
                        <th style="width: 20%;">ID
                        </th>
                        <th style="width: 20%;">用户名
                        </th>
                        <th style="width: 20%;">公司代码</th>
                        <th style="width: 20%;">公司名称</th>
                        <th style="width: 10%;"></th>
                        <th style="width: 10%;"></th>
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
                    <%#Eval("ID")%>
                </td>
                <td>
                    <%#Eval("UserName")%>
                </td>
                <td>
                    <%#Eval("CompanyCode")%>
                </td>
                <td>
                    <%#Eval("CompanyName")%>
                </td>
                <td class="textcenter">
                    <a href='UserEdit.aspx?id=<%#Eval("ID")%>'>编辑 </a>
                </td>
                <td class="textcenter">
                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="delete" CommandArgument='<%#Eval("ID")%>' >删除</asp:LinkButton>
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
        PrevPageText="<" FirstPageText="<<" LastPageText=">>" ShowCustomInfoSection="Right">
    </webdiyer:AspNetPager>
</asp:Content>

