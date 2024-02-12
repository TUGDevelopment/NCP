<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CS.aspx.cs" Inherits="src_CS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <div>Barcode result: <span id="dbr"></span></div>
      <div class="select">
        <label for="videoSource">Video source: </label><select id="videoSource"></select>&nbsp<button id="go" style="display: none;">Capture</button>
        <input type=button value="Take Snapshot" onClick="take_snapshot()">
      </div>
      <br />
      <div>
        <video muted autoplay id="video" playsinline="true"></video>
        <canvas id="pcCanvas" width="640" height="480" style="display: none; float: bottom;"></canvas>
        <canvas id="mobileCanvas" width="240" height="320" style="display: none; float: bottom;"></canvas>
      </div>  
    </form>
    <script src="js/video.js"></script>
    	<script type="text/javascript">
            function take_snapshot() {
                debugger;
                var video = document.getElementById("video");

                alert("xxxx");
        		}
	</script> 
</body>
</html>
