
// hide sender name in dropdown list
var selectedName = document.getElementById("sender").textContent;
var selectedOption = "option".concat(selectedName);
document.getElementById(selectedOption).style.display = "none";
// ----

var receiverName = "";

//if (receiverName != "") {
//    setInterval(function () {
//        LoadData();
//    }, 5000);
//}

setInterval(function () {
    LoadData();
}, 5000);


function LoadData() {

    if (receiverName == "") {
        document.getElementById("chatMessage").disabled = true;
        document.getElementById("sendButton").disabled = true;
        document.getElementById("messageList").hidden = true;
        document.getElementById("chatReminder").hidden = false;
    }
    else {
        document.getElementById("chatMessage").disabled = false;
        document.getElementById("sendButton").disabled = false;
        document.getElementById("messageList").hidden = false;
        document.getElementById("chatReminder").hidden = true;
    }
    $("#messageList tbody tr").remove();
    $.ajax({
        type: 'POST',
        url: '@Url.Action("GetMessage")',
        dataType: 'json',
        data: { receiver: receiverName },
        success: function (data) {
            //var items = '';
            $.each(data, function (index, item) {
                var rows;
                if (item.FromName == selectedName) {
                    rows = "<tr>" + "<td class='prtoducttd'>" + "<div class='sentMessage'>" + item.Content + "<br/>" + "Posted on: " + FormatDateString(item.DateTime) + "</div>" + "</td>" + "</tr>";
                }
                else {
                    rows = "<tr>" + "<td class='prtoducttd'>" + "<div class='receivedMessage'>" + item.Content + "<br/>" + "Posted on: " + FormatDateString(item.DateTime) + "</div>" + "</td>" + "</tr>";
                }
                $('#messageList tbody').append(rows);
            });
        },
        error: function (ex) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });
    return false;
}

// filter the chat messages by selected receiver name
document.getElementById("receiverList").addEventListener("change", function () {
    receiverName = document.getElementById("receiverList").value;
    document.getElementById("receiver").innerHTML = receiverName;
    LoadData();
    console.log($("#sender").text());
    console.log($("#receiver").text());
});

// count the number of character in textarea
document.getElementById("chatMessage").addEventListener("keyup", function () {
    var messageLength = $("#chatMessage").val().length;
    if (messageLength < 99) {
        document.getElementById("counterMessage").innerHTML = "This message can cantain " + (100 - messageLength) + " more characters.";
        document.getElementById("counterMessage").style.color = "gray";
    }
    else if (messageLength == 99) {
        document.getElementById("counterMessage").innerHTML = "This message can cantain one more character.";
        document.getElementById("counterMessage").style.color = "blue";
    }
    else {
        document.getElementById("counterMessage").innerHTML = "Maximum 100 characters per message!";
        document.getElementById("counterMessage").style.color = "red";
    }
});


$(document).ready(function () {
    LoadData();
    $("#sendButton").click(function () {
        //alert("");
        var chat = {};
        chat.FromName = $("#sender").text();
        chat.ToName = $("#receiver").text();
        chat.Content = $("#chatMessage").val();
        $.ajax({
            type: "POST",
            url: '@Url.Action("CreateMessage")',
            data: '{chatHistory: ' + JSON.stringify(chat) + '}',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                // alert("Data has been added successfully.");
                LoadData();
                $("#chatMessage").val("");
            },
            error: function () {
                alert("Error while inserting data. Please note the message can not be empty!");
            }
        });

        return false;
    });

});


// convert Datetime format to string for showing in table
function FormatDateString(logDate) {
    var d = new Date(parseInt(logDate.substr(6)));
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var hour = d.getHours();
    var minutes = d.getMinutes();
    var sec = d.getSeconds();

    return month + "/" + day + "/" + year + " " + hour + ":" + minutes + ":" + sec;
}
// ----

