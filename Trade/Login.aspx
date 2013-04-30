<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        p { margin-bottom: 5px; }
        fieldset { border-radius: 4px; }
        legend { color: Blue; }
        .login { padding: 5px; }
        .login p > label:first-child { display: inline-block; width: 55px; }

        input[type="text"], input[type="password"] { display: inline-block; height: 18px; padding: 4px; margin-bottom: 9px; font-size: 13px; line-height: 18px; color: #222; border: 1px solid #BBB; background-color: #F9F9F9; border-radius: 2px; }
    </style>
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript">
        $(function () {
            function autoSize() {
                var top = ($(window).height() - $(".page").height() * 2) / 3;
                if (top <= 0) { top = 0; }
                $(".page").css("margin-top", top);
            };
            autoSize();
            $(window).resize(autoSize);
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page" style="width: 450px; padding: 10px; margin: auto;">
            <h2 style="font-weight: bold;">渤海商品交易所成都临远投资管理有限公司
            </h2>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>帐户信息</legend>
                    <div style="padding: 5px;">
                        <table>
                            <tr>
                                <td>
                                    <div style="width: 64px; height: 64px; background: url(./Images/Login.png)">
                                    </div>

                                </td>
                                <td>
                                    <div style="margin-left: 10px;">
                                        <p>
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">用户名:</asp:Label>
                                            <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" Width="159"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                CssClass="failureNotification" ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。"
                                                ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                        </p>
                                        <p>
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密码:</asp:Label>
                                            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" Width="159"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                CssClass="failureNotification" ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                        </p>
                                        <%--                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server" />
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">保持登录状态</asp:Label>
                    </p>--%>
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <p class="submitButton" style="margin-left: 230px;">
                            <asp:Button ID="LoginButton" runat="server" Text="登录" ValidationGroup="LoginUserValidationGroup"
                                OnClick="LoginButton_Click" CssClass="button white" />
                        </p>
                    </div>
                </fieldset>

            </div>
            <div style="position: absolute; bottom: 20px; text-align: center; color: #808080; margin-left:40px;">©2012 www.boce025.com  蜀ICP备12021897号</div>
        </div>
    </form>
</body>
</html>
