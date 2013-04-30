<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransactionEdit.aspx.cs" Inherits="TransactionEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <link href="Styles/jQuery/redmond/jquery-ui-1.8.10.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.10.custom.min.js"></script>
     <script type="text/javascript" src="Scripts/common.js"></script>
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
                <td></td>
                <td>
                    <asp:FileUpload ID="fileUploadTrade" runat="server" /></td>
            </tr>
            <tr>
                <td>结算日期</td>
                <td>
                    <asp:TextBox ID="dpTradeDate" runat="server" CssClass="date" Width="100"></asp:TextBox><span class="reqired">*</span></td>
            </tr>
        </table>

        <br />

        <asp:Button ID="btnImport" runat="server" Text="导入" OnClick="btnImport_Click" />
    </div>

</asp:Content>

