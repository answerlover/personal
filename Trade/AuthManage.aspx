<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AuthManage.aspx.cs" Inherits="AuthManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>公司切换</legend>
        <div>
            <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="CompanyName" DataValueField="CompanyCode" Width="200"></asp:DropDownList>
            <asp:Button ID="btnOK" runat="server" Text="确定" OnClick="btnOK_Click" />
        </div>
    </fieldset>
    <fieldset>
        <legend>自动公海</legend>
        <div>
            公海条件
            <ul>
                <li>开户30天都没有交易记录</li>
                <li>出现过交易记录但是90天内都无新记录</li>
                <li>或者7天没有更改备注</li>
            </ul>
            <asp:Button ID="btnAutoGonghai" runat="server" Text="执行" OnClick="btnAutoGonghai_Click" />
        </div>
    </fieldset>
</asp:Content>

