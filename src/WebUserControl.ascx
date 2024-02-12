<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebUserControl.ascx.cs" Inherits="src_WebUserControl" %>
<script src="js/jquery-1.11.3.min.js"></script>  
<span id="dbr"></span>
  <div class="select">
    <label for="videoSource">Video source: </label>
    <select id="videoSource"></select>&nbsp<button id="go">Scan</button>&nbsp<button id="btnsave" onclick="fnclose()">close</button>
  </div>
  <br />
  <div> 
    <video muted autoplay id="video" playsinline="true" class="overlay"></video>
    <canvas id="pcCanvas" width="640" height="480" style="display: none; float: bottom;"></canvas>
    <canvas id="mobileCanvas" width="240" height="320" style="display: none; float: bottom;"></canvas>
  </div> 
<script async src="js/zxing.js"></script>
<script src="js/video.js"></script>
    <script type="text/javascript">
            //    function OnBtnClientClick(s, e) {
            //window.parent.HidePopupAndShowInfo('Client', "test");
        //}
//window.onload = load();
        $(function () {
            debugger;
            doLoad();
        });    
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