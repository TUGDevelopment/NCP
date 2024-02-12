<%@ Control Language="C#" AutoEventWireup="true" CodeFile="login.ascx.cs" Inherits="UserControls_EditForms_login" %>
<script type="text/javascript">
    function close_window() {
        var x = confirm('Are You sure want to exit:');
        if (x) window.close();
    }
    </script>
<div class="mainMenu">
<dx:ASPxPopupControl ID="pcLogin" runat="server" Width="320" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcLogin"
        HeaderText="Login" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbLogin.Focus(); }" Init="function(s, e) { s.Show();}" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <%--<asp:Login ID="Login1" runat="server" OnAuthenticate="ValidateUser"/>--%>
                            <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" Width="100%" Height="100%">
                                <Items>
                                    <dx:LayoutItem Caption="Username">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                
                                                <dx:ASPxTextBox ID="tbLogin" runat="server" Width="100%" ClientInstanceName="tbLogin">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom" CausesValidation="True">
                                                        <RequiredField ErrorText="Username required" IsRequired="True" />
                                                        <RegularExpression ErrorText="Login required" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Password">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox ID="tbPassword" runat="server" Width="100%" Password="True">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom">
                                                        <RequiredField ErrorText="Password required" IsRequired="True" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <%--<dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxCheckBox ID="chbRemember" runat="server" Text="Remember me">
                                                </dx:ASPxCheckBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>--%>
                                    <dx:LayoutItem ShowCaption="False" Paddings-PaddingTop="19">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <table>
                                                    <tr>
                                                        <td>
                                                    </td>
                                                        <td>
                                                            <dx:ASPxButton ID="btnImageAndText" runat="server" Text="Login" AutoPostBack="false" OnClick="btOK_Click">
                                                            <Image  Url="~/Content/Images/icons8-customer-32.png"></Image></dx:ASPxButton> 
                                                           <dx:ASPxButton ID="btnBlueBall" runat="server" AutoPostBack="False" AllowFocus="False" RenderMode="Link" EnableTheming="False">
                                                                <Image>
                                                                    <SpriteProperties CssClass="blueBall" HottrackedCssClass="blueBallHottracked" PressedCssClass="blueBallPressed" />
                                                                </Image> 
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--<dx:ASPxButton ID="btOK" runat="server" Text="Login" Width="80px" AutoPostBack="False" OnClick="btOK_Click" Style="float: left; margin-right: 8px">
                                                    <Image  Url="~/Content/Images/Update.gif"></Image>
                                                    <ClientSideEvents Click="function(s, e) { if(ASPxClientEdit.ValidateGroup('entryGroup')) alert('test');}" />
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="btCancel" runat="server" Text="Close" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                    <Image  Url="~/Content/Images/Cancel.gif"></Image>
                                                    <ClientSideEvents Click="close_window" />
                                                </dx:ASPxButton>--%>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:ASPxFormLayout>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
                <%--<div>
                    <a href="javascript:ShowCreateAccountWindow();" id="hl1" style="float: right; margin: 14px 0 10px 0;">Create Account</a>
                </div>--%>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dx:ASPxPopupControl></div>
