<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Chat.aspx.vb" Inherits="Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-2.1.1.js"></script>
    <script src="Scripts/jquery.signalR-2.1.1.min.js"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            // Proxy created on the fly
            var chat = $.connection.chat;

            // Declare a function on the chat hub so the server can invoke it
            chat.client.addMessage = function (message) {
                $('#messages').append('<li>' + message + '</li>');
            };


            // Start the connection
            $.connection.hub.start()
             .then(function() {
                $("#broadcast").click(function () {
                    // Call the chat method on the server
                    chat.server.send($('#msg').val());
                });
            });
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
