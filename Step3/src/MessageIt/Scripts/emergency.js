(function () {
  var emergencyHub = $.connection.emergencyHub;

  emergencyHub.client.showAlert = function (name, message) {
    var msg = "The message [" + message + "] was sent to " + name;
    toastr.warning(msg, "YELLOW ALERT!!!");
  };

  $.connection.hub.start().done(function () {
    console.log("Hub started");    
  });
})();