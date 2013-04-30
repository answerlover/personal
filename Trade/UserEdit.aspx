<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserEdit.aspx.cs" Inherits="UserEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table cellpadding="0">
        <thead>
            <tr>
                <th style="width: 80px;"></th>
                <th></th>
            </tr>
        </thead>
        <tr>
            <td align="center" colspan="2">
                <asp:Label ID="txtTitle" runat="server" Text="用户编辑" Font-Bold="true"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">用户名</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" ReadOnly="true" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                    CssClass="failureNotification" ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。"
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server">密码</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" ReadOnly="true" MaxLength="10"></asp:TextBox><asp:CheckBox ID="ckbRestPassword" runat="server" Text="重设密码" AutoPostBack="True" OnCheckedChanged="ckbRestPassword_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td>角色
            </td>
            <td>
                <asp:DropDownList ID="ddlRole" runat="server" DataTextField="RoleDescription" DataValueField="RoleID" Width="100"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>公司
            </td>
            <td>
                <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="CompanyName" DataValueField="CompanyCode" Width="200" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>上级领导
            </td>
            <td>
                <asp:DropDownList ID="ddlUser" runat="server" DataTextField="UserName" DataValueField="ID" Width="200"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td></td>
            <td></td>
        </tr>
    </table>
    <div style="margin-top:10px; margin-bottom: 5px;">

        <asp:Button ID="btnOK" runat="server"
            Text="确认" ValidationGroup="LoginUserValidationGroup" OnClick="btnOK_Click" />

    </div>

</asp:Content>
