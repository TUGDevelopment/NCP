<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsqrcode.aspx.cs" Inherits="jsqrcode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style>
  .overlay {
    position: fixed;
    width: 100%;
    height: 100%;
    left: 10;
    top: 10;
    background: rgba(51,51,51,0.7);
    z-index: 10;
  }
  </style>
</head>
  <script src="js/jquery-1.11.3.min.js"></script>

<body onbeforeunload="refreshAndClose();" onLoad='doLoad()''>
<form id="form1" runat="server">
  <div>Barcode result: <span id="dbr"></span></div>
  <div class="select">
    <label for="videoSource">Video source: </label><select id="videoSource"></select>&nbsp<button id="go">Scan</button>&nbsp<button id="btnsave" onclick="fnclose()">close</button>
  </div>
  <br />
  <div style="position: relative; width: 100%; height: 100%"> 
    <video muted autoplay id="video" playsinline="true" class="overlay"></video>
    <canvas id="pcCanvas" width="640" height="480" style="display: none; float: bottom;"></canvas>
    <canvas id="mobileCanvas" width="240" height="320" style="display: none; float: bottom;"></canvas>
  </div>  
    </form>
</body>

<script async src="js/zxing.js"></script>
<script src="js/video.js"></script>
    <script type="text/javascript">
            //    function OnBtnClientClick(s, e) {
            //window.parent.HidePopupAndShowInfo('Client', "test");
        //}
//window.onload = load();

        var t;
        function doLoad() {
            debugger;
            //t = setTimeout("window.close()", 30);
            alert("xx");
        }
        function refreshAndClose() {
            //window.opener.location.reload(true);
            window.close();
        }
        function load(result) {
            //alert('document is ready.');
            window.parent.HidePopupAndShowInfo('Client', result);
        }
        function fnclose() {
            load('');
        }
        </script>
</html>
