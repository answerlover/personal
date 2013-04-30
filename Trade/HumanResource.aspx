<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HumanResource.aspx.cs" Inherits="UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/ListEdit.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top:5px; margin-bottom:5px;">
        
    </div>
    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Contacts" NodeIndent="10" ShowLines="True">
        <HoverNodeStyle Font-Underline="False" />
        <Nodes>         
        </Nodes>
        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
        <ParentNodeStyle Font-Bold="True" ForeColor="#5555DD" />
        <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" />
    </asp:TreeView>

</asp:Content>

