$("#msgform").submit(function (event) {
  /* stop form from submitting normally */
  event.preventDefault();

  var name = $('#name').val(),
      message = $('#message').val();

  $.ajax({
    method: "POST",
    url: "api/message",
    data: { name: name, message: message },

    // x is a FormatedMessage, so access its content field for the 
    // message that was sent
    success: function (x) {
      toastr.info("Message sent: " + x.content);
    },
    error: function (x) {
      console.log(x);
      toastr.error(x.responseJSON, ":( boooo");
    }
  });
});