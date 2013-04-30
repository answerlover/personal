<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TradeTransactionList.aspx.cs" Inherits="TradeTransactionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/ListEdit.css" rel="stylesheet" type="text/css" />
    <link href="Styles/jQuery/redmond/jquery-ui-1.8.10.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.10.custom.min.js"></script>
    <script type="text/javascript" src="Scripts/common.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".date").datepicker();
            //$("#tabs").tabs();


            $(".cbkAllCheckbox input[type=checkbox]").click(function () {
                var isChecked = !!$(this).attr("checked");
                $(".itemCheckbox input[type=checkbox]").each(function () {
                    $(this).attr("checked", isChecked);
                });
            })
        });

    </script>
    <style type="text/css">
        .ui-tabs-panel { font-size: 10pt; font-family: Arial,sans-serif; }
    </style>
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
                <td>
                    <asp:TextBox ID="txtTraderName" runat="server" Width="200"></asp:TextBox></td>

                <td></td>
            </tr>
            <tr>
                <td>销售员:
                </td>
                <td>
                    <asp:DropDownList ID="ddlSeller" runat="server" DataTextField="UserName" DataValueField="ID" Width="100"></asp:DropDownList>
                </td>
                <td>结算日期:</td>
                <td>
                    <asp:TextBox ID="dpTradeDate_Begin" runat="server" CssClass="date" Width="90"></asp:TextBox>
                    -
                        <asp:TextBox ID="dpTradeDate_End" runat="server" CssClass="date" Width="90"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-left: 50px;" /></td>
            </tr>
        </table>

    </div>
    <div style="margin-top: 5px; margin-bottom: 5px;">
        <asp:Button ID="btnAdd" runat="server" Text="添加" PostBackUrl="~/TransactionEdit.aspx" />
        <span style="width: 10px; display: inline-block;"></span>
        <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
    </div>
    <div id="tabs">
        <asp:RadioButtonList ID="rblViewType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblViewType_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="汇总信息" Value="Summary"></asp:ListItem>
            <asp:ListItem Text="详细信息" Value="Detail" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
        <%--        <ul>
            <li><a href="#tabs-1">汇总信息</a></li>
            <li><a href="#tabs-2">详细信息</a></li>
        </ul>--%>
        <asp:Panel ID="pnl1" runat="server">
            <asp:ListView ID="ListView_Summary" runat="server" DataKeyNames="ID" DataMember="ID">
                <LayoutTemplate>
                    <table class="table-border listtable" style="width: 100%;" cellspacing="0">
                        <thead>
                            <tr>
                                <th style="width: 20%;">业务员
                                </th>
                                <th style="width: 20%;">成交数量
                                </th>
                                <th style="width: 20%;">交易手续费</th>
                                <th style="width: 20%;">授权服务机构手续费</th>
                                <th style="width: 20%;">交易所手续费</th>
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
                            <%#Eval("UserName")%>
                        </td>
                        <td class="textright">
                            <%#Eval("TradeCount")%>
                        </td>
                        <td class="textright">
                            <%#Eval("TradeFee")%>
                        </td>
                        <td class="textright">
                            <%#Eval("OrganizationFee")%>
                        </td>
                        <td class="textright">
                            <%#Eval("ExchangeFee")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    no data
                </EmptyDataTemplate>
            </asp:ListView>
        </asp:Panel>
        <asp:Panel ID="pnl2" runat="server">
            <asp:ListView ID="ListView1" runat="server" DataKeyNames="ID" DataMember="ID" ClientIDMode="Static">
                <LayoutTemplate>
                    <table class="table-border listtable" style="width: 100%;" cellspacing="0">
                        <thead>
                            <tr>
                                <th style="width: 10%;">
                                    <asp:CheckBox ID="cbkAll" runat="server" CssClass="cbkAllCheckbox" />
                                </th>
                                <th style="width: 10%;">交易商代码
                                </th>
                                <th style="width: 10%;">交易商名称
                                </th>
                                <th style="width: 10%;">成交数量
                                </th>
                                <th style="width: 10%;">交易手续费</th>
                                <th style="width: 10%;">授权服务机构手续费</th>
                                <th style="width: 10%;">交易所手续费</th>
                                <th style="width: 10%;">结算日期</th>
                                <th style="width: 10%;">导入日期</th>
                                <th style="width: 10%;">销售员</th>
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
                        <td class="textcenter">
                            <asp:CheckBox ID="cbkRow" runat="server" CssClass="itemCheckbox" value="ID" />
                        </td>
                        <td>
                            <%#Eval("TraderCode")%>
                        </td>
                        <td>
                            <%#Eval("TraderName")%>
                        </td>
                        <td class="textright">
                            <%#Eval("TradeCount")%>
                        </td>
                        <td class="textright">
                            <%#  IsShowDetailFee ?Eval("TradeFee"):"N/A"                               
                            %>
                        </td>
                        <td class="textright">
                            <%#Eval("OrganizationFee")%>
                        </td>
                        <td class="textright">
                            <%#  IsShowDetailFee ?Eval("ExchangeFee"):"N/A"                               
                            %>
                        </td>
                        <td class="textcenter">
                            <%#Eval("SettlementDate","{0:yyyy-MM-dd}")%>
                        </td>
                        <td class="textcenter">
                            <%#Eval("InDate")%>
                        </td>
                        <td class="textcenter">
                            <%#Eval("RefUser.UserName")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    no data
                </EmptyDataTemplate>
            </asp:ListView>
            <webdiyer:AspNetPager ID="anPager" runat="server" CssClass="pager" CustomInfoClass="info"
                CustomInfoSectionWidth="" NumericButtonCount="10" CustomInfoHTML="<font color=''>%currentPageIndex%</font>/%PageCount% 页 <font color=''>[共%RecordCount%条]</font>" SubmitButtonClass="gobtn"
                PageSize="30" OnPageChanged="anPager_PageChanged"
                UrlRewritePattern="TradeTransactionList_{0}.html" PagingButtonSpacing="" NextPageText=">"
                PrevPageText="<" FirstPageText="<<" LastPageText=">>" ShowCustomInfoSection="Right" ShowPageIndexBox="Never">
            </webdiyer:AspNetPager>
        </asp:Panel>
    </div>
</asp:Content>
