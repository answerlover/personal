<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CompanyEdit.aspx.cs" Inherits="CompanyEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
        <tr>
            <td>
                <table cellpadding="0">
                    <tr>
                        <td align="center" colspan="2">用户编辑</td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="CompanyNameLabel" runat="server" AssociatedControlID="CompanyName">公司代码:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="CompanyCode" runat="server" CssClass="textEntry"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="CompanyNameRequired" runat="server" ControlToValidate="CompanyCode"
                                CssClass="failureNotification" ErrorMessage="必须填写“公司代码”。" ToolTip="必须填写“公司代码”。"
                                ValidationGroup="ValidationGroup1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="CompanyName">公司名称:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="CompanyName" runat="server" CssClass="textEntry"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CompanyName"
                                CssClass="failureNotification" ErrorMessage="必须填写“公司名称”。" ToolTip="必须填写“公司名称”。"
                                ValidationGroup="ValidationGroup1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">
                            <asp:Button ID="btnOK" runat="server"
                                Text="确认" ValidationGroup="ValidationGroup1" OnClick="btnOK_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
