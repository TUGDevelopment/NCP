<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="_Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        var timerHandle = -1;

        var goToNextRow = false;
        var currentEditRowIndex;

        function OnBatchEditStartEditing(s, e) {
            currentEditRowIndex = e.visibleIndex;
            clearTimeout(timerHandle);
        }

    	function OnEndCallback(s, e) {
    	    GoToNextRow()
    	}

    	function GoToNextRow() {
    	    if (goToNextRow) {
    	        goToNextRow = false;
    	        ASPxGridView1.batchEditApi.StartEdit(++currentEditRowIndex, 2);
    	        ASPxGridView1.GetEditor('C2').Focus();
    	    }
        }

    	function OnBodyKeyPress(event) {
            if (event.keyCode == 13 && IsASPxGridViewEditing()) {
                goToNextRow = true;
                ASPxGridView1.UpdateEdit();
    	    }
    	}


    	function OnBatchEditEndEditing(s, e) {
    	    timerHandle = setTimeout(function () {
    	        s.UpdateEdit();}, 500);
    	}
      
        function OnFocusedRowChanged(s, e) {
//                timerHandle = setTimeout(function () {
//                    s.UpdateEdit();
//                }, 500); 
        }

        function IsASPxGridViewEditing() {

            return ASPxGridView1.GetEditor("C2") != null;
        }


     



    </script>
</head>
<body onkeypress="OnBodyKeyPress(event)">


    <form id="frmMain" runat="server">
    <dx:ASPxCheckBox ID="BatchUpdateCheckBox" runat="server" Text="Handle BatchUpdate event"
        AutoPostBack="true" />
    <dx:ASPxGridView ID="ASPxGridView1" ClientInstanceName="ASPxGridView1" runat="server" KeyFieldName="ID" OnBatchUpdate="Grid_BatchUpdate"
        OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting">

        <SettingsBehavior AllowSelectSingleRowOnly="true" AllowFocusedRow="false" />
        <SettingsEditing Mode="Batch" BatchEditSettings-EditMode="Row" />

        <ClientSideEvents EndCallback="OnEndCallback" FocusedRowChanged="OnFocusedRowChanged"  BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing" />
     

        <Columns>
            <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" />
            <dx:GridViewDataColumn FieldName="C1"  ReadOnly ="true">
                
                
                <EditItemTemplate>
                    <dx:ASPxLabel ID="lblUserName" OnInit="lblUserName_OnInit"  runat="server" ></dx:ASPxLabel>
                </EditItemTemplate>

            </dx:GridViewDataColumn>
            <dx:GridViewDataSpinEditColumn FieldName="C2" />
            <%--<dx:GridViewDataTextColumn FieldName="C3" />
            <dx:GridViewDataCheckColumn FieldName="C4" />--%>
        </Columns>
        
        
    </dx:ASPxGridView>
    </form>
</body>
</html>