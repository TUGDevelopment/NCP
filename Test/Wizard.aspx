<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Wizard.aspx.cs" Inherits="Test_Wizard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <div>
                <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" BackColor="#FFFBD6" BorderColor="#FFDFAD" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" Height="210px" OnNextButtonClick="Wizard1_NextButtonClick" Width="496px">
                    <HeaderStyle BackColor="#FFCC66" BorderColor="#FFFBD6" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" Font-Size="0.9em" ForeColor="#333333" HorizontalAlign="Center" />
                    <NavigationButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
                    <SideBarButtonStyle ForeColor="White" />
                    <SideBarStyle BackColor="#990000" Font-Size="0.9em" VerticalAlign="Top" />
                    <WizardSteps>
                        <asp:WizardStep ID="WizardStep1" runat="server" Title="Student Details">
                            <table style="width: 100%;">
                                <tr>
                                    <td><strong>Student Details</strong></td>
                            
                                </tr>
                                <tr>
                                    <td>Student FirstName</td>
                                    <td><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Student LastName</td>
                                    <td><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>


                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep2" runat="server" Title="Student Course Details">
                             <table style="width: 100%;">
                            <tr>
                                <td><strong>Student Course Detail</strong></td>
                       
                            </tr>
                            <tr>
                                <td>Student Course</td>
                                <td><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                       
                            </tr>
                            <tr>
                                <td>Student Branch</td>
                                <td><asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                        
                            </tr>
                        </table>

                        </asp:WizardStep>
                         <asp:WizardStep ID="WizardStep3" runat="server" Title="Student Personal Details">
                        <table style="width: 100%;">
                            <tr>
                                <td><strong>Student Personal Detail</strong></td>
                      
                            </tr>
                            <tr>
                                <td>Student EmailId</td>
                                <td><asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                        
                            </tr>
                            <tr>
                                <td>Student City</td>
                                <td><asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                       
                            </tr>
                            <tr>
                                <td>Student State</td>
                                <td><asp:TextBox ID="TextBox7" runat="server"></asp:TextBox></td>
                       
                            </tr>
                        </table>

                    </asp:WizardStep>

                         <asp:WizardStep ID="WizardStep4" runat="server" Title="Student Summary">
                     
                             <table class="auto-style1">
                                 <tr>
                                     <td><strong>Student Details</strong></td>
                             
                                     <td>&nbsp;</td>
                             
                                 </tr>
                                 <tr>
                                     <td>
                                         Student FirstName:</td>
                                     <td>
                                         <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                         </td>
                                     <td>&nbsp;</td>
                             
                                 </tr>
                                 <tr>
                                     <td>Student LastName:</td>
                             
                                     <td>
                                         <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                     </td>
                             
                                 </tr>
                         
                                 <tr>
                                     <td></td>
                                     <td>&nbsp;</td>
                                 </tr>
                                 <tr>
                                     <td><strong>Student Course Details</strong></td>
                                     <td>&nbsp;</td>
                                 </tr>
                         
                                 <tr>
                                     <td>Student Course:</td>
                                     <td>
                                         <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                     </td>
                                 </tr>
                                  <tr>
                                     <td>Student Branch:</td>
                                     <td>
                                         <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                      </td>
                                 </tr>
                         
                                 <tr>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                 </tr>
                                 <tr>
                                     <td><strong>Student Personal Details</strong></td>
                                     <td>&nbsp;</td>
                                 </tr>
                                 <tr>
                                     <td>Student EmailId:</td>
                                     <td>
                                         <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>Student City:</td>
                                     <td>
                                         <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>Student State:</td>
                                     <td>
                                         <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                                     </td>
                                 </tr>
                         
                             </table>
                        </asp:WizardStep>
                        </WizardSteps>         
                    </asp:Wizard>
                </div>
    </form>
</body>
</html>
