<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Chat.aspx.vb" Inherits="Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-2.0.3.min.js"></script>
    <script src="Scripts/jquery.signalR-0.5.2.min.js"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            // Proxy created on the fly
            var chat = $.connection.chat;

            // Declare a function on the chat hub so the server can invoke it
            chat.addMessage = function (message) {
                $('#messages').append('<li>' + message + '</li>');
            };

            $("#broadcast").click(function () {
                // Call the chat method on the server
                chat.send($('#msg').val());
            });

            // Start the connection
            $.connection.hub.start();
        });
    </script>
</head>
<body>
    <input type="text" id="msg" />
    <input type="button" id="broadcast" value="broadcast" />

    <ul id="messages">
    </ul>

</body>
</html>
